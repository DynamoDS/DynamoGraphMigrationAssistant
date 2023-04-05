using Dynamo.Wpf.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using static Dynamo.ViewModels.SearchViewModel;

namespace DynamoGraphUpdaterViewExtension
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

        public DynamoGraphUpdaterViewExtension()
        {
            InitializeViewExtension();
        }

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
            
            _viewLoadedParamsReference.AddExtensionMenuItem(DynamoGraphUpdaterMenuItem);
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
        private void AddToSidebar()
        {
            InitializeViewExtension();

            _viewLoadedParamsReference?.AddToExtensionsSideBar(this, View);
        }
        public override void Closed()
        {
            if (DynamoGraphUpdaterMenuItem != null) DynamoGraphUpdaterMenuItem.IsChecked = false;
        }
    }
}
