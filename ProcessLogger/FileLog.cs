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
        private readonly string _name;
        private readonly FileStream _stream;

        public ReverseFileReader(string name)
        {
            this._name = name;
            this._stream = new(_name, FileMode.Open, FileAccess.Read); 
        }

        public void Dispose()
        {
            ((IDisposable)_stream).Dispose();
        }

        public long GetFileSize()
        {
            return _stream.Length;
        }

        public string ReadLine()
        {
            throw new NotImplementedException();
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