using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Framework;
using ESRI.ArcGIS.esriSystem;
using System.Runtime.InteropServices;

namespace ParcelFabricCurveByInference
{
    public class myProgessor
    {
        public virtual bool Continue()
        {
            return true;
        }

        public virtual void setStepProgressorProperties(int MaxRange, String Message, int MinRange = 0, int Position = 0, int StepValue = 1)
        {
        }


        public virtual void Step()
        {
        }

    }

    public class myAOProgressor : myProgessor, IDisposable
    {
        CancelTracker cancelTracker = new CancelTracker();
        ProgressDialogFactory progressDialogFactory = new ProgressDialogFactory();
        IStepProgressor stepProgress;
        IMouseCursor pMouseCursor;

        public myAOProgressor()
        {
            pMouseCursor = new MouseCursorClass();
            pMouseCursor.SetCursor(2);
        }

        public override bool Continue()
        {
            return cancelTracker.Continue();
        }

        public override void Step()
        {
            stepProgress.Step();
        }

        public override void setStepProgressorProperties(int MaxRange, String Message, int MinRange = 0, int Position = 0, int StepValue = 1)
        {
            if (stepProgress != null)
            {
                stepProgress.Hide();
                Marshal.ReleaseComObject(stepProgress);
            }

            stepProgress = progressDialogFactory.Create(cancelTracker, 0);
            IProgressDialog2 progressDialog = (IProgressDialog2)stepProgress;
            progressDialog.Animation = esriProgressAnimationTypes.esriProgressSpiral;
            progressDialog.Title = "Inferring Curved Segments";

            stepProgress.Message = Message;
            if (MaxRange != MinRange)  //for some reason, if you set the values the same, the progressor doesn't go away
            {
                stepProgress.MinRange = MinRange;
                stepProgress.MaxRange = MaxRange;
                stepProgress.StepValue = StepValue;
            }
            stepProgress.Show();
        }

        public void Dispose()
        {
            if (pMouseCursor != null)
                Marshal.ReleaseComObject(pMouseCursor);
            if (stepProgress != null)
            {
                //Not sure if this should be disposed as a IProgressDialog2 or IStepProgressor.  I don't know if it matters
                //IProgressDialog2::HideDialog needs to be called or the dialog hangs around
                //IStepProgressor::Hide isn't enough
                ((IProgressDialog2)stepProgress).HideDialog();
                Marshal.ReleaseComObject((IProgressDialog2)stepProgress);
            }
        }
    }
}
