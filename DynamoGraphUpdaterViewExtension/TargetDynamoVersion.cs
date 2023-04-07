using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dynamo.Core;

namespace DynamoGraphUpdater
{
    public class TargetDynamoVersion : NotificationObject
    {
        public string DisplayName => $"{Version} / {Host} {HostVersion}";
        public string Version { get; set; }
        public string Host { get; set; }
        public string HostVersion { get; set; }
        public bool NodeSpacingSuggested { get; set; }
        public bool IfNodeSuggested { get; set; }
        public bool PythonSuggested { get; set; }

        public TargetDynamoVersion(string version, string host, string hostVersion, bool nodeSpacingSuggested, bool ifNodeSuggested, bool pythonSuggested)
        {
            Version = version;
            Host = host;
            HostVersion = hostVersion;
            NodeSpacingSuggested = nodeSpacingSuggested;
            IfNodeSuggested = ifNodeSuggested;
            PythonSuggested = pythonSuggested;
        }
    }
}
