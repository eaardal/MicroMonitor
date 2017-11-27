using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using CommonServiceLocator;
using MediatR;
using MicroMonitor.Actions;
using MicroMonitor.Helpers;
using MicroMonitor.Infrastructure;
using MicroMonitor.Model;
using MicroMonitor.Views.MainView;

namespace MicroMonitor
{
    //class KeyboardActions
    //{
    //    public static void TryToggleHeaderPanel(KeyEventArgs e, MainWindowModel model)
    //    {
    //        if (e.Key == Key.E)
    //        {
    //            var mediator = ServiceLocator.Current.GetInstance<IMediator>();
    //            mediator.Send(new ToggleHeaderPanelVisibility(Visibility.Collapsed));
    //        }
    //    }

    //    public static void TryOpenPeekWindowForLogEntryUnderMouseCursor(KeyEventArgs e, MainWindowModel model)
    //    {
    //        if (e.Key == Key.LeftShift)
    //        {
    //            var ele = Mouse.DirectlyOver as UIElement;

    //            var textBlock = ele as TextBlock;

    //            if (textBlock != null)
    //            {
    //                var logEntry = textBlock.DataContext as MicroLogEntry;

    //                if (logEntry == null)
    //                {
    //                    Logger.Debug($"Could not cast TextBlock.DataContext to {typeof(MicroLogEntry).FullName}");
    //                    return;
    //                }

    //                PeekWindow.Open((Window)e.Source, logEntry);
    //            }
    //        }
    //    }

    //    public static void TryRefresh(KeyEventArgs e, MainWindowModel model)
    //    {
    //        if (e.Key == Key.R || e.Key == Key.R && KeyboardFacade.IsLeftCtrlDown())
    //        {
    //            var mediator = ServiceLocator.Current.GetInstance<IMediator>();
    //            mediator.Send(new RefreshEventLogEntries());
    //        }
    //    }

    //    public static void TryOpenPeekWindowForNumericKey(KeyEventArgs e, MainWindowModel model)
    //    {
    //        if (e.Key >= Key.D1 && e.Key <= Key.D9)
    //        {
    //            var key = e.Key.ToString().Last();
    //            var keyNum = int.Parse(key.ToString());

    //            if (keyNum > _logEntries.Count())
    //            {
    //                return;
    //            }

    //            var logEntry = _logEntries.ElementAt(keyNum - 1);

    //            if (KeyboardFacade.IsLeftCtrlDown())
    //            {
    //                OpenNewDetailsWindow(logEntry, true);
    //            }
    //            else
    //            {
    //                PeekWindow.Open(_window, logEntry, KeyboardFacade.IsLeftShiftDown());
    //            }
    //        }
    //    }
    //}
}
