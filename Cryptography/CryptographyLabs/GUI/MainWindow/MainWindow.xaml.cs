using System.Windows;
using Autofac;
using CryptographyLabs.GUI;

namespace CryptographyLabs
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var lifetimeScope = AppContainer.Build().BeginLifetimeScope(nameof(MainWindowVM));
            DataContext = lifetimeScope.Resolve<MainWindowVM>();
        }
    }
}