using System;
using System.IO;
using Dynamo.Core;

namespace DynamoGraphMigrationAssistant.Controls
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


        public int PythonNodesChangedCount { get; set; } = 0;
        public int IfNodesChangedCount { get; set; } = 0;
        public int NodeSpaceChangedCount { get; set; } = 0;

        public string GraphRevisionStatus =>
            $"{NodeSpaceChangedCount} nodes resized. {PythonNodesChangedCount} Python nodes migrated. {IfNodesChangedCount} nodes updated.";

        public FileInfo DynamoGraph { get; set; }
        public string Name => DynamoGraph.Name;
        public string FullName => DynamoGraph.FullName;
        public string GraphJson { get; set; }

        public UpdateableGraphViewModel(Version currentVersion, string dynamoGraph)
        {
            CurrentVersion = currentVersion;
            TruncatedVersion = new Version(currentVersion.Major,currentVersion.Minor);
            DynamoGraph = new FileInfo(dynamoGraph);
            CalculateDynamoProduct();
            Update = true;

            GraphJson = File.ReadAllText(dynamoGraph);
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
