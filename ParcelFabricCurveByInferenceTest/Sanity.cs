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
            CurveByInference result = Framework.RunTest("NegativeRadiusTestFD", "NegativeRadiusTest", "ObjectID = 356638");

            #region expectedData

            List<InferredCurve> expectedResults = new List<InferredCurve>() { 
                 new InferredCurve(356638, "test", new List<RelatedCurve>() {
                      new RelatedCurve(356637, -864.159989779026, 152265, CurveByInference.RelativeOrientation.From_To)     }){
                      ParallelCurves = new List<RelatedCurve>() {
                      },
                      TangentLines = new List<RelatedLine>() {
                      }}};

            #endregion

            Framework.AssertInferredCurvesAreEqual(expectedResults, result.Curves);
        }


        [TestMethod]
        public void SharedExample_30()
        {
            CurveByInference result = Framework.RunTest("SharedExample", "Fabric1510478", "ObjectID = 30");

            #region expectedData

            List<InferredCurve> expectedResults = new List<InferredCurve>() { 
             new InferredCurve(30, "test", new List<RelatedCurve>() {
                  new RelatedCurve(37, 207.696, 24, CurveByInference.RelativeOrientation.From_To)     }){
                  InferredRadius = 207.696, InferredCenterpointID = 24, 
                  ParallelCurves = new List<RelatedCurve>() {
                  },
                  
                    //ParallelCurves = new List<RelatedCurve>() {          
                    //    new RelatedCurve(35, 207.695399526566, 24, CurveByInference.RelativeOrientation.From_From),
                    //    new RelatedCurve(173, 207.695258779861, 24, CurveByInference.RelativeOrientation.From_From),
                    //    new RelatedCurve(1151, 207.695258779861, 24, CurveByInference.RelativeOrientation.From_From),
                    //    new RelatedCurve(1177, 207.695399526566, 24, CurveByInference.RelativeOrientation.From_From)
                    //}
                  TangentLines = new List<RelatedLine>() {
                       new RelatedLine(25, -174.20082313486, 0.122142482787701, CurveByInference.RelativeOrientation.To_From),
                       new RelatedLine(29, -82.1752583298569, 91.9034223222166, CurveByInference.RelativeOrientation.From_To),
                       new RelatedLine(33, -82.1752583298569, 91.9034223222166, CurveByInference.RelativeOrientation.From_From)
                  }}};
            #endregion

            Framework.AssertInferredCurvesAreEqual(expectedResults, result.Curves);
        }
    }
}
