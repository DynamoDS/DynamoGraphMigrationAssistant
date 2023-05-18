using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace DynamoGraphMigrationAssistant
{
    public class Utilities
    {
        /// <summary>
        /// Returns true if both paths exist
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool IsValidPath(string path)
        {
            return Directory.Exists(path);
        }
        /// <summary>
        ///     Returns a list of files of given path and extension
        /// </summary>
        /// <param name="path"></param>
        /// <param name="extension"></param>
        /// <returns></returns>
        public static IEnumerable<string> GetAllFilesOfExtension(string path, string extension = ".dyn")
        {
            var files = Directory.EnumerateFiles(path, "*.*", SearchOption.AllDirectories)
                .Where(x => extension.Equals(Path.GetExtension(x).ToLowerInvariant()));

            return files;
        }
        /// <summary>
        /// Returns the calculated version for the given graph path.
        /// </summary>
        /// <param name="graphPath">The path of the dyn</param>
        /// <returns></returns>
        public static string GetDynamoVersionForGraph(string graphPath)
        {
            var jsonText = File.ReadAllText(graphPath);
            JObject o = JObject.Parse(jsonText);
            string versionString = (string)o.SelectToken("View.Dynamo.Version");
            Version version = Version.Parse(versionString);

            return $"{version.Major}.{version.Minor}";
        }
    }
}
