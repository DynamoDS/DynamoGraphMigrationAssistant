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
    }
}
