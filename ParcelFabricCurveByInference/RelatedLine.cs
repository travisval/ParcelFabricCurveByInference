using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ParcelFabricCurveByInference
{
    public class RelatedLine
    {
        public int ObjectID { get; set; }

        public double Slope { get; set; }

        public CurveByInference.RelativeOrientation Orientation { get; set; }

        public string Header { get; set; }

        public RelatedLine(int id, double slope, CurveByInference.RelativeOrientation orientation)
        {
            this.ObjectID = id;
            this.Slope = slope;
            this.Orientation = orientation;

            this.Header = String.Format("{0}: Slope:{1}", id, slope);
        }
    }
}
