using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ParcelFabricCurveByInference
{
    public class RelatedLine : LineBase
    {
        public double Angle { get; set; }
        public double DeltaAngle { get; set; }

        public RelatedLine(int id, double angle, CurveByInference.RelativeOrientation orientation)
            : this(id, angle, double.NaN, orientation)
        {
        }
        public RelatedLine(int id, double angle, double deltaAngle, CurveByInference.RelativeOrientation orientation)
            : base (id, orientation)
        {
            this.Angle = angle;
            this.DeltaAngle = deltaAngle;
        }
    }

    public class RelatedLineComparer : IEqualityComparer<RelatedLine>
    {
        bool compareOIDs, DelataOnly;

        public RelatedLineComparer(bool compareOIDs = false, bool deltaOnly = false)
        {
            this.compareOIDs = compareOIDs;
            this.DelataOnly = deltaOnly;
        }

        public bool Equals(RelatedLine x, RelatedLine y)
        {
            if (DelataOnly)
                return Math.Abs(x.DeltaAngle - y.DeltaAngle) < 0.005;
            if (compareOIDs && x.ObjectID != y.ObjectID)
                return false;
            return (x.Orientation == y.Orientation && Math.Abs(x.Angle - y.Angle) < 0.005 && Math.Abs(x.DeltaAngle - y.DeltaAngle) < 0.005);
        }

        public int GetHashCode(RelatedLine obj)
        {
            if (DelataOnly)
                return (int)(obj.DeltaAngle * 100000);
            if (compareOIDs)
                return (int)((int)obj.Orientation * obj.ObjectID * obj.Angle);
            return (int)(obj.Angle);
        }
    }

    public abstract class LineBase
    {
        public int ObjectID { get; set; }
        CurveByInference.RelativeOrientation _Orientation;
        public CurveByInference.RelativeOrientation Orientation 
        {
            get { return _Orientation; } 
            set {
                _Orientation = value;
                isAtStart = value == CurveByInference.RelativeOrientation.To_From || value == CurveByInference.RelativeOrientation.To_To;
                isAtEnd = value == CurveByInference.RelativeOrientation.From_To || value == CurveByInference.RelativeOrientation.From_From;
                isEqual = value == CurveByInference.RelativeOrientation.Reverse || value == CurveByInference.RelativeOrientation.Same;
            }
        }

        System.Windows.Visibility _UpdateVisibility = System.Windows.Visibility.Collapsed;
        public System.Windows.Visibility UpdateVisibility { get { return _UpdateVisibility; } protected set { _UpdateVisibility = value; } }

        public bool isAtStart { get; private set; }
        public bool isAtEnd { get; private set; }
        public bool isEqual { get; private set; }
        
        protected LineBase(int ObjectID, CurveByInference.RelativeOrientation Orientation)
        {
            this.ObjectID = ObjectID;
            this.Orientation = Orientation;
        }
    }
}
