using System;
using System.Collections.Generic;
using Dynamo.Core;

namespace DynamoGraphUpdater.Controls
{
    public class UpdateableGraphsViewModel : NotificationObject
    {
       
        public Version CurrentVersion { get; set; }
        public Version TruncatedVersion { get; set; }

        private TargetDynamoVersion _targetVersion;
        public TargetDynamoVersion TargetVersion
        {
            get => _targetVersion;
            set { _targetVersion = value;
                PythonFixes = TargetVersion.PythonSuggested;
                IfNodes = TargetVersion.IfNodeSuggested;
                NodeSpacing = TargetVersion.NodeSpacingSuggested;
                CheckFixCriteria();
                RaisePropertyChanged(nameof(TargetVersion));
            }
        }
        public List<UpdateableGraphViewModel> UpdateableGraphs { get; set; }

        //python fixes
        private bool _pythonFixes;
        public bool PythonFixes 
        {
            get => _pythonFixes;
            set
            {
                _pythonFixes = value;
                RaisePropertyChanged(nameof(PythonFixes));
            }
        }
        //if nodes
        private bool _ifNodes;
        public bool IfNodes
        {
            get => _ifNodes;
            set
            {
                _ifNodes = value;
                RaisePropertyChanged(nameof(IfNodes));
            }
        }
        private bool _ifVisibility;
        public bool IfVisibility
        {
            get => _ifVisibility;
            set
            {
                _ifVisibility = value;
                RaisePropertyChanged(nameof(IfVisibility));
            }
        }
        //node spacing
        private bool _nodeSpacing;
        public bool NodeSpacing
        {
            get => _nodeSpacing;
            set
            {
                _nodeSpacing = value;
                RaisePropertyChanged(nameof(NodeSpacing));
            }
        }
        private bool _nodeSpacingVisibility;
        public bool NodeSpacingVisibility
        {
            get => _nodeSpacingVisibility;
            set
            {
                _nodeSpacingVisibility = value;
                RaisePropertyChanged(nameof(NodeSpacingVisibility));
            }
        }

        public bool InputOrder { get; set; }

        public UpdateableGraphsViewModel(Version truncatedVersion, List<UpdateableGraphViewModel> updateableGraphs)
        {
            TruncatedVersion = truncatedVersion;
            UpdateableGraphs = updateableGraphs;
            InputOrder = false;
        }

        private void CheckFixCriteria()
        {
            var ifNodeRefactored = new Version(2, 12);
            var dynamoUiRefresh = new Version(2, 13);
            var targetVersion = new Version(TargetVersion.Version);


            //for node spacing
            NodeSpacingVisibility = targetVersion >= dynamoUiRefresh && TruncatedVersion < targetVersion;

            //for if node
            IfVisibility = targetVersion >= ifNodeRefactored && TruncatedVersion <= targetVersion;

            //for versions already in target
            PythonFixes = targetVersion != TruncatedVersion;
            IfNodes = targetVersion != TruncatedVersion;
            NodeSpacing = targetVersion != TruncatedVersion;
        }
    }
}
