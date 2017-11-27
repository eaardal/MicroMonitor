using System.Windows;
using MediatR;

namespace MicroMonitor.Actions
{
    class ToggleHeaderPanelVisibility : IRequest
    {
        public Visibility Visibility { get; }

        public ToggleHeaderPanelVisibility(Visibility visibility)
        {
            Visibility = visibility;
        }
    }
}
