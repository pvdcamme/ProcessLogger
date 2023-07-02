using System.Diagnostics;


/** A very early prototype to gather log the process run-times
*/
foreach (var name in ProcessLogger.RunningProcesses())
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
