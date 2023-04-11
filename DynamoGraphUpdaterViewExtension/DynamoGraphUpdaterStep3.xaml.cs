using System.Windows;
using System.Windows.Controls;

namespace DynamoGraphUpdater
{
    /// <summary>
    /// Interaction logic for DynamoGraphUpdaterStep1.xaml
    /// </summary>
    public partial class DynamoGraphUpdaterStep3 : Page
    {
        public DynamoGraphUpdaterStep3(DynamoGraphUpdaterViewModel vm)
        {
            InitializeComponent();
            DataContext = vm;
            this.Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            this.SaveInNewFolderRadioButton.IsChecked = true;
        }
    }
}
