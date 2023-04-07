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
        public string Product { get; set; }
        public Version CurrentVersion { get; set; }
        public Version TruncatedVersion { get; set; }
        public TargetDynamoVersion TargetDynamoVersion { get; set; }
        private bool _update;
        public bool Update
        {
            get { return _update; }
            set { _update = value; 
                RaisePropertyChanged(nameof(Update));
            }
        }
        public int PythonNodesChangedCount { get; set; }
        public int IfNodesChangedCount { get; set; }
        public int NodeSpaceChangedCount { get; set; }

        public FileInfo DynamoGraph { get; set; }
        public string Name => DynamoGraph.Name;
        public string FullName => DynamoGraph.FullName;

        public UpdateableGraphViewModel(Version currentVersion, string dynamoGraph)
        {
            CurrentVersion = currentVersion;
            TruncatedVersion = new Version(currentVersion.Major,currentVersion.Minor);
            DynamoGraph = new FileInfo(dynamoGraph);
            CalculateDynamoProduct();
            Update = true;
        }

        public void CalculateDynamoProduct()
        {
            var fileText = File.ReadAllText(DynamoGraph.FullName);


            if (fileText.Contains("RevitNodes.dll"))
            {
                Product = "Revit";
            }

            if (fileText.Contains("Civil3DNodes.dll"))
            {
                Product = "Civil3D";
            }

        }
    }
}
