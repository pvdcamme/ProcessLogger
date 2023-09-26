﻿namespace ProcessLogger
{
    // Tracks processes of a whole system.
    // Much decoupling from the actual Processtracker. This helps testing.
    public class SystemTracker
    {
        private readonly Dictionary<string, IProcessTracker> processes = new();
        private readonly Func<string, IProcessTracker> trackerFactory;

        // A lower coupling approach, mostly useful for testing purposes.
        // Initilizers can be added as more functionality is implemented.
        public SystemTracker(Func<string, IProcessTracker> trackerFactory)
        {
            this.trackerFactory = trackerFactory;
        }

        // Updates the saved counters based on the currently known processes.
        // Creates new counters when necessary.
        public IEnumerable<string> MergeUnknownProcesses(IEnumerable<string> currentProcesses)
        {
            HashSet<string> unknown = new(currentProcesses);
            HashSet<string> current = new(currentProcesses);
            current.IntersectWith(processes.Keys);

            foreach (string alreadySeen in current)
            {
                unknown.Remove(alreadySeen);
            }

            foreach (string notYetSeen in unknown)
            {
                processes.Add(notYetSeen, trackerFactory(notYetSeen));
            }

            return unknown;
        }

        // Reports Processor load stats for the currently tracked processes.
        public IEnumerable<(string, float)> GetTrackedProcesses()
        {
            HashSet<string> toRemove = new();
            foreach (var process in processes)
            {
                string name = process.Key;
                var counter = process.Value;
                float cpuload = counter.GetProcessorTime();
                if (counter.IsFailing())
                {
                    toRemove.Add(name);
                }
                yield return (name, cpuload);
            }

            foreach(var failed in toRemove)
            {
                processes.Remove(failed);
            }
        }
    }
}
