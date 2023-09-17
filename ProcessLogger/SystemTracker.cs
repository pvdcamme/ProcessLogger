using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessLogger
{
    // Tracks all process of the whole system.
    public class SystemTracker
    {
        private readonly Dictionary<string, IProcessTracker> processes = new();

        public IEnumerable<string> UnknownProcesses(IEnumerable<string> currentProcesses)
        {
            HashSet<string> unknown = new(currentProcesses);
            HashSet<string> current = new(currentProcesses);
            current.IntersectWith(processes.Keys);

            foreach (string alreadySeen in current)
            {
                unknown.Remove(alreadySeen);
            }

            return unknown;
        }

        public IEnumerable<string> GetTrackedProcesses()
        {
            return processes.Keys;
        }
    }
}
