namespace ProcessLogger_Test
{
    using ProcessLogger;
    using System.Linq.Expressions;

    public class ProcessTests
    {
        [Fact]
        public void ExpectsSomeProcesses()
        {
            var currentProcesses = ProcessLogger.RunningProcesses();
            Assert.NotEmpty(currentProcesses);
        }


        [Fact]
        public void CheckHasNextValues()
        {
            foreach (ProcessLogger aLogger in ProcessLogger.RunningProcesses())
            {
                Assert.True(aLogger.GetProcessorTime() >= 0, "Postive CPU time expexted");
            }
        }
    }
}