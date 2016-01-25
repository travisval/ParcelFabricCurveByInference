using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ParcelFabricCurveByInference
{
    public enum RelationTypes { Tangent, Parallel, Same, Reverse }
    public class RelatedCurve
    {
        public int ObjectID { get; set; }

        public double Radius { get; set; }
        public int CenterpointID { get; set; }

        public CurveByInference.RelativeOrientation Orientation { get; set; }

        public RelationTypes RelationType { get; set; }

        public string Header { get; set; }

        public RelatedCurve(int id, double radius, int centerpointID, RelationTypes relationType)
        {
            this.ObjectID = id;
            this.Radius = radius;
            this.CenterpointID = centerpointID;
            this.RelationType = relationType;

            this.Header = String.Format("{0}: CPID:{1} rad:{2}", id, centerpointID, radius);
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
