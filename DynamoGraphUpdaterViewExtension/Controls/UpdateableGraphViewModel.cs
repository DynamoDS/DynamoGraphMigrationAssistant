using System;
using System.Collections.Generic;
using Dynamo.Core;
using Dynamo.ViewModels;

namespace DynamoGraphUpdater.Controls
{
    public class UpdateableGraphViewModel : NotificationObject
    {
        internal Version DynamoVersion { get; set; }
        internal bool PythonFixes { get; set; }
        internal bool IfNodes { get; set; }
        internal bool NodeSpacing { get; set; }

        internal List<string> DynamoGraphs { get; set; }
       

    }
}
