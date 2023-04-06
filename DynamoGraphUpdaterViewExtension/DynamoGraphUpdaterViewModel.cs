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

        private int _currentPageIndex;
        public int CurrentPageIndex
        {
            get { return _currentPageIndex; }
            set { _currentPageIndex = value; RaisePropertyChanged(nameof(CurrentPageIndex)); RaisePropertyChanged(nameof(CurrentPage)); }
        }
        private int _currentPage;
        public int CurrentPage
        {
            get { return _currentPageIndex + 1; }
            set { _currentPage = value; RaisePropertyChanged(nameof(CurrentPage)); }
        }


        public DynamoGraphUpdaterViewModel(ViewLoadedParams p)
        {
            if (p == null) return;
            _viewLoadedParamsInstance = p;

            CurrentPageIndex = 0;
        }
        public void Dispose()
        {
            CurrentWorkspace = null;
            DynamoViewModel = null;
        }
    }
}
