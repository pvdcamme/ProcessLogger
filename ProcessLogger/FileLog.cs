using System.Reflection.Metadata;
using System.Runtime.ConstrainedExecution;
using System.Text;

namespace FileLog
{
    // A single Logged value, as captured at a specific moment.    
    public readonly struct LogEntry
    {
        public string Key { get; }
        public float Value { get; }
        public long When { get; }

        LogEntry(string key, float value, long when)
        {
            Key = key;
            Value = value;
            When = when;
        }

        public LogEntry(string key, float value)
        {
            Key = key;
            Value = value;
            When = DateTime.Now.Ticks;
        }
        public readonly override string ToString()
        {
            return $"{When}::{Key}::{Value}";
        }

        internal static LogEntry FromLine(string line)
        {
            string[] parts = line.Split("::");
            var when = long.Parse(parts[0]);
            var key = parts[1];
            var val = float.Parse(parts[2]);

            return new(key, val, when);
        }
    }

    public class ReverseFileReader: IDisposable
    {
        private readonly byte[] newLineSplt = Encoding.UTF8.GetBytes("\r\n");
        private readonly string _name;
        private readonly List<string> _lines;
        private long _read_offset;
        private readonly int _bufferSize = 1024 * 32;

        public ReverseFileReader(string name)
        {
            _name = name;
            _lines = new List<string>();

            using FileStream _file = new(_name, FileMode.Open);
            _read_offset = _file.Length;
        }

        public void Dispose()
        {
            // Not required at the moment.
        }

        private static int CollectLastBuffer(string name, long endPosition, byte[] toFill)
        {            
            using FileStream file = new(name, FileMode.Open);
            long toread = Math.Min(toFill.LongLength, endPosition);
            file.Position = endPosition - toread;
            file.ReadExactly(toFill, 0, (int) toread);
            return (int)toread;
        }

        private void FillBuffer()
        {
            if(_read_offset == 0 )
            {
                return;
            }
            byte[] buffer = new byte[_bufferSize];
            int totalRead = CollectLastBuffer(_name,_read_offset, buffer);
            
            int lastLineCollection = totalRead - newLineSplt.Length;
            for(int ctr=lastLineCollection; ctr >= 0; ctr--) 
            {                
                bool hasFound = true;
                for(int innerCtr= 0; innerCtr < newLineSplt.Length && hasFound; innerCtr++)
                {
                    hasFound = (newLineSplt[innerCtr] == buffer[ctr + innerCtr]) && hasFound;
                }
                if (hasFound && (lastLineCollection - ctr) > newLineSplt.Length)
                {
                    int lineStart = ctr + newLineSplt.Length;
                    var res = Encoding.UTF8.GetString(buffer, lineStart, lastLineCollection-lineStart);
                    _lines.Add(res);
                }
                if (hasFound)
                {
                    lastLineCollection = ctr;
                }
            }
            if (totalRead < _bufferSize)
            {
                var res = Encoding.UTF8.GetString(buffer, 0, lastLineCollection);
                _lines.Add(res);
                _read_offset = 0;
            }
            else
            {
                _read_offset -= totalRead - (lastLineCollection + newLineSplt.Length);
            }
            _lines.Reverse();
        }

        public string ReadLine()
        {
            if (_lines.Count == 0)
            {
                FillBuffer();
            }
            if (_lines.Count > 0)
            {
                var res = _lines.Last();
                _lines.RemoveAt(_lines.Count - 1); 
                return res;
            } else
            {
                return "";
            }            
        }
    }
    

    // Persistenly saves/restores Logentries.
    public class FileLog
    {
        public static FileLog InUserDocuments(string Name)
        {
            string docPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string finalDest = Path.Combine(docPath, Name);
            return new FileLog(finalDest);
        }
        private string SavePath { get; }

        public FileLog(string SavePath)
        {
            this.SavePath = SavePath;
        }

        public void Reset()
        {
            File.Delete(SavePath);
        }

        public void AddEntry(string key, float val)
        {
            LogEntry entry = new(key, val);
            AddEntry(entry);
        }

        public void AddEntry(LogEntry entry)
        {
            using StreamWriter output = new(SavePath, true);
            output.WriteLine(entry.ToString());
        }

        public IEnumerable<String> GetKeys()
        {
            HashSet<String> keys = new();
            foreach (var Entry in GetEntries())
            {
                keys.Add(Entry.Key);
            }
            return keys;
        }

        public IEnumerable<LogEntry> GetEntries()
        {
            if (File.Exists(SavePath))
            {
                using StreamReader reader = new(SavePath);
                string? line = reader.ReadLine();
                while (line != null)
                {
                    yield return LogEntry.FromLine(line);
                    line = reader.ReadLine();
                }
            }
            // Empty is ok
        }
    }
}