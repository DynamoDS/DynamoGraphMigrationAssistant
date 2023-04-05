using System.Windows;
using System.Windows.Input;

namespace DynamoGraphUpdater
{
    /// <summary>
    /// Interaction logic for DynamoGraphUpdaterView.xaml
    /// </summary>
    public partial class DynamoGraphUpdaterView : Window
    {
        public DynamoGraphUpdaterView(DynamoGraphUpdaterViewModel vm)
        {
            InitializeComponent();

            DataContext = vm;
        }
        private void CloseButton_OnClick(object sender, RoutedEventArgs e)
        {
            Hide();
        }
        /// <summary>
        /// Lets the user drag this window around with their left mouse button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UIElement_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton != MouseButton.Left) return;
            DragMove();
        }
    }
}
