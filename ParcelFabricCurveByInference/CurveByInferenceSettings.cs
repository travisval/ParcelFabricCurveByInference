using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace ParcelFabricCurveByInference
{
    [Serializable]
    public class CurveByInferenceSettings
    {
        private static CurveByInferenceSettings instance;

        [XmlIgnore]
        public System.Windows.Visibility SettingVisibility { get { return DisplaySettings ? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed; } }

        [XmlElement]
        public bool DisplaySettings { get; set; }

        [XmlElement]
        public double AngleToleranceTangentCompareInDegrees { get; set; }

        [XmlElement]
        public double StraightLinesBreakLessThanInDegrees { get; set; }

        [XmlElement]
        public double MaximumDeltaInDegrees { get; set; }

        [XmlElement]
        public double ExcludeTangentsShorterThanScaler { get; set; }

        [XmlElement]
        public double OrthogonalSearchDistance { get; set; }

        public double dLargeAngleBreakInDegrees = 3;
        /// <summary>
        /// The length of the lines that are extended from either end of the query line when looking for tangents
        /// </summary>
        public double TangentQueryLength = 0.2;
        /// <summary>
        /// The size of the buffer applies to the lines that are extended from either end of the query line when looking for tangents
        /// </summary>
        public double TangentQueryBuffer = 0.1;

        private CurveByInferenceSettings()
        {
            DisplaySettings = false;

            AngleToleranceTangentCompareInDegrees = 1.5;
            StraightLinesBreakLessThanInDegrees = 0.033;
            MaximumDeltaInDegrees = 2.5;
            ExcludeTangentsShorterThanScaler = 0.9;
            OrthogonalSearchDistance = 70;
        }

        public static CurveByInferenceSettings Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new CurveByInferenceSettings();
                    try
                    {
                        string codeBase = System.Reflection.Assembly.GetExecutingAssembly().CodeBase;
                        UriBuilder uri = new UriBuilder(codeBase);
                        string path = Uri.UnescapeDataString(uri.Path);
                        path = Path.GetDirectoryName(path);

                        string fileName = Path.Combine(path, "CurveByInterface.xml");

                        if (File.Exists(fileName))
                        {
                            XmlSerializer serializer = new XmlSerializer(typeof(CurveByInferenceSettings));

                            using (StreamReader reader = new StreamReader(fileName))
                            {
                                instance = (CurveByInferenceSettings)serializer.Deserialize(reader);
                            }
                        }
                    }
                    catch (Exception exx)
                    {
                        System.Diagnostics.Debug.WriteLine(exx.Message);
                    }
                }
                return instance;
            }
        }

    }
}
