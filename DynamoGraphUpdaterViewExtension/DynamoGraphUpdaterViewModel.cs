using System;
using Dynamo.Core;
using Dynamo.Graph.Workspaces;
using Dynamo.ViewModels;
using Dynamo.Wpf.Extensions;

namespace DynamoGraphUpdaterViewExtension
{
    public class DynamoGraphUpdaterViewModel : NotificationObject, IDisposable
    {
        #region Fields and Properties
        private readonly ViewLoadedParams _viewLoadedParamsInstance;
        internal DynamoViewModel DynamoViewModel;
        internal HomeWorkspaceModel CurrentWorkspace;
        #endregion

        public DynamoGraphUpdaterViewModel(ViewLoadedParams p)
        {
            if (p == null) return;
            _viewLoadedParamsInstance = p;
        }
        public void Dispose()
        {
            CurrentWorkspace = null;
            DynamoViewModel = null;
        }
    }
}
