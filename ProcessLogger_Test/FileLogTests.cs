

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
                FileLog.InDocuments("test.txt");
            }
            catch (Exception)
            {
                Assert.Fail("Can't create");

            }
        }
    }
}
