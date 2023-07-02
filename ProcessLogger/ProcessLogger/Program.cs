using System.Diagnostics;


/** A very early prototype to gather log the process run-times
*/

var processes = ProcessLogger.RunningProcesses();
for(int ctr = 0; ctr < 10; ++ctr)
{
    foreach (var proc in processes)
    {
        try
        {
            float time = proc.GetProcessorTime();
            if (time > 0){
                Console.WriteLine(proc.Name + " -- " + time);
            }
        }
        catch (System.InvalidOperationException)
        {
            Console.WriteLine(proc.Name + " does not support getting processor Time");
        }
    }
    Thread.Sleep(1000);
}
