

namespace ProcessLogger.Test
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
            foreach(var entry in toAdd)
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
    }

}
