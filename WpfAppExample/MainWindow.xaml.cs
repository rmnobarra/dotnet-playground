using System.Windows;

namespace WpfAppExample
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            label1.Content = "Olá, Mundo!";
        }
    }
}
