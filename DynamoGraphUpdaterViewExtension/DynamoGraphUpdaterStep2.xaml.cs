using System.Windows.Controls;

namespace DynamoGraphUpdater
{
    /// <summary>
    /// Interaction logic for DynamoGraphUpdaterStep1.xaml
    /// </summary>
    public partial class DynamoGraphUpdaterStep2 : Page
    {
        public DynamoGraphUpdaterStep2(DynamoGraphUpdaterViewModel vm)
        {
            InitializeComponent();

            DataContext = vm;
        }

        private void Selector_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!IsLoaded) return;
            var viewModel = DataContext as DynamoGraphUpdaterViewModel;

            foreach (var graph in viewModel.UpdateableGraphs)
            {
                graph.TargetVersion = this.TargetVersionComboBox.SelectedItem as TargetDynamoVersion;
            }
        }
    }
}
