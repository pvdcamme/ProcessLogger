namespace ProcessLogger_Test
{
    using ProcessLogger;
    public class PocressLogger
    {        
        [Fact]
        public void ExpectsSomeProcesses()
        {
            var current_processes = ProcessLogger.RunningProcesses();
            Assert.NotEmpty(current_processes);
        }
    }
}