using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ParcelFabricCurveByInference
{
    public class RelatedLine : LineBase
    {
        public double Angle { get; set; }
        public RelatedLine(int id, double angle, CurveByInference.RelativeOrientation orientation)
            : base (id, orientation)
        {
            this.Angle = angle;
            
        }
    }

    public class RelatedLineComparer : IEqualityComparer<RelatedLine>
    {
        bool compareOIDs;

        public RelatedLineComparer(bool compareOIDs = false)
        {
            this.compareOIDs = compareOIDs;
        }

        public bool Equals(RelatedLine x, RelatedLine y)
        {
            if (compareOIDs)
                return (x.Orientation == y.Orientation && x.ObjectID == y.ObjectID && Math.Abs(x.Angle - y.Angle) < 0.005);
            return (Math.Abs(x.Angle - y.Angle) < 0.005);
        }

        public int GetHashCode(RelatedLine obj)
        {
            if (compareOIDs)
                return (int)((int)obj.Orientation * obj.ObjectID * obj.Angle);
            return (int)(obj.Angle);
        }
    }

    public abstract class LineBase
    {
        public int ObjectID { get; set; }
        public CurveByInference.RelativeOrientation Orientation { get; set; }

        System.Windows.Visibility _UpdateVisibility = System.Windows.Visibility.Collapsed;
        public System.Windows.Visibility UpdateVisibility { get { return _UpdateVisibility; } protected set { _UpdateVisibility = value; } }

        protected LineBase(int ObjectID, CurveByInference.RelativeOrientation Orientation)
        {
            this.ObjectID = ObjectID;
            this.Orientation = Orientation;
        }
    }
}
