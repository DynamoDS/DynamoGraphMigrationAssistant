using System;
using Dynamo.Controls;
using Dynamo.Logging;
using Dynamo.Wpf.Views.PackageManager;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using DynamoGraphMigrationAssistant.ViewModels;
using static Dynamo.ViewModels.SearchViewModel;
using System.Text.RegularExpressions;

namespace DynamoGraphMigrationAssistant.Views
{
    /// <summary>
    /// Interaction logic for MigrationSettingsView.xaml
    /// </summary>
    public partial class MigrationSettingsView : Window
    {
        public MigrationSettingsView(MigrationSettingsViewModel migrationSettingsViewModel)
        {
            DataContext = migrationSettingsViewModel;

            InitializeComponent();
        }

        /// <summary>
        /// handler for preferences dialog dragging action. When the TitleBar is clicked this method will be executed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MigrationSettingsView_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //Drag functionality when the TitleBar is clicked with the left button and dragged to another place
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
                Dynamo.Logging.Analytics.TrackEvent(
                    Actions.Move,
                    Categories.Preferences);
            }
        }
        /// <summary>
        /// Dialog close button handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            var migrationSettingsViewModel = DataContext as MigrationSettingsViewModel;
            MigrationSettings.SerializeModels(migrationSettingsViewModel.MigrationSettings);
            Close();
        }
        // Number input textbox validation
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            if (e.Text.Equals("."))
            {
                TextBox textBox = sender as TextBox;
                e.Handled = textBox.Text.Contains(".");
                return;
            }

            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
        // Number input start character validation
        private void StartNumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
