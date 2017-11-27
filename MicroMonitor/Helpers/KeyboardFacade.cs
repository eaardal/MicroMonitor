using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using CommonServiceLocator;
using MediatR;
using MicroMonitor.Actions;
using MicroMonitor.Infrastructure;
using MicroMonitor.Model;
using MicroMonitor.Views.MainView;

namespace MicroMonitor.Helpers
{
    class KeyboardFacade
    {
        private readonly IMediator _mediator;

        public KeyboardFacade(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public static bool IsLeftShiftDown()
        {
            var state = Keyboard.GetKeyStates(Key.LeftShift);
            return (state & KeyStates.Down) > 0;
        }

        public static bool IsLeftCtrlDown()
        {
            var state = Keyboard.GetKeyStates(Key.LeftCtrl);
            return (state & KeyStates.Down) > 0;
        }

        public static bool IsLeftAltDown()
        {
            var state = Keyboard.GetKeyStates(Key.LeftAlt);
            return (state & KeyStates.Down) > 0;
        }
        
        //public void TryOpenPeekWindowForLogEntryUnderMouseCursor(KeyEventArgs e, MainWindowModel model)
        //{
        //    if (e.Key == Key.LeftShift)
        //    {
        //        var ele = Mouse.DirectlyOver as UIElement;

        //        var textBlock = ele as TextBlock;

        //        if (textBlock != null)
        //        {
        //            var logEntry = textBlock.DataContext as MicroLogEntry;

        //            if (logEntry == null)
        //            {
        //                Logger.Debug($"Could not cast TextBlock.DataContext to {typeof(MicroLogEntry).FullName}");
        //                return;
        //            }

        //            PeekWindow.Open((Window)e.Source, logEntry);
        //        }
        //    }
        //}

        public void TryRefresh(KeyEventArgs e, MainWindowModel model)
        {
            if (e.Key == Key.R || e.Key == Key.R && KeyboardFacade.IsLeftCtrlDown())
            {
                var mediator = ServiceLocator.Current.GetInstance<IMediator>();
                mediator.Send(new RefreshEventLogEntries());
            }
        }

        public void TryOpenPeekWindowForNumericKey(KeyEventArgs e, MainWindowModel model)
        {
            if (e.Key >= Key.D1 && e.Key <= Key.D9)
            {
                var key = e.Key.ToString().Last();
                var keyNum = int.Parse(key.ToString());

                if (keyNum > _logEntries.Count())
                {
                    return;
                }

                var logEntry = _logEntries.ElementAt(keyNum - 1);

                if (KeyboardFacade.IsLeftCtrlDown())
                {
                    OpenNewDetailsWindow(logEntry, true);
                }
                else
                {
                    PeekWindow.Open(_window, logEntry, KeyboardFacade.IsLeftShiftDown());
                }
            }
        }
    }
}
