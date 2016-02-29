using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ParcelFabricCurveByInference
{
    public class RelatedCurve : LineBase
    {
        public double Radius { get; set; }
        public int CenterpointID { get; set; }
        public double DistanceToLine { get; set; }

        //controls the visibilty of update related UI components
        static CurveByInference.RelativeOrientation[] UpdateableOrientations = new CurveByInference.RelativeOrientation[] {
            CurveByInference.RelativeOrientation.To_To,
            CurveByInference.RelativeOrientation.To_From,
            CurveByInference.RelativeOrientation.From_To,
            CurveByInference.RelativeOrientation.From_From
        };
        public RelatedCurve(int id, double radius, int centerpointID, CurveByInference.RelativeOrientation orientation)
            : this(id, radius, centerpointID, 0.0, orientation)
        {
        }
        public RelatedCurve(int id, double radius, int centerpointID, double distanceToLine, CurveByInference.RelativeOrientation orientation)
            : base (id, orientation)
        {
            this.Radius = radius;
            this.CenterpointID = centerpointID;
            this.DistanceToLine = distanceToLine;
            if (UpdateableOrientations.Contains(Orientation))
                UpdateVisibility = System.Windows.Visibility.Visible;
        }

    }
    public class RelatedCurveComparer : IEqualityComparer<RelatedCurve>
    {
        bool compareOIDs;

        public RelatedCurveComparer(bool compareOIDs = false)
        {
            this.compareOIDs = compareOIDs;
        }

        public bool Equals(RelatedCurve x, RelatedCurve y)
        {
            if (compareOIDs)
                return (x.ObjectID == y.ObjectID && x.CenterpointID == y.CenterpointID && Math.Abs(x.Radius - y.Radius) < 0.005);
            return (x.CenterpointID == y.CenterpointID && Math.Abs(x.Radius - y.Radius) < 0.005);
        }

        public int GetHashCode(RelatedCurve obj)
        {
            if (compareOIDs)
                return (int)(obj.ObjectID * obj.CenterpointID * obj.Radius);
            return (int)(obj.CenterpointID * obj.Radius);
        }
    }
}
