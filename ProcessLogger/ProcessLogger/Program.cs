using System.Diagnostics;
using ProcessLogger;


/** A very early prototype to gather log the process run-times
*/

var processes = ProcessLogger.ProcessTracker.RunningProcesses();
var log  = FileLog.FileLog.InDocuments("logged_process.txt");
while(true) 
{
    foreach (var proc in processes)
    {
        float procTime = proc.GetProcessorTime();
        if (procTime > 0)
        {
            Console.WriteLine($"logging {proc.Name}");
            log.AddEntry(proc.Name, procTime);
        }        
    }
    Thread.Sleep(10000);
}    
