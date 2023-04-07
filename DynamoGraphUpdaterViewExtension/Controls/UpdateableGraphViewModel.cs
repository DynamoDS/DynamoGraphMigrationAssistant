using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Dynamo.Core;
using Dynamo.ViewModels;

namespace DynamoGraphUpdater.Controls
{
    public class UpdateableGraphViewModel : NotificationObject
    {
        public Version CurrentVersion { get; set; }
        public string TruncatedVersion { get; set; }
        public Version TargetVersion { get; set; }
        public bool Update { get; set; }

        public int PythonNodesChangedCount { get; set; }
        public int IfNodesChangedCount { get; set; }
        public int NodeSpaceChangedCount { get; set; }

        public FileInfo DynamoGraph { get; set; }
       
        public UpdateableGraphViewModel(Version currentVersion, string dynamoGraph)
        {
            CurrentVersion = currentVersion;
            TruncatedVersion = $"{currentVersion.Major}.{currentVersion.Minor}.x";
            DynamoGraph = new FileInfo(dynamoGraph);
        }
    }
}
