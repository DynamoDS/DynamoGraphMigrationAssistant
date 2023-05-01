using DynamoGraphMigrationAssistant.ViewModels;
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

namespace DynamoGraphMigrationAssistant.Views.Steps
{
    /// <summary>
    /// Interaction logic for DynamoMigrationAssistantStep4.xaml
    /// </summary>
    public partial class DynamoGraphMigrationAssistantStep4 : Page
    {
        public DynamoGraphMigrationAssistantStep4(DynamoGraphMigrationAssistantViewModel vm)
        {
            InitializeComponent();
            DataContext = vm;
        }
    }
}
