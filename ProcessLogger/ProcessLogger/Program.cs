using System.Diagnostics;


/** A very early prototype to gather log the process run-times
*/
foreach (var proc in ProcessLogger.RunningProcesses())
{
    try
    {
        PerformanceCounter counter = new("Process", "% Processor Time", proc.name, true);
        Console.WriteLine(proc.name + " -- " + counter.NextValue());
    }
    catch (System.InvalidOperationException)
    {
        Console.WriteLine(proc.name + " does not support getting processor Time");
    }
}
