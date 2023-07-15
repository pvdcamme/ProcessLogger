
using System.Diagnostics;

namespace ProcessLogger
{
    /** Logs results from a single Process.
Provides factory methods to generalize easily create an instance for the 
whole system.()
*/
    public class ProcessLogger
    {

        public static IEnumerable<ProcessLogger> RunningProcesses()
        {
            PerformanceCounterCategory cat = new("Process");
            List<ProcessLogger> result = new();

            foreach (string name in cat.GetInstanceNames())
            {
                result.Add(new ProcessLogger(name));
            }
            return result;
        }

        public string Name { get; }
        private PerformanceCounter Counter { get; }
        private int ReadFailures;

        private ProcessLogger(string name)
        {
            this.Name = name;
            this.Counter = new("Process", "% Processor Time", name, true);
        }

        public bool IsFailing()
        {
            return ReadFailures > 0;
        }

        public float GetProcessorTime()
        {
            const float DEFAULT_TIME = 0;
            try
            {
                return Counter.NextValue();
            }
            catch (InvalidOperationException)
            {
                ReadFailures++;
            }
            return DEFAULT_TIME;
        }
    }
}