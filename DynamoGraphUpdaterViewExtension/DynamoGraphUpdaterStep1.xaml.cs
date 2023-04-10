using System.Windows;
using System.Windows.Controls;

namespace DynamoGraphUpdater
{
    /// <summary>
    /// Interaction logic for DynamoGraphUpdaterStep1.xaml
    /// </summary>
    public partial class DynamoGraphUpdaterStep1 : Page
    {
        public DynamoGraphUpdaterStep1(DynamoGraphUpdaterViewModel vm)
        {
            InitializeComponent();
            DataContext = vm;
            this.Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            DynamoGraphUpdaterViewModel vm = DataContext as DynamoGraphUpdaterViewModel;

            if (vm.SingleGraphMode)
            {
                this.CurrentFileRadioButton.IsChecked = true;
            }
            else
            {
                this.FromFolderRadioButton.IsChecked = true;
            }
        }
    }
}
