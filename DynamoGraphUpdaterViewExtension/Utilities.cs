using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TreeView;

namespace DynamoGraphUpdater
{
    public class Utilities
    {
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
        public static Version GetDynamoVersionForGraph(string graphPath)
        {
            var jsonText = File.ReadAllText(graphPath);
            JObject o = JObject.Parse(jsonText);
            string versionString = (string)o.SelectToken("View.Dynamo.Version");
            Version version = Version.Parse(versionString);

            return version;
        }
    }
}
