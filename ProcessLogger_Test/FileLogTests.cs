

namespace ProcessLogger.Test
{
    using FileLog;
    public class FileLogTests
    {


        [Fact]
        public void CreateFromDocuments()
        {
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
            var logger = FileLog.InDocuments("test.txt");
            logger.Reset();
            logger.AddEntry("test", 1f);

            var savedResults = logger.GetEntries();
            Assert.Single(savedResults);

            LogEntry soleResult = savedResults.Single();

            Assert.Equal("test", soleResult.Key);
            Assert.Equal(1f, soleResult.Value);
        }
    }
}
