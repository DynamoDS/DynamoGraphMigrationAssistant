using Dynamo.Wpf.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using DynamoGraphMigrationAssistant.ViewModels;
using DynamoGraphMigrationAssistant.Views;
using Newtonsoft.Json;

namespace DynamoGraphMigrationAssistant
{
    public class DynamoGraphMigrationAssistantViewExtension : ViewExtensionBase, IViewExtension
    {
        /// <summary>
        /// The name of the extension.
        /// </summary>
        public override string Name => Properties.Resources.ExtensionName;
        /// <summary>
        /// The GUID of the extension
        /// </summary>
        public override string UniqueId => "57B476EF-844D-4719-A6A6-B649CB998868";

        public MenuItem DynamoMigrationAssistantMenuItem;
        private ViewLoadedParams _viewLoadedParamsReference;

        internal DynamoMigrationAssistantView View;
        internal DynamoGraphMigrationAssistantModel Model;
        internal DynamoGraphMigrationAssistantViewModel ViewModel;


        public override void Dispose()
        {
            // Cleanup
            //Model?.Dispose();
            ViewModel?.Dispose();
            View = null;
            ViewModel = null;
        }
        public override void Loaded(ViewLoadedParams viewLoadedParams)
        {
            if (viewLoadedParams == null) throw new ArgumentNullException(nameof(viewLoadedParams));

            _viewLoadedParamsReference = viewLoadedParams;

            // Add a button to Dynamo View menu to manually show the window
            DynamoMigrationAssistantMenuItem = new MenuItem { Header = Properties.Resources.HeaderText };

            DynamoMigrationAssistantMenuItem.Click += DynamoMigrationAssistantMenuItemOnClick;

            _viewLoadedParamsReference.AddExtensionMenuItem(DynamoMigrationAssistantMenuItem);

            //InitializeViewExtension();
        }

        private void DynamoMigrationAssistantMenuItemOnClick(object sender, RoutedEventArgs e)
        {
            Model = new DynamoGraphMigrationAssistantModel(_viewLoadedParamsReference);
            ViewModel = new DynamoGraphMigrationAssistantViewModel(Model);
            View = new DynamoMigrationAssistantView(ViewModel)
            {
                Owner = _viewLoadedParamsReference.DynamoWindow
            };

            View.ShowDialog();
        }

        public void Shutdown()
        {
            Dispose();
        }
        private void InitializeViewExtension()
        {
            
        }
       
        public override void Closed()
        {
            if (DynamoMigrationAssistantMenuItem != null) DynamoMigrationAssistantMenuItem.IsChecked = false;
        }
    }
}
