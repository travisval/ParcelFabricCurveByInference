using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using ESRI.ArcGIS.Geometry;

namespace ParcelFabricCurveByInference
{
    public enum UpdateAction { Leave, Update }

    public class InferredCurve : INotifyPropertyChanged
    {
        public int ObjectID { get; set; }
        public string Description { get; set; }
        public string LayerName { get; private set; }

        public IPoint FromPoint { get; set; }
        public IPoint ToPoint { get; set; }

        public RelatedCurve _Accepted;
        public RelatedCurve Accepted { get { return _Accepted; } set { _Accepted = value; Action = (value == null) ? UpdateAction.Leave : UpdateAction.Update;  RaisePropertyChanged("Accepted", "SetVisibility", "ItemColor"); } }

        public UpdateAction _Action;
        public UpdateAction Action { get { return _Action; } set { _Action = value; RaisePropertyChanged("Action"); } }

        public List<RelatedCurve> _TangentCurves;
        public List<RelatedCurve> TangentCurves { get { return _TangentCurves; } set { _TangentCurves = value; RaisePropertyChanged("TangentCurves", "TangentVisibility", "TangentCurveCount"); } }
        public System.Windows.Visibility TangentVisibility { get { return (TangentCurves == null || TangentCurves.Count == 0) ? System.Windows.Visibility.Collapsed : System.Windows.Visibility.Visible; } }
        public int TangentCurveCount { get { return (TangentCurves == null) ? 0 : TangentCurves.Count; } } 

        public List<RelatedCurve> _ParallelCurves;
        public List<RelatedCurve> ParallelCurves { get { return _ParallelCurves; } set { _ParallelCurves = value; RaisePropertyChanged("ParallelCurves", "ParallelVisibility", "ParallelCurveCount"); } }
        public System.Windows.Visibility ParallelVisibility { get { return (ParallelCurves == null || ParallelCurves.Count == 0) ? System.Windows.Visibility.Collapsed : System.Windows.Visibility.Visible; } }
        public int ParallelCurveCount { get { return (ParallelCurves == null) ? 0 : ParallelCurves.Count; } } 

        public List<RelatedLine> _TangentLines;
        public List<RelatedLine> TangentLines { get { return _TangentLines; } set { _TangentLines = value; RaisePropertyChanged("TangentLines", "TangentLineVisibility", "TangentLineCount"); } }
        public System.Windows.Visibility TangentLineVisibility { get { return (TangentLines == null || TangentLines.Count == 0) ? System.Windows.Visibility.Collapsed : System.Windows.Visibility.Visible; } }
        public int TangentLineCount { get { return (TangentLines == null) ? 0 : TangentLines.Count; } } 

        public string Header { get; set; }
        public System.Windows.Visibility SetVisibility { get { return (Accepted == null) ? System.Windows.Visibility.Collapsed : System.Windows.Visibility.Visible; } }

        public InferredCurve(int id, string LayerName, List<RelatedCurve> tangentCurves)
        {
            this.ObjectID = id;
            this.LayerName = LayerName;
            this.Header = string.Format("{0} - {1}", LayerName, ObjectID);

            TangentCurves = tangentCurves;
            ParallelCurves = new List<RelatedCurve>();
            TangentLines = new List<RelatedLine>();
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

        static RelatedLineComparer relativeLineComparer = new RelatedLineComparer(false);
        static RelatedLineComparer relativeLineComparerWithOID = new RelatedLineComparer(true);

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

            if (!this.TangentLines.OrderBy(i => i.ObjectID).SequenceEqual(other.TangentLines.OrderBy(i => i.ObjectID), relativeLineComparerWithOID))
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
