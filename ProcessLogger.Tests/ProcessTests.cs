namespace ProcessLogger.Tests
{
    using ProcessLogger;

    public class ProcessTests
    {
        // At least some processes should be running.
        [Fact]
        public void ExpectsSomeProcesses()
        {
            var currentProcesses = ProcessTracker.RunningProcesses();
            Assert.NotEmpty(currentProcesses);
        }


        [Fact]
        public void HasProcessTime()
        {
            foreach (var aLogger in ProcessTracker.RunningProcesses())
            {
                Assert.True(aLogger.GetProcessorTime() >= 0, "Postive CPU time expexted");
                Assert.NotEmpty(aLogger.Name);
            }
        }
    }
}