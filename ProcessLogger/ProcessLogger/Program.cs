using System.Diagnostics;
using ProcessLogger;


/** A very early prototype to gather log the process run-times
*/

var processes = ProcessLogger.ProcessTracker.RunningProcesses();
for (int ctr = 0; ctr < 10; ++ctr)
{
    foreach (var proc in processes)
    {
        float time = proc.GetProcessorTime();
        if (time > 0)
        {
            Console.WriteLine(proc.Name + " -- " + time);
        }
    }
    Thread.Sleep(1000);
}
