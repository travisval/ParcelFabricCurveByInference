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
using System.Windows.Shapes;
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

        private void Find_Click(object sender, RoutedEventArgs e)
        {
            CurveByInference context = new CurveByInference();
            this.DataContext = context;

            context.FindCurves(false);
        }
        private void Update_Click(object sender, RoutedEventArgs e)
        {
            CurveByInference context = new CurveByInference();
            this.DataContext = context;

            context.FindCurves(true);
        }
        
        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            CurveByInference context = new CurveByInference();
            this.DataContext = context;
        }

        private void Flash_Click(object sender, RoutedEventArgs e)
        {
            InferredCurve curve = getInferredCurveFromMenuItem(sender);
            if (curve != null)
            {
                IFeatureLayer fl = getLayerByLayerName(curve.LayerName);
                if (fl == null)
                    MessageBox.Show(String.Format("The layer {0} could not be found", curve.LayerName));


                IFeature feature = fl.FeatureClass.GetFeature(curve.ObjectID);

                IFeatureIdentifyObj featIdentify = new FeatureIdentifyObject();
                featIdentify.Feature = feature;
                IIdentifyObj identify = featIdentify as IIdentifyObj;
                identify.Flash(ArcMap.Document.ActivatedView.ScreenDisplay);

                Marshal.ReleaseComObject(featIdentify);
                Marshal.ReleaseComObject(feature);
            }
        }
        private void ZoomTo_Click(object sender, RoutedEventArgs e)
        {
            InferredCurve curve = getInferredCurveFromMenuItem(sender);
            if (curve != null)
            {
                IFeatureLayer fl = getLayerByLayerName(curve.LayerName);
                if (fl == null)
                    MessageBox.Show(String.Format("The layer {0} could not be found", curve.LayerName));


                IFeature feature = fl.FeatureClass.GetFeature(curve.ObjectID);


                IEnvelope extent = feature.Shape.Envelope;
                extent.Expand(1.2, 1.2, true);
                ArcMap.Document.ActivatedView.Extent = extent;
                ArcMap.Document.ActivatedView.Refresh();
                ArcMap.Document.ActivatedView.ScreenDisplay.UpdateWindow();

                IFeatureIdentifyObj featIdentify = new FeatureIdentifyObject();
                featIdentify.Feature = feature;
                IIdentifyObj identify = featIdentify as IIdentifyObj;
                identify.Flash(ArcMap.Document.ActivatedView.ScreenDisplay);

                Marshal.ReleaseComObject(featIdentify);
                Marshal.ReleaseComObject(feature);
            }
        }
        private void ZoomToRelated_Click(object sender, RoutedEventArgs e)
        {
            InferredCurve curve = getInferredCurveFromMenuItem(sender);
            if (curve != null)
            {
                IFeatureLayer fl = getLayerByLayerName(curve.LayerName);
                if (fl == null)
                    MessageBox.Show(String.Format("The layer {0} could not be found", curve.LayerName));


                IFeature feature = fl.FeatureClass.GetFeature(curve.ObjectID);


                IEnvelope extent = feature.Shape.Envelope;
                foreach (RelatedCurve rc in curve.ParallelCurves)
                {
                    IFeature relatedFeature = fl.FeatureClass.GetFeature(rc.ObjectID);
                    extent.Union(relatedFeature.Extent);
                    Marshal.ReleaseComObject(relatedFeature);
                }
                foreach (RelatedCurve rc in curve.TangentCurves)
                {
                    IFeature relatedFeature = fl.FeatureClass.GetFeature(rc.ObjectID);
                    extent.Union(relatedFeature.Extent);
                    Marshal.ReleaseComObject(relatedFeature);
                }
                extent.Expand(1.2, 1.2, true);

                ArcMap.Document.ActivatedView.Extent = extent;
                ArcMap.Document.ActivatedView.Refresh();
                ArcMap.Document.ActivatedView.ScreenDisplay.UpdateWindow();

                Marshal.ReleaseComObject(feature);
            }
        }
        private void SelectRelated_Click(object sender, RoutedEventArgs e)
        {
            InferredCurve curve = getInferredCurveFromMenuItem(sender);
            if (curve != null)
            {
                IFeatureLayer fl = getLayerByLayerName(curve.LayerName);
                if (fl == null)
                    MessageBox.Show(String.Format("The layer {0} could not be found", curve.LayerName));
                
                ArcMap.Document.FocusMap.ClearSelection();

                IFeature feature = fl.FeatureClass.GetFeature(curve.ObjectID);
                ArcMap.Document.FocusMap.SelectFeature(fl, feature);
                IEnvelope extent = feature.Shape.Envelope;

                foreach (RelatedCurve rc in curve.ParallelCurves)
                {
                    IFeature relatedFeature = fl.FeatureClass.GetFeature(rc.ObjectID);
                    ArcMap.Document.FocusMap.SelectFeature(fl, relatedFeature);
                    extent.Union(relatedFeature.Extent);
                    Marshal.ReleaseComObject(relatedFeature);
                }
                foreach (RelatedCurve rc in curve.TangentCurves)
                {
                    IFeature relatedFeature = fl.FeatureClass.GetFeature(rc.ObjectID);
                    ArcMap.Document.FocusMap.SelectFeature(fl, relatedFeature);
                    extent.Union(relatedFeature.Extent);
                    Marshal.ReleaseComObject(relatedFeature);
                }
                extent.Expand(1.2, 1.2, true);
                ArcMap.Document.ActivatedView.Extent = extent;
                ArcMap.Document.ActivatedView.Refresh();
                ArcMap.Document.ActivatedView.ScreenDisplay.UpdateWindow();

                Marshal.ReleaseComObject(feature);
            }
        }
        private void ApplyChange_Click(object sender, RoutedEventArgs e)
        {
            InferredCurve curve = getInferredCurveFromMenuItem(sender);
            if (curve != null)
            {
                IFeatureLayer fl = getLayerByLayerName(curve.LayerName);
                if (fl == null)
                    MessageBox.Show(String.Format("The layer {0} could not be found", curve.LayerName));

                CurveByInference context = new CurveByInference();
                context.FindCurves(true, string.Format("{0} = {1}", fl.FeatureClass.OIDFieldName, curve.ObjectID));
            }
        }

        InferredCurve getInferredCurveFromMenuItem(object sender)
        {
            MenuItem mi = sender as MenuItem;
            if (mi != null)
            {
                return mi.CommandParameter as InferredCurve;
            }
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

    }
}
