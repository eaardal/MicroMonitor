using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using CommonServiceLocator;
using MediatR;
using MicroMonitor.Helpers;
using MicroMonitor.Infrastructure;
using MicroMonitor.Model;
using MicroMonitor.Requests;
using MicroMonitor.Views.MainView;

namespace MicroMonitor
{
    class KeyboardActions
    {
        public static void TryToggleHeaderPanel(KeyEventArgs e, MainWindowModel model)
        {
            if (e.Key == Key.E)
            {
                model.HeaderPanelVisibility = Visibility.Collapsed;
            }
        }

        public static void TryOpenPeekWindowForLogEntryUnderMouseCursor(KeyEventArgs e, MainWindowModel model)
        {
            if (e.Key == Key.LeftShift)
            {
                var ele = Mouse.DirectlyOver as UIElement;

                var textBlock = ele as TextBlock;

                if (textBlock != null)
                {
                    var logEntry = textBlock.DataContext as MicroLogEntry;

                    if (logEntry == null)
                    {
                        Logger.Debug($"Could not cast TextBlock.DataContext to {typeof(MicroLogEntry).FullName}");
                        return;
                    }

                    PeekWindow.Open((Window)e.Source, logEntry);
                }
            }
        }

        public static void TryRefresh(KeyEventArgs e, MainWindowModel model)
        {
            if (e.Key == Key.R || e.Key == Key.R && KeyboardFacade.IsLeftCtrlDown())
            {
                var mediator = ServiceLocator.Current.GetInstance<IMediator>();
                mediator.Send(new RefreshLogEvents());
            }
        }
    }
}
