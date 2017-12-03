namespace MicroMonitor.UnitTests
{
    internal interface ITestDataBuilder<out T>
    {
        T Build();
    }
}