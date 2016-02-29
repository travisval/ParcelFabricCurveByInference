using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ParcelFabricCurveByInference;

namespace ParcelFabricCurveByInferenceTest
{
    [TestClass]
    public class Development
    {
        #region Junction

        //tests confirming a radius with the use of a junction perpendicular line
        [TestMethod]
        public void Junction_Comfirmed()
        {
            CurveByInference result = Framework.RunFeatureClassTest("TangentDevTests", "CenterPoint", "TangentLines", "Objectid = 114");

            List<InferredCurve> expectedResults = new List<InferredCurve>() { 
                new InferredCurve(114, "test", new List<RelatedCurve>() {
                    new RelatedCurve(113, 707.106758, 3, CurveByInference.RelativeOrientation.From_To)     }){
                    InferredRadius = 707.106758, InferredCenterpointID = 3, 
                    ParallelCurves = new List<RelatedCurve>() {
                    },
                    TangentLines = new List<RelatedLine>() {
                        new RelatedLine(121, -134.999994271838, 89.9590509563687, CurveByInference.RelativeOrientation.From_To)
                    }}};

            Framework.AssertInferredCurvesAreEqual(expectedResults, result.Curves);
        }
        [TestMethod]
        public void Junction_Comfirmed_Reverse()
        {
            CurveByInference result = Framework.RunFeatureClassTest("TangentDevTests", "CenterPoint", "TangentLines", "Objectid = 116");

            List<InferredCurve> expectedResults = new List<InferredCurve>() { 
                 new InferredCurve(116, "test", new List<RelatedCurve>() {
                      new RelatedCurve(113, -707.106758, 3, CurveByInference.RelativeOrientation.To_To)     }){
                      InferredRadius = -707.106758, InferredCenterpointID = 3, 
                      ParallelCurves = new List<RelatedCurve>() {
                      },
                      TangentLines = new List<RelatedLine>() {
                           new RelatedLine(121, 45.0000057281621, 89.9549587045525, CurveByInference.RelativeOrientation.To_To)
                      }}};

            Framework.AssertInferredCurvesAreEqual(expectedResults, result.Curves);
        }

        [TestMethod]
        public void Junction_Reject()
        {
            CurveByInference result = Framework.RunFeatureClassTest("TangentDevTests", "CenterPoint", "TangentLines", "Objectid = 125");

            List<InferredCurve> expectedResults = new List<InferredCurve>() { };

            Framework.AssertInferredCurvesAreEqual(expectedResults, result.Curves);
        }
        [TestMethod]
        public void Junction_Reject_Reverse()
        {
            CurveByInference result = Framework.RunFeatureClassTest("TangentDevTests", "CenterPoint", "TangentLines", "Objectid = 128");

            List<InferredCurve> expectedResults = new List<InferredCurve>() { };

            Framework.AssertInferredCurvesAreEqual(expectedResults, result.Curves);
        }

        #endregion

        #region Curve with Tangent

        //tests confirming a radius with straight line tanget (running perfectly east - west)
        [TestMethod]
        public void Tangent_Confirmer_Flat()
        {
            CurveByInference result = Framework.RunFeatureClassTest("TangentDevTests", "CenterPoint", "TangentLines", "Objectid = 9");

            List<InferredCurve> expectedResults = new List<InferredCurve>() { 
             new InferredCurve(9, "test", new List<RelatedCurve>() {
                  new RelatedCurve(5, 818.826249, 1, CurveByInference.RelativeOrientation.To_From)     }){
                  InferredRadius = 818.826249, InferredCenterpointID = 1,
                  ParallelCurves = new List<RelatedCurve>() {
                  },
                  TangentLines = new List<RelatedLine>() {
                       new RelatedLine(7, 180, 0, CurveByInference.RelativeOrientation.From_To)
            }}};

            Framework.AssertInferredCurvesAreEqual(expectedResults, result.Curves);
        }
        [TestMethod]
        public void Tangent_Confirmer_Flat_Reverse()
        {
            CurveByInference result = Framework.RunFeatureClassTest("TangentDevTests", "CenterPoint", "TangentLines", "Objectid = 26");

            List<InferredCurve> expectedResults = new List<InferredCurve>() { 
                 new InferredCurve(26, "test", new List<RelatedCurve>() {
                      new RelatedCurve(5, -818.826249, 1, CurveByInference.RelativeOrientation.From_From)     }){
                      InferredRadius = -818.826249, InferredCenterpointID = 1,  
                      ParallelCurves = new List<RelatedCurve>() {
                      },
                      TangentLines = new List<RelatedLine>() {
                            new RelatedLine(7, 0, 0, CurveByInference.RelativeOrientation.To_To)
                      }}};

            Framework.AssertInferredCurvesAreEqual(expectedResults, result.Curves);
        }

        //tests confirming a radius with straight line tanget
        [TestMethod]
        public void Tangent_Confirmer_Angle()
        {
            CurveByInference result = Framework.RunFeatureClassTest("TangentDevTests", "CenterPoint", "TangentLines", "Objectid = 16");

            List<InferredCurve> expectedResults = new List<InferredCurve>() { 
                 new InferredCurve(16, "test", new List<RelatedCurve>() {
                      new RelatedCurve(15, 1023.030777, 1, CurveByInference.RelativeOrientation.From_To)     }){
                      InferredRadius = 1023.030777, InferredCenterpointID = 1,  
                      ParallelCurves = new List<RelatedCurve>() {
                      },
                      TangentLines = new List<RelatedLine>() {
                           new RelatedLine(14, -64.2418875563753, 0.0262830909297305, CurveByInference.RelativeOrientation.To_To)
                      }}};

            Framework.AssertInferredCurvesAreEqual(expectedResults, result.Curves);
        }
        [TestMethod]
        public void Tangent_Confirmer_Angle_Reverse()
        {
            CurveByInference result = Framework.RunFeatureClassTest("TangentDevTests", "CenterPoint", "TangentLines", "Objectid = 25");

            List<InferredCurve> expectedResults = new List<InferredCurve>() { 
                 new InferredCurve(25, "test", new List<RelatedCurve>() {
                      new RelatedCurve(15, -1023.030777, 1, CurveByInference.RelativeOrientation.To_To)     }){
                      InferredRadius = -1023.030777, InferredCenterpointID = 1,  
                      ParallelCurves = new List<RelatedCurve>() {
                      },
                      TangentLines = new List<RelatedLine>() {
                           new RelatedLine(14, 115.758112443625, 0.0262830909435974, CurveByInference.RelativeOrientation.From_To)
                      }}};

            Framework.AssertInferredCurvesAreEqual(expectedResults, result.Curves);
        }

        //tests confirming a radius with straight line tanget (running perfectly north - south)
        [TestMethod]
        public void Tangent_Confirmer_Vertical()
        {
            CurveByInference result = Framework.RunFeatureClassTest("TangentDevTests", "CenterPoint", "TangentLines", "Objectid = 23");

            List<InferredCurve> expectedResults = new List<InferredCurve>() { 
                new InferredCurve(23, "test", new List<RelatedCurve>() {
                    new RelatedCurve(20, 752.060666, 1, CurveByInference.RelativeOrientation.From_To)     }){
                      InferredRadius = 752.060666, InferredCenterpointID = 1,  
                    ParallelCurves = new List<RelatedCurve>() {
                    },
                    TangentLines = new List<RelatedLine>() {
                        new RelatedLine(22, 90, 0, CurveByInference.RelativeOrientation.To_From)
                    }}};

            Framework.AssertInferredCurvesAreEqual(expectedResults, result.Curves);
        }
        [TestMethod]
        public void Tangent_Confirmer_Vertical_Reverse()
        {
            CurveByInference result = Framework.RunFeatureClassTest("TangentDevTests", "CenterPoint", "TangentLines", "Objectid = 24");

            List<InferredCurve> expectedResults = new List<InferredCurve>() { 
                 new InferredCurve(24, "test", new List<RelatedCurve>() {
                      new RelatedCurve(20, -752.060666, 1, CurveByInference.RelativeOrientation.To_To)     }){
                      InferredRadius = -752.060666, InferredCenterpointID = 1,  
                      ParallelCurves = new List<RelatedCurve>() {
                      },
                      TangentLines = new List<RelatedLine>() {
                           new RelatedLine(22, -90, 0, CurveByInference.RelativeOrientation.From_From)
                      }}};

            Framework.AssertInferredCurvesAreEqual(expectedResults, result.Curves);
        }


        [TestMethod]
        public void Tangent_Confirmer_40()
        {
            CurveByInference result = Framework.RunFeatureClassTest("TangentDevTests", "CenterPoint", "TangentLines", "Objectid = 40");

            List<InferredCurve> expectedResults = new List<InferredCurve>() { 
                 new InferredCurve(40, "test", new List<RelatedCurve>() {
                      new RelatedCurve(39, 707.106758, 3, CurveByInference.RelativeOrientation.From_To)     }){
                      ParallelCurves = new List<RelatedCurve>() {
                      },
                      TangentLines = new List<RelatedLine>() {
                           new RelatedLine(37, -135, 89.9590452282066, CurveByInference.RelativeOrientation.To_To),
                           new RelatedLine(41, 45, 90.0409547717934, CurveByInference.RelativeOrientation.To_From),
                           new RelatedLine(57, 45, 90.0409547717934, CurveByInference.RelativeOrientation.To_To),
                           new RelatedLine(58, -135, 89.9590452282066, CurveByInference.RelativeOrientation.To_From)
                      }}};

            Framework.AssertInferredCurvesAreEqual(expectedResults, result.Curves);

        }
        [TestMethod]
        public void Tangent_Confirmer_40_Reverse()
        {
            CurveByInference result = Framework.RunFeatureClassTest("TangentDevTests", "CenterPoint", "TangentLines", "Objectid = 56");

            List<InferredCurve> expectedResults = new List<InferredCurve>() { 
                new InferredCurve(56, "test", new List<RelatedCurve>() {
                    new RelatedCurve(39, -707.106758, 3, CurveByInference.RelativeOrientation.To_To)     }){
                    ParallelCurves = new List<RelatedCurve>() {
                    },
                    TangentLines = new List<RelatedLine>() {
                        new RelatedLine(37, 45, 89.9549529763904, CurveByInference.RelativeOrientation.From_To),
                        new RelatedLine(41, -135, 90.0450470236096, CurveByInference.RelativeOrientation.From_From),
                        new RelatedLine(57, -135, 90.0450470236096, CurveByInference.RelativeOrientation.From_To),
                        new RelatedLine(58, 45, 89.9549529763904, CurveByInference.RelativeOrientation.From_From)
                    }}};

            Framework.AssertInferredCurvesAreEqual(expectedResults, result.Curves);
        }

        #endregion

        #region parallel curve

        [TestMethod]
        public void Parallel()
        {
            CurveByInference result = Framework.RunFeatureClassTest("TangentDevTests", "CenterPoint", "TangentLines", "Objectid = 132 and Objectid <> 134");

            List<InferredCurve> expectedResults = new List<InferredCurve>() { 
                 new InferredCurve(132, "test", new List<RelatedCurve>() {
                      new RelatedCurve(131, 707.106758, 6, CurveByInference.RelativeOrientation.From_To)     }){
                      InferredRadius = 707.106758, InferredCenterpointID = 6, 
                      ParallelCurves = new List<RelatedCurve>() {
                            new RelatedCurve(141, 707.10653754906, 6, CurveByInference.RelativeOrientation.Parallel)          },
                      TangentLines = new List<RelatedLine>() {

                      }}};

            Framework.AssertInferredCurvesAreEqual(expectedResults, result.Curves);
        }

        [TestMethod]
        public void Parallel_withPositiveBlocker()
        {
            CurveByInference result = Framework.RunFeatureClassTest("TangentDevTests", "CenterPoint", "TangentLines", "Objectid = 143");

            List<InferredCurve> expectedResults = new List<InferredCurve>() { 
                new InferredCurve(143, "test", new List<RelatedCurve>() {
                    new RelatedCurve(142, 707.106758, 7, CurveByInference.RelativeOrientation.From_To)     }){
                    ParallelCurves = new List<RelatedCurve>() {
                    },
                    TangentLines = new List<RelatedLine>() {
                        new RelatedLine(144, 45, 90.0409547336785, CurveByInference.RelativeOrientation.To_From),
                        new RelatedLine(145, 45, 90.0409547336785, CurveByInference.RelativeOrientation.To_To)
                    }}};

            Framework.AssertInferredCurvesAreEqual(expectedResults, result.Curves);
        }

        #endregion
    }
}
