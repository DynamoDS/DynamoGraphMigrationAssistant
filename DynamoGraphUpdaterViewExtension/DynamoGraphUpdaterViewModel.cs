using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using Dynamo.Core;
using Dynamo.Graph.Workspaces;
using Dynamo.ViewModels;
using Dynamo.Wpf.Extensions;
using DynamoGraphUpdater.Controls;

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
        /// Collection of graphs loaded for exporting
        /// </summary>
        public ObservableCollection<UpdateableGraphViewModel> UpdateableGraphs { get; set; }

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

        /// <summary>
        ///     The source path containing dynamo graphs to be exported
        /// </summary>
        public PathViewModel SourcePathViewModel { get; set; }

        public DynamoGraphUpdaterViewModel(ViewLoadedParams p)
        {
            if (p == null) return;
            _viewLoadedParamsInstance = p;

            //store our source path and subscribe to changed event
            SourcePathViewModel = new PathViewModel
            { Type = PathType.Source, Owner = _viewLoadedParamsInstance.DynamoWindow };

            SourcePathViewModel.PropertyChanged += SourcePathPropertyChanged;

            CurrentPageIndex = 0;
        }
        // Handles source path changed
        private void SourcePathPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            var pathVM = sender as PathViewModel;
            if (pathVM == null) return;
            if (propertyChangedEventArgs.PropertyName == nameof(PathViewModel.FolderPath))
            {
                if (pathVM.Type == PathType.Source)
                {
                    SourceFolderChanged(pathVM);
                }
                else
                {
                    //TargetFolderChanged();
                }

                //RaisePropertyChanged(nameof(CanExport));
            }
        }
        // Update graphs if source folder is changed by the UI
        private void SourceFolderChanged(PathViewModel pathVM)
        {
            UpdateableGraphs = new ObservableCollection<UpdateableGraphViewModel>();

            var files = Utilities.GetAllFilesOfExtension(pathVM.FolderPath)?.OrderBy(x => x);
            if (files == null)
                return;

            var groupedDyns = files.GroupBy(Utilities.GetDynamoVersionForGraph);

            foreach (var dynGroup in groupedDyns.OrderBy(g => g.Key))
            {
                UpdateableGraphViewModel updateableGraphViewModel = new UpdateableGraphViewModel(dynGroup.Key, dynGroup.ToList());
              
                UpdateableGraphs.Add(updateableGraphViewModel);
            }

            RaisePropertyChanged(nameof(UpdateableGraphs));
        }
        public void Dispose()
        {
            CurrentWorkspace = null;
            DynamoViewModel = null;
        }
    }
}
