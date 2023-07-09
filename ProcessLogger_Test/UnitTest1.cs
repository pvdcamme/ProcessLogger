namespace ProcessLogger_Test
{
    using ProcessLogger;
    public class UnitTest1
    {        
        [Fact]
        public void ExpectsSomeProcesses()
        {
            var current_processes = ProcessLogger.RunningProcesses();
            Assert.NotEmpty(current_processes);
        }
    }
}