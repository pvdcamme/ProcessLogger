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

        internal static LogEntry FromLine()
        {
            throw new NotImplementedException();
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
            using(StreamWriter output = new StreamWriter(SavePath))
            {
                var entry = new LogEntry(key, val);
                output.WriteLine(entry.ToString()); 
            }
        }

        public IEnumerable<LogEntry> GetEntries()
        {
            List<LogEntry> entries = new();
            return entries;
        }
    }
}
