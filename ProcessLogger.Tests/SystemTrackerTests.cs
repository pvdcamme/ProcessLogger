namespace ProcessLogger.Tests
{
    public class SystemTrackerTests
    {
        // The most simple implementation of a ProcessTracker.
        class DummyTracker : IProcessTracker
        {
            // For later testing, a dummy always returns the same time
            public static readonly float PROCESSOR_TIME = 34f;
            public string Name { get; }
            public DummyTracker(string name)
            {
                this.Name = name;
            }

            float IProcessTracker.GetProcessorTime()
            {
                return PROCESSOR_TIME;
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
            Assert.Equal(dummyProcesses.Length, tracker.GetTrackedProcesses().Count());
        }

        [Fact]
        public void MergingTheTrackedProcesses()
        {
            string[] initialProcesses = { "aaa", "asdf", "anotherTest" };
            string[] nextProcesseses = { "bbb", "the full test", "anotherTest" };

            SystemTracker tracker = new(dummyFactory);
            tracker.MergeUnknownProcesses(initialProcesses);
            var finalList = tracker.MergeUnknownProcesses(nextProcesseses);

            //Manually counted from above lists.
            Assert.Contains("bbb", finalList);
            Assert.Contains("the full test", finalList);
            Assert.Equal(2, finalList.Count());
            Assert.Equal(5, tracker.GetTrackedProcesses().Count());

            // Count should stay the same even after repeated requests.
            Assert.Equal(5, tracker.GetTrackedProcesses().Count());
        }

        [Fact]
        public void CheckProcessorTime()
        {
            string[] initialProcesses = { "aaa", "asdf", "anotherTest" };
            SystemTracker tracker = new(dummyFactory);
            tracker.MergeUnknownProcesses(initialProcesses);

            foreach (var (_, ProcessorTime) in tracker.GetTrackedProcesses())
            {
                Assert.Equal(DummyTracker.PROCESSOR_TIME, ProcessorTime);
            }
        }
    }
}
