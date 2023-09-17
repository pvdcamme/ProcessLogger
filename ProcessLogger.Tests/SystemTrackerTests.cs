using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessLogger.Tests
{
    public class SystemTrackerTests
    {
        [Fact]
        public void InitiallyEmptState()
        {           
                SystemTracker tracker = new();
                Assert.Empty(tracker.GetTrackedProcesses());            
        }

        [Fact]
        public void FillingUpTheTracker()
        {
            string[] dummyProcesses = { "aaa", "asdf", "anotherTest" };

            SystemTracker tracker = new();
            var unknown = tracker.UnknownProcesses(dummyProcesses);
            Assert.Equal(dummyProcesses, unknown);
        }
    }
}
