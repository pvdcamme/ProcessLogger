﻿using System.Diagnostics;
using System.Drawing.Drawing2D;
using ProcessLogger;


/** A very early prototype to gather log the process run-times
*/

var processes = ProcessLogger.ProcessTracker.RunningProcesses();
var log  = FileLog.FileLog.InDocuments("logged_process.txt");
while(true) 
{
    Console.Write("write new logs");
    foreach (var proc in processes)
    {
        float procTime = proc.GetProcessorTime();
        if (procTime > 0)
        {
            log.AddEntry(proc.Name, procTime);
        }        
    }
    Thread.Sleep(10000);
}    
