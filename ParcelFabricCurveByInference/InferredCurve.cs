using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using ESRI.ArcGIS.Geometry;

namespace ParcelFabricCurveByInference
{
    public class InferredCurve : INotifyPropertyChanged
    {
        public int ObjectID { get; set; }
        public string Description { get; set; }
        public string LayerName { get; private set; }

        public IPoint FromPoint { get; set; }
        public IPoint ToPoint { get; set; }

        public RelatedCurve _Accepted;
        public RelatedCurve Accepted { get { return _Accepted; } set { _Accepted = value; RaisePropertyChanged("Accepted", "SetVisibility", "ItemColor"); } }

        public System.Windows.Media.Brush ItemColor { get { return (Accepted == null) ? System.Windows.Media.Brushes.Black : System.Windows.Media.Brushes.Blue; } }

        public List<RelatedCurve> _TangentCurves;
        public List<RelatedCurve> TangentCurves { get { return _TangentCurves; } set { _TangentCurves = value; RaisePropertyChanged("TangentCurves", "TangentVisibility"); } }
        public System.Windows.Visibility TangentVisibility { get { return (TangentCurves == null || TangentCurves.Count == 0) ? System.Windows.Visibility.Collapsed : System.Windows.Visibility.Visible; } }

        public List<RelatedCurve> _ParallelCurves;
        public List<RelatedCurve> ParallelCurves { get { return _ParallelCurves; } set { _ParallelCurves = value; RaisePropertyChanged("ParallelCurves", "ParallelVisibility"); } }
        public System.Windows.Visibility ParallelVisibility { get { return (ParallelCurves == null || ParallelCurves.Count == 0) ? System.Windows.Visibility.Collapsed : System.Windows.Visibility.Visible; } }

        public List<RelatedLine> TangentLines = new List<RelatedLine>();

        public string Header { get; set; }
        public System.Windows.Visibility SetVisibility { get { return (Accepted == null) ? System.Windows.Visibility.Collapsed : System.Windows.Visibility.Visible; } }

        public InferredCurve(int id, string LayerName, List<RelatedCurve> tangentCurves)
        {
            this.ObjectID = id;
            this.LayerName = LayerName;
            this.Header = string.Format("{0} - {1}", LayerName, ObjectID);

            TangentCurves = tangentCurves;
            ParallelCurves = new List<RelatedCurve>();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void RaisePropertyChanged(params string[] properties)
        {
            try
            {
                //if (UIDispatcher != null && !UIDispatcher.CheckAccess())
                //    UIDispatcher.Invoke(() => { RaisePropertyChanged(properties); });


                if (PropertyChanged != null)
                    foreach (string property in properties)
                        PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
            catch (Exception exx)
            {
                System.Diagnostics.Debug.WriteLine(exx.Message);
            }
        }

        static RelatedCurveComparer relativeComparer = new RelatedCurveComparer(false);
        static RelatedCurveComparer relativeComparerWithOID = new RelatedCurveComparer(true);
        public override bool Equals(object obj)
        {
            InferredCurve other = obj as InferredCurve;
            if (other == null)
                return false;

            if (this.ObjectID != other.ObjectID)
                return false;


            if (this.Accepted != null)
            {
                //if (!this.Accepted.Equals(other.Accepted))
                if (!relativeComparer.Equals(this.Accepted, other.Accepted))
                    return false;
            }
            else if (other.Accepted != null)
            {
                return false;
            }

            if (!this.TangentCurves.OrderBy(i => i.ObjectID).SequenceEqual(other.TangentCurves.OrderBy(i => i.ObjectID), relativeComparerWithOID))
                return false;

            if (!this.ParallelCurves.OrderBy(i => i.ObjectID).SequenceEqual(other.ParallelCurves.OrderBy(i => i.ObjectID), relativeComparerWithOID))
                return false;

            return true;
        }

        public override int GetHashCode()
        {
            int hash = ObjectID;
            if (Accepted != null)
                hash += Accepted.GetHashCode();
            if (TangentCurves != null)
                hash += TangentCurves.Sum(w => w.GetHashCode());
            if (ParallelCurves != null)
                hash += TangentCurves.Sum(w => w.GetHashCode());

            return hash;
        }
    }

}
