using System;
using System.Drawing.Printing;
using System.IO;
using System.Xml.Serialization;

namespace DynamoGraphMigrationAssistant
{
    public class MigrationSettings
    {
        [XmlElement("ScaleFactorX")]
        public double ScaleFactorX { get; set; } = 1.5;

        [XmlElement("ScaleFactorY")]
        public double ScaleFactorY { get; set; } = 2.25;

        [XmlElement("InputOrderAsNumbers")]
        public bool InputOrderAsNumbers { get; set; } = true;

        [XmlElement("InputOrderStartNumber")]
        public int InputOrderStartNumber { get; set; } = 0;

        public static void SerializeModels(string filename, MigrationSettings settings)
        {
            var xmls = new XmlSerializer(settings.GetType());
            var writer = new StreamWriter(filename);
            xmls.Serialize(writer, settings);
            writer.Close();
        }
        public static MigrationSettings DeserializeModels(string filename)
        {
            try
            {
                //load our settings
                var extensionDirectory = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location)?.Replace("bin", "extra");
                var settingsFile = Path.Combine(extensionDirectory, "MigrationSettings.xml");

                //if the settings file exists, use it, if not load with default
                if (File.Exists(settingsFile))
                {
                    var fs = new FileStream(filename, FileMode.Open);
                    var xmls = new XmlSerializer(typeof(MigrationSettings));
                    return (MigrationSettings)xmls.Deserialize(fs);
                }

                return new MigrationSettings();
            }
            catch (Exception)
            {
                return new MigrationSettings();
            }

        }
    }
}
