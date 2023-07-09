namespace ProcessLogger_Test
{
    using ProcessLogger;
    using System.Linq.Expressions;

    public class ProcessTests
    {        
        [Fact]
        public void ExpectsSomeProcesses()
        {
            var current_processes = ProcessLogger.RunningProcesses();
            Assert.NotEmpty(current_processes);
        }


        [Fact]
        public void CheckHasNextValues()
        {
            var current_processes = ProcessLogger.RunningProcesses();
            foreach(ProcessLogger aLogger in ProcessLogger.RunningProcesses())
            {
                Assert.True(aLogger.GetProcessorTime() >= 0, "Postive CPU time expexted");
            }            
        }
    }
}