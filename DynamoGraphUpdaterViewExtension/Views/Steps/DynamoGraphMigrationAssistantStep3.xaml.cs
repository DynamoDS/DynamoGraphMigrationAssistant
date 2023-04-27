using System.Windows;
using System.Windows.Controls;
using DynamoGraphMigrationAssistant.ViewModels;

namespace DynamoGraphMigrationAssistant.Views.Steps
{
    /// <summary>
    /// Interaction logic for DynamoMigrationAssistantStep1.xaml
    /// </summary>
    public partial class DynamoGraphMigrationAssistantStep3 : Page
    {
        public DynamoGraphMigrationAssistantStep3(DynamoGraphMigrationAssistantViewModel vm)
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
