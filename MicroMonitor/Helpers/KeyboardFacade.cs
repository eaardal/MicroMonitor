using System.Windows.Input;

namespace MicroMonitor.Helpers
{
    class KeyboardFacade
    {
        public static bool IsLeftShiftDown()
        {
            var shiftState = Keyboard.GetKeyStates(Key.LeftShift);
            return (shiftState & KeyStates.Down) > 0;
        }
    }
}
