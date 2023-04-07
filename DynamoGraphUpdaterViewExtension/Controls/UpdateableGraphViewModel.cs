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
        public Version TargetVersion { get; set; }
        public bool Update { get; set; }

        public int PythonNodesChangedCount { get; set; }
        public int IfNodesChangedCount { get; set; }
        public int NodeSpaceChangedCount { get; set; }

        public FileInfo DynamoGraph { get; set; }
        public string Name => DynamoGraph.Name;

        public UpdateableGraphViewModel(Version currentVersion, string dynamoGraph)
        {
            CurrentVersion = currentVersion;
            TruncatedVersion = new Version(currentVersion.Major,currentVersion.Minor);
            DynamoGraph = new FileInfo(dynamoGraph);
            CalculateDynamoProduct();
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
