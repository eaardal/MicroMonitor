using System.Windows;
using MediatR;

namespace MicroMonitor.Actions
{
    class ToggleHeaderPanelVisibility : INotification
    {
        public Visibility Visibility { get; }

        public ToggleHeaderPanelVisibility(Visibility visibility)
        {
            Visibility = visibility;
        }
    }

    class ToggleOverlayVisibility : INotification
    {
        public Visibility Visibility { get; }

        public ToggleOverlayVisibility(Visibility visibility)
        {
            Visibility = visibility;
        }
    }
}
