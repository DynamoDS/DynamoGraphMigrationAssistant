using System.IO;
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

            DynamoViewModel = _viewLoadedParamsInstance.DynamoWindow.DataContext as DynamoViewModel;
        }

        internal string ReplaceIfNodes(UpdateableGraphViewModel updateableGraph)
        {
            return string.Empty;
        }

        internal string MigratePython(UpdateableGraphViewModel updateableGraph)
        {
            return string.Empty;
        }

        internal string FixNodeSpacing(UpdateableGraphViewModel updateableGraph)
        {
            return string.Empty;
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
    }
}
