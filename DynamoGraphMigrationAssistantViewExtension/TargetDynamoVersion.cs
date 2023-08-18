using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using Dynamo.Core;
using Dynamo.Utilities;
using Newtonsoft.Json;

namespace DynamoGraphMigrationAssistant
{
    public class TargetDynamoVersion : NotificationObject
    {
        public string DisplayName => $"{Version} / {Host} {HostVersion}";
        public string Version { get; set; }
        public string VersionForFile { get; set; }
        public string Host { get; set; }
        public string HostVersion { get; set; }
        public bool NodeSpacingSuggested { get; set; }
        public bool IfNodeSuggested { get; set; }
        public bool PythonSuggested { get; set; }

        private static readonly string _extensionDirectory = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location)?.Replace("bin", "extra");
        private static readonly string __versionDictionary = Path.Combine(_extensionDirectory, "VersionDictionary.json");

        public TargetDynamoVersion(string version, string host, string hostVersion, bool nodeSpacingSuggested, bool ifNodeSuggested, bool pythonSuggested)
        {
            Version = version;
            VersionForFile = $"{version}.0";
            Host = host;
            HostVersion = hostVersion;
            NodeSpacingSuggested = nodeSpacingSuggested;
            IfNodeSuggested = ifNodeSuggested;
            PythonSuggested = pythonSuggested;
        }


        public static ObservableCollection<TargetDynamoVersion> LoadPotentialVersions(string host)
        {
            try
            {
                //if the settings file exists, use it, if not load with default
                if (File.Exists(__versionDictionary))
                {
                    var jsonString = File.ReadAllText(__versionDictionary);
                    var versionList =  JsonConvert.DeserializeObject<List<TargetDynamoVersion>>(jsonString);

                    if (string.IsNullOrWhiteSpace(host))
                    {
                        return DefaultVersions();
                    }
                    return versionList.Where(v => v.Host.Equals(host)).ToObservableCollection();
                }

                return DefaultVersions();
            }
            catch (Exception)
            {
                return DefaultVersions();
            }
        }

        public static  ObservableCollection<TargetDynamoVersion> DefaultVersions()
        {
            return new ObservableCollection<TargetDynamoVersion>()
            {
                new TargetDynamoVersion("2.5", "Revit", "2021", false, false, true),
                new TargetDynamoVersion("2.6", "Revit", "2021.1", false, false, true),
                new TargetDynamoVersion("2.10", "Revit", "2022", false, false, true),
                new TargetDynamoVersion("2.12", "Revit", "2022.1", false, true, true),
                new TargetDynamoVersion("2.13", "Revit", "2023", true, true, true),
                new TargetDynamoVersion("2.16", "Revit", "2023.1", true, true, true),
                new TargetDynamoVersion("2.17", "Revit", " 2024", true, true, true),
                new TargetDynamoVersion("2.18", "Revit", " 2024.1", true, true, true),
                new TargetDynamoVersion("2.10", "Civil3D", "2022", false, false, true),
                new TargetDynamoVersion("2.13", "Civil3D", "2023", true, true, true),
                new TargetDynamoVersion("2.17", "Civil3D", " 2024", true, true, true),
            };
        }
    }
}
