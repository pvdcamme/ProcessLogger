using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessLogger.Tests
{
    public class SystemTracketTests
    {
        [Fact]
        public void Creating()
        {
            try
            {
                SystemTracker tracker = new();
                Assert.Empty(tracker.GetTrackedProcesses());
            }
            catch (Exception)
            {
                Assert.Fail("Should be able to create an instance");
            }
        }
    }
}
