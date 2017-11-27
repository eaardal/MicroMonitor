using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using MediatR;
using MicroMonitor.Infrastructure;
using MicroMonitor.Model;
using MicroMonitor.Reducers;
using MicroMonitor.Views.MainView;

namespace MicroMonitor.Actions
{
    class PeekWindowActionsHandler : IRequestHandler<OpenPeekWindowUnderMouseCursor>
    {
        private readonly IMediator _mediator;

        public PeekWindowActionsHandler(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public void Handle(OpenPeekWindowUnderMouseCursor message)
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

                _mediator.Send(new ShowPeekWindow(logEntry))

                PeekWindow.Open((Window)message.KeyEventArgs.Source, logEntry);
            }
        }
    }
}
