

namespace ProcessLogger.Tests
{
    using FileLog;
    public class FileLogTests
    {
        [Fact]
        public void CreateFromDocuments()
        {
            // Can we actually instantiatie a Logger.
            try
            {
                var logger = FileLog.InDocuments("test.txt");
                logger.Reset();
            }
            catch (Exception)
            {
                Assert.Fail("Can't create");

            }
        }

        [Fact]
        public void AddSingleEntry()
        {
            // The most basic entry-test, just a single entry.
            var logger = FileLog.InDocuments("test.txt");
            logger.Reset();
            logger.AddEntry("test", 1f);

            var savedResults = logger.GetEntries();
            Assert.Single(savedResults);

            LogEntry soleResult = savedResults.Single();

            Assert.Equal("test", soleResult.Key);
            Assert.Equal(1f, soleResult.Value);
        }

        [Fact]
        public void AddMultipleEntries()
        {
            // Slightly more complicated test. Allows to also verify the timestamps.
            var logger = FileLog.InDocuments("test.txt");
            logger.Reset();
            float[] toAdd = { 1f, 23f, 45f, 0f, -12f, 100000f };
            const string keyName = "test";
            foreach (var entry in toAdd)
            {
                logger.AddEntry(keyName, entry);
            }

            int entryCount = 0;
            long previousMoment = long.MinValue;
            foreach (var entry in logger.GetEntries())
            {
                Assert.Equal(keyName, entry.Key);
                Assert.Equal(toAdd[entryCount], entry.Value);
                Assert.InRange(entry.When, previousMoment, long.MaxValue);

                previousMoment = entry.When;
                entryCount++;
            }
            Assert.Equal(toAdd.Length, entryCount);
        }

        [Fact]
        public void AddMultipleKeys()
        {
            // Using multiple keys, values. Including repetition.
            // Including some odd Floating point values.
            string[] keys = { "test", "jos", "test", "balloon", "test", "jos", "dfds", "ff" };
            float[] val = { -1f, 23f, 34f, 12.3f, 34f, float.NaN, float.MinValue, float.MaxValue };

            // Testing the test.
            Assert.Equal(keys.Length, val.Length);

            var logger = FileLog.InDocuments("test.txt");
            logger.Reset();

            for (int ctr = 0; ctr < keys.Length; ++ctr)
            {
                logger.AddEntry(keys[ctr], val[ctr]);
            }

            int savedCtr = 0;
            foreach (var entry in logger.GetEntries())
            {
                Assert.Equal(keys[savedCtr], entry.Key);
                Assert.Equal(val[savedCtr], entry.Value);

                savedCtr++;
            }
        }
    }
}
