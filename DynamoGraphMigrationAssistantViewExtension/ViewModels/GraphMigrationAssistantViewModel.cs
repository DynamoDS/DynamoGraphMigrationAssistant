﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Permissions;
using System.Text;
using System.Windows;
using System.Windows.Threading;
using Dynamo.Configuration;
using Dynamo.Controls;
using Dynamo.Core;
using Dynamo.Graph.Nodes;
using Dynamo.Graph.Workspaces;
using Dynamo.Models;
using Dynamo.Scheduler;
using Dynamo.UI.Commands;
using Dynamo.Utilities;
using Dynamo.ViewModels;
using Dynamo.Wpf.Extensions;
using DynamoGraphMigrationAssistant.Controls;
using DynamoGraphMigrationAssistant.Views;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Directory = System.IO.Directory;
using Path = System.IO.Path;
using String = System.String;

namespace DynamoGraphMigrationAssistant.ViewModels
{
    public enum MigrationPhase
    {
        Open,
        PerformMigration,
        Save
    };

    public class GraphMigrationAssistantViewModel : NotificationObject, IDisposable
    {

        #region Fields and Properties
        private readonly ViewLoadedParams viewLoadedParamsInstance;
        internal DynamoViewModel DynamoViewModel;
        internal HomeWorkspaceModel CurrentWorkspace;
        private Dictionary<int, GraphViewModel> graphDictionary = new Dictionary<int, GraphViewModel>();
        private MigrationPhase phase = MigrationPhase.Open;
        private bool locked = false;
        private bool isAvailable = true;
        private bool finished = true;
        private DynamoScheduler scheduler;
        int progress = 0;

        public StringBuilder sb;


        /// <summary>
        /// Is UI Available
        /// </summary>
        public bool Available
        {
            get => isAvailable;
            set
            {
                if (isAvailable == value) return;
                isAvailable = value;
                RaisePropertyChanged(nameof(Available));
            }
        }

        /// <summary>
        /// Collection of potential target Dynamo versions
        /// </summary>
        public ObservableCollection<TargetDynamoVersion> TargetDynamoVersions { get; set; }
        public TargetDynamoVersion TargetDynamoVersion { get; set; }

        private int _targetVersionIndex = 0;
        /// <summary>
        /// The selected index. Based on current Dynamo version.
        /// </summary>
        public int TargetVersionIndex
        {
            get => _targetVersionIndex;
            set
            {
                if (_targetVersionIndex == value) return;
                _targetVersionIndex = value; 
                RaisePropertyChanged(nameof(CanExport));
            }
        }

        /// <summary>
        /// Collection of graphs loaded for exporting
        /// </summary>
        public ObservableCollection<GraphViewModel> Graphs { get; set; }

        /// <summary>
        /// The source path containing dynamo graphs to be exported
        /// </summary>
        public PathViewModel SourcePathViewModel { get; set; }

        /// <summary>
        /// The target path where the images will be stored
        /// </summary>
        public PathViewModel TargetPathViewModel { get; set; }

        public MigrationSettings MigrationSettings { get; set; }
        /// <summary>
        /// The migration settings view model
        /// </summary>
        public MigrationSettingsViewModel MigrationSettingsViewModel { get; set; }

        private bool _canExport;
        /// <summary>
        /// Checks if both folder paths have been set
        /// </summary>
        public bool CanExport
        {
            get
            {
                if (TargetPathViewModel?.FolderPath == null)
                    return false;
                if (SourcePathViewModel?.FolderPath == null && !SingleGraph)
                    return false;
                if (SourcePathViewModel?.FolderPath == null && SingleGraph)
                    return Utilities.IsValidPath(TargetPathViewModel.FolderPath) && TargetDynamoVersion != null && IsTrustedFolder;
                if (SourcePathViewModel?.FolderPath != null)
                    return Utilities.IsValidPath(TargetPathViewModel.FolderPath) && Utilities.IsValidPath(SourcePathViewModel.FolderPath) && TargetDynamoVersion != null && IsTrustedFolder;
                return false;
            }
            private set
            {
                if (_canExport == value) return;
                _canExport = value;
                RaisePropertyChanged(nameof(CanExport));
            }
        }

        private bool _replaceIfNodes = true;
        /// <summary>
        /// Replace if nodes to refactored if
        /// </summary>
        public bool ReplaceIfNodes
        {
            get => _replaceIfNodes;
            set
            {
                if (_replaceIfNodes == value) return;
                _replaceIfNodes = value;
                RaisePropertyChanged(nameof(ReplaceIfNodes));
            }
        }

        private bool _fixNodeSpacing = true;
        /// <summary>
        /// With Dynamo 2.13 came larger nodes. this spaces them out to fix that.
        /// </summary>
        public bool FixNodeSpacing
        {
            get => _fixNodeSpacing;
            set
            {
                if (_fixNodeSpacing == value) return;
                _fixNodeSpacing = value;
                RaisePropertyChanged(nameof(FixNodeSpacing));
            }
        }

        private bool _fixInputOrder = false;
        /// <summary>
        /// Dynamo Player can now support input order alphabetically. Historically users created their inputs in order to fix this.
        /// </summary>
        public bool FixInputOrder
        {
            get => _fixInputOrder;
            set
            {
                if (_fixInputOrder == value) return;
                _fixInputOrder = value;
                RaisePropertyChanged(nameof(FixInputOrder));
            }
        }

        private bool _fixInputLinebreaks = false;
        /// <summary>
        /// Previously users entered line breaks into inputs for directions. This offers a fix with new pinned notes.
        /// </summary>
        public bool FixInputLinebreaks
        {
            get => _fixInputLinebreaks;
            set
            {
                if (_fixInputLinebreaks == value) return;
                _fixInputLinebreaks = value;
                RaisePropertyChanged(nameof(FixInputLinebreaks));
            }
        }

        private bool _resume = false;
        /// <summary>
        ///     When this flag is set to true, will attempt to resume progress
        ///     based on the log file in the current destination folder
        /// </summary>
        public bool Resume
        {
            get => _resume;
            set
            {
                if (_resume == value) return;
                _resume = value;
                RaisePropertyChanged(nameof(Resume));
                TargetFolderChanged();
            }
        }

        private bool _singleGraph = false;
        /// <summary>
        /// This override allows users to export a single graph
        /// The current graph will be exported to the Target folder
        /// </summary>
        public bool SingleGraph
        {
            get => _singleGraph;
            set
            {
                if (_singleGraph == value) return;
                _singleGraph = value;
                RaisePropertyChanged(nameof(SingleGraph));
                RaisePropertyChanged(nameof(CanExport));
            }
        }

        private bool _isKeepFolderStructure = true;
        /// <summary>
        /// Contains user preference to retain folder structure for images
        /// </summary>
        public bool IsKeepFolderStructure
        {
            get => _isKeepFolderStructure;
            set
            {
                if (_isKeepFolderStructure != value)
                {
                    _isKeepFolderStructure = value;
                    RaisePropertyChanged(nameof(IsKeepFolderStructure));
                }
            }
        }

        /// <summary>
        /// marks the source folder as trusted
        /// </summary>
        private bool _isTrustedFolder = false;
        public bool IsTrustedFolder
        {
            get => _isTrustedFolder;
            set
            {
                if (_isTrustedFolder == value) return;
                _isTrustedFolder = value;
                RaisePropertyChanged(nameof(IsTrustedFolder));
                RaisePropertyChanged(nameof(CanExport));
            }
        }
        /// <summary>
        /// show the trust checkbox to the user if needed
        /// </summary>
        private bool _trustCheckboxVisible = false;
        public bool TrustCheckboxVisible
        {
            get => _trustCheckboxVisible;
            set
            {
                if (_trustCheckboxVisible == value) return;
                _trustCheckboxVisible = value;
                RaisePropertyChanged(nameof(TrustCheckboxVisible));
            }
        }


        private bool _stopButtonVisible = false;
        /// <summary>
        /// show and hide the stop button to the user
        /// </summary>
        public bool StopButtonVisible
        {
            get => _stopButtonVisible;
            set
            {
                if (_stopButtonVisible == value) return;
                _stopButtonVisible = value;
                RaisePropertyChanged(nameof(StopButtonVisible));
            }
        }
        private bool _viewOutputButtonVisible = false;
        /// <summary>
        /// show and hide the view output button to the user
        /// </summary>
        public bool ViewOutputButtonVisible
        {
            get => _viewOutputButtonVisible;
            set
            {
                if (_viewOutputButtonVisible == value) return;
                _viewOutputButtonVisible = value;
                RaisePropertyChanged(nameof(ViewOutputButtonVisible));
            }
        }

        private string _notificationMessage;
        private Dispatcher _dispatcher;
        private Queue<string> _graphQueue;

        /// <summary>
        /// Contains notification text displayed on the UI
        /// </summary>
        public string NotificationMessage
        {
            get => _notificationMessage;

            set
            {
                if (_notificationMessage != value)
                {
                    _notificationMessage = value;
                    RaisePropertyChanged(nameof(NotificationMessage));
                }
            }
        }

        public DelegateCommand ExportGraphsCommand { get; set; }
        public DelegateCommand CancelCommand { get; set; }
        public DelegateCommand ViewOutputCommand { get; set; }
        public DelegateCommand ViewHelpDocumentationCommand { get; set; }
        public DelegateCommand EditGraphSettingsCommand { get; set; }

        #endregion

        #region Loading

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="p"></param>
        public GraphMigrationAssistantViewModel(ViewLoadedParams p)
        {
            if (p == null) return;

            viewLoadedParamsInstance = p;

            //load our settings
            MigrationSettings = MigrationSettings.DeserializeModels();
            MigrationSettingsViewModel = new MigrationSettingsViewModel(MigrationSettings);


            p.CurrentWorkspaceChanged += OnCurrentWorkspaceChanged;
            p.CurrentWorkspaceCleared += OnCurrentWorkspaceCleared;

            viewLoadedParamsInstance = p;

            viewLoadedParamsInstance.CurrentWorkspaceChanged += OnCurrentWorkspaceChanged;
            viewLoadedParamsInstance.CurrentWorkspaceCleared += OnCurrentWorkspaceCleared;

            TrustCheckboxVisible = false;

            if (viewLoadedParamsInstance.CurrentWorkspaceModel is HomeWorkspaceModel)
            {
                CurrentWorkspace = viewLoadedParamsInstance.CurrentWorkspaceModel as HomeWorkspaceModel;
                DynamoViewModel = viewLoadedParamsInstance.DynamoWindow.DataContext as DynamoViewModel;
                _dispatcher = viewLoadedParamsInstance.DynamoWindow.Dispatcher;
                scheduler = DynamoViewModel.Model.Scheduler;
            }

            TargetPathViewModel = new PathViewModel
            { Type = PathType.Target, Owner = viewLoadedParamsInstance.DynamoWindow };
            SourcePathViewModel = new PathViewModel
            { Type = PathType.Source, Owner = viewLoadedParamsInstance.DynamoWindow };

            _dispatcher.Hooks.DispatcherInactive += OnDispatcherFinished;

            TargetPathViewModel.PropertyChanged += PathPropertyChanged;
            SourcePathViewModel.PropertyChanged += PathPropertyChanged;

            ExportGraphsCommand = new DelegateCommand(ExportGraphs);
            CancelCommand = new DelegateCommand(Cancel);
            ViewOutputCommand = new DelegateCommand(ViewOutput);
            ViewHelpDocumentationCommand = new DelegateCommand(ViewHelpDocumentation);
            EditGraphSettingsCommand = new DelegateCommand(EditSettings);
            _graphQueue = new Queue<string>();

            sb = new StringBuilder();

            //LoadTargetDynamoVersions();
        }



        // Handles the path being changed
        private void PathPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            if (!(sender is PathViewModel pathVm)) return;
            if (propertyChangedEventArgs.PropertyName == nameof(PathViewModel.FolderPath))
            {
                if (pathVm.Type == PathType.Source)
                {
                    Version currentDynamoVersion = new Version(DynamoViewModel.Model.Version);
                    Version dynamoVersionWithFileTrust = new Version(2, 16, 0);

                    if (currentDynamoVersion > dynamoVersionWithFileTrust)
                    {
                        //use reflection to maintain compatibility between Dynamo 2.13 and up
                        MethodInfo addToTrustedLocations = typeof(PreferenceSettings).GetMethod("IsTrustedLocation",
                            BindingFlags.Public | BindingFlags.Instance);
                        var isTrusted = (bool)addToTrustedLocations.Invoke(DynamoViewModel.PreferenceSettings,
                                new object[] { pathVm.FolderPath });

                        if (!isTrusted)
                        {
                            TrustCheckboxVisible = true;
                            IsTrustedFolder = false;
                        }
                        else
                        {
                            IsTrustedFolder = true;
                        }

                        SourceFolderChanged(pathVm);
                    }
                    else
                    {
                        TrustCheckboxVisible = false;
                        IsTrustedFolder = true;
                        SourceFolderChanged(pathVm);
                    }
                }
                else
                {
                    TargetFolderChanged();
                }

                RaisePropertyChanged(nameof(CanExport));
            }
        }

        // target version changed
        internal void TargetVersionChanged()
        {
            RaisePropertyChanged(nameof(CanExport));
        }

        // Update graphs if source folder is changed by the UI
        private void SourceFolderChanged(PathViewModel pathVm)
        {
            Graphs = new ObservableCollection<GraphViewModel>();

            graphDictionary = new Dictionary<int, GraphViewModel>();

            var files = Utilities.GetAllFilesOfExtension(pathVm.FolderPath)?.OrderBy(x => x);
            if (files == null)
                return;

            foreach (var graph in files)
            {
                var name = Path.GetFileNameWithoutExtension(graph);
                var uniqueName = Path.GetFullPath(graph);
                var graphVM = new GraphViewModel { GraphName = name, UniqueName = uniqueName };

                if (TargetDynamoVersion != null)
                {
                    graphVM.InTargetVersion = graphVM.Version.Equals(TargetDynamoVersion.Version);
                }
                else
                {
                    graphVM.InTargetVersion = false;
                }

                //check if the graph is in the target version
                if (graphVM.InTargetVersion)
                {
                    //graphVM.Exported = true;
                    Graphs.Add(graphVM);
                    graphDictionary[uniqueName.GetHashCode()] = graphVM;
                }
                else
                {
                    Graphs.Add(graphVM);
                    graphDictionary[uniqueName.GetHashCode()] = graphVM;
                }

            }

            NotificationMessage = String.Format(Properties.Resources.NotificationMsg, Graphs.Count.ToString());
            RaisePropertyChanged(nameof(Graphs));

            LoadTargetDynamoVersions();

            CheckVersions();
        }

        private void TargetFolderChanged()
        {
            //throw error if the user picks the source path as the target path.
            if (TargetPathViewModel.FolderPath.Equals(SourcePathViewModel.FolderPath))
            {
                NotificationMessage = Properties.Resources.NotificationMsgWarningTargetCannotBeSource;
                TargetPathViewModel.FolderPath = string.Empty;
                MessageBox.Show(Properties.Resources.NotificationMsgWarningTargetCannotBeSource);
                return;
            }



            var log = GetLogFileInformation();

            // Do not enqueue the file if it is already in the log file
            if (_resume && log != null)
            {
                Graphs.RemoveAll(x => log.Contains(x.UniqueName));
                graphDictionary = Graphs.ToDictionary(x => x.UniqueName.GetHashCode(), x => x);
            }

            if (Graphs == null) return;

            NotificationMessage = String.Format(Properties.Resources.NotificationMsg, Graphs.Count.ToString());
            RaisePropertyChanged(nameof(Graphs));
            RaisePropertyChanged(nameof(CanExport));
        }

        internal void CheckVersions()
        {
            var graphList = Graphs.ToList();


            foreach (var graphViewModel in graphList)
            {
                Version original = Version.Parse(graphViewModel.Version);

                if (TargetDynamoVersion != null)
                {
                    Version target = Version.Parse(TargetDynamoVersion.Version);

                    if (original >= target)
                    {
                        graphViewModel.InTargetVersion = true;
                    }
                    else
                    {
                        graphViewModel.InTargetVersion = false;
                    }
                }
                else
                {
                    graphViewModel.InTargetVersion = false;
                }

              
            }
            TargetVersionChanged();

        }

        private void OnCurrentWorkspaceCleared(IWorkspaceModel workspace)
        {
            CurrentWorkspace = viewLoadedParamsInstance.CurrentWorkspaceModel as HomeWorkspaceModel;
            if (CurrentWorkspace == null) return;
        }

        private void OnCurrentWorkspaceChanged(IWorkspaceModel workspace)
        {
            CurrentWorkspace = workspace as HomeWorkspaceModel;
            if (CurrentWorkspace == null) return;
        }


        #endregion

        #region Methods
        /// <summary>
        /// Load the potential Dynamo versions to migrate to. This allows us to preselect the suggestion migration tasks.
        /// </summary>
        private void LoadTargetDynamoVersions()
        {
            //Load our target versions from json
            if (Graphs!= null)
            {
                //calculate which product is used most for version selection.
                var whatProductIsUsedMost = Graphs.GroupBy(g => g.Product).OrderByDescending(g => g.Count()).FirstOrDefault()?.Key;

                if (whatProductIsUsedMost is null)
                {
                    TargetDynamoVersions = TargetDynamoVersion.DefaultVersions();
                }
                else
                {
                    TargetDynamoVersions = TargetDynamoVersion.LoadPotentialVersions(whatProductIsUsedMost);
                }

                //now set the target based on what version is open
                try
                {
                    Version currentDynamoVersion = Version.Parse(DynamoViewModel.Version);
                    string stringVersion = $"{currentDynamoVersion.Major}.{currentDynamoVersion.Minor}";
                    var potential = TargetDynamoVersions.First(v => v.Version.Equals(stringVersion));
                    TargetVersionIndex = TargetDynamoVersions.IndexOf(potential);
                }
                catch (Exception)
                {
                    //couldn't select index, just leave at 0
                }

                RaisePropertyChanged("TargetDynamoVersions");
                RaisePropertyChanged("TargetVersionIndex");
            }
        }

        /// <summary>
        /// The main method executing the migration
        /// </summary>
        /// <param name="obj"></param>
        private void ExportGraphs(object obj)
        {
            if (string.IsNullOrEmpty(TargetPathViewModel.FolderPath) ||
                string.IsNullOrEmpty(SourcePathViewModel.FolderPath) && !SingleGraph)
                return;

            if (SingleGraph)
            {
                NotificationMessage = string.Empty;
                ExportCurrentGraph();
                return;
            }

            ResetUi();

            //show the stop button
            StopButtonVisible = true;
            ViewOutputButtonVisible = false;
            Available = false;

            //add the folder to trust if the user checked it
            if (IsTrustedFolder)
            {
                MethodInfo addToTrustedLocations = typeof(PreferenceSettings).GetMethod("AddTrustedLocation",
                    BindingFlags.NonPublic | BindingFlags.Instance);
                if (addToTrustedLocations != null)
                    addToTrustedLocations.Invoke(DynamoViewModel.PreferenceSettings,
                        new object[] { SourcePathViewModel.FolderPath });
            }

            foreach (var file in Graphs.Where(g => !g.InTargetVersion && !g.IsXmlBased).ToList().Select(x => x.UniqueName).ToList())
            {
                _graphQueue.Enqueue(file);
            }

            //copy the ones already in the path
            foreach (var fileInTarget in Graphs.Where(g => g.InTargetVersion || g.IsXmlBased))
            {
                string message = Properties.Resources.FileCopiedLogMessage;
                var graphName = GetDynPath(fileInTarget.UniqueName);

                if (fileInTarget.IsXmlBased)
                {
                    FileInfo fInfo = new FileInfo(graphName);
                    var directory = fInfo.DirectoryName;

                    var newDirectory = Path.Combine(directory, "_tooOldToMigrate");
                    if (!Directory.Exists(newDirectory))
                    {
                        Directory.CreateDirectory(newDirectory);
                    }

                    graphName = graphName.Replace(directory,newDirectory);

                    //for the log
                    message = Properties.Resources.XmlFileCopiedLogMessage;
                }

                File.Copy(fileInTarget.UniqueName, graphName, true);

                fileInTarget.Exported = true;

                //log the changes

                EnterLog(string.Format(message, fileInTarget.GraphName));

                progress++;
            }
        }

        private void ExportCurrentGraph()
        {
            SaveGraph();
            NotificationMessage = String.Format(Properties.Resources.FinishSingleMsg, CurrentWorkspace.Name);
        }

        private string[] GetLogFileInformation()
        {
            var filePath = TargetPathViewModel.FolderPath + "\\log.txt";
            if (File.Exists(filePath))
            {
                return File.ReadAllLines(filePath);
            }

            return null;
        }

        private void OnDispatcherFinished(object sender, EventArgs e)
        {
            if (locked || _graphQueue.Count > 0)
            {
                if (scheduler.HasPendingTasks) return;

                switch (phase)
                {
                    case (MigrationPhase.Open):
                        locked = true;
                        OpenGraph(_graphQueue.Dequeue());
                        break;
                    case (MigrationPhase.PerformMigration):
                        ExportGraph();
                        break;
                    case (MigrationPhase.Save):
                        locked = false;
                        Iterate();
                        SaveGraph();
                        break;
                }
            }

            if (_graphQueue.Count == 0 && !finished && !locked)
            {
                finished = true;
                InformFinish((progress).ToString());
            }
        }

        private void Iterate()
        {
            NotificationMessage = String.Format(Properties.Resources.ProcessMsg, (progress + 1).ToString(), graphDictionary.Count.ToString());

            var filename = Path.GetFullPath(CurrentWorkspace.FileName);
            if (!filename.EndsWith(".dyn"))
            {
                filename = $"{filename}.dyn";
            }
            var hashCode = filename.GetHashCode();

            graphDictionary.TryGetValue(hashCode, out GraphViewModel graphViewModel);
            if (graphViewModel != null)
            {
                graphViewModel.Exported = true;
            }


            EnterLog(CurrentWorkspace.FileName);

            progress++;
        }

        private void EnterLog(string entry)
        {
            sb.Append(entry);
            sb.Append(Environment.NewLine);

            File.AppendAllText(TargetPathViewModel.FolderPath + "\\log.txt", sb.ToString());
            sb.Clear();
        }

        private void OpenGraph(string file)
        {
            phase = MigrationPhase.PerformMigration;

            OpenDynamoGraph(file);
        }

        private void ExportGraph()
        {
            phase = MigrationPhase.Save;

            //replace if nodes
            if (ReplaceIfNodes)
            {
                MigrateIfToRefactoredIf();
            }

            //fix node spacing
            if (FixNodeSpacing)
            {
                FixNodeSpacingForGraph();
            }

            if (FixInputOrder)
            {
                FormatInputOrderForGraph();
            }

            if (FixInputLinebreaks)
            {
                MigrateInputLinebreaksToPinnedNotes();
            }
        }


        public void MigrateIfToRefactoredIf()
        {
            int ifNodeReplacementCount = 0;

            var nodeName = "CoreNodeModels.Logic.RefactoredIf";

            if(DynamoViewModel.CurrentSpaceViewModel is null) return;

            var ifNodesToReplace = DynamoViewModel.CurrentSpaceViewModel.Nodes.Where(n =>
                n.NodeModel.GetType().FullName.Equals("CoreNodeModels.Logic.If")).ToList();

            foreach (var nodeViewModel in ifNodesToReplace)
            {
                List<DynamoModel.MakeConnectionCommand> connectionCommands = new List<DynamoModel.MakeConnectionCommand>();

                var inports = nodeViewModel.NodeModel.InPorts.ToList();
                var outports = nodeViewModel.NodeModel.OutPorts.ToList();

                //zoom to node
                //DynamoViewModel.CurrentSpaceViewModel.FocusNodeCommand.Execute(nodeViewModel.NodeModel.GUID);

                DynamoModel.CreateNodeCommand createRefactoredIf =
                    new DynamoModel.CreateNodeCommand(Guid.NewGuid().ToString(), nodeName, nodeViewModel.X, nodeViewModel.Y, false, false);

                DynamoViewModel.ExecuteCommand(createRefactoredIf);

                NodeViewModel refactoredIfNode = DynamoViewModel.CurrentSpaceViewModel.Nodes.LastOrDefault();

                //connect inports
                foreach (var inport in inports)
                {
                    foreach (var connector in inport.Connectors)
                    {
                        var guid = connector.Start.Owner.GUID;

                        var portIndex = connector.Start.Index;

                        //begin the connection
                        connectionCommands.Add(new DynamoModel.MakeConnectionCommand(guid, portIndex, PortType.Output,
                            DynamoModel.MakeConnectionCommand.Mode.Begin));

                        //end the connection
                        connectionCommands.Add(
                            new DynamoModel.MakeConnectionCommand(refactoredIfNode.NodeModel.GUID, connector.End.Index, PortType.Input,
                                DynamoModel.MakeConnectionCommand.Mode.End));
                    }
                }
                //connect outports
                foreach (var outport in outports)
                {
                    foreach (var connector in outport.Connectors)
                    {
                        var guid = connector.End.Owner.GUID;

                        var portIndex = connector.Start.Index;

                        //begin the connection
                        connectionCommands.Add(
                            new DynamoModel.MakeConnectionCommand(refactoredIfNode.NodeModel.GUID, portIndex,
                                PortType.Output,
                                DynamoModel.MakeConnectionCommand.Mode.Begin));

                        //end the connection
                        connectionCommands.Add(new DynamoModel.MakeConnectionCommand(guid, connector.End.Index,
                            PortType.Input,
                            DynamoModel.MakeConnectionCommand.Mode.End));
                    }
                }

                foreach (var connectionCommand in connectionCommands)
                {
                    DynamoViewModel.ExecuteCommand(connectionCommand);
                }

                DynamoModel.DeleteModelCommand delete = new DynamoModel.DeleteModelCommand(nodeViewModel.NodeModel.GUID);

                DynamoViewModel.ExecuteCommand(delete);

                ifNodeReplacementCount++;
            }

            //log the changes
            EnterLog(string.Format(Properties.Resources.IfNodeReplacementLogMessage, ifNodeReplacementCount));
        }

        private void FixNodeSpacingForGraph()
        {
            int nodesMovedCount = 0;

            foreach (var nodeViewModel in DynamoViewModel.CurrentSpaceViewModel.Nodes)
            {
                var originalX = nodeViewModel.X;
                var originalY = nodeViewModel.Y;

                nodeViewModel.X = originalX * MigrationSettings.ScaleFactorX;
                nodeViewModel.Y = originalY * MigrationSettings.ScaleFactorY;

                nodeViewModel.NodeModel.ReportPosition();

                nodesMovedCount++;
            }

            foreach (var noteViewModel in DynamoViewModel.CurrentSpaceViewModel.Notes)
            {
                var originalX = noteViewModel.Left;
                var originalY = noteViewModel.Top;

                noteViewModel.Left = originalX * MigrationSettings.ScaleFactorX;
                noteViewModel.Top = originalY * MigrationSettings.ScaleFactorY;

                noteViewModel.Model.ReportPosition();

                nodesMovedCount++;
            }

            try
            {
                DynamoViewModel.FitViewCommand.Execute(null);
            }
            catch (Exception)
            {
                //suppress, the fit view command failed
            }



            //log the changes
            EnterLog(string.Format(Properties.Resources.NodesMovedLogMessage, nodesMovedCount));
        }

        //optional fixes
        private void FormatInputOrderForGraph()
        {
            var startCharacter = MigrationSettings.InputOrderStartLetter;
            int startCharacterAsNumber = Utilities.ColumnIndexByName(startCharacter);
            int startNumber = MigrationSettings.InputOrderStartNumber;

            var padding = DynamoViewModel.CurrentSpaceViewModel.Nodes.Count.ToString().Length + 1;

            foreach (var nodeViewModel in DynamoViewModel.CurrentSpaceViewModel.Nodes.OrderBy(n => n.Y))
            {
                if (nodeViewModel.IsSetAsInput)
                {
                    object next = null;
                    string currentName = nodeViewModel.Name;
                    if (!MigrationSettings.InputOrderAsNumbers)
                    {
                        next = Utilities.ColumnNameByIndex(startCharacterAsNumber++);
                    }
                    else
                    {
                        next = startNumber++.ToString().PadLeft(padding, '0');
                    }

                    nodeViewModel.NodeModel.Name = $"{next}| {currentName}";
                }
            }
        }



        private void MigrateInputLinebreaksToPinnedNotes()
        {

            foreach (var nodeViewModel in DynamoViewModel.CurrentSpaceViewModel.Nodes)
            {
                if (nodeViewModel.IsSetAsInput && nodeViewModel.NodeModel.Name.Contains(Environment.NewLine))
                {
                    var bustedUpName = nodeViewModel.NodeModel.Name.Split(
                         new string[] { Environment.NewLine },
                         StringSplitOptions.None);
                    if (bustedUpName.Length < 2) { continue; }

                    string textForNote = bustedUpName[0];
                    string newNodeName = bustedUpName[1];

                    //create our note
                    var noteGuid = Guid.NewGuid();
                    DynamoModel.RecordableCommand createNote = new DynamoModel.CreateNoteCommand(noteGuid, textForNote, nodeViewModel.X, nodeViewModel.Y - 35, false);

                    DynamoViewModel.Model.ExecuteCommand(createNote);

                    var newNote = DynamoViewModel.CurrentSpaceViewModel.Notes.First(note => note.Model.GUID.Equals(noteGuid));

                    DynamoModel.SelectModelCommand select = new DynamoModel.SelectModelCommand(nodeViewModel.NodeModel.GUID, ModifierKeys.None);
                    DynamoViewModel.Model.ExecuteCommand(select);

                    MethodInfo pinToNode = typeof(NoteViewModel).GetMethod("PinToNode",
                        BindingFlags.NonPublic | BindingFlags.Instance);
                    pinToNode.Invoke(newNote, new object[] { nodeViewModel.NodeModel });


                    //finally set the node name
                    nodeViewModel.NodeModel.Name = newNodeName;
                }
            }
        }


        private void MigratePython()
        {
            //PythonMigrationAssistantViewModel
            //foreach (var nodeViewModel in DynamoViewModel.CurrentSpaceViewModel.Nodes)
            //{
            //    if (nodeViewModel.NodeModel is PythonNode pythonNode)
            //    {
            //        MethodInfo pinToNode = typeof(NoteViewModel).GetMethod("PinToNode",
            //            BindingFlags.NonPublic | BindingFlags.Instance);
            //        pinToNode.Invoke(newNote, new object[] { nodeViewModel.NodeModel });
            //    }
            //}
            //PythonNode.
        }

        private void SaveGraph()
        {
            phase = MigrationPhase.Open;

            var graphName = GetDynPath(CurrentWorkspace.FileName);

            DynamoViewModel.SaveAsCommand.Execute(graphName);

            //open the dynamo graph as a json file to modify version and run settings
            var jsonDoc = File.ReadAllText(graphName);
            var jObject = (JObject)JsonConvert.DeserializeObject(jsonDoc);

            if (jObject.TryGetValue("View", out var value))
            {
                JObject viewObject = value.ToObject<JObject>();
                if (viewObject.TryGetValue("Dynamo", out value))
                {
                    JObject dynamoObject = value.ToObject<JObject>();

                    //set the version to the user selection
                    dynamoObject["Version"] = this.TargetDynamoVersion.VersionForFile;

                    //set the file to automatic
                    dynamoObject["RunType"] = _originalRunType;

                    //set the view back to the file
                    viewObject["Dynamo"] = dynamoObject;
                }

                jObject["View"] = viewObject;
            }
            var newJson = JsonConvert.SerializeObject(jObject, Formatting.Indented);

            File.WriteAllText(graphName, newJson);

        }


        private void ResetUi()
        {
            progress = 0;
            graphDictionary.Values.ToList().ForEach(x => x.Exported = false);
            finished = false;

            File.Delete(TargetPathViewModel.FolderPath + "\\log.txt");
        }

        private string GetDynPath(string graph)
        {
            var graphName = Path.GetFileName(graph);
            var directory = Path.GetDirectoryName(graph);
            if (directory == null) return null;

            var graphFolder = Path.GetFullPath(directory);
            var newFolder = IsKeepFolderStructure && !SingleGraph ?
                TargetPathViewModel.FolderPath + graphFolder.Substring(SourcePathViewModel.FolderPath.Length) :
                TargetPathViewModel.FolderPath;

            if (_isKeepFolderStructure)
            {
                Directory.CreateDirectory(newFolder);
            }

            return Path.Combine(newFolder, graphName);
        }

        private void InformFinish(string count)
        {
            var successMessage = String.Format(Properties.Resources.FinishMsg, count);
            var owner = Window.GetWindow(viewLoadedParamsInstance.DynamoWindow);

            EnterLog(successMessage);

            MessageBox.Show(owner, successMessage, Properties.Resources.FinishMsgTitle, MessageBoxButton.OK,
                MessageBoxImage.Information);

            //show the view output button
            StopButtonVisible = false;
            ViewOutputButtonVisible = true;

            Available = true;
        }



        // TODO: Should we bubble errors to Dynamo?
        private string _originalRunType = "Automatic";
        private void OpenDynamoGraph(string path)
        {
            try
            {
                Tuple<string, bool> openParameters = new Tuple<string, bool>(path, true);
                DynamoViewModel.OpenCommand.Execute(openParameters);

                //read the original run mode
                _originalRunType = Utilities.GetRunType(path);
            }
            catch (Exception e)
            {
                string msg = e.Message;
            }
        }


        private void Cancel(object obj)
        {
            _graphQueue.Clear();
            Reset();
        }
        private void ViewOutput(object obj)
        {
            Process.Start(TargetPathViewModel.FolderPath);
        }

        private void ViewHelpDocumentation(object obj)
        {
            var uri = new Uri("DynamoGraphMigrationAssistantViewExtension;MigrationAssistantHelpDoc.html", UriKind.Relative);
            viewLoadedParamsInstance.ViewModelCommandExecutive.OpenDocumentationLinkCommand(uri);
        }

        private void EditSettings(object obj)
        {
            var dynamoView = viewLoadedParamsInstance.DynamoWindow as DynamoView;
            MigrationSettingsView view = new MigrationSettingsView(dynamoView, MigrationSettingsViewModel);

            view.ShowDialog();
        }

        private void Reset()
        {
            phase = MigrationPhase.PerformMigration;
            locked = false;
        }

        #endregion

        #region Closing

        /// <summary>
        /// Remove event handlers
        /// </summary>
        public void Dispose()
        {
            TargetPathViewModel.PropertyChanged -= PathPropertyChanged;
            SourcePathViewModel.PropertyChanged -= PathPropertyChanged;
            _dispatcher.Hooks.DispatcherInactive -= OnDispatcherFinished;

            CurrentWorkspace = null;
            DynamoViewModel = null;
            _dispatcher = null;
            scheduler = null;
        }

        #endregion

        #region Utility

        /// <summary>
        /// Force the Dispatcher to empty it's queue
        /// </summary>
        [SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
        public static void DoEvents()
        {
            var frame = new DispatcherFrame();
            Dispatcher.CurrentDispatcher.Invoke(DispatcherPriority.Background,
                new DispatcherOperationCallback(ExitFrame), frame);
            Dispatcher.PushFrame(frame);
        }

        /// <summary>
        /// Helper method for DispatcherUtil
        /// </summary>
        /// <param name="frame"></param>
        /// <returns></returns>
        private static object ExitFrame(object frame)
        {
            ((DispatcherFrame)frame).Continue = false;
            return null;
        }


        #endregion
    }
}