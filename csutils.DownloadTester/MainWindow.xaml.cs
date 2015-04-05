using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace csutils.DownloadTester
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            MainViewModel mvm = new MainViewModel();
            DataContext = mvm;
        }

        public MainViewModel ViewModel
        {
            get
            {
                return (MainViewModel)DataContext;
            }
        }

        private void reset_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void pause_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.DownloadManager.Pause();
        }
        private void start_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.DownloadManager.StartAsync();
        }
    }
}
