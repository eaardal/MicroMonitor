using MediatR;

namespace MicroMonitor.Infrastructure
{
    public abstract class Action : INotification
    {
        public new abstract string ToString();
    }
}
