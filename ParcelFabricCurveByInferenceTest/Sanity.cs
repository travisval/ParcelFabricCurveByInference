using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ParcelFabricCurveByInference;

namespace ParcelFabricCurveByInferenceTest
{
    [TestClass]
    public class Sanity
    {
        [TestMethod]
        public void NegativeRadiusTest()
        {
            CurveByInference result = Framework.RunTest("NegativeRadiusTestFD", "NegativeRadiusTest");

            #region expectedData

            List<InferredCurve> expectedResults = new List<InferredCurve>() { 
                 new InferredCurve(356638, "test", new List<RelatedCurve>() {
                      new RelatedCurve(356637, -864.159989779026, 152265, CurveByInference.RelativeOrientation.From_To)     }){
                      ParallelCurves = new List<RelatedCurve>() {
                      },
                      TangentLines = new List<RelatedLine>() {

                      }},
                 new InferredCurve(356650, "test", new List<RelatedCurve>() {
                      new RelatedCurve(356651, 154.773869942494, 152269, CurveByInference.RelativeOrientation.To_From)     }){
                      ParallelCurves = new List<RelatedCurve>() {
                      },
                      TangentLines = new List<RelatedLine>() {

                      }},
                 new InferredCurve(356655, "test", new List<RelatedCurve>() {
                      new RelatedCurve(356654, 196.383390812253, 152272, CurveByInference.RelativeOrientation.From_To)     }){
                      ParallelCurves = new List<RelatedCurve>() {
                      },
                      TangentLines = new List<RelatedLine>() {
                           new RelatedLine(356656, Framework.toDegrees(-0.104993798047734), CurveByInference.RelativeOrientation.To_From)
                      }},
                 new InferredCurve(356663, "test", new List<RelatedCurve>() {
                      new RelatedCurve(356664, -399.413630657088, 152274, CurveByInference.RelativeOrientation.To_From)     }){
                      ParallelCurves = new List<RelatedCurve>() {
                      },
                      TangentLines = new List<RelatedLine>() {

                      }},
                 new InferredCurve(371318, "test", new List<RelatedCurve>() {
                      new RelatedCurve(371319, 1726.12593027848, 158475, CurveByInference.RelativeOrientation.To_From)     }){
                      ParallelCurves = new List<RelatedCurve>() {
                      },
                      TangentLines = new List<RelatedLine>() {
                           new RelatedLine(371317, Framework.toDegrees(3.09543077695016), CurveByInference.RelativeOrientation.From_To)
                      }},
                 new InferredCurve(371322, "test", new List<RelatedCurve>() {
                      new RelatedCurve(371321, -1696.07550002682, 158476, CurveByInference.RelativeOrientation.From_To),
                      new RelatedCurve(376534, -1696.07550002682, 158476, CurveByInference.RelativeOrientation.From_From)     }){
                      ParallelCurves = new List<RelatedCurve>() {
                      },
                      TangentLines = new List<RelatedLine>() {

                      }},
                 new InferredCurve(375549, "test", new List<RelatedCurve>() {
                      new RelatedCurve(375548, -426.287570033533, 160866, CurveByInference.RelativeOrientation.From_To)     }){
                      ParallelCurves = new List<RelatedCurve>() {
                      },
                      TangentLines = new List<RelatedLine>() {
                           new RelatedLine(375550, Framework.toDegrees(-2.65890100353497), CurveByInference.RelativeOrientation.To_From),
                           new RelatedLine(375621, Framework.toDegrees(-2.65890100353497), CurveByInference.RelativeOrientation.To_To),
                           new RelatedLine(375623, Framework.toDegrees(-2.78567756470225), CurveByInference.RelativeOrientation.From_From)
                      }},
                 new InferredCurve(375622, "test", new List<RelatedCurve>() {
                      new RelatedCurve(375548, 426.287570033533, 160866, CurveByInference.RelativeOrientation.To_To)     }){
                      ParallelCurves = new List<RelatedCurve>() {
                      },
                      TangentLines = new List<RelatedLine>() {
                           new RelatedLine(375550, Framework.toDegrees(0.482691650054827), CurveByInference.RelativeOrientation.From_From),
                           new RelatedLine(375621, Framework.toDegrees(0.482691650054828), CurveByInference.RelativeOrientation.From_To),
                           new RelatedLine(375623, Framework.toDegrees(0.355915088887542), CurveByInference.RelativeOrientation.To_From)
                      }},
                 new InferredCurve(376510, "test", new List<RelatedCurve>() {
                      new RelatedCurve(376511, 543.236862578124, 161476, CurveByInference.RelativeOrientation.To_From),
                      new RelatedCurve(378231, 543.236862578124, 161476, CurveByInference.RelativeOrientation.To_To)     }){
                      InferredRadius = 543.236862578124, InferredCenterpointID = 161476, 
                      ParallelCurves = new List<RelatedCurve>() {
                      },
                      TangentLines = new List<RelatedLine>() {
                           new RelatedLine(376509, Framework.toDegrees(0.378301273409633), CurveByInference.RelativeOrientation.From_To),
                           new RelatedLine(378233, Framework.toDegrees(0.378286133425), CurveByInference.RelativeOrientation.From_From)
                      }},
                 new InferredCurve(376521, "test", new List<RelatedCurve>() {
                      new RelatedCurve(376520, 535.045129978397, 161484, CurveByInference.RelativeOrientation.From_To),
                      new RelatedCurve(378222, 535.045129978397, 161484, CurveByInference.RelativeOrientation.From_From)     }){
                      ParallelCurves = new List<RelatedCurve>() {
                      },
                      TangentLines = new List<RelatedLine>() {
                           new RelatedLine(376522, Framework.toDegrees(-0.108157814977166), CurveByInference.RelativeOrientation.To_From),
                           new RelatedLine(378220, Framework.toDegrees(-0.108157814977166), CurveByInference.RelativeOrientation.To_To)
                      }},
                 new InferredCurve(378208, "test", new List<RelatedCurve>() {
                      new RelatedCurve(378207, -449.806979278138, 161797, CurveByInference.RelativeOrientation.From_To),
                      new RelatedCurve(378209, -449.809999037129, 161796, CurveByInference.RelativeOrientation.To_From)     }){
                      ParallelCurves = new List<RelatedCurve>() {
                      },
                      TangentLines = new List<RelatedLine>() {

                      }},
                 new InferredCurve(378212, "test", new List<RelatedCurve>() {
                      new RelatedCurve(378211, -450.132869251069, 161794, CurveByInference.RelativeOrientation.From_To)     }){
                      ParallelCurves = new List<RelatedCurve>() {
                            new RelatedCurve(375548, -459.432762870721, 160866, CurveByInference.RelativeOrientation.Parallel)          },
                      TangentLines = new List<RelatedLine>() {

                      }},
                 new InferredCurve(378221, "test", new List<RelatedCurve>() {
                      new RelatedCurve(376520, -535.045129978397, 161484, CurveByInference.RelativeOrientation.To_To),
                      new RelatedCurve(378222, -535.045129978397, 161484, CurveByInference.RelativeOrientation.To_From)     }){
                      ParallelCurves = new List<RelatedCurve>() {
                      },
                      TangentLines = new List<RelatedLine>() {
                           new RelatedLine(376522, Framework.toDegrees(3.03343483861263), CurveByInference.RelativeOrientation.From_From),
                           new RelatedLine(378220, Framework.toDegrees(3.03343483861263), CurveByInference.RelativeOrientation.From_To)
                      }},
                 new InferredCurve(378232, "test", new List<RelatedCurve>() {
                      new RelatedCurve(376511, -543.236862578124, 161476, CurveByInference.RelativeOrientation.From_From),
                      new RelatedCurve(378231, -543.236862578124, 161476, CurveByInference.RelativeOrientation.From_To)     }){
                      InferredRadius = -543.236862578124, InferredCenterpointID = 161476,
                      ParallelCurves = new List<RelatedCurve>() {
                      },
                      TangentLines = new List<RelatedLine>() {
                           new RelatedLine(376509, Framework.toDegrees(-2.76329138018016), CurveByInference.RelativeOrientation.To_To),
                           new RelatedLine(378233, Framework.toDegrees(-2.76330652016479), CurveByInference.RelativeOrientation.To_From)
                      }}};

            #endregion

            Framework.AssertInferredCurvesAreEqual(expectedResults, result.Curves);
        }


        [TestMethod]
        public void SharedExample_30()
        {
            CurveByInference result = Framework.RunTest("SharedExample", "Fabric1510478", "ObjectID = 30");

            #region expectedData

            //string constructorString = Framework.GenerateConstructorStatment(result.Curves);
            List<InferredCurve> expectedResults = new List<InferredCurve>() { 
                new InferredCurve(30, "test", new List<RelatedCurve>() {          
                    new RelatedCurve(37, 207.696, 24, CurveByInference.RelativeOrientation.From_From) }){
                        InferredRadius = 207.696, InferredCenterpointID = 24,
                        ParallelCurves = new List<RelatedCurve>() {          
                            new RelatedCurve(35, 207.695399526566, 24, CurveByInference.RelativeOrientation.From_From),
                            new RelatedCurve(173, 207.695258779861, 24, CurveByInference.RelativeOrientation.From_From),
                            new RelatedCurve(1151, 207.695258779861, 24, CurveByInference.RelativeOrientation.From_From),
                            new RelatedCurve(1177, 207.695399526566, 24, CurveByInference.RelativeOrientation.From_From)
                        }
                }};

            #endregion

            Framework.AssertInferredCurvesAreEqual(expectedResults, result.Curves);
        }
    }
}
