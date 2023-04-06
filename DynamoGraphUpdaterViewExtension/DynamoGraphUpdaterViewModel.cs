using System;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using Dynamo.Core;
using Dynamo.Graph.Workspaces;
using Dynamo.ViewModels;
using Dynamo.Wpf.Extensions;

namespace DynamoGraphUpdater
{
    public class DynamoGraphUpdaterViewModel : NotificationObject, IDisposable
    {
        #region Fields and Properties
        private readonly ViewLoadedParams _viewLoadedParamsInstance;
        internal DynamoViewModel DynamoViewModel;
        internal HomeWorkspaceModel CurrentWorkspace;
        #endregion

        /// <summary>
        ///     Collection of graphs loaded for exporting
        /// </summary>
        public ObservableCollection<Page> NavigationPages { get; set; }

        private int _currentPage;
        public int CurrentPage
        {
            get { return _currentPage; }
            set { _currentPage = value; }
        }



        public DynamoGraphUpdaterViewModel(ViewLoadedParams p)
        {
            if (p == null) return;
            _viewLoadedParamsInstance = p;

            CurrentPage = 0;
        }
        public void Dispose()
        {
            CurrentWorkspace = null;
            DynamoViewModel = null;
        }
    }
}
