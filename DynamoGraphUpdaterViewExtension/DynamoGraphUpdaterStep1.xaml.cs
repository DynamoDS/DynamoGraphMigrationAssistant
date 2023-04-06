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
        }
    }
}
