namespace ProcessLogger.Tests
{
    public class SystemTrackerTests
    {
        // The most simple implementation of a ProcessTracker.
        class DummyTracker : IProcessTracker
        {
            public string Name { get; }
            public DummyTracker(string name)
            {
                this.Name = name;
            }

            float IProcessTracker.GetProcessorTime()
            {
                return 0;
            }

            bool IProcessTracker.IsFailing()
            {
                return false;
            }
        }

        readonly Func<string, IProcessTracker> dummyFactory = (string name) => { return new DummyTracker(name); };

        [Fact]
        public void InitiallyEmptState()
        {           
                SystemTracker tracker = new(dummyFactory);
                Assert.Empty(tracker.GetTrackedProcesses());            
        }

        [Fact]
        public void FillingUpTheTracker()
        {
            string[] dummyProcesses = { "aaa", "asdf", "anotherTest" };

            SystemTracker tracker = new(dummyFactory);
            var unknown = tracker.MergeUnknownProcesses(dummyProcesses);
            Assert.Equal(dummyProcesses, unknown);
        }

        [Fact]
        public void MergingTheTracker()
        {
            string[] initialProcesses = { "aaa", "asdf", "anotherTest" };
            string[] nextProcesseses = { "bbb", "the full test", "anotherTest" };

            SystemTracker tracker = new(dummyFactory);
            tracker.MergeUnknownProcesses(initialProcesses);
            var finalList = tracker.MergeUnknownProcesses(nextProcesseses);

            Assert.Contains("bbb", finalList);
            Assert.Contains("the full test", finalList);
            Assert.Equal(2, finalList.Count());
        }
    }
}
