using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace AvalonDockCollectionSynchronization
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private object _lockObj = new object();

        public ObservableCollection<string> WorkTimes { get; }

        public MainWindow()
        {
            WorkTimes = new ObservableCollection<string>();

            // Enable synchronization so we do have to worry about background thread access
            BindingOperations.EnableCollectionSynchronization(WorkTimes, _lockObj);

            DataContext = this;
            InitializeComponent();
        }

        private void DoWork_Click(object sender, RoutedEventArgs e)
        {
            Task.Run(async () =>
            {
                // Simulate some CPU/IO bound work
                await Task.Delay(500).ConfigureAwait(false);

                // Add our work to the collection
                // We're NOT on the UI thread here but that shouldn't matter due to setting up the EnableCollectionSynchronization
                // This line is NOT compatible with AvalonDock
                WorkTimes.Add($"{DateTime.Now}");

                // This line IS compatible with AvalonDock but isn't compatible with unit testing 
                // Application.Current.Dispatcher.BeginInvoke(() => WorkTimes.Add($"{DateTime.Now}"));
            });
        }
    }
}
