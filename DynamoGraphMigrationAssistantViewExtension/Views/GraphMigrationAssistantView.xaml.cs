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
using DynamoGraphMigrationAssistant.ViewModels;

namespace DynamoGraphMigrationAssistant.Views
{
    /// <summary>
    /// Interaction logic for GraphMigrationAssistantView.xaml
    /// </summary>
    public partial class GraphMigrationAssistantView : UserControl
    {
        public GraphMigrationAssistantView(GraphMigrationAssistantViewModel vm)
        {
            InitializeComponent();
            DataContext = vm;
        }

        private void Selector_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(!IsLoaded) return;

            ComboBox versionCombobox = sender as ComboBox;

            GraphMigrationAssistantViewModel vm = versionCombobox.DataContext as GraphMigrationAssistantViewModel;

            vm.TargetDynamoVersion = versionCombobox.SelectedItem as TargetDynamoVersion;
            

            vm.FixNodeSpacing = vm.TargetDynamoVersion.NodeSpacingSuggested;
            vm.ReplaceIfNodes = vm.TargetDynamoVersion.IfNodeSuggested;

            if (vm.Graphs is null) return;
            
            var graphList = vm.Graphs.ToList();

            //vm.Graphs.Clear();

            foreach (var graphViewModel in graphList)
            {
                Version original = Version.Parse(graphViewModel.Version);

                Version target = Version.Parse(vm.TargetDynamoVersion.Version);

                if (original >= target)
                {
                    graphViewModel.InTargetVersion = true;
                }
                else
                {
                    graphViewModel.InTargetVersion = false;

                }
            }
            vm.TargetVersionChanged();
        }
    }
}
