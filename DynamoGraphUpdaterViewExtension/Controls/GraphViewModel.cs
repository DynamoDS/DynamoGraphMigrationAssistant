using System.IO;
using Dynamo.Core;

namespace DynamoGraphMigrationAssistant.Controls
{
    public class GraphViewModel : NotificationObject
    {
        private bool exported;
        private bool inTargetVersion;
        private string graphName;
        private string product;

        /// <summary>
        /// The name of the Graph
        /// </summary>
        public string GraphName
        {
            get => graphName;
            set
            {
                if (value != graphName)
                {
                    graphName = value;
                    RaisePropertyChanged(nameof(GraphName));
                }
            }
        }

        /// <summary>
        /// Shows if the graph has been successfully exported
        /// </summary>
        public bool Exported
        {
            get => exported;
            set
            {
                if (value != exported)
                {
                    exported = value;
                    RaisePropertyChanged(nameof(Exported));
                }
            }
        }

        /// <summary>
        /// Shows if the graph is in the target version
        /// </summary>
        public bool InTargetVersion
        {
            get => inTargetVersion;
            set
            {
                if (value != inTargetVersion)
                {
                    inTargetVersion = value;
                    RaisePropertyChanged(nameof(InTargetVersion));
                }
            }
        }

        public string UniqueName { get; set; }

        public string Product => CalculateDynamoProduct();
        public string Version => Utilities.GetDynamoVersionForGraph(UniqueName);
        public string CalculateDynamoProduct()
        {
            var fileText = File.ReadAllText(UniqueName);

            if (fileText.Contains("RevitNodes.dll"))
            {
                return "Revit";
            }

            if (fileText.Contains("Civil3DNodes.dll"))
            {
               return "Civil3D";
            }

            return string.Empty;
        }
    }
}