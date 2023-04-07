using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using NUnit.Framework;

namespace DynamoGraphUpdater
{
    /// <summary>
    /// Interaction logic for DynamoGraphUpdaterView.xaml
    /// </summary>
    public partial class DynamoGraphUpdaterView : Window
    {
        private List<Page> StepsPages = new List<Page>();
        DynamoGraphUpdaterViewModel viewModel;

        public DynamoGraphUpdaterView(DynamoGraphUpdaterViewModel vm)
        {
            InitializeComponent();

            viewModel = vm;
            DataContext = vm;

            StepsPages.Add(new DynamoGraphUpdaterStep1(vm));
            StepsPages.Add(new DynamoGraphUpdaterStep2(vm));

            NavigationFrame.Navigate(new DynamoGraphUpdaterStep1(vm));

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
        }

        private void BackButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (viewModel.CurrentPageIndex == 0) return;

            viewModel.CurrentPageIndex--;

            this.NavigationFrame.Navigate(StepsPages[viewModel.CurrentPageIndex]);
        }
    }
}
