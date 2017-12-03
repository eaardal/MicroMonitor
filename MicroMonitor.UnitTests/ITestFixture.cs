namespace MicroMonitor.UnitTests
{
    internal interface ITestFixture<out T>
    {
        T CreateSut();
    }
}