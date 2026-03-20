using gamon;
using NUnit.Framework;

// Placing [SetUpFixture] outside any namespace makes it assembly-level:
// NUnit runs OneTimeSetUp once before any test in the entire assembly.
[SetUpFixture]
public class TestAssemblySetup
{
    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        General.LogOfProgram ??= new Logger();
    }
}
