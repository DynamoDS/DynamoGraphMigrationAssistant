using System.IO;
using Dynamo.Core;

namespace DynamoGraphMigrationAssistant.Controls
{
    public class GraphViewModel : NotificationObject
    {
        private bool exported;
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

        public string UniqueName { get; set; }

        public string Product => CalculateDynamoProduct();

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