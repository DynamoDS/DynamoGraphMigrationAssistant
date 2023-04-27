using System.Windows;
using System.Windows.Controls;
using DynamoGraphMigrationAssistant.ViewModels;

namespace DynamoGraphMigrationAssistant.Views.Steps
{
    /// <summary>
    /// Interaction logic for DynamoMigrationAssistantStep1.xaml
    /// </summary>
    public partial class DynamoGraphMigrationAssistantStep1 : Page
    {
        public DynamoGraphMigrationAssistantStep1(DynamoGraphMigrationAssistantViewModel vm)
        {
            InitializeComponent();
            DataContext = vm;
            this.Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            DynamoGraphMigrationAssistantViewModel vm = DataContext as DynamoGraphMigrationAssistantViewModel;

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
