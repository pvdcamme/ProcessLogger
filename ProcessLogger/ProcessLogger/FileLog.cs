using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace FileLog
{
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

        public void AddEntry(string key, float val)
        {
            using(StreamWriter output = new StreamWriter(SavePath))
            {
                output.WriteLine($"{key} :: {val}");
            }
        }
    }
}
