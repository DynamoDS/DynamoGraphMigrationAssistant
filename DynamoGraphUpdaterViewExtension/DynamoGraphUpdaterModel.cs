using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Dynamo.Graph.Workspaces;
using Dynamo.ViewModels;
using Dynamo.Wpf.Extensions;
using DynamoGraphUpdater.Controls;

namespace DynamoGraphUpdater
{
    public class DynamoGraphUpdaterModel
    {
        private readonly ViewLoadedParams _viewLoadedParamsInstance;
        internal DynamoViewModel DynamoViewModel;
        internal HomeWorkspaceModel CurrentWorkspace;
        public DynamoGraphUpdaterModel(ViewLoadedParams viewLoadedParams)
        {
            _viewLoadedParamsInstance = viewLoadedParams;
            if (_viewLoadedParamsInstance.CurrentWorkspaceModel is HomeWorkspaceModel homeWorkspaceModel)
            {
                if (homeWorkspaceModel.Nodes.Any())
                {
                    CurrentWorkspace = homeWorkspaceModel;
                }
               
            }

            DynamoViewModel = _viewLoadedParamsInstance.DynamoWindow.DataContext as DynamoViewModel;
        }

        internal void UpgradeGraphs(ObservableCollection<UpdateableGraphsViewModel> updateableGraphs)
        {
            foreach (var updateableGraphSet in updateableGraphs)
            {
                //iterate through individual graphs
                foreach (var updateableGraph in updateableGraphSet.UpdateableGraphs)
                {
                    if (updateableGraphSet.IfNodes)
                    {
                        ReplaceIfNodes(updateableGraph);
                    }

                    if (updateableGraphSet.PythonFixes)
                    {
                        MigratePython(updateableGraph);
                    }

                    if (updateableGraphSet.NodeSpacing)
                    {
                        FixNodeSpacing(updateableGraph);
                    }

                    //finally save it

                }
                
            }
        }


        internal void ReplaceIfNodes(UpdateableGraphViewModel updateableGraph)
        {
            string originalIfNodeName = "CoreNodeModels.Logic.If";
            string refactoredIfNodeName = "CoreNodeModels.Logic.RefactoredIf";

            updateableGraph.IfNodesChangedCount = Regex.Matches(updateableGraph.GraphJson, originalIfNodeName).Count;

            updateableGraph.GraphJson = updateableGraph.GraphJson.Replace(originalIfNodeName, refactoredIfNodeName);
        }

        internal void MigratePython(UpdateableGraphViewModel updateableGraph)
        {
            //not implemented yet
        }

        internal void FixNodeSpacing(UpdateableGraphViewModel updateableGraph)
        {
            //not implemented yet
        }

        /// <summary>
        /// Build source path for graph location
        /// </summary>
        /// <returns></returns>
        internal PathViewModel SourcePathViewModel()
        {
            return new PathViewModel
                { Type = PathType.Source, Owner = _viewLoadedParamsInstance.DynamoWindow };
        }
        /// <summary>
        /// Build target path for graph location
        /// </summary>
        /// <returns></returns>
        internal PathViewModel TargetPathViewModel()
        {
            return new PathViewModel
                { Type = PathType.Target, Owner = _viewLoadedParamsInstance.DynamoWindow };
        }
    }
}
