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

            //string constructorString = Framework.GenerateConstructorStatment(result.Curves);
            List<InferredCurve> expectedResults = new List<InferredCurve>() { 
                 new InferredCurve(356663, "test", new List<RelatedCurve>() {
                      new RelatedCurve(356664, -399.413630657088, 152274, RelationTypes.Tangent)        }){
                   ParallelCurves = new List<RelatedCurve>() {
                    }},
                 new InferredCurve(371318, "test", new List<RelatedCurve>() {
                      new RelatedCurve(371319, 1726.12593027848, 158475, RelationTypes.Tangent)        }){
                   ParallelCurves = new List<RelatedCurve>() {
                    }},
                 new InferredCurve(371322, "test", new List<RelatedCurve>() {
                      new RelatedCurve(371321, -1696.07550002682, 158476, RelationTypes.Tangent),
                      new RelatedCurve(376534, -1696.07550002682, 158476, RelationTypes.Tangent)        }){
                   Accepted = new RelatedCurve(371321, -1696.07550002682, 158476, RelationTypes.Tangent), 
                   ParallelCurves = new List<RelatedCurve>() {
                    }},
                 new InferredCurve(375549, "test", new List<RelatedCurve>() {
                      new RelatedCurve(375548, -426.287570033533, 160866, RelationTypes.Tangent)        }){
                   ParallelCurves = new List<RelatedCurve>() {
                    }},
                 new InferredCurve(375622, "test", new List<RelatedCurve>() {
                      new RelatedCurve(375548, 426.287570033533, 160866, RelationTypes.Tangent)        }){
                   ParallelCurves = new List<RelatedCurve>() {
                    }},
                 new InferredCurve(376510, "test", new List<RelatedCurve>() {
                      new RelatedCurve(376511, 543.236862578124, 161476, RelationTypes.Tangent),
                      new RelatedCurve(378231, 543.236862578124, 161476, RelationTypes.Tangent)        }){
                   Accepted = new RelatedCurve(376511, 543.236862578124, 161476, RelationTypes.Tangent), 
                   ParallelCurves = new List<RelatedCurve>() {
                    }},
                 new InferredCurve(376521, "test", new List<RelatedCurve>() {
                      new RelatedCurve(376520, 535.045129978397, 161484, RelationTypes.Tangent),
                      new RelatedCurve(378222, 535.045129978397, 161484, RelationTypes.Tangent)        }){
                   Accepted = new RelatedCurve(376520, 535.045129978397, 161484, RelationTypes.Tangent), 
                   ParallelCurves = new List<RelatedCurve>() {
                    }},
                 new InferredCurve(378208, "test", new List<RelatedCurve>() {
                      new RelatedCurve(378207, -449.806979278138, 161797, RelationTypes.Tangent),
                      new RelatedCurve(378209, -449.809999037129, 161796, RelationTypes.Tangent)        }){
                   ParallelCurves = new List<RelatedCurve>() {
                    }},
                 new InferredCurve(378221, "test", new List<RelatedCurve>() {
                      new RelatedCurve(376520, -535.045129978397, 161484, RelationTypes.Tangent),
                      new RelatedCurve(378222, -535.045129978397, 161484, RelationTypes.Tangent)        }){
                   Accepted = new RelatedCurve(376520, -535.045129978397, 161484, RelationTypes.Tangent), 
                   ParallelCurves = new List<RelatedCurve>() {
                    }},
                 new InferredCurve(378232, "test", new List<RelatedCurve>() {
                      new RelatedCurve(376511, -543.236862578124, 161476, RelationTypes.Tangent),
                      new RelatedCurve(378231, -543.236862578124, 161476, RelationTypes.Tangent)        }){
                   Accepted = new RelatedCurve(376511, -543.236862578124, 161476, RelationTypes.Tangent), 
                   ParallelCurves = new List<RelatedCurve>() {
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
                    new RelatedCurve(37, 207.696, 24, RelationTypes.Tangent) }){
                        Accepted = new RelatedCurve(37, 207.696, 24, RelationTypes.Tangent), 
                        ParallelCurves = new List<RelatedCurve>() {          
                            new RelatedCurve(35, 207.695399526566, 24, RelationTypes.Tangent),
                            new RelatedCurve(173, 207.695258779861, 24, RelationTypes.Tangent),
                            new RelatedCurve(1151, 207.695258779861, 24, RelationTypes.Tangent),
                            new RelatedCurve(1177, 207.695399526566, 24, RelationTypes.Tangent)
                        }
                }};

            #endregion

            Framework.AssertInferredCurvesAreEqual(expectedResults, result.Curves);
        }
    }
}
