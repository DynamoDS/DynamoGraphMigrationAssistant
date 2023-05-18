using Dynamo.Graph.Workspaces;
using Dynamo.ViewModels;
using Dynamo.Wpf.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamoGraphMigrationAssistant.Models
{
    public class GraphMigrationAssistantModel
    {
        internal ViewLoadedParams _viewLoadedParamsInstance;
        internal DynamoViewModel DynamoViewModel;
        internal HomeWorkspaceModel CurrentWorkspace;
        public GraphMigrationAssistantModel(ViewLoadedParams viewLoadedParams)
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
        
    }
}
