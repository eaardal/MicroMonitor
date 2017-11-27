using System.Windows.Input;

namespace MicroMonitor.Helpers
{
    class KeyboardFacade
    {
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
    }
}
