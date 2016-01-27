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
        #region Curve with Tangent

        //tests confirming a radius with straight line tanget (running perfectly east - west)
        [TestMethod]
        public void Tangent_Confirmer_Flat()
        {
            CurveByInference result = Framework.RunFeatureClassTest("TangentDevTests", "CenterPoint", "TangentLines", "Objectid = 9");

            List<InferredCurve> expectedResults = new List<InferredCurve>() { 
                     new InferredCurve(9, "test", new List<RelatedCurve>() {
                          new RelatedCurve(5, 818.826249, 1, RelationTypes.Tangent)}){
                            Accepted = new RelatedCurve(5, 818.826249, 1, RelationTypes.Tangent), 
                            ParallelCurves = new List<RelatedCurve>() {
                      }}};

            Framework.AssertInferredCurvesAreEqual(expectedResults, result.Curves);
        }
        [TestMethod]
        public void Tangent_Confirmer_Flat_Reverse()
        {
            CurveByInference result = Framework.RunFeatureClassTest("TangentDevTests", "CenterPoint", "TangentLines", "Objectid = 26");

            List<InferredCurve> expectedResults = new List<InferredCurve>() { 
                     new InferredCurve(26, "test", new List<RelatedCurve>() {
                          new RelatedCurve(5, -818.826249, 1, RelationTypes.Tangent)}){
                            Accepted = new RelatedCurve(5, -818.826249, 1, RelationTypes.Tangent), 
                            ParallelCurves = new List<RelatedCurve>() {
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
                new RelatedCurve(15, 1023.030777, 1, RelationTypes.Tangent)        }){
                    Accepted = new RelatedCurve(15, 1023.030777, 1, RelationTypes.Tangent), 
                    ParallelCurves = new List<RelatedCurve>() {}}};

            Framework.AssertInferredCurvesAreEqual(expectedResults, result.Curves);
        }
        [TestMethod]
        public void Tangent_Confirmer_Angle_Reverse()
        {
            CurveByInference result = Framework.RunFeatureClassTest("TangentDevTests", "CenterPoint", "TangentLines", "Objectid = 25");
            Console.WriteLine(Framework.GenerateConstructorStatment(result.Curves));
            List<InferredCurve> expectedResults = new List<InferredCurve>() { 
                new InferredCurve(25, "test", new List<RelatedCurve>() {
                new RelatedCurve(15, -1023.030777, 1, RelationTypes.Tangent)        }){
                    Accepted = new RelatedCurve(15, -1023.030777, 1, RelationTypes.Tangent), 
                    ParallelCurves = new List<RelatedCurve>() {}}};

            Framework.AssertInferredCurvesAreEqual(expectedResults, result.Curves);
        }

        //tests confirming a radius with straight line tanget (running perfectly north - south)
        [TestMethod]
        public void Tangent_Confirmer_Vertical()
        {
            CurveByInference result = Framework.RunFeatureClassTest("TangentDevTests", "CenterPoint", "TangentLines", "Objectid = 23");

            List<InferredCurve> expectedResults = new List<InferredCurve>() { 
                new InferredCurve(23, "test", new List<RelatedCurve>() {
                new RelatedCurve(20, 752.060666, 1, RelationTypes.Tangent)}){
                    Accepted = new RelatedCurve(20, 752.060666, 1, RelationTypes.Tangent), 
                    ParallelCurves = new List<RelatedCurve>() {
                    }}};

            Framework.AssertInferredCurvesAreEqual(expectedResults, result.Curves);
        }
        [TestMethod]
        public void Tangent_Confirmer_Vertical_Reverse()
        {
            CurveByInference result = Framework.RunFeatureClassTest("TangentDevTests", "CenterPoint", "TangentLines", "Objectid = 24");

            List<InferredCurve> expectedResults = new List<InferredCurve>() { 
                new InferredCurve(24, "test", new List<RelatedCurve>() {
                new RelatedCurve(20, -752.060666, 1, RelationTypes.Tangent)}){
                    Accepted = new RelatedCurve(20, -752.060666, 1, RelationTypes.Tangent), 
                    ParallelCurves = new List<RelatedCurve>() {
                    }}};

            Framework.AssertInferredCurvesAreEqual(expectedResults, result.Curves);
        }

        #endregion
    }
}
