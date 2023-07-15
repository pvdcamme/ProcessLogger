

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
        public void AddEntries()
        {
            var logger = FileLog.InDocuments("test.txt");
            logger.Reset();
            logger.AddEntry("test", 1f);
            Assert.NotEmpty(logger.GetEntries());
        }
    }
}
