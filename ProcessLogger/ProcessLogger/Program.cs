﻿using System.Diagnostics;


/** A very early prototype to gather log the process run-times
*/
PerformanceCounterCategory cat = new("Process");
string[] runningProcesses = cat.GetInstanceNames();
foreach (var name in runningProcesses)
{
    try
    {
        PerformanceCounter counter = new("Process", "% Processor Time", name, true);
        Console.WriteLine(name + " -- " + counter.NextValue());

    }
    catch (System.InvalidOperationException)
    {
        Console.WriteLine(name + " does not support getting processor Time");
    }
}