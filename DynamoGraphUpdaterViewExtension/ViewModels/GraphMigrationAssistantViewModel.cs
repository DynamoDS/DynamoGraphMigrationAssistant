using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Permissions;
using System.Text;
using System.Windows;
using System.Windows.Threading;
using Dynamo.Core;
using Dynamo.Graph.Nodes;
using Dynamo.Graph.Workspaces;
using Dynamo.Models;
using Dynamo.Scheduler;
using Dynamo.UI.Commands;
using Dynamo.Utilities;
using Dynamo.ViewModels;
using Dynamo.Wpf.Extensions;
using Dynamo.Wpf.Utilities;
using DynamoGraphMigrationAssistant.Controls;
using DynamoGraphMigrationAssistant.Models;
using ProtoCore.AST;
using PythonNodeModels;

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
        private GraphMigrationAssistantModel _graphMigrationAssistantModel;
        private Dictionary<int, GraphViewModel> graphDictionary = new Dictionary<int, GraphViewModel>();
        private MigrationPhase phase = MigrationPhase.Open;
        private bool locked = false;
        private bool finished = true;
        private DynamoScheduler scheduler;
        int progress = 0;

        public StringBuilder sb;


        /// <summary>
        /// Collection of potential target Dynamo versions
        /// </summary>
        public ObservableCollection<TargetDynamoVersion> TargetDynamoVersions { get; set; }
        public TargetDynamoVersion TargetDynamoVersion { get; set; }

        /// <summary>
        ///     Collection of graphs loaded for exporting
        /// </summary>
        public ObservableCollection<GraphViewModel> Graphs { get; set; }

        /// <summary>
        ///     The source path containing dynamo graphs to be exported
        /// </summary>
        public PathViewModel SourcePathViewModel { get; set; }

        /// <summary>
        ///     The target path where the images will be stored
        /// </summary>
        public PathViewModel TargetPathViewModel { get; set; }

        private bool canExport;
        /// <summary>
        ///     Checks if both folder paths have been set
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
                    return Utilities.IsValidPath(TargetPathViewModel.FolderPath) && TargetDynamoVersion != null;
                if (SourcePathViewModel?.FolderPath != null)
                    return Utilities.IsValidPath(TargetPathViewModel.FolderPath) && Utilities.IsValidPath(SourcePathViewModel.FolderPath) && TargetDynamoVersion != null;
                return false;
            }
            private set
            {
                if (canExport != value)
                {
                    canExport = value;
                    RaisePropertyChanged(nameof(CanExport));
                }
            }
        }

        private bool replaceIfNodes = true;
        /// <summary>
        /// Replace if nodes to refactored if
        /// </summary>
        public bool ReplaceIfNodes
        {
            get
            {
                return replaceIfNodes;
            }
            set
            {
                if (replaceIfNodes != value)
                {
                    replaceIfNodes = value;
                    RaisePropertyChanged(nameof(ReplaceIfNodes));
                }
            }
        }

        private bool fixNodeSpacing = true;
        /// <summary>
        /// With Dynamo 2.13 came larger nodes. this spaces them out to fix that.
        /// </summary>
        public bool FixNodeSpacing
        {
            get
            {
                return fixNodeSpacing;
            }
            set
            {
                if (fixNodeSpacing != value)
                {
                    fixNodeSpacing = value;
                    RaisePropertyChanged(nameof(FixNodeSpacing));
                }
            }
        }
        private bool fixInputOrder = false;
        /// <summary>
        /// Dynamo Player can now support input order alphabetically. Historically users created their inputs in order to fix this.
        /// </summary>
        public bool FixInputOrder
        {
            get
            {
                return fixInputOrder;
            }
            set
            {
                if (fixInputOrder != value)
                {
                    fixInputOrder = value;
                    RaisePropertyChanged(nameof(FixInputOrder));
                }
            }
        }

        private bool fixInputLinebreaks = false;
        /// <summary>
        /// Previously users entered line breaks into inputs for directions. This offers a fix with new pinned notes.
        /// </summary>
        public bool FixInputLinebreaks
        {
            get
            {
                return fixInputLinebreaks;
            }
            set
            {
                if (fixInputLinebreaks != value)
                {
                    fixInputLinebreaks = value;
                    RaisePropertyChanged(nameof(FixInputLinebreaks));
                }
            }
        }

        private bool resume = false;
        /// <summary>
        ///     When this flag is set to true, will attempt to resume progress
        ///     based on the log file in the current destination folder
        /// </summary>
        public bool Resume
        {
            get
            {
                return resume;
            }
            set
            {
                if (resume != value)
                {
                    resume = value;
                    RaisePropertyChanged(nameof(Resume));
                    TargetFolderChanged();
                }
            }
        }

        private bool singleGraph = false;
        /// <summary>
        ///     This override allows users to export a single graph
        ///     The current graph will be exported to the Target folder
        /// </summary>
        public bool SingleGraph
        {
            get
            {
                return singleGraph;
            }
            set
            {
                if (singleGraph != value)
                {
                    singleGraph = value;
                    RaisePropertyChanged(nameof(SingleGraph));
                    RaisePropertyChanged(nameof(CanExport));
                }
            }
        }

        private bool isKeepFolderStructure = true;
        /// <summary>
        ///     Contains user preference to retain folder structure for images
        /// </summary>
        public bool IsKeepFolderStructure
        {
            get
            {
                return isKeepFolderStructure;
            }
            set
            {
                if (isKeepFolderStructure != value)
                {
                    isKeepFolderStructure = value;
                    RaisePropertyChanged(nameof(IsKeepFolderStructure));
                }
            }
        }

        private string notificationMessage;
        private Dispatcher dispatcher;
        private Queue<string> graphQueue;
        private bool export = true;

        /// <summary>
        ///     Contains notification text displayed on the UI
        /// </summary>
        public string NotificationMessage
        {
            get
            {
                return notificationMessage;
            }

            set
            {
                if (notificationMessage != value)
                {
                    notificationMessage = value;
                    RaisePropertyChanged(nameof(notificationMessage));
                }
            }
        }

        public DelegateCommand ExportGraphsCommand { get; set; }
        public DelegateCommand CancelCommand { get; set; }

        #endregion

        #region Loading

        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="p"></param>
        public GraphMigrationAssistantViewModel(ViewLoadedParams p)
        {
            if (p == null) return;

            viewLoadedParamsInstance = p;

            p.CurrentWorkspaceChanged += OnCurrentWorkspaceChanged;
            p.CurrentWorkspaceCleared += OnCurrentWorkspaceCleared;

            viewLoadedParamsInstance = p;

            viewLoadedParamsInstance.CurrentWorkspaceChanged += OnCurrentWorkspaceChanged;
            viewLoadedParamsInstance.CurrentWorkspaceCleared += OnCurrentWorkspaceCleared;

            if (viewLoadedParamsInstance.CurrentWorkspaceModel is HomeWorkspaceModel)
            {
                CurrentWorkspace = viewLoadedParamsInstance.CurrentWorkspaceModel as HomeWorkspaceModel;
                DynamoViewModel = viewLoadedParamsInstance.DynamoWindow.DataContext as DynamoViewModel;
                dispatcher = viewLoadedParamsInstance.DynamoWindow.Dispatcher;
                scheduler = DynamoViewModel.Model.Scheduler;
            }

            //if(DisablePrompts) DynamoViewModel.Model.DisablePrompts = true;

            TargetPathViewModel = new PathViewModel
            { Type = PathType.Target, Owner = viewLoadedParamsInstance.DynamoWindow };
            SourcePathViewModel = new PathViewModel
            { Type = PathType.Source, Owner = viewLoadedParamsInstance.DynamoWindow };

            dispatcher.Hooks.DispatcherInactive += OnDispatcherFinished;

            TargetPathViewModel.PropertyChanged += SourcePathPropertyChanged;
            SourcePathViewModel.PropertyChanged += SourcePathPropertyChanged;

            ExportGraphsCommand = new DelegateCommand(ExportGraphs);
            CancelCommand = new DelegateCommand(Cancel);

            graphQueue = new Queue<string>();

            sb = new StringBuilder();

            LoadTargetDynamoVersions();
        }

        private void

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
        private void SourceFolderChanged(PathViewModel pathVM)
        {
            Graphs = new ObservableCollection<GraphViewModel>();

            graphDictionary = new Dictionary<int, GraphViewModel>();

            var files = Utilities.GetAllFilesOfExtension(pathVM.FolderPath)?.OrderBy(x => x);
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
        }

        private void TargetFolderChanged()
        {
            var log = GetLogFileInformation();

            // Do not enqueue the file if it is already in the log file
            if (resume && log != null)
            {
                Graphs.RemoveAll(x => log.Contains(x.UniqueName));
                graphDictionary = Graphs.ToDictionary(x => x.UniqueName.GetHashCode(), x => x);
            }

            if (Graphs == null) return;

            NotificationMessage = String.Format(Properties.Resources.NotificationMsg, Graphs.Count.ToString());
            RaisePropertyChanged(nameof(Graphs));
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

            CurrentWorkspace.EvaluationCompleted += CurrentWorkspaceOnEvaluationCompleted;
        }

        private void CurrentWorkspaceOnEvaluationCompleted(object sender, EvaluationCompletedEventArgs e)
        {
            CurrentWorkspace.EvaluationCompleted -= CurrentWorkspaceOnEvaluationCompleted;
        }

        #endregion

        #region Methods
        /// <summary>
        /// Load the potential Dynamo versions to migrate to. This allows us to preselect the suggestion migration tasks.
        /// </summary>
        private void LoadTargetDynamoVersions()
        {
            //Load our target versions from json TODO: make this read extra folder
            TargetDynamoVersions = new ObservableCollection<TargetDynamoVersion>()
            {
                new TargetDynamoVersion("2.5", "Revit", "2021", false, false, true),
                new TargetDynamoVersion("2.6", "Revit", "2021.1", false, false, true),
                new TargetDynamoVersion("2.10", "Revit", "2022", false, false, true),
                new TargetDynamoVersion("2.12", "Revit", "2022.1", false, true, true),
                new TargetDynamoVersion("2.13", "Revit", "2023", true, true, true),
                new TargetDynamoVersion("2.16", "Revit", "2023.1", true, true, true),
                new TargetDynamoVersion("2.17", "Revit", " 2024", true, true, true),
                new TargetDynamoVersion("2.10", "Civil3D", "2022", false, false, true),
                new TargetDynamoVersion("2.13", "Civil3D", "2023", true, true, true),
                new TargetDynamoVersion("2.17", "Civil3D", " 2024", true, true, true),
            };


            ////pick the potential versions from what our graphs mostly consist of
            //var whatProductIsUsedMost = graphs.GroupBy(g => g.Product).OrderByDescending(g => g.Count()).FirstOrDefault()?.Key;

            //if (whatProductIsUsedMost is null)
            //{
            //    TargetVersions = PotentialTargetVersions;
            //}
            //else
            //{
            //    TargetVersions = new ObservableCollection<TargetDynamoVersion>(PotentialTargetVersions.Where(p => p.Host.Equals(whatProductIsUsedMost)));
            //}
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

            foreach (var file in Graphs.Where(g => !g.InTargetVersion).ToList().Select(x => x.UniqueName).ToList())
            {
                graphQueue.Enqueue(file);
            }

            //copy the ones already in the path
            foreach (var fileInTarget in Graphs.Where(g => g.InTargetVersion))
            {
                var graphName = GetDynPath(fileInTarget.UniqueName);

                File.Copy(fileInTarget.UniqueName,graphName,true);
                
                fileInTarget.Exported = true;

                //log the changes
                EnterLog(string.Format(Properties.Resources.FileCopiedLogMessage, fileInTarget.GraphName));

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
            if (locked || graphQueue.Count > 0)
            {
                if (scheduler.HasPendingTasks) return;

                switch (phase)
                {
                    case (MigrationPhase.Open):
                        locked = true;
                        OpenGraph(graphQueue.Dequeue());
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

            if (graphQueue.Count == 0 && !finished && !locked)
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

            var ifNodesToReplace = DynamoViewModel.CurrentSpaceViewModel.Nodes.Where(n =>
                n.NodeModel.GetType().FullName.Equals("CoreNodeModels.Logic.If")).ToList();

            foreach (var nodeViewModel in ifNodesToReplace)
            {
                List<DynamoModel.MakeConnectionCommand> connectionCommands = new List<DynamoModel.MakeConnectionCommand>();

                var inports = nodeViewModel.NodeModel.InPorts.ToList();
                var outports = nodeViewModel.NodeModel.OutPorts.ToList();

                //zoom to node
                DynamoViewModel.CurrentSpaceViewModel.FocusNodeCommand.Execute(nodeViewModel.NodeModel.GUID);

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

        //TODO: tie this to settings
        private double xScaleFactor = 1.5;
        private double yScaleFactor = 2.25;
        private void FixNodeSpacingForGraph()
        {
            int nodesMovedCount = 0;

            foreach (var nodeViewModel in DynamoViewModel.CurrentSpaceViewModel.Nodes)
            {
                //zoom to node
                DynamoViewModel.CurrentSpaceViewModel.FocusNodeCommand.Execute(nodeViewModel.NodeModel.GUID);

                var originalX = nodeViewModel.X;
                var originalY = nodeViewModel.Y;

                nodeViewModel.X = originalX * xScaleFactor;
                nodeViewModel.Y = originalY * yScaleFactor;

                nodeViewModel.NodeModel.ReportPosition();

                nodesMovedCount++;
            }

            foreach (var noteViewModel in DynamoViewModel.CurrentSpaceViewModel.Notes)
            {
                //zoom to node
                DynamoViewModel.CurrentSpaceViewModel.FocusNodeCommand.Execute(noteViewModel.Model.GUID);

                var originalX = noteViewModel.Left;
                var originalY = noteViewModel.Top;

                noteViewModel.Left = originalX * xScaleFactor;
                noteViewModel.Top = originalY * yScaleFactor;

                noteViewModel.Model.ReportPosition();

                nodesMovedCount++;
            }

            //log the changes
            EnterLog(string.Format(Properties.Resources.NodesMovedLogMessage, nodesMovedCount));
        }

        //optional fixes
        private void FormatInputOrderForGraph()
        {
            int startNumber = 1;//TODO: move to setttings
            var padding = DynamoViewModel.CurrentSpaceViewModel.Nodes.Count.ToString().Length + 1;

            foreach (var nodeViewModel in DynamoViewModel.CurrentSpaceViewModel.Nodes.OrderBy(n => n.Y))
            {
                if (nodeViewModel.IsSetAsInput)
                {
                    string currentName = nodeViewModel.Name;
                    var next = startNumber++.ToString().PadLeft(padding, '0');
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
                   var bustedUpName =  nodeViewModel.NodeModel.Name.Split(
                        new string[] { Environment.NewLine },
                        StringSplitOptions.None);
                    if(bustedUpName.Length < 2) { continue;}

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

            if (isKeepFolderStructure)
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

            MessageBoxService.Show(owner, successMessage, Properties.Resources.FinishMsgTitle, MessageBoxButton.OK, MessageBoxImage.Information);
        }


        private void CleanUp(string image)
        {
            if (File.Exists(image))
                File.Delete(image);
        }

        // TODO: Should we bubble errors to Dynamo?
        private void OpenDynamoGraph(string path)
        {
            try
            {
                DynamoViewModel.OpenCommand.Execute(path);
            }
            catch (Exception)
            {
            }
        }

        private void Cancel(object obj)
        {
            graphQueue.Clear();
            Reset();
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
            TargetPathViewModel.PropertyChanged -= SourcePathPropertyChanged;
            SourcePathViewModel.PropertyChanged -= SourcePathPropertyChanged;
            dispatcher.Hooks.DispatcherInactive -= OnDispatcherFinished;

            CurrentWorkspace = null;
            DynamoViewModel = null;
            dispatcher = null;
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

        /// <summary>
        /// Removes all items matching condition
        /// Source: https://stackoverflow.com/questions/5118513/removeall-for-observablecollections
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="coll"></param>
        /// <param name="condition"></param>
        /// <returns></returns>
        private static int RemoveAll<T>(ObservableCollection<T> coll, Func<T, bool> condition)
        {
            var itemsToRemove = coll.Where(condition).ToList();

            foreach (var itemToRemove in itemsToRemove)
            {
                coll.Remove(itemToRemove);
            }

            return itemsToRemove.Count;
        }

        #endregion
    }
}