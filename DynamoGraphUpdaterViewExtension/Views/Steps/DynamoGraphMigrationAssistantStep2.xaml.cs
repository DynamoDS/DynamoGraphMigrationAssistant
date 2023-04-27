using System.Windows.Controls;
using DynamoGraphMigrationAssistant.ViewModels;

namespace DynamoGraphMigrationAssistant.Views.Steps
{
    /// <summary>
    /// Interaction logic for DynamoMigrationAssistantStep1.xaml
    /// </summary>
    public partial class DynamoGraphMigrationAssistantStep2 : Page
    {
        public DynamoGraphMigrationAssistantStep2(DynamoGraphMigrationAssistantViewModel vm)
        {
            InitializeComponent();

            DataContext = vm;
        }

        private void Selector_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!IsLoaded) return;
            var viewModel = DataContext as DynamoGraphMigrationAssistantViewModel;

            foreach (var graph in viewModel.UpdateableGraphs)
            {
                graph.TargetVersion = this.TargetVersionComboBox.SelectedItem as TargetDynamoVersion;
            }
        }
    }
}
