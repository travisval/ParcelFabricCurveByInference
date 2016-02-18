using ESRI.ArcGIS.Geodatabase;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ParcelFabricCurveByInference;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ESRI.ArcGIS.GeoDatabaseExtensions;

namespace ParcelFabricCurveByInferenceTest
{
    [TestClass]
    public  class Framework
    {
        static bool AOInitilialized = false;
        static ESRI.ArcGIS.esriSystem.IAoInitialize aoInitialize;

        static string baseGeodatabasePath;
        static string baseDevGeodatabasePath;
        static IWorkspace workspace;
       
        [AssemblyInitialize()]
        public static void AssemblyInit(TestContext context)
        {
            baseGeodatabasePath = System.IO.Path.GetFullPath(System.IO.Path.Combine(context.TestDir, "..\\..\\UnitTestData.gdb"));
            baseDevGeodatabasePath = System.IO.Path.GetFullPath(System.IO.Path.Combine(context.TestDir, "..\\..\\UnitTestDatasets.gdb"));
            if (!AOInitilialized)
            {
                ESRI.ArcGIS.RuntimeManager.Bind(ESRI.ArcGIS.ProductCode.Desktop);

                aoInitialize = new ESRI.ArcGIS.esriSystem.AoInitialize();
                
                var available = aoInitialize.IsProductCodeAvailable(ESRI.ArcGIS.esriSystem.esriLicenseProductCode.esriLicenseProductCodeAdvanced);

                if(available != ESRI.ArcGIS.esriSystem.esriLicenseStatus.esriLicenseAvailable)
                    throw new Exception("Advanced License is not available.");

                var status = aoInitialize.Initialize(ESRI.ArcGIS.esriSystem.esriLicenseProductCode.esriLicenseProductCodeAdvanced);

                if (status != ESRI.ArcGIS.esriSystem.esriLicenseStatus.esriLicenseCheckedOut)
                    throw new Exception("Failure to acquire Advanced License.");
            }
        }
        [AssemblyCleanup()]
        public static void AssemblyCleanup()
        {
            //if(aoInitialize != null)
            //    aoInitialize.Shutdown();
        }

        public static CurveByInference RunTest(String FeatureDatasetName, String CadastralFabricName, string whereClause = null, string OrderBy = null, String GDBPath = null)
        {
            if (String.IsNullOrEmpty(GDBPath))
                GDBPath = baseGeodatabasePath;

            if (!System.IO.Directory.Exists(GDBPath))
                Assert.Inconclusive(String.Format("The path '{0}' can not be found", GDBPath));

            //Load the geodatabase
            IWorkspaceFactory workspaceFactory = new ESRI.ArcGIS.DataSourcesGDB.FileGDBWorkspaceFactory();
            workspace = workspaceFactory.OpenFromFile(GDBPath, 0);

            IFeatureWorkspace featureWorksapce = (IFeatureWorkspace)workspace;
            IFeatureDataset featureDataset = featureWorksapce.OpenFeatureDataset(FeatureDatasetName);
            IFeatureDatasetExtensionContainer featureDatasetExtContainer = (IFeatureDatasetExtensionContainer)featureDataset;
            IDatasetContainer2 datasetContainer = (IDatasetContainer2)featureDatasetExtContainer.FindExtension(esriDatasetType.esriDTCadastralFabric);
            ICadastralFabric cadFabric = (ICadastralFabric)datasetContainer.get_DatasetByName(esriDatasetType.esriDTCadastralFabric, CadastralFabricName);

            IFeatureClass featureClass = (IFeatureClass)cadFabric.get_CadastralTable(esriCadastralFabricTable.esriCFTLines);

            int idxParcelIDFld = featureClass.Fields.FindField("ParcelID");
            int idxCENTERPTID = featureClass.Fields.FindField("CenterPointID");
            int idxRADIUS = featureClass.Fields.FindField("Radius");
            
            CurveByInference curveByInference = new CurveByInference() { messageBox = new MyMessageBox() };
            curveByInference.FindCurves("test", featureClass, null, whereClause, idxRADIUS, idxCENTERPTID, idxParcelIDFld, new myProgessor());

            Console.WriteLine(Framework.GenerateConstructorStatment(curveByInference.Curves));
            return curveByInference;
        }

        public static CurveByInference RunFeatureClassTest(String featureDataset, String CenterpointFC, String LineFC, string whereClause = null, string OrderBy = null, String GDBPath = null)
        {
            if (String.IsNullOrEmpty(GDBPath))
                GDBPath = baseDevGeodatabasePath;

            if (!System.IO.Directory.Exists(GDBPath))
                Assert.Inconclusive(String.Format("The path '{0}' can not be found", GDBPath));

            //Load the geodatabase
            IWorkspaceFactory workspaceFactory = new ESRI.ArcGIS.DataSourcesGDB.FileGDBWorkspaceFactory();
            workspace = workspaceFactory.OpenFromFile(GDBPath, 0);

            IFeatureWorkspace featureWorksapce = (IFeatureWorkspace)workspace;

            IFeatureClassContainer featuredataset = (IFeatureClassContainer)featureWorksapce.OpenFeatureDataset(featureDataset);
            
            IFeatureClass featureClass = featuredataset.get_ClassByName(LineFC);
            
            int idxParcelIDFld = featureClass.Fields.FindField("ParcelID");
            int idxCENTERPTID = featureClass.Fields.FindField("CenterPointID");
            int idxRADIUS = featureClass.Fields.FindField("Radius");
            
            CurveByInference curveByInference = new CurveByInference() { messageBox = new MyMessageBox() };
            curveByInference.FindCurves("test", featureClass, null, whereClause, idxRADIUS, idxCENTERPTID, idxParcelIDFld, new myProgessor());

            Console.WriteLine(Framework.GenerateConstructorStatment(curveByInference.Curves));
            return curveByInference;
        }

        [TestMethod]
        public void TestEqual()
        {
            List<InferredCurve> expectedResults = new List<InferredCurve>() { 
                        new InferredCurve(5, "test", new List<RelatedCurve> {
                                    new RelatedCurve(16, 10, 1, CurveByInference.RelativeOrientation.From_From),
                                    new RelatedCurve(10, 10, 1, CurveByInference.RelativeOrientation.From_From)
                                })};

            List<InferredCurve> results = new List<InferredCurve>() { 
                        new InferredCurve(5, "test", new List<RelatedCurve> {
                                    new RelatedCurve(16, 10, 1, CurveByInference.RelativeOrientation.From_From),
                                    new RelatedCurve(10, 10, 1, CurveByInference.RelativeOrientation.From_From)
                                })};

            Framework.AssertInferredCurvesAreEqual(expectedResults, results);
        }
        [TestMethod]
        public void TestEqivalent()
        {
            List<InferredCurve> expectedResults = new List<InferredCurve>() { 
                        new InferredCurve(5, "test", new List<RelatedCurve> {
                                    new RelatedCurve(10, 10, 1, CurveByInference.RelativeOrientation.From_From),
                                    new RelatedCurve(16, 10, 1, CurveByInference.RelativeOrientation.From_From)
                                })};

            List<InferredCurve> results = new List<InferredCurve>() { 
                        new InferredCurve(5, "test", new List<RelatedCurve> {
                                    new RelatedCurve(16, 10, 1, CurveByInference.RelativeOrientation.From_From),
                                    new RelatedCurve(10, 10, 1, CurveByInference.RelativeOrientation.From_From)
                                })};

            Framework.AssertInferredCurvesAreEqual(expectedResults, results);
        }
        [TestMethod]
        [ExpectedException(typeof(AssertFailedException), "CollectionAssert.AreEqual failed. (Element at index 0 do not match.)")]
        public void TestOIDDiff()
        {
            List<InferredCurve> expectedResults = new List<InferredCurve>() { 
                        new InferredCurve(15, "test", new List<RelatedCurve> {
                                    new RelatedCurve(10, 10, 1, CurveByInference.RelativeOrientation.From_From),
                                    new RelatedCurve(16, 10, 1, CurveByInference.RelativeOrientation.From_From)
                                })};

            List<InferredCurve> results = new List<InferredCurve>() { 
                        new InferredCurve(5, "test", new List<RelatedCurve> {
                                    new RelatedCurve(16, 10, 1, CurveByInference.RelativeOrientation.From_From),
                                    new RelatedCurve(10, 10, 1, CurveByInference.RelativeOrientation.From_From)
                                })};

            Framework.AssertInferredCurvesAreEqual(expectedResults, results);
        }
        [TestMethod]
        [ExpectedException(typeof(AssertFailedException), "CollectionAssert.AreEqual failed. (Element at index 0 do not match.)")]
        public void TestRelatedCurveCountDiff()
        {
            List<InferredCurve> expectedResults = new List<InferredCurve>() { 
                        new InferredCurve(5, "test", new List<RelatedCurve> {
                                    new RelatedCurve(10, 10, 1, CurveByInference.RelativeOrientation.From_From)
                                })};

            List<InferredCurve> results = new List<InferredCurve>() { 
                        new InferredCurve(5, "test", new List<RelatedCurve> {
                                    new RelatedCurve(16, 10, 1, CurveByInference.RelativeOrientation.From_From),
                                    new RelatedCurve(10, 10, 1, CurveByInference.RelativeOrientation.From_From)
                                })};

            Framework.AssertInferredCurvesAreEqual(expectedResults, results);
        }
        [TestMethod]
        [ExpectedException(typeof(AssertFailedException), "CollectionAssert.AreEqual failed. (Element at index 0 do not match.)")]
        public void TestRelatedOIDCountDiff()
        {
            List<InferredCurve> expectedResults = new List<InferredCurve>() { 
                        new InferredCurve(5, "test", new List<RelatedCurve> {
                                    new RelatedCurve(6, 10, 1, CurveByInference.RelativeOrientation.From_From),
                                    new RelatedCurve(10, 10, 1, CurveByInference.RelativeOrientation.From_From)
                                })};

            List<InferredCurve> results = new List<InferredCurve>() { 
                        new InferredCurve(5, "test", new List<RelatedCurve> {
                                    new RelatedCurve(16, 10, 1, CurveByInference.RelativeOrientation.From_From),
                                    new RelatedCurve(10, 10, 1, CurveByInference.RelativeOrientation.From_From)
                                })};

            Framework.AssertInferredCurvesAreEqual(expectedResults, results);
        }
        [TestMethod]
        [ExpectedException(typeof(AssertFailedException), "CollectionAssert.AreEqual failed. (Element at index 0 do not match.)")]
        public void TestRelatedCurveRadDiff()
        {
            List<InferredCurve> expectedResults = new List<InferredCurve>() { 
                        new InferredCurve(5, "test", new List<RelatedCurve> {
                                    new RelatedCurve(16, 10, 1, CurveByInference.RelativeOrientation.From_From),
                                    new RelatedCurve(10, 10, 1, CurveByInference.RelativeOrientation.From_From)
                                })};

            List<InferredCurve> results = new List<InferredCurve>() { 
                        new InferredCurve(5, "test", new List<RelatedCurve> {
                                    new RelatedCurve(16, 10, 1, CurveByInference.RelativeOrientation.From_From),
                                    new RelatedCurve(10, 9.5, 1, CurveByInference.RelativeOrientation.From_From)
                                })};

            Framework.AssertInferredCurvesAreEqual(expectedResults, results);
        }
        [TestMethod]
        public void TestRelatedCurveSlightRadDiff()
        {
            List<InferredCurve> expectedResults = new List<InferredCurve>() { 
                        new InferredCurve(5, "test", new List<RelatedCurve> {
                                    new RelatedCurve(16, 10, 1, CurveByInference.RelativeOrientation.From_From),
                                    new RelatedCurve(10, 10, 1, CurveByInference.RelativeOrientation.From_From)
                                })};

            List<InferredCurve> results = new List<InferredCurve>() { 
                        new InferredCurve(5, "test", new List<RelatedCurve> { 
                                    new RelatedCurve(16, 10.000001, 1, CurveByInference.RelativeOrientation.From_From),
                                    new RelatedCurve(10, 10, 1, CurveByInference.RelativeOrientation.From_From)
                                })};

            Framework.AssertInferredCurvesAreEqual(expectedResults, results);
        }
        [TestMethod]
        [ExpectedException(typeof(AssertFailedException), "CollectionAssert.AreEqual failed. (Element at index 0 do not match.)")]
        public void TestRelatedCurveCentridIDDiff()
        {
            List<InferredCurve> expectedResults = new List<InferredCurve>() { 
                        new InferredCurve(5, "test", new List<RelatedCurve> {
                                    new RelatedCurve(6, 10, 2, CurveByInference.RelativeOrientation.From_From),
                                    new RelatedCurve(10, 10, 1, CurveByInference.RelativeOrientation.From_From)
                                })};

            List<InferredCurve> results = new List<InferredCurve>() { 
                        new InferredCurve(5, "test", new List<RelatedCurve> { 
                                    new RelatedCurve(16, 10, 1, CurveByInference.RelativeOrientation.From_From),
                                    new RelatedCurve(10, 10, 1, CurveByInference.RelativeOrientation.From_From)
                                })};

            Framework.AssertInferredCurvesAreEqual(expectedResults, results);
        }

        [TestMethod]
        [ExpectedException(typeof(AssertFailedException), "CollectionAssert.AreEqual failed. (Element at index 0 do not match.)")]
        public void TestRelatedCurveOrientationDiff()
        {
            List<InferredCurve> expectedResults = new List<InferredCurve>() { 
                        new InferredCurve(5, "test", new List<RelatedCurve> {
                                    new RelatedCurve(6, 10, 2, CurveByInference.RelativeOrientation.To_To),
                                    new RelatedCurve(10, 10, 1, CurveByInference.RelativeOrientation.From_From)
                                })};

            List<InferredCurve> results = new List<InferredCurve>() { 
                        new InferredCurve(5, "test", new List<RelatedCurve> { 
                                    new RelatedCurve(16, 10, 1, CurveByInference.RelativeOrientation.From_From),
                                    new RelatedCurve(10, 10, 1, CurveByInference.RelativeOrientation.From_From)
                                })};

            Framework.AssertInferredCurvesAreEqual(expectedResults, results);
        }


        [TestMethod]
        public void TestRelatedMultipleDiff()
        {
            List<InferredCurve> expectedResults = new List<InferredCurve>() { 
                new InferredCurve(5, "test", new List<RelatedCurve> {
                                    new RelatedCurve(6, 10, 2, CurveByInference.RelativeOrientation.From_From),
                                    new RelatedCurve(7, 10, 1, CurveByInference.RelativeOrientation.From_From)
                                }),    
                new InferredCurve(15, "test", new List<RelatedCurve> {
                                    new RelatedCurve(8, 10, 2, CurveByInference.RelativeOrientation.From_From),
                                    new RelatedCurve(9, 10, 1, CurveByInference.RelativeOrientation.From_From)
                                })};

            List<InferredCurve> results = new List<InferredCurve>() { 
                new InferredCurve(5, "test", new List<RelatedCurve> {
                                    new RelatedCurve(6, 10, 2, CurveByInference.RelativeOrientation.From_From),
                                    new RelatedCurve(7, 10, 1, CurveByInference.RelativeOrientation.From_From)
                                }),        
                new InferredCurve(15, "test", new List<RelatedCurve> {
                                    new RelatedCurve(8, 10, 2, CurveByInference.RelativeOrientation.From_From),
                                    new RelatedCurve(9, 10, 1, CurveByInference.RelativeOrientation.From_From)
                                })};

            Framework.AssertInferredCurvesAreEqual(expectedResults, results);
        }
        [TestMethod]
        [ExpectedException(typeof(AssertFailedException), "CollectionAssert.AreEqual failed. (Element at index 0 do not match.)")]
        public void TestRelatedMultipleDiff2()
        {
            List<InferredCurve> expectedResults = new List<InferredCurve>() { 
                new InferredCurve(5, "test", new List<RelatedCurve> {
                                    new RelatedCurve(6, 10, 2, CurveByInference.RelativeOrientation.From_From),
                                    new RelatedCurve(7, 10, 1, CurveByInference.RelativeOrientation.From_From)
                                }),        
                new InferredCurve(15, "test", new List<RelatedCurve> {
                                    new RelatedCurve(8, 10, 2, CurveByInference.RelativeOrientation.From_From),
                                    new RelatedCurve(9, 10, 1, CurveByInference.RelativeOrientation.From_From)
                                })};

            List<InferredCurve> results = new List<InferredCurve>() { 
                new InferredCurve(15, "test", new List<RelatedCurve> {
                                    new RelatedCurve(6, 10, 2, CurveByInference.RelativeOrientation.From_From),
                                    new RelatedCurve(7, 10, 1, CurveByInference.RelativeOrientation.From_From)
                                }),        
                new InferredCurve(5, "test", new List<RelatedCurve> { 
                                    new RelatedCurve(8, 10, 2, CurveByInference.RelativeOrientation.From_From),
                                    new RelatedCurve(9, 10, 1, CurveByInference.RelativeOrientation.From_From)
                                })};

            Framework.AssertInferredCurvesAreEqual(expectedResults, results);
        }
        [TestMethod]
        [ExpectedException(typeof(AssertFailedException), "CollectionAssert.AreEqual failed. (Element at index 0 do not match.)")]
        public void TestRelatedMultipleDiff3()
        {
            List<InferredCurve> expectedResults = new List<InferredCurve>() { 
                new InferredCurve(5, "test", new List<RelatedCurve> { 
                                    new RelatedCurve(6, 10, 2, CurveByInference.RelativeOrientation.From_From),
                                    new RelatedCurve(7, 10, 1, CurveByInference.RelativeOrientation.From_From)
                                }),       
                new InferredCurve(15, "test", new List<RelatedCurve> { 
                                    new RelatedCurve(8, 10, 2, CurveByInference.RelativeOrientation.From_From),
                                    new RelatedCurve(9, 10, 1, CurveByInference.RelativeOrientation.From_From)
                                })};

            List<InferredCurve> results = new List<InferredCurve>() { 
                new InferredCurve(6, "test", new List<RelatedCurve> { 
                                    new RelatedCurve(6, 10, 2, CurveByInference.RelativeOrientation.From_From),
                                    new RelatedCurve(7, 10, 1, CurveByInference.RelativeOrientation.From_From)
                                }),        
                new InferredCurve(15, "test", new List<RelatedCurve> { 
                                    new RelatedCurve(8, 10, 2, CurveByInference.RelativeOrientation.From_From),
                                    new RelatedCurve(9, 10, 1, CurveByInference.RelativeOrientation.From_From)
                                })};

            Framework.AssertInferredCurvesAreEqual(expectedResults, results);
        }
        [TestMethod]
        [ExpectedException(typeof(AssertFailedException), "CollectionAssert.AreEqual failed. (Element at index 1 do not match.)")]
        public void TestRelatedMultipleDiff4()
        {
            List<InferredCurve> expectedResults = new List<InferredCurve>() { 
                new InferredCurve(5, "test", new List<RelatedCurve> { 
                                    new RelatedCurve(6, 10, 2, CurveByInference.RelativeOrientation.From_From),
                                    new RelatedCurve(7, 10, 1, CurveByInference.RelativeOrientation.From_From)
                                }),        
                new InferredCurve(15, "test", new List<RelatedCurve> { 
                                    new RelatedCurve(8, 10, 2, CurveByInference.RelativeOrientation.From_From),
                                    new RelatedCurve(9, 10, 1, CurveByInference.RelativeOrientation.From_From)
                                })
                        };

            List<InferredCurve> results = new List<InferredCurve>() { 
                new InferredCurve(5, "test", new List<RelatedCurve> { 
                                    new RelatedCurve(6, 10, 2, CurveByInference.RelativeOrientation.From_From),
                                    new RelatedCurve(7, 10, 1, CurveByInference.RelativeOrientation.From_From)
                                }),        
                new InferredCurve(16, "test", new List<RelatedCurve> { 
                                    new RelatedCurve(8, 10, 2, CurveByInference.RelativeOrientation.From_From),
                                    new RelatedCurve(9, 10, 1, CurveByInference.RelativeOrientation.From_From)
                                })
                        };

            Framework.AssertInferredCurvesAreEqual(expectedResults, results);
        }
        [TestMethod]
        [ExpectedException(typeof(AssertFailedException), "CollectionAssert.AreEqual failed. (Element at index 1 do not match.)")]
        public void TestRelatedMultipleDiff5()
        {
            List<InferredCurve> expectedResults = new List<InferredCurve>() { 
                new InferredCurve(5, "test", new List<RelatedCurve> { 
                                    new RelatedCurve(6, 10, 2, CurveByInference.RelativeOrientation.From_From),
                                    new RelatedCurve(7, 10, 1, CurveByInference.RelativeOrientation.From_From)
                                }),        
                new InferredCurve(15, "test", new List<RelatedCurve> { 
                                    new RelatedCurve(8, 10, 2, CurveByInference.RelativeOrientation.From_From),
                                    new RelatedCurve(9, 10, 1, CurveByInference.RelativeOrientation.From_From)
                                })
                        };

            List<InferredCurve> results = new List<InferredCurve>() { 
                new InferredCurve(5, "test", new List<RelatedCurve> { 
                                    new RelatedCurve(6, 10, 2, CurveByInference.RelativeOrientation.From_From),
                                    new RelatedCurve(7, 10, 1, CurveByInference.RelativeOrientation.From_From)
                                }),        
                new InferredCurve(15, "test", new List<RelatedCurve> { 
                                    new RelatedCurve(18, 10, 2, CurveByInference.RelativeOrientation.From_From),
                                    new RelatedCurve(9, 10, 1, CurveByInference.RelativeOrientation.From_From)
                                })
                        };

            Framework.AssertInferredCurvesAreEqual(expectedResults, results);
        }


        public static void AssertInferredCurvesAreEqual(System.Collections.ICollection expected, System.Collections.ICollection result)
        {
            Assert.AreEqual(expected.Count, result.Count, "List lengths should be equal");

            CollectionAssert.AreEqual(expected, result);
        }

        internal static string GenerateConstructorStatment(IEnumerable<InferredCurve> result)
        {
            StringBuilder strBuilder = new StringBuilder();

            strBuilder.Append("List<InferredCurve> expectedResults = new List<InferredCurve>() { " + Environment.NewLine);
            strBuilder.Append(String.Join(",\n", (from c in result select GenerateConstructorStatment_Curve(c)).ToArray()));
            strBuilder.Append("};");
            return strBuilder.ToString();
        }

        static string CreateCurve        =  "     new InferredCurve({0}, \"test\", new List<RelatedCurve>() {{" + Environment.NewLine;
        static string CreateRelatedCurve =  "          new RelatedCurve({0}, {1}, {2}, CurveByInference.RelativeOrientation.{3})";
        static string CreateCurveClose =    "     }){" + Environment.NewLine;
        static string AcceptedCreate =      "          InferredRadius = {0}, InferredCenterpointID = {1}, " + Environment.NewLine;
        static string ParallelCreate =      "          ParallelCurves = new List<RelatedCurve>() {" + Environment.NewLine;
        static string CreateCurveItem =     "                new RelatedCurve({0}, {1}, {2}, CurveByInference.RelativeOrientation.{3})";
        static string ParallelCreateClose = "          }," + Environment.NewLine;
        static string LineCreate =          "          TangentLines = new List<RelatedLine>() {" + Environment.NewLine;
        static string CreateLineItem =      "               new RelatedLine({0}, {1}, {2}, CurveByInference.RelativeOrientation.{3})";
        static string LineCreateClose =     "          }}";

        static string ListJoin =            "," + Environment.NewLine;

        private static object GenerateConstructorStatment_Curve(InferredCurve curve)
        {
            StringBuilder strBuilder = new StringBuilder();
            strBuilder.Append(String.Format(CreateCurve, curve.ObjectID));
            strBuilder.Append(String.Join(ListJoin, (from r in curve.TangentCurves select String.Format(CreateRelatedCurve, r.ObjectID, r.Radius, r.CenterpointID, r.Orientation)).ToArray()));
            strBuilder.Append(CreateCurveClose);

            if(curve.HasValue)
                strBuilder.AppendFormat(AcceptedCreate, curve.InferredRadius, curve.InferredCenterpointID);

            strBuilder.Append(ParallelCreate);
            strBuilder.Append(String.Join(ListJoin, (from r in curve.ParallelCurves select String.Format(CreateCurveItem, r.ObjectID, r.Radius, r.CenterpointID, r.Orientation)).ToArray()));
            strBuilder.Append(ParallelCreateClose);
            strBuilder.Append(LineCreate);
            strBuilder.Append(String.Join(ListJoin, (from r in curve.TangentLines select String.Format(CreateLineItem, r.ObjectID, r.Angle, r.DeltaAngle, r.Orientation)).ToArray()));
            strBuilder.Append(Environment.NewLine + LineCreateClose);

            return strBuilder.ToString();
        }

        public static double ToRadians(double degrees)
        {
            return degrees / (180 / Math.PI);
        }
        public static double toDegrees(double radians)
        {
            return radians * (180 / Math.PI);
        }
    }
}
