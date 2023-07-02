
using System.Diagnostics;

class ProcessLogger{
    public static string[] RunningProcesses(){
        PerformanceCounterCategory cat = new("Process");
        return cat.GetInstanceNames();
    }
}