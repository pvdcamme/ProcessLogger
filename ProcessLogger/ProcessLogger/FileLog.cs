using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Reflection.PortableExecutable;

namespace FileLog
{
    public readonly struct LogEntry
    {
        public string Key { get; }
        public float Value { get; }
        public long When { get; }

        public LogEntry(string key, float value, long when)
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

    public class FileLog
    {
        public static FileLog InDocuments(string Name)
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
            using(StreamWriter output = new(SavePath,true))
            {
                var entry = new LogEntry(key, val);
                output.WriteLine(entry.ToString()); 
            }
        }

        public IEnumerable<LogEntry> GetEntries()
        {
            List<LogEntry> entries = new();

            using(StreamReader reader = new(SavePath))
            {
                string? line = reader.ReadLine();
                while (line != null) 
                {
                    entries.Add(LogEntry.FromLine(line));
                    line = reader.ReadLine();
                }
            }
            return entries;

        }
    }
}
