namespace ProcessLogger_Test
{
    using ProcessLogger;

    public class ProcessTests
    {
        [Fact]
        public void ExpectsSomeProcesses()
        {
            var currentProcesses = ProcessTracker.RunningProcesses();
            Assert.NotEmpty(currentProcesses);
        }


        [Fact]
        public void CheckHasNextValues()
        {
            foreach (ProcessTracker aLogger in ProcessTracker.RunningProcesses())
            {
                Assert.True(aLogger.GetProcessorTime() >= 0, "Postive CPU time expexted");
            }
        }
    }
}