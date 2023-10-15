﻿using System.Reflection.Metadata;
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
        private static readonly byte[] reverseSearch = Encoding.UTF8.GetBytes("\r\n").Reverse().ToArray();
        private readonly string _name;
        private readonly List<string> _lines;
        private long _read_offset;
        private readonly int _bufferSize = 1024 * 32;

        public ReverseFileReader(string name)
        {
            _name = name;
            _lines = new();

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

        private static bool IsEndOfNewline(byte[] buffer, int offset)
        {
            int innerCtr;
            for (innerCtr = 0; innerCtr < reverseSearch.Length; innerCtr++)
            {
                if (reverseSearch[innerCtr] != buffer[offset - innerCtr])
                {
                    return false;
                }
            }
            return innerCtr == reverseSearch.Length;
        }

        private void FillBuffer()
        {
            if (_read_offset == 0)
            {
                return;
            }

            byte[] buffer = new byte[_bufferSize];
            int totalRead = CollectLastBuffer(_name, _read_offset, buffer);
            bool atFileEnd = totalRead < _bufferSize;

            int lastLineCollection = totalRead - 1;
            for (int ctr = lastLineCollection; ctr >= reverseSearch.Length; ctr--)
            {
                if (!IsEndOfNewline(buffer, ctr))
                {
                    continue;
                }

                bool hasMoreContent = (lastLineCollection - ctr) > reverseSearch.Length;
                if (hasMoreContent)
                {
                    int lineStart = ctr + 1; // Don't need last char of newline
                    var res = Encoding.UTF8.GetString(buffer, lineStart, lastLineCollection - lineStart);
                    _lines.Add(res);
                }

                
                lastLineCollection = ctr - reverseSearch.Length + 1;
                ctr = lastLineCollection - reverseSearch.Length;                
            }

            if (atFileEnd)
            {
                var res = Encoding.UTF8.GetString(buffer, 0, lastLineCollection);
                _lines.Add(res);
                _read_offset = 0;
            }
            else
            {
                _read_offset -= totalRead - (lastLineCollection + reverseSearch.Length);
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