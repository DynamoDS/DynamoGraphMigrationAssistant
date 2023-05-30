using Dynamo.Wpf.Extensions;
using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using CoreNodeModels.Input;
using Dynamo.Extensions;
using Dynamo.Graph.Workspaces;
using Dynamo.WorkspaceDependency;
using DynamoGraphMigrationAssistant.ViewModels;
using DynamoGraphMigrationAssistant.Views;
using Directory = System.IO.Directory;

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

      
        internal GraphMigrationAssistantView View;
        internal GraphMigrationAssistantViewModel ViewModel;

        public DynamoGraphMigrationAssistantViewExtension()
        {
            InitializeViewExtension();
        }
        public override void Startup(ViewStartupParams viewStartupParams)
        {
        }

        private void ExtensionLoaderOnExtensionLoading(IExtension obj)
        {
            var thing = obj.Name;
        }

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
            DynamoMigrationAssistantMenuItem = new MenuItem { Header = Properties.Resources.HeaderText, IsCheckable = true};

            DynamoMigrationAssistantMenuItem.Checked += MenuItemCheckHandler;
            DynamoMigrationAssistantMenuItem.Unchecked += MenuItemUnCheckHandler;

            _viewLoadedParamsReference.AddExtensionMenuItem(DynamoMigrationAssistantMenuItem);

            //InitializeViewExtension();
        }

        public void Shutdown()
        {
            Dispose();
        }

        private void MenuItemCheckHandler(object sender, RoutedEventArgs e)
        {
            AddToSidebar();
        }

        private void MenuItemUnCheckHandler(object sender, RoutedEventArgs e)
        {
            this.Dispose();

            _viewLoadedParamsReference.CloseExtensioninInSideBar(this);
        }

        private void InitializeViewExtension()
        {
            ViewModel = new GraphMigrationAssistantViewModel(_viewLoadedParamsReference);
            View = new GraphMigrationAssistantView(ViewModel);
        }
        private void AddToSidebar()
        {
            InitializeViewExtension();

            //if the user is launching the extension from the starting view, simply launch a new blank workspace.
            //(The starting view has a graph view model at all times, so it is difficult to know)
            //TODO: see if there is a better event for this.
            if (!ViewModel.DynamoViewModel.CurrentSpaceViewModel.Nodes.Any())
            {
                ViewModel.DynamoViewModel.NewHomeWorkspaceCommand.Execute(null);
            }

            _viewLoadedParamsReference?.AddToExtensionsSideBar(this, View);
        }
        public override void Closed()
        {
            if (DynamoMigrationAssistantMenuItem != null) DynamoMigrationAssistantMenuItem.IsChecked = false;
        }
    }
}
