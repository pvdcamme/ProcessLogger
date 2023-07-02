using System.Diagnostics;


/** A very early prototype to gather log the process run-times
*/
foreach (var proc in ProcessLogger.RunningProcesses())
{
    try
    {
        PerformanceCounter counter = new("Process", "% Processor Time", proc.Name, true);
        Console.WriteLine(proc.Name + " -- " + counter.NextValue());
    }
    catch (System.InvalidOperationException)
    {
        Console.WriteLine(proc.Name + " does not support getting processor Time");
    }
}
