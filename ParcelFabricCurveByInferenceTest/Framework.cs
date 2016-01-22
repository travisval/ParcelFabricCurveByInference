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
        static IWorkspace workspace;
       
        [AssemblyInitialize()]
        public static void AssemblyInit(TestContext context)
        {
            baseGeodatabasePath = System.IO.Path.GetFullPath(System.IO.Path.Combine(context.TestDir, "..\\..\\UnitTestData.gdb"));

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

        public static CurveByInference RunTest(String FeatureDatasetName, String CadastralFabricName, string OrderBy = null, String GDBPath = null)
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

            HashSet<int> parcelHash = new HashSet<int>();

            CurveByInference curveByInference = new CurveByInference() { messageBox = new MyMessageBox() };
            curveByInference.FindCurves("test", featureClass, null, null, parcelHash, idxRADIUS, idxCENTERPTID, idxParcelIDFld, new myProgessor());

            return curveByInference;
        }
        public static void AssertInferredCurvesAreEqual(System.Collections.ICollection expected, System.Collections.ICollection result)
        {
            Assert.AreEqual(expected.Count, result.Count, "List lengths should be equal");

            CollectionAssert.AreEqual(expected, result);
        }

        [TestMethod]
        public void TestEqual()
        {
            List<InferredCurve> expectedResults = new List<InferredCurve>() { 
                        new InferredCurve(5, "test", new List<RelatedCurve> {
                                    new RelatedCurve(16, 10, 1, RelationTypes.Tangent),
                                    new RelatedCurve(10, 10, 1, RelationTypes.Tangent)
                                })};

            List<InferredCurve> results = new List<InferredCurve>() { 
                        new InferredCurve(5, "test", new List<RelatedCurve> {
                                    new RelatedCurve(16, 10, 1, RelationTypes.Tangent),
                                    new RelatedCurve(10, 10, 1, RelationTypes.Tangent)
                                })};

            Framework.AssertInferredCurvesAreEqual(expectedResults, results);
        }
        [TestMethod]
        public void TestEqivalent()
        {
            List<InferredCurve> expectedResults = new List<InferredCurve>() { 
                        new InferredCurve(5, "test", new List<RelatedCurve> {
                                    new RelatedCurve(10, 10, 1, RelationTypes.Tangent),
                                    new RelatedCurve(16, 10, 1, RelationTypes.Tangent)
                                })};

            List<InferredCurve> results = new List<InferredCurve>() { 
                        new InferredCurve(5, "test", new List<RelatedCurve> {
                                    new RelatedCurve(16, 10, 1, RelationTypes.Tangent),
                                    new RelatedCurve(10, 10, 1, RelationTypes.Tangent)
                                })};

            Framework.AssertInferredCurvesAreEqual(expectedResults, results);
        }
        [TestMethod]
        [ExpectedException(typeof(AssertFailedException), "CollectionAssert.AreEqual failed. (Element at index 0 do not match.)")]
        public void TestOIDDiff()
        {
            List<InferredCurve> expectedResults = new List<InferredCurve>() { 
                        new InferredCurve(15, "test", new List<RelatedCurve> {
                                    new RelatedCurve(10, 10, 1, RelationTypes.Tangent),
                                    new RelatedCurve(16, 10, 1, RelationTypes.Tangent)
                                })};

            List<InferredCurve> results = new List<InferredCurve>() { 
                        new InferredCurve(5, "test", new List<RelatedCurve> {
                                    new RelatedCurve(16, 10, 1, RelationTypes.Tangent),
                                    new RelatedCurve(10, 10, 1, RelationTypes.Tangent)
                                })};

            Framework.AssertInferredCurvesAreEqual(expectedResults, results);
        }
        [TestMethod]
        [ExpectedException(typeof(AssertFailedException), "CollectionAssert.AreEqual failed. (Element at index 0 do not match.)")]
        public void TestRelatedCurveCountDiff()
        {
            List<InferredCurve> expectedResults = new List<InferredCurve>() { 
                        new InferredCurve(5, "test", new List<RelatedCurve> {
                                    new RelatedCurve(10, 10, 1, RelationTypes.Tangent)
                                })};

            List<InferredCurve> results = new List<InferredCurve>() { 
                        new InferredCurve(5, "test", new List<RelatedCurve> {
                                    new RelatedCurve(16, 10, 1, RelationTypes.Tangent),
                                    new RelatedCurve(10, 10, 1, RelationTypes.Tangent)
                                })};

            Framework.AssertInferredCurvesAreEqual(expectedResults, results);
        }
        [TestMethod]
        [ExpectedException(typeof(AssertFailedException), "CollectionAssert.AreEqual failed. (Element at index 0 do not match.)")]
        public void TestRelatedOIDCountDiff()
        {
            List<InferredCurve> expectedResults = new List<InferredCurve>() { 
                        new InferredCurve(5, "test", new List<RelatedCurve> {
                                    new RelatedCurve(6, 10, 1, RelationTypes.Tangent),
                                    new RelatedCurve(10, 10, 1, RelationTypes.Tangent)
                                })};

            List<InferredCurve> results = new List<InferredCurve>() { 
                        new InferredCurve(5, "test", new List<RelatedCurve> {
                                    new RelatedCurve(16, 10, 1, RelationTypes.Tangent),
                                    new RelatedCurve(10, 10, 1, RelationTypes.Tangent)
                                })};

            Framework.AssertInferredCurvesAreEqual(expectedResults, results);
        }
        [TestMethod]
        [ExpectedException(typeof(AssertFailedException), "CollectionAssert.AreEqual failed. (Element at index 0 do not match.)")]
        public void TestRelatedCurveRadDiff()
        {
            List<InferredCurve> expectedResults = new List<InferredCurve>() { 
                        new InferredCurve(5, "test", new List<RelatedCurve> {
                                    new RelatedCurve(16, 10, 1, RelationTypes.Tangent),
                                    new RelatedCurve(10, 10, 1, RelationTypes.Tangent)
                                })};

            List<InferredCurve> results = new List<InferredCurve>() { 
                        new InferredCurve(5, "test", new List<RelatedCurve> {
                                    new RelatedCurve(16, 10, 1, RelationTypes.Tangent),
                                    new RelatedCurve(10, 9.5, 1, RelationTypes.Tangent)
                                })};

            Framework.AssertInferredCurvesAreEqual(expectedResults, results);
        }
        [TestMethod]
        public void TestRelatedCurveSlightRadDiff()
        {
            List<InferredCurve> expectedResults = new List<InferredCurve>() { 
                        new InferredCurve(5, "test", new List<RelatedCurve> {
                                    new RelatedCurve(16, 10, 1, RelationTypes.Tangent),
                                    new RelatedCurve(10, 10, 1, RelationTypes.Tangent)
                                })};

            List<InferredCurve> results = new List<InferredCurve>() { 
                        new InferredCurve(5, "test", new List<RelatedCurve> { 
                                    new RelatedCurve(16, 10.000001, 1, RelationTypes.Tangent),
                                    new RelatedCurve(10, 10, 1, RelationTypes.Tangent)
                                })};

            Framework.AssertInferredCurvesAreEqual(expectedResults, results);
        }
        [TestMethod]
        [ExpectedException(typeof(AssertFailedException), "CollectionAssert.AreEqual failed. (Element at index 0 do not match.)")]
        public void TestRelatedCurveCentridIDDiff()
        {
            List<InferredCurve> expectedResults = new List<InferredCurve>() { 
                        new InferredCurve(5, "test", new List<RelatedCurve> {
                                    new RelatedCurve(6, 10, 2, RelationTypes.Tangent),
                                    new RelatedCurve(10, 10, 1, RelationTypes.Tangent)
                                })};

            List<InferredCurve> results = new List<InferredCurve>() { 
                        new InferredCurve(5, "test", new List<RelatedCurve> { 
                                    new RelatedCurve(16, 10, 1, RelationTypes.Tangent),
                                    new RelatedCurve(10, 10, 1, RelationTypes.Tangent)
                                })};

            Framework.AssertInferredCurvesAreEqual(expectedResults, results);
        }

        [TestMethod]
        public void TestRelatedMultipleDiff()
        {
            List<InferredCurve> expectedResults = new List<InferredCurve>() { 
                new InferredCurve(5, "test", new List<RelatedCurve> {
                                    new RelatedCurve(6, 10, 2, RelationTypes.Tangent),
                                    new RelatedCurve(7, 10, 1, RelationTypes.Tangent)
                                }),    
                new InferredCurve(15, "test", new List<RelatedCurve> {
                                    new RelatedCurve(8, 10, 2, RelationTypes.Tangent),
                                    new RelatedCurve(9, 10, 1, RelationTypes.Tangent)
                                })};

            List<InferredCurve> results = new List<InferredCurve>() { 
                new InferredCurve(5, "test", new List<RelatedCurve> {
                                    new RelatedCurve(6, 10, 2, RelationTypes.Tangent),
                                    new RelatedCurve(7, 10, 1, RelationTypes.Tangent)
                                }),        
                new InferredCurve(15, "test", new List<RelatedCurve> {
                                    new RelatedCurve(8, 10, 2, RelationTypes.Tangent),
                                    new RelatedCurve(9, 10, 1, RelationTypes.Tangent)
                                })};

            Framework.AssertInferredCurvesAreEqual(expectedResults, results);
        }
        [TestMethod]
        [ExpectedException(typeof(AssertFailedException), "CollectionAssert.AreEqual failed. (Element at index 0 do not match.)")]
        public void TestRelatedMultipleDiff2()
        {
            List<InferredCurve> expectedResults = new List<InferredCurve>() { 
                new InferredCurve(5, "test", new List<RelatedCurve> {
                                    new RelatedCurve(6, 10, 2, RelationTypes.Tangent),
                                    new RelatedCurve(7, 10, 1, RelationTypes.Tangent)
                                }),        
                new InferredCurve(15, "test", new List<RelatedCurve> {
                                    new RelatedCurve(8, 10, 2, RelationTypes.Tangent),
                                    new RelatedCurve(9, 10, 1, RelationTypes.Tangent)
                                })};

            List<InferredCurve> results = new List<InferredCurve>() { 
                new InferredCurve(15, "test", new List<RelatedCurve> {
                                    new RelatedCurve(6, 10, 2, RelationTypes.Tangent),
                                    new RelatedCurve(7, 10, 1, RelationTypes.Tangent)
                                }),        
                new InferredCurve(5, "test", new List<RelatedCurve> { 
                                    new RelatedCurve(8, 10, 2, RelationTypes.Tangent),
                                    new RelatedCurve(9, 10, 1, RelationTypes.Tangent)
                                })};

            Framework.AssertInferredCurvesAreEqual(expectedResults, results);
        }
        [TestMethod]
        [ExpectedException(typeof(AssertFailedException), "CollectionAssert.AreEqual failed. (Element at index 0 do not match.)")]
        public void TestRelatedMultipleDiff3()
        {
            List<InferredCurve> expectedResults = new List<InferredCurve>() { 
                new InferredCurve(5, "test", new List<RelatedCurve> { 
                                    new RelatedCurve(6, 10, 2, RelationTypes.Tangent),
                                    new RelatedCurve(7, 10, 1, RelationTypes.Tangent)
                                }),       
                new InferredCurve(15, "test", new List<RelatedCurve> { 
                                    new RelatedCurve(8, 10, 2, RelationTypes.Tangent),
                                    new RelatedCurve(9, 10, 1, RelationTypes.Tangent)
                                })};

            List<InferredCurve> results = new List<InferredCurve>() { 
                new InferredCurve(6, "test", new List<RelatedCurve> { 
                                    new RelatedCurve(6, 10, 2, RelationTypes.Tangent),
                                    new RelatedCurve(7, 10, 1, RelationTypes.Tangent)
                                }),        
                new InferredCurve(15, "test", new List<RelatedCurve> { 
                                    new RelatedCurve(8, 10, 2, RelationTypes.Tangent),
                                    new RelatedCurve(9, 10, 1, RelationTypes.Tangent)
                                })};

            Framework.AssertInferredCurvesAreEqual(expectedResults, results);
        }
        [TestMethod]
        [ExpectedException(typeof(AssertFailedException), "CollectionAssert.AreEqual failed. (Element at index 1 do not match.)")]
        public void TestRelatedMultipleDiff4()
        {
            List<InferredCurve> expectedResults = new List<InferredCurve>() { 
                new InferredCurve(5, "test", new List<RelatedCurve> { 
                                    new RelatedCurve(6, 10, 2, RelationTypes.Tangent),
                                    new RelatedCurve(7, 10, 1, RelationTypes.Tangent)
                                }),        
                new InferredCurve(15, "test", new List<RelatedCurve> { 
                                    new RelatedCurve(8, 10, 2, RelationTypes.Tangent),
                                    new RelatedCurve(9, 10, 1, RelationTypes.Tangent)
                                })
                        };

            List<InferredCurve> results = new List<InferredCurve>() { 
                new InferredCurve(5, "test", new List<RelatedCurve> { 
                                    new RelatedCurve(6, 10, 2, RelationTypes.Tangent),
                                    new RelatedCurve(7, 10, 1, RelationTypes.Tangent)
                                }),        
                new InferredCurve(16, "test", new List<RelatedCurve> { 
                                    new RelatedCurve(8, 10, 2, RelationTypes.Tangent),
                                    new RelatedCurve(9, 10, 1, RelationTypes.Tangent)
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
                                    new RelatedCurve(6, 10, 2, RelationTypes.Tangent),
                                    new RelatedCurve(7, 10, 1, RelationTypes.Tangent)
                                }),        
                new InferredCurve(15, "test", new List<RelatedCurve> { 
                                    new RelatedCurve(8, 10, 2, RelationTypes.Tangent),
                                    new RelatedCurve(9, 10, 1, RelationTypes.Tangent)
                                })
                        };

            List<InferredCurve> results = new List<InferredCurve>() { 
                new InferredCurve(5, "test", new List<RelatedCurve> { 
                                    new RelatedCurve(6, 10, 2, RelationTypes.Tangent),
                                    new RelatedCurve(7, 10, 1, RelationTypes.Tangent)
                                }),        
                new InferredCurve(15, "test", new List<RelatedCurve> { 
                                    new RelatedCurve(18, 10, 2, RelationTypes.Tangent),
                                    new RelatedCurve(9, 10, 1, RelationTypes.Tangent)
                                })
                        };

            Framework.AssertInferredCurvesAreEqual(expectedResults, results);
        }

        internal static string GenerateConstructorStatment(IEnumerable<InferredCurve> result)
        {
            StringBuilder strBuilder = new StringBuilder();

            strBuilder.Append("List<InferredCurve> expectedResults = new List<InferredCurve>() { \n");
            strBuilder.Append(String.Join(",\n", (from c in result select GenerateConstructorStatment_Curve(c)).ToArray()));
            strBuilder.Append("};");
            return strBuilder.ToString();
        }

        private static object GenerateConstructorStatment_Curve(InferredCurve curve)
        {
            StringBuilder strBuilder = new StringBuilder();
            strBuilder.Append(String.Concat("     new InferredCurve(",curve.ObjectID,", \"test\", new List<RelatedCurve>() {"));
            strBuilder.Append(String.Join(",\n", (from r in curve.TangentCurves select String.Format("          new RelatedCurve({0}, {1}, {2}, RelationTypes.Tangent)", r.ObjectID, r.Radius, r.CenterpointID)).ToArray()));
            strBuilder.Append("        })");
                    
            strBuilder.Append("{");
            if(curve.Accepted != null) 
                strBuilder.AppendFormat("Accepted = new RelatedCurve({0}, {1}, {2}, RelationTypes.Tangent), \n", curve.Accepted.ObjectID, curve.Accepted.Radius, curve.Accepted.CenterpointID);
  
            strBuilder.Append(" ParallelCurves = new List<RelatedCurve>() {");
            strBuilder.Append(String.Join(",\n", (from r in curve.ParallelCurves select String.Format("          new RelatedCurve({0}, {1}, {2}, RelationTypes.Tangent)", r.ObjectID, r.Radius, r.CenterpointID)).ToArray()));
            strBuilder.Append("        }}");

            return strBuilder.ToString();
        }
    }
}
