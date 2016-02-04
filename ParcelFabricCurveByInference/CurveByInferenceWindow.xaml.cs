using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Carto;
using System.Runtime.InteropServices;
using ESRI.ArcGIS.CartoUI;

namespace ParcelFabricCurveByInference
{
    /// <summary>
    /// Designer class of the dockable window add-in. It contains WPF user interfaces that
    /// make up the dockable window.
    /// </summary>
    public partial class CurveByInferenceWindow : UserControl
    {
        CurveByInference context { get { return this.DataContext as CurveByInference; } }

        public CurveByInferenceWindow()
        {
            InitializeComponent();

            this.DataContext = new CurveByInference();
            PropertiesDebug.DataContext = CurveByInferenceSettings.Instance;

            //if this window is created, make sure that the extension is as well
            ESRI.ArcGIS.esriSystem.IExtension extension = CurveByInferenceExtension.GetExtension();
        }

        /// <summary>
        /// Implementation class of the dockable window add-in. It is responsible for 
        /// creating and disposing the user interface class of the dockable window.
        /// </summary>
        public class AddinImpl : ESRI.ArcGIS.Desktop.AddIns.DockableWindow
        {
            private System.Windows.Forms.Integration.ElementHost m_windowUI;

            public AddinImpl()
            {
            }

            protected override IntPtr OnCreateChild()
            {
                m_windowUI = new System.Windows.Forms.Integration.ElementHost();
                m_windowUI.Child = new CurveByInferenceWindow();

                return m_windowUI.Handle;
            }

            protected override void Dispose(bool disposing)
            {
                if (m_windowUI != null)
                    m_windowUI.Dispose();

                base.Dispose(disposing);
            }

        }
                
        private IFeature GetFeature(string layerName, int objectid)
        {
            IFeatureLayer fl = getLayerByLayerName(layerName);
            if (fl == null)
                MessageBox.Show(String.Format("The layer {0} could not be found", layerName));

            return fl.FeatureClass.GetFeature(objectid);
        }
        private void Flash(IFeature feature)
        {
            IFeatureIdentifyObj featIdentify = new FeatureIdentifyObject();
            featIdentify.Feature = feature;
            IIdentifyObj identify = featIdentify as IIdentifyObj;
            if(identify != null)
                identify.Flash(ArcMap.Document.ActivatedView.ScreenDisplay);

            Marshal.ReleaseComObject(featIdentify);
        }
        private void ZoomTo(IEnvelope extent)
        {
            if (extent.Height == 0)
                extent.Height = extent.Width / 4;
            if (extent.Width == 0)
                extent.Width = extent.Height / 4;
            extent.Expand(1.2, 1.2, true);
            ArcMap.Document.ActivatedView.Extent = extent;
            ArcMap.Document.ActivatedView.Refresh();
            ArcMap.Document.ActivatedView.ScreenDisplay.UpdateWindow();
        }
        private void PanTo(IFeature feature)
        {
            double X = (feature.Extent.XMin + feature.Extent.XMax) / 2.0;
            double Y = (feature.Extent.YMin + feature.Extent.YMax) / 2.0;

            IEnvelope currentEnv = ArcMap.Document.ActivatedView.Extent;
            currentEnv.CenterAt(new ESRI.ArcGIS.Geometry.Point() { X = X, Y = Y });
            ArcMap.Document.ActivatedView.Extent = currentEnv;

            ArcMap.Document.ActivatedView.Refresh();
            
        }
        private void SelectAndZoom(InferredCurve curve, bool zoom, bool select, bool related)
        {
            IFeatureLayer fl = getLayerByLayerName(curve.LayerName);
            if (fl == null)
                MessageBox.Show(String.Format("The layer {0} could not be found", curve.LayerName));

            List<int> oids = new List<int>();
            oids.Add(curve.ObjectID);
            if (related)
            {
                oids.AddRange(curve.ParallelCurves.Select(w => w.ObjectID));
                oids.AddRange(curve.TangentCurves.Select(w => w.ObjectID));
                oids.AddRange(curve.TangentLines.Select(w => w.ObjectID));
            }

            IEnvelope extent = null;

            if (select)
                ArcMap.Document.FocusMap.ClearSelection();

            IFeatureCursor featureCursor = fl.FeatureClass.GetFeatures(oids.ToArray(), true);
            IFeature feature = null;
            while ((feature = featureCursor.NextFeature()) != null)
            {
                if (extent == null)
                    extent = feature.Shape.Envelope;
                else
                    extent.Union(feature.Shape.Envelope);

                if (select)
                    ArcMap.Document.FocusMap.SelectFeature(fl, feature);

                Marshal.ReleaseComObject(feature);
            }

            if (zoom)
            {
                ZoomTo(extent);
            }
            else
            {
                ArcMap.Document.ActivatedView.Refresh();
            }
        }
                
        InferredCurve getInferredCurveFromMenuItem(object sender)
        {
            MenuItem mi = sender as MenuItem;
            if (mi != null)
                return mi.CommandParameter as InferredCurve;
            return null;
        }
        LineBase getRelatedCurveFromMenuItem(object sender)
        {
            MenuItem mi = sender as MenuItem;
            if (mi != null)
                return mi.CommandParameter as LineBase;
            return null;
        }
        IFeatureLayer getLayerByLayerName(string Name)
        {
            IEnumLayer layers = ArcMap.Document.FocusMap.get_Layers(null, true);
            ILayer layer = null;
            while((layer = layers.Next()) != null)
            {
                if (layer.Name == Name)
                    return layer as IFeatureLayer;
            }
            Marshal.ReleaseComObject(layers);
            return null;
        }
        
        #region Inferred Curve Context Menu Events

        private void FlashInferred_Click(object sender, RoutedEventArgs e)
        {
            InferredCurve curve = getInferredCurveFromMenuItem(sender);
            if (curve != null)
            {
                IFeature feature = GetFeature(curve.LayerName, curve.ObjectID);
                Flash(feature);
                Marshal.ReleaseComObject(feature);
            }
        }
        
        private void SelectInferred_Click(object sender, RoutedEventArgs e)
        {
            InferredCurve curve = getInferredCurveFromMenuItem(sender);
            if (curve != null)
            {
                SelectAndZoom(curve, false, true, false);
            }
        }

        private void PanToInferred_Click(object sender, RoutedEventArgs e)
        {
            InferredCurve curve = getInferredCurveFromMenuItem(sender);
            if (curve != null)
            {
                IFeature feature = GetFeature(curve.LayerName, curve.ObjectID);
                PanTo(feature);
                Marshal.ReleaseComObject(feature);
            }
        }

        private void ZoomToInferred_Click(object sender, RoutedEventArgs e)
        {
            InferredCurve curve = getInferredCurveFromMenuItem(sender);
            if (curve != null)
            {
                SelectAndZoom(curve, true, false, false);
            }
        }

        private void ZoomToInferredRelated_Click(object sender, RoutedEventArgs e)
        {
            InferredCurve curve = getInferredCurveFromMenuItem(sender);
            if (curve != null)
            {
                SelectAndZoom(curve, true, false, true);
            }
        }
        
        private void SelectInferredRelated_Click(object sender, RoutedEventArgs e)
        {
            InferredCurve curve = getInferredCurveFromMenuItem(sender);
            if (curve != null)
            {
                SelectAndZoom(curve, false, true, true);
            }
        }

        private void ApplyInferredChange_Click(object sender, RoutedEventArgs e)
        {
            InferredCurve curve = getInferredCurveFromMenuItem(sender);
            if (curve != null && context != null && context.SelectedItem != null)
            {
                context.UpdateCurves(new InferredCurve[] { curve });
            }
        }

        #endregion

        #region Related Curve Context Menu Events

        private void FlashRelated_Click(object sender, RoutedEventArgs e)
        {
            LineBase curve = getRelatedCurveFromMenuItem(sender);
            if (curve != null && context != null && context.SelectedItem != null)
            {
                IFeature feature = GetFeature(context.SelectedItem.LayerName, curve.ObjectID);
                Flash(feature);
                Marshal.ReleaseComObject(feature);
            }
        }

        private void ZoomToRelated_Click(object sender, RoutedEventArgs e)
        {
            LineBase curve = getRelatedCurveFromMenuItem(sender);
            if (curve != null && context != null && context.SelectedItem != null)
            {
                IFeature feature = GetFeature(context.SelectedItem.LayerName, curve.ObjectID);
                ZoomTo(feature.Shape.Envelope);
                Marshal.ReleaseComObject(feature);
            }
        }

        private void PanToRelated_Click(object sender, RoutedEventArgs e)
        {
            LineBase curve = getRelatedCurveFromMenuItem(sender);
            if (curve != null && context != null && context.SelectedItem != null)
            {
                IFeature feature = GetFeature(context.SelectedItem.LayerName, curve.ObjectID);
                PanTo(feature);
                Marshal.ReleaseComObject(feature);
            }
        }

        private void SelectRelated_Click(object sender, RoutedEventArgs e)
        {
            LineBase curve = getRelatedCurveFromMenuItem(sender);
            if (curve != null && context != null && context.SelectedItem != null)
            {
                IFeatureLayer fl = getLayerByLayerName(context.SelectedItem.LayerName);
                if (fl == null)
                    MessageBox.Show(String.Format("The layer {0} could not be found", context.SelectedItem.LayerName));

                IFeature feature = fl.FeatureClass.GetFeature(curve.ObjectID);
                ArcMap.Document.FocusMap.SelectFeature(fl, feature);
                Marshal.ReleaseComObject(feature);
            }    
            
        }

        private void UpdateValuesRelated_Click(object sender, RoutedEventArgs e)
        {
            RelatedCurve curve = getRelatedCurveFromMenuItem(sender) as RelatedCurve;
            if (curve != null && context != null && context.SelectedItem != null)
            {
                context.SelectedItem.InferredRadius = curve.Radius;
                context.SelectedItem.InferredCenterpointID = curve.CenterpointID;
            }
        }

        #endregion

        #region button click events

        private void Find_Click(object sender, RoutedEventArgs e)
        {
            CurveByInference context = new CurveByInference();
            this.DataContext = context;

            context.FindCurves();
        }
        private void Update_Click(object sender, RoutedEventArgs e)
        {
            CurveByInference context = this.DataContext as CurveByInference;
            if(context != null)
                context.UpdateCurves();
        }
        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            CurveByInference context = new CurveByInference();
            this.DataContext = context;
        }


        #endregion

    }
}
