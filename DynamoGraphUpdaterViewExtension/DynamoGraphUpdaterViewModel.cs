using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
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
        private DynamoGraphUpdaterModel _model;
        #endregion


        /// <summary>
        /// Collection of graphs loaded for upgrading
        /// </summary>
        public ObservableCollection<UpdateableGraphsViewModel> UpdateableGraphs { get; set; }

        /// <summary>
        /// The potential target versions
        /// </summary>
         public ObservableCollection<TargetDynamoVersion> PotentialTargetVersions { get; set; }
        public ObservableCollection<TargetDynamoVersion> TargetVersions { get; set; }

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

        private bool _singleGraphMode;
        public bool SingleGraphMode
        {
            get { return _singleGraphMode; }
            set { _singleGraphMode = value; RaisePropertyChanged(nameof(SingleGraphMode));}
        }

        private bool _multipleGraphMode;
        public bool MultipleGraphMode
        {
            get { return _multipleGraphMode; }
            set { _multipleGraphMode = value; RaisePropertyChanged(nameof(MultipleGraphMode)); }
        }

        /// <summary>
        /// The source path containing dynamo graphs to be exported
        /// </summary>
        public PathViewModel SourcePathViewModel { get; set; }

        public DynamoGraphUpdaterViewModel(DynamoGraphUpdaterModel model)
        {
            if (model == null) return;
            _model = model;
            
            //store our source path and subscribe to changed event
            SourcePathViewModel = _model.SourcePathViewModel();
            SourcePathViewModel.PropertyChanged += SourcePathPropertyChanged;

            CurrentPageIndex = 0;

            if (_model.CurrentWorkspace != null)
            {
                SingleGraphMode = true;
                MultipleGraphMode = false;
            }
            else
            {
                MultipleGraphMode = true;
                SingleGraphMode = false;
            }

            //Load our target versions from json TODO: make this read extra folder
            PotentialTargetVersions = new ObservableCollection<TargetDynamoVersion>()
            {
                new TargetDynamoVersion("2.5", "Revit", "2021", false, false, true),
                new TargetDynamoVersion("2.6", "Revit", "2021.1", false, false, true),
                new TargetDynamoVersion("2.10", "Revit", "2022", false, false, true),
                new TargetDynamoVersion("2.12", "Revit", "2022.1", false, true, true),
                new TargetDynamoVersion("2.16", "Revit", "2023", true, true, true),
                new TargetDynamoVersion("2.17", "Revit", " 2024", true, true, true),

                new TargetDynamoVersion("2.10", "Civil3D", "2022", false, false, true),
                new TargetDynamoVersion("2.13", "Civil3D", "2023", true, true, true),
                new TargetDynamoVersion("2.17", "Civil3D", " 2024", true, true, true),
            };

          

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
            UpdateableGraphs = new ObservableCollection<UpdateableGraphsViewModel>();

            var files = Utilities.GetAllFilesOfExtension(pathVM.FolderPath)?.OrderBy(x => x);
            if (files == null)
                return;

            var graphs = new List<UpdateableGraphViewModel>();

            foreach (var file in files)
            {
                graphs.Add(new UpdateableGraphViewModel(Utilities.GetDynamoVersionForGraph(file), file));

            }

            var groupedByVersion = graphs.GroupBy(g => g.TruncatedVersion);

            foreach (var group in groupedByVersion.OrderBy(g => g.Key.Major).ThenBy(g => g.Key.Minor))
            {
                var updateableGraph = new UpdateableGraphsViewModel(group.Key, group.ToList());

                UpdateableGraphs.Add(updateableGraph);
            }


            //pick the potential versions from what our graphs mostly consist of
            var whatProductIsUsedMost = graphs.GroupBy(g => g.Product).OrderByDescending(g => g.Count()).FirstOrDefault()?.Key;

            if (whatProductIsUsedMost is null)
            {
                TargetVersions = PotentialTargetVersions;
            }
            else
            {
                TargetVersions = new ObservableCollection<TargetDynamoVersion>(PotentialTargetVersions.Where(p => p.Host.Equals(whatProductIsUsedMost)));
            }
            RaisePropertyChanged(nameof(TargetVersions));
            RaisePropertyChanged(nameof(UpdateableGraphs));
        }


        public void Dispose()
        {
            //CurrentWorkspace = null;
            _model = null;
        }
    }
}
