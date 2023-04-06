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
    }
}
