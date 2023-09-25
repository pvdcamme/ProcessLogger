﻿namespace ProcessLogger
{
    // Tracks all processes of the whole system.
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

        public IEnumerable<string> UnknownProcesses(IEnumerable<string> currentProcesses)
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

        public IEnumerable<string> GetTrackedProcesses()
        {
            return processes.Keys;
        }
    }
}
