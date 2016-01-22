using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using ESRI.ArcGIS.Framework;
using ESRI.ArcGIS.esriSystem;

namespace ParcelFabricCurveByInference
{
    public class CurveByInferenceExtension : ESRI.ArcGIS.Desktop.AddIns.Extension
    {
        private static IExtension s_extension;
        private static IDockableWindow s_CurveByInfrenceWindow;

        public CurveByInferenceExtension()
        {            
            //update the dockable window so that it has the correct version
            UID dockWinID = new UIDClass();
            dockWinID.Value = ThisAddIn.IDs.CurveByInferenceWindow;
            s_CurveByInfrenceWindow = ArcMap.DockableWindowManager.GetDockableWindow(dockWinID);
            s_CurveByInfrenceWindow.Caption = String.Format("{0} ({1})", s_CurveByInfrenceWindow.Caption, ThisAddIn.Version);
        }

        protected override void OnStartup()
        {
            //
            // TODO: Uncomment to start listening to document events
            //
            // WireDocumentEvents();


        }

        private void WireDocumentEvents()
        {
            //
            // TODO: Sample document event wiring code. Change as needed
            //

            // Named event handler
            ArcMap.Events.NewDocument += delegate() { ArcMap_NewDocument(); };

            // Anonymous event handler
            ArcMap.Events.BeforeCloseDocument += delegate()
            {
                // Return true to stop document from closing
                ESRI.ArcGIS.Framework.IMessageDialog msgBox = new ESRI.ArcGIS.Framework.MessageDialogClass();
                return msgBox.DoModal("BeforeCloseDocument Event", "Abort closing?", "Yes", "No", ArcMap.Application.hWnd);
            };

        }

        internal static IExtension GetExtension()
        {
            if (s_extension == null)
            {
                UID extID = new UIDClass();
                extID.Value = ThisAddIn.IDs.CurveByInferenceExtension;
                s_extension = ArcMap.Application.FindExtensionByCLSID(extID);
            }
            return s_extension;
        }

        internal static IDockableWindow GetSystemActivityWindowWindow()
        {
            if (s_extension == null)
                GetExtension();

            if (s_CurveByInfrenceWindow == null)
            {
                UID dockWinID = new UIDClass();
                dockWinID.Value = ThisAddIn.IDs.CurveByInferenceWindow;
                s_CurveByInfrenceWindow = ArcMap.DockableWindowManager.GetDockableWindow(dockWinID);

                s_CurveByInfrenceWindow.Caption = String.Format("{0} ({1})", s_CurveByInfrenceWindow.Caption, ThisAddIn.Version);
            }
            return s_CurveByInfrenceWindow;
        }

        void ArcMap_NewDocument()
        {
            // TODO: Handle new document event
        }

    }

}
