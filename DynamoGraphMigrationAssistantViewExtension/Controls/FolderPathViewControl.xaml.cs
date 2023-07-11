using System;
using System.Diagnostics;
using System.Windows.Controls;
using System.Windows.Input;

namespace DynamoGraphMigrationAssistant.Controls
{
    /// <summary>
    /// Interaction logic for PathViewControl.xaml
    /// </summary>
    public partial class FolderPathViewControl : UserControl
    {
        public FolderPathViewControl()
        {
            InitializeComponent();
        }
        private void PathTextBlock_OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var textBlock = (TextBlock)sender;
            if (textBlock == null || string.IsNullOrEmpty(textBlock.Text)) return;

            try
            {
                Process.Start(textBlock.Text);
            }
            catch (Exception)
            {
                // ignored
            }
        }

    }
}
