
using System.Diagnostics;

/** Logs results from a single Process.
Provides factory methods to generalize easily create an instance for the 
whole system.()
*/
class ProcessLogger{

    public static IEnumerable<ProcessLogger> RunningProcesses(){
        PerformanceCounterCategory cat = new("Process");
        List<ProcessLogger> result = new();

        foreach(string name in cat.GetInstanceNames()){
            result.Add(new ProcessLogger(name));
        }
        return result;
    }

    private PerformanceCounter Counter;

    public string Name {get;}
    

    private ProcessLogger(string name){
        this.Name = name;
        this.Counter = new("Process", "% Processor Time", name, true);
    }

    public float GetProcessorTime(){
        return Counter.NextValue();
    }
}