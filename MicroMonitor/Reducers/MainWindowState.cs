using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using MicroMonitor.Infrastructure;
using MicroMonitor.Model;

namespace MicroMonitor.Reducers
{
    public class MainWindowState : ObservableObject
    {
        private Visibility _headerPanelVisibility = Visibility.Collapsed;
        private Visibility _overlayVisibility = Visibility.Collapsed;
        private int _windowWidth;
        private int _windowHeight;
        private double _windowLeft;
        private double _windowTop;
        private string _nextReadText;
        private string _lastReadText;
        private bool _isCloseAllDetailWindowsButtonEnabled;
        private Window _window;
        private int _traversingIndex;
        private bool _isActivatedOnce;
        private Border _currentMouseOverBorder;

        public Border CurrentMouseOverBorder
        {
            get => _currentMouseOverBorder;
            set => SetProperty(ref _currentMouseOverBorder, value);
        }

        public Visibility HeaderPanelVisibility
        {
            get => _headerPanelVisibility;
            set => SetProperty(ref _headerPanelVisibility, value);
        }

        public Visibility OverlayVisibility
        {
            get => _overlayVisibility;
            set => SetProperty(ref _overlayVisibility, value);
        }

        public int WindowWidth
        {
            get => _windowWidth;
            set => SetProperty(ref _windowWidth, value);
        }

        public int WindowHeight
        {
            get => _windowHeight;
            set => SetProperty(ref _windowHeight, value);
        }

        public double WindowLeft
        {
            get => _windowLeft;
            set => SetProperty(ref _windowLeft, value);
        }

        public double WindowTop
        {
            get => _windowTop;
            set => SetProperty(ref _windowTop, value);
        }

        public string NextReadText
        {
            get => _nextReadText;
            set => SetProperty(ref _nextReadText, value);
        }

        public string LastReadText
        {
            get => _lastReadText;
            set => SetProperty(ref _lastReadText, value);
        }

        public ObservableCollection<GroupedMicroLogEntry> GroupedLogEntries { get; } = new ObservableCollection<GroupedMicroLogEntry>();

        public bool IsCloseAllDetailWindowsButtonEnabled
        {
            get => _isCloseAllDetailWindowsButtonEnabled;
            set => SetProperty(ref _isCloseAllDetailWindowsButtonEnabled, value);
        }

        public Window Window
        {
            get => _window;
            set => SetProperty(ref _window, value);
        }

        public ObservableCollection<MicroLogEntry> LogEntries { get; } = new ObservableCollection<MicroLogEntry>();

        public int TraversingIndex
        {
            get => _traversingIndex;
            set => SetProperty(ref _traversingIndex, value);
        }

        public bool IsActivatedOnce
        {
            get => _isActivatedOnce;
            set => SetProperty(ref _isActivatedOnce, value);
        }
    }
}
