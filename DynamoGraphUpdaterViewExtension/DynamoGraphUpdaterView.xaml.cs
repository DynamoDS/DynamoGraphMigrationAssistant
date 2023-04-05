using System.Windows.Controls;

namespace DynamoGraphUpdaterViewExtension
{
    /// <summary>
    /// Interaction logic for DynamoGraphUpdaterView.xaml
    /// </summary>
    public partial class DynamoGraphUpdaterView : UserControl
    {
        public DynamoGraphUpdaterView(DynamoGraphUpdaterViewModel vm)
        {
            InitializeComponent();

            DataContext = vm;
        }
    }
}
