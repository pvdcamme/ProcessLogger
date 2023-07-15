using FileLog;

namespace ProcessLogger.Test
{
    public class FileLogTests
    {

        [Fact]
        public void CreateFromDocuments()
        {
            try
            {
                FileLog.FileLog.InDocuments("test.txt");
            }
            catch (Exception)
            {
                Assert.Fail("Can't create");

            }
        }

    }
}
