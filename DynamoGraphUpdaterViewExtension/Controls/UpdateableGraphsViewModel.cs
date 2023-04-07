using System;
using System.Collections.Generic;
using Dynamo.Core;

namespace DynamoGraphUpdater.Controls
{
    public class UpdateableGraphsViewModel : NotificationObject
    {
       
        public Version CurrentVersion { get; set; }
        public Version TruncatedVersion { get; set; }
        public Version TargetVersion { get; set; }
        public List<UpdateableGraphViewModel> UpdateableGraphs { get; set; }
        public bool PythonFixes { get; set; }
        public bool IfNodes { get; set; }
        public bool NodeSpacing { get; set; }
        public bool InputOrder { get; set; }

        public UpdateableGraphsViewModel(Version truncatedVersion, List<UpdateableGraphViewModel> updateableGraphs)
        {
            TruncatedVersion = truncatedVersion;
            UpdateableGraphs = updateableGraphs;

            PythonFixes = true;
            IfNodes = true;
            NodeSpacing = true;
            InputOrder = false;
        }

        
    }
}
