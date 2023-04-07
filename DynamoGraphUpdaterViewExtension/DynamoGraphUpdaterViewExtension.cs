using Dynamo.Wpf.Extensions;
using System;
using System.Windows;
using System.Windows.Controls;

namespace DynamoGraphUpdater
{
    public class DynamoGraphUpdaterViewExtension : ViewExtensionBase, IViewExtension
    {
        /// <summary>
        /// The name of the extension.
        /// </summary>
        public override string Name => Properties.Resources.ExtensionName;
        /// <summary>
        /// The GUID of the extension
        /// </summary>
        public override string UniqueId => "57B476EF-844D-4719-A6A6-B649CB998868";

        public MenuItem DynamoGraphUpdaterMenuItem;
        private ViewLoadedParams _viewLoadedParamsReference;

        internal DynamoGraphUpdaterView View;
        internal DynamoGraphUpdaterViewModel ViewModel;


        public override void Dispose()
        {
            // Cleanup
            ViewModel?.Dispose();
            View = null;
            ViewModel = null;
        }
        public override void Loaded(ViewLoadedParams viewLoadedParams)
        {
            if (viewLoadedParams == null) throw new ArgumentNullException(nameof(viewLoadedParams));

            _viewLoadedParamsReference = viewLoadedParams;

            // Add a button to Dynamo View menu to manually show the window
            DynamoGraphUpdaterMenuItem = new MenuItem { Header = Properties.Resources.HeaderText };

            DynamoGraphUpdaterMenuItem.Click += DynamoGraphUpdaterMenuItemOnClick;

            _viewLoadedParamsReference.AddExtensionMenuItem(DynamoGraphUpdaterMenuItem);

            InitializeViewExtension();
        }

        private void DynamoGraphUpdaterMenuItemOnClick(object sender, RoutedEventArgs e)
        {
            View.Owner = _viewLoadedParamsReference.DynamoWindow;

            View.ShowDialog();
        }

        public void Shutdown()
        {
            Dispose();
        }
        private void InitializeViewExtension()
        {
            ViewModel = new DynamoGraphUpdaterViewModel(_viewLoadedParamsReference);
            View = new DynamoGraphUpdaterView(ViewModel);
        }
       
        public override void Closed()
        {
            if (DynamoGraphUpdaterMenuItem != null) DynamoGraphUpdaterMenuItem.IsChecked = false;
        }
    }
}
