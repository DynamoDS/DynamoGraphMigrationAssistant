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

        [XmlElement("InputOrderStartLetter")]
        public string InputOrderStartLetter { get; set; } = "A";

        private static readonly string _extensionDirectory = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location)?.Replace("bin", "extra");
        private static readonly string _settingsFile = Path.Combine(_extensionDirectory, "MigrationSettings.xml");

        public static void SerializeModels(MigrationSettings settings)
        {
            try
            {
                var xmls = new XmlSerializer(settings.GetType());
                var writer = new StreamWriter(_settingsFile);
                xmls.Serialize(writer, settings);
                writer.Close();
            }
            catch (Exception)
            {
              //writing settings failed, continue to use default
            }
         
        }
        public static MigrationSettings DeserializeModels()
        {
            try
            {
                //if the settings file exists, use it, if not load with default
                if (File.Exists(_settingsFile))
                {
                    var fs = new FileStream(_settingsFile, FileMode.Open);
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
