
using System.Diagnostics;

/** Logs results from a single Process.
Provides factory methods to generalize easily create an instance for the 
whole system.
*/
class ProcessLogger{
    public static string[] RunningProcessesNames(){
        PerformanceCounterCategory cat = new("Process");
        return cat.GetInstanceNames();
    }

    public static IEnumerable<ProcessLogger> RunningProcesses(){
        PerformanceCounterCategory cat = new("Process");
        List<ProcessLogger> result = new();

        foreach(string name in cat.GetInstanceNames()){
            result.Add(new ProcessLogger(name));
        }
        return result;
    }


    private string name;
    private ProcessLogger(string name){
        this.name = name;
    }
}