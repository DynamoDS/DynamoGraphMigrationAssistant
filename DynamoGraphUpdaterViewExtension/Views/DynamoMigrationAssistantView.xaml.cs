using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using DynamoGraphMigrationAssistant.ViewModels;
using DynamoGraphMigrationAssistant.Views.Steps;

namespace DynamoGraphMigrationAssistant.Views
{
    /// <summary>
    /// Interaction logic for DynamoGraphUpdaterView.xaml
    /// </summary>
    public partial class DynamoMigrationAssistantView : Window
    {
        private List<Page> StepsPages = new List<Page>();
        DynamoGraphMigrationAssistantViewModel viewModel;

        public DynamoMigrationAssistantView(DynamoGraphMigrationAssistantViewModel vm)
        {
            InitializeComponent();

            viewModel = vm;
            DataContext = vm;

            StepsPages.Add(new DynamoGraphMigrationAssistantStep1(vm));
            StepsPages.Add(new DynamoGraphMigrationAssistantStep2(vm));
            StepsPages.Add(new DynamoGraphMigrationAssistantStep3(vm));

            NavigationFrame.Navigate(new DynamoGraphMigrationAssistantStep1(vm));

            this.IsVisibleChanged += OnIsVisibleChanged;
        }

        private void OnIsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            viewModel.CurrentPageIndex = 0;
            NavigationFrame.Navigate(StepsPages[0]);
        }

        
        private void CloseButton_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
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

        private void NextButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (viewModel.CurrentPageIndex == (StepsPages.Count-1)) return;

            viewModel.CurrentPageIndex++;

            this.NavigationFrame.Navigate(StepsPages[viewModel.CurrentPageIndex]);
            SetButtonText();
        }

        private void BackButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (viewModel.CurrentPageIndex == 0) return;

            viewModel.CurrentPageIndex--;

            this.NavigationFrame.Navigate(StepsPages[viewModel.CurrentPageIndex]);
            SetButtonText();
        }

        private void SetButtonText()
        {
            //set the button text
            switch (viewModel.CurrentPageIndex)
            {
                case 0:
                    this.NextButton.Content = "Next";
                    this.BackButton.Visibility = Visibility.Hidden;
                    break;
                case 1:
                    this.NextButton.Content = "Next";
                    this.BackButton.Visibility = Visibility.Visible;
                    break;
                case 2:
                    this.NextButton.Content = "Upgrade";
                    this.BackButton.Visibility = Visibility.Visible;
                    break;
                case 3:
                    this.NextButton.Content = "Finished";
                    this.BackButton.Visibility = Visibility.Visible;
                    break;
            }
        }
    }
}
