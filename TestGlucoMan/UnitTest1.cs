using NUnit.Framework;

namespace TestGlucoMan
{
    [TestFixture]
    public class GeneralTests
    {
        [SetUp]
        public void Setup()
        {
            // Setup generale per i test
        }

        [Test]
        public void SampleTest_ShouldPass()
        {
            // Test di esempio che dovrebbe sempre passare
            Assert.That(true, Is.True);
        }
    }
}