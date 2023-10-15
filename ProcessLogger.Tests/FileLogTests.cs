

namespace ProcessLogger.Tests
{
    using FileLog;    

    public class FileLogTests
    {
        // Basic instantiation of a Logger.
        [Fact]
        public void CreateFromDocuments()
        {
            try
            {
                var logger = FileLog.InUserDocuments("test.txt");
                logger.Reset();
            }
            catch (Exception)
            {
                Assert.Fail("Can not create logger");
            }
        }


        // The most basic entry-test, just a single entry.
        [Fact]
        public void AddSingleEntry()
        {
            var logger = FileLog.InUserDocuments("test.txt");
            logger.Reset();
            logger.AddEntry("test", 1f);

            var savedResults = logger.GetEntries();
            Assert.Single(savedResults);

            LogEntry soleResult = savedResults.Single();

            Assert.Equal("test", soleResult.Key);
            Assert.Equal(1f, soleResult.Value);
            Assert.Contains(soleResult.Key, logger.GetKeys());
            Assert.Single(logger.GetEntries());
            Assert.Single(logger.GetKeys());
        }

        [Fact]
        public void EmptyLogger()
        {
            var logger = FileLog.InUserDocuments("test.txt");
            logger.Reset();
            Assert.Empty(logger.GetEntries());
        }

        // Can the logger the reinstantied over-and-over?
        [Fact]
        public void MultipleCreations()
        {
            try
            {
                string sameFileName = "test.txt";
                // 10x should be enough to check cleanup properly happens.
                for (int ctr = 0; ctr < 10; ctr++)
                {
                    Console.WriteLine($"Beginning with ${ctr}");
                    var logger = FileLog.InUserDocuments(sameFileName);
                    logger.AddEntry("something", 1);
                    logger.Reset();
                }
                var lastLogger = FileLog.InUserDocuments(sameFileName);
                // Also empty when not yet resetted?
                Assert.Empty(lastLogger.GetEntries());
                lastLogger.Reset();
            }
            catch (Exception ex)
            {
                Assert.Fail("Should not fail:" + ex);
            }
        }

        // Slightly more complicated test. Allows to also verify the timestamps.
        [Fact]
        public void AddMultipleEntriesOfSameKey()
        {
            var logger = FileLog.InUserDocuments("test.txt");
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
                Assert.True(entryCount < toAdd.Length);
                Assert.Equal(keyName, entry.Key);
                Assert.Equal(toAdd[entryCount], entry.Value);
                Assert.InRange(entry.When, previousMoment, long.MaxValue);

                previousMoment = entry.When;
                entryCount++;
            }
            // Seen them all.
            Assert.Equal(toAdd.Length, entryCount);
        }

        // Do the odd floating point values also work?
        [Fact]
        public void AddOddFloatVals()
        {
            var logger = FileLog.InUserDocuments("test.txt");
            logger.Reset();

            string anyKey = "strange";
            float[] oddValues = { float.NaN, float.NegativeInfinity, float.PositiveInfinity, float.MinValue, float.MaxValue };
            foreach (float odd in oddValues)
            {
                logger.AddEntry(anyKey, odd);
            }

            foreach (var (origVal, savedVal) in oddValues.Zip(logger.GetEntries()))
            {
                Assert.Equal(origVal, savedVal.Value);
            }
        }

        // Using multiple keys, values. Including repetition.
        [Fact]
        public void AddMultipleKeys()
        {
            string[] keys = { "test", "jos", "test", "balloon", "test", "jos", "dfds", "ff" };
            float[] val = { -1f, 23f, 34f, 12.3f, 34f, float.NaN, float.MinValue, float.MaxValue };

            // Testing the test.
            Assert.Equal(keys.Length, val.Length);

            var logger = FileLog.InUserDocuments("test.txt");
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
            Assert.Equal(new HashSet<string>(keys), new HashSet<string>(logger.GetKeys()));
        }
    }
    public class ReverseFileTests
    {
        [Fact]
        public void CheckConstuctorAndcleanup()
        {
            string tmpFileName = Path.GetTempFileName();
            {
                using ReverseFileReader aReader = new(tmpFileName);
            }

            File.Delete(tmpFileName);
        }

        [Fact]
        public void CheckSingleLine()
        {
            const string testTxt = "test";
            string tmpFileName = Path.GetTempFileName();
            {
                using StreamWriter fillUp = new(tmpFileName);
                fillUp.WriteLine(testTxt);
            }
            {
                using ReverseFileReader aReader = new(tmpFileName);
                string lastLine = aReader.ReadLine(); ;
                Assert.Equal(testTxt, lastLine);
            }
            File.Delete(tmpFileName);
        }

        [Fact]
        public void CheckMultipleLines()
        {
            string[] testLines = { "Test", "another test", "more testing", "peninultimute line", "and a last line" };
            string tmpFileName = Path.GetTempFileName();
            {
                using StreamWriter fillUp = new(tmpFileName);
                foreach (var line in testLines)
                {
                    fillUp.WriteLine(line);
                }
            }
            {
                using ReverseFileReader aReader = new(tmpFileName);
                foreach (var line in testLines.Reverse())
                {
                    var readLine = aReader.ReadLine();
                    Assert.Equal(line, readLine);
                }
            }
            File.Delete(tmpFileName);
        }
        [Fact]
        public void CheckManyLines()
        {
            const int expectedLines = 1_000_000;
            string tmpFileName = Path.GetTempFileName();
            {
                using StreamWriter fillUp = new(tmpFileName);
                for (int ctr = 0; ctr < expectedLines; ++ctr)
                {
                    fillUp.WriteLine($"ctr is {ctr}");
                }
            }
            {
                using ReverseFileReader aReader = new(tmpFileName);
                string res;
                int actuallyRead = 0;
                while((res = aReader.ReadLine()).Length > 0){
                    Console.WriteLine(res);
                    actuallyRead++;
                }
                Assert.Equal(expectedLines, actuallyRead);

            }
            File.Delete(tmpFileName);
        }
    }
}
