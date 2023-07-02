using System.Diagnostics;


/** A very early prototype to gather log the process run-times
*/
foreach (var proc in ProcessLogger.RunningProcesses())
{
    try
    {
        Console.WriteLine(proc.Name + " -- " + proc.GetProcessorTime());
    }
    catch (System.InvalidOperationException)
    {
        Console.WriteLine(proc.Name + " does not support getting processor Time");
    }
}
