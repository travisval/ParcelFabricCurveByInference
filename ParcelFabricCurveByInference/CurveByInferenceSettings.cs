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
        /// <summary>
        /// The max difference (in degrees) between a straight line and the inferred curve before the angles are not considered equal
        /// </summary>
        public double MaxTangentLineAngleInDegrees = 0.25;

        /// <summary>
        /// The number of time larger a overlapping straight line must be to be considered
        /// </summary>
        public double TangentOverlapScaleFactor = 3;
        public int PerpendicularTolerance = 15;
        
        public string RadiusFieldName { get; set; }
        public string CenterpointIDFieldName { get; set; }
        public string ParcelIDFieldName { get; set; }
        public string FromPointFieldName { get; set; }
        public string ToPointFieldName { get; set; }
        public string SystemStartDateFieldName { get; set; }

        public string CategoryFieldName { get; set; }
        public string SequenceFieldName { get; set; }
        public string TypeFieldName { get; set; }
        public string HistoricalFieldName { get; set; }
        public string LineParametersFieldName { get; set; }
        public string DensifyTypeName { get; set; }
        public string ArcLengthFieldName { get; set; }
        public string DistanceFieldName { get; set; }

        public string BearingFieldName { get; set; }

        public class FieldPositions
        {
            public int RadiusFieldIdx { get; set; }
            public int CenterpointIDFieldIdx { get; set; }
            public int ParcelIDFieldIdx { get; set; }
            public int FromPointFieldIdx { get; set; }
            public int ToPointFieldIdx { get; set; }
            public int SystemStartDateFieldIdx { get; set; }

            public int CategoryFieldIdx { get; set; }
            public int SequenceFieldIdx { get; set; }
            public int TypeFieldIdx { get; set; }
            public int HistoricalFieldIdx { get; set; }
            public int LineParametersFieldIdx { get; set; }
            public int DensifyTypeIdx { get; set; }
            public int ArcLengthFieldIdx { get; set; }
            public int DistanceFieldIdx { get; set; }

            public int BearingFieldIdx { get; set; }

            public bool ValidCheckFields = true;
            public bool ValidUpdateFields;

            public FieldPositions(ESRI.ArcGIS.Geodatabase.ITable featureClass)
            {
                ValidCheckFields = (RadiusFieldIdx = featureClass.Fields.FindField(CurveByInferenceSettings.Instance.RadiusFieldName)) != -1 && ValidCheckFields;
                ValidCheckFields = (CenterpointIDFieldIdx = featureClass.Fields.FindField(CurveByInferenceSettings.Instance.CenterpointIDFieldName)) != -1 && ValidCheckFields;

                ValidUpdateFields = (ParcelIDFieldIdx = featureClass.Fields.FindField(CurveByInferenceSettings.Instance.ParcelIDFieldName)) != -1 && ValidCheckFields;
                ValidUpdateFields = (FromPointFieldIdx = featureClass.Fields.FindField(CurveByInferenceSettings.Instance.FromPointFieldName)) != -1 && ValidUpdateFields;
                ValidUpdateFields = (ToPointFieldIdx = featureClass.Fields.FindField(CurveByInferenceSettings.Instance.ToPointFieldName)) != -1 && ValidUpdateFields;
                ValidUpdateFields = (SystemStartDateFieldIdx = featureClass.Fields.FindField(CurveByInferenceSettings.Instance.SystemStartDateFieldName)) != -1 && ValidUpdateFields;
                ValidUpdateFields = (CategoryFieldIdx = featureClass.Fields.FindField(CurveByInferenceSettings.Instance.CategoryFieldName)) != -1 && ValidUpdateFields;
                ValidUpdateFields = (SequenceFieldIdx = featureClass.Fields.FindField(CurveByInferenceSettings.Instance.SequenceFieldName)) != -1 && ValidUpdateFields;
                ValidUpdateFields = (TypeFieldIdx = featureClass.Fields.FindField(CurveByInferenceSettings.Instance.TypeFieldName)) != -1 && ValidUpdateFields;
                ValidUpdateFields = (HistoricalFieldIdx = featureClass.Fields.FindField(CurveByInferenceSettings.Instance.HistoricalFieldName)) != -1 && ValidUpdateFields;
                ValidUpdateFields = (LineParametersFieldIdx = featureClass.Fields.FindField(CurveByInferenceSettings.Instance.LineParametersFieldName)) != -1 && ValidUpdateFields;
                ValidUpdateFields = (DensifyTypeIdx = featureClass.Fields.FindField(CurveByInferenceSettings.Instance.DensifyTypeName)) != -1 && ValidUpdateFields;
                ValidUpdateFields = (ArcLengthFieldIdx = featureClass.Fields.FindField(CurveByInferenceSettings.Instance.ArcLengthFieldName)) != -1 && ValidUpdateFields;
                ValidUpdateFields = (DistanceFieldIdx = featureClass.Fields.FindField(CurveByInferenceSettings.Instance.DistanceFieldName)) != -1 && ValidUpdateFields;
                ValidUpdateFields = (BearingFieldIdx = featureClass.Fields.FindField(CurveByInferenceSettings.Instance.BearingFieldName)) != -1 && ValidUpdateFields;
                //ValidUpdateFields = ( Idx = featureClass.Fields.FindField(CurveByInferenceSettings.Instance.Name)) != -1 && ValidUpdateFields;
            }

        }

        private CurveByInferenceSettings()
        {
            DisplaySettings = false;

            AngleToleranceTangentCompareInDegrees = 1.5;
            StraightLinesBreakLessThanInDegrees = 0.033;
            MaximumDeltaInDegrees = 2.5;
            ExcludeTangentsShorterThanScaler = 0.9;
            OrthogonalSearchDistance = 70;

            RadiusFieldName = "Radius";
            CenterpointIDFieldName = "CenterPointID";
            ParcelIDFieldName = "ParcelID";
            FromPointFieldName = "FromPointID";
            ToPointFieldName = "ToPointID";
            SystemStartDateFieldName = "SystemStartDate";

            CategoryFieldName = "Category";
            SequenceFieldName = "Sequence";
            TypeFieldName = "Type";
            HistoricalFieldName = "Historical";
            LineParametersFieldName = "LineParameters";
            DensifyTypeName = "DensifyType";

            DistanceFieldName = "Distance";
            ArcLengthFieldName = "ArcLength";

            BearingFieldName = "Bearing";
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
