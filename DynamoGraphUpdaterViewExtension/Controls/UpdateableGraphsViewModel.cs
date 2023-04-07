using System;
using System.Collections.Generic;
using Dynamo.Core;

namespace DynamoGraphUpdater.Controls
{
    public class UpdateableGraphsViewModel : NotificationObject
    {
        public Version CurrentVersion { get; set; }
        public string TruncatedVersion { get; set; }
        public Version TargetVersion { get; set; }
        List<UpdateableGraphViewModel> UpdateableGraphs { get; set; }
        public bool PythonFixes { get; set; }
        public bool IfNodes { get; set; }
        public bool NodeSpacing { get; set; }

        public UpdateableGraphsViewModel(string truncatedVersion, List<UpdateableGraphViewModel> updatableGraphs)
        {
            TruncatedVersion = truncatedVersion;
            UpdateableGraphs = updatableGraphs;

            PythonFixes = true;
            IfNodes = true;
            NodeSpacing = true;
        }
    }
}
