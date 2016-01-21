using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using ESRI.ArcGIS.Framework;

namespace ParcelFabricCurveByInference
{
    public class CurveByInferenceButton : ESRI.ArcGIS.Desktop.AddIns.Button
    {
        public CurveByInferenceButton()
        {
        }

        protected override void OnClick()
        {
            IDockableWindow window = CurveByInferenceExtension.GetSystemActivityWindowWindow();
            if (window != null)
                window.Show(!window.IsVisible());

            ArcMap.Application.CurrentTool = null;
        }
        protected override void OnUpdate()
        {
            Enabled = ArcMap.Application != null;
        }
    }

}
