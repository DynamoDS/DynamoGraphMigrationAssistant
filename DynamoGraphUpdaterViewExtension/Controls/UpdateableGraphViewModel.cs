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
        public UpdateableGraphViewModel() { }
        public Version TargetVersion { get; set; }
        public bool Update { get; set; }
        public bool PythonFixes { get; set; }
        public bool IfNodes { get; set; }
        public bool NodeSpacing { get; set; }

        public List<FileInfo> DynamoGraphs { get; set; }
       
        public UpdateableGraphViewModel(Version currentVersion, List<string> dynamoGraphs)
        {
            CurrentVersion = currentVersion;
            TruncatedVersion = $"{currentVersion.Major}.{currentVersion.Minor}.x";
            PythonFixes = true;
            IfNodes = true;
            NodeSpacing = true;
            DynamoGraphs = dynamoGraphs.Select(f => new FileInfo(f)).ToList();
        }
    }
}
