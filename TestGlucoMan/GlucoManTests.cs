using gamon;
using NUnit.Framework;

namespace TestGlucoMan
{
    [TestFixture]
    public class GeneralTests
    {
        [SetUp]
        public void Setup()
        {
            // general setup, for all tests
            General.LogOfProgram = new Logger("XXXXXXXXXXXXX", true,
                    @"GlucoMan_Log.txt",
                    @"GlucoMan_Errors.txt",
                    @"GlucoMan_Debug.txt",
                    @"GlucoMan_Prompts.txt",
                    @"GlucoMan_Data.txt");
        }
        [Test]
        public void SampleTest_ShouldPass()
        {
            // Sample test that should always pass
            Assert.That(true, Is.True);
        }
    }
}