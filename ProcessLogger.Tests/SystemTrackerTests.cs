namespace ProcessLogger.Tests
{
    public class SystemTrackerTests
    {
        // The most simple implementation of a ProcessTracker.
        // Used for mocking
        class DummyTracker : IProcessTracker
        {
            // For later testing, a dummy always returns the same time
            public static readonly float PROCESSOR_TIME = 34f;
            public bool IsFailing { get; set; }
            public string Name { get; }
            public DummyTracker(string name)
            {
                this.Name = name;

                // Initially always good.
                this.IsFailing = false;
            }

            float IProcessTracker.GetProcessorTime()
            {
                return PROCESSOR_TIME;
            }

            bool IProcessTracker.IsFailing()
            {
                return IsFailing;
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

        [Fact]
        public void CheckFailureRemoval()
        {
            string[] initialProcesses = { "aaa", "asdf", "anotherTest" };
            Dictionary<string, DummyTracker> dummies = new();

            // Use side-effects to track the created mocks.
            DummyTracker savedDummies(string name)
            {
                DummyTracker aDummy = new(name);
                dummies.Add(name, aDummy);
                return aDummy;
            }

            SystemTracker tracker = new((Func<string, DummyTracker>)savedDummies);
            tracker.MergeUnknownProcesses(initialProcesses);
            
            int expectFails = 0;
            Random r = new();
            foreach(var dummy in dummies.Values){
                dummy.IsFailing = r.NextSingle() < 0.5;
                if (dummy.IsFailing)
                {
                    expectFails++;
                }
            }
            // A fails is reported once more.
            Assert.Equal(initialProcesses.Length, tracker.GetTrackedProcesses().Count());

            // Failed should now gone
            Assert.Equal(initialProcesses.Length - expectFails, tracker.GetTrackedProcesses().Count());
            foreach (var (name, _) in tracker.GetTrackedProcesses())
            {
                var matchingMock = dummies.GetValueOrDefault(name);
                Assert.True(matchingMock != null);
                Assert.False(matchingMock.IsFailing);
            }
        }
        [Fact]
        public void checkFullProcessnames()
        {
            Assert.True(SystemTracker.ProcessNames().Count() > 5);
        }
    }
}
