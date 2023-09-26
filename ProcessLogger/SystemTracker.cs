namespace ProcessLogger
{
    // Tracks processes of the whole system.
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
        }
    }
}
