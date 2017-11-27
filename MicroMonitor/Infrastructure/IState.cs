namespace MicroMonitor.Infrastructure
{
    internal interface IState<out T>
    {
        T State { get; }
    }
}