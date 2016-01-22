using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ParcelFabricCurveByInference
{
    public class MyMessageBox
    {
        public virtual DialogResult Show(string text)
        {
            Console.WriteLine("My Message Box.Show(\"{0}\"", text);
            return DialogResult.OK;
        }

        public virtual DialogResult Show(string text, string caption)
        {
            Console.WriteLine("My Message Box.Show(\"{0}\",\"{1}\"", text, caption);
            return DialogResult.OK;
        }

        public virtual DialogResult Show(string text, string caption, MessageBoxButtons buttons)
        {
            Console.WriteLine("My Message Box.Show(\"{0}\",\"{1}\",\"{2}\"", text, caption, buttons);
            return DialogResult.OK;
        }
    }


    public class FormsMessageBox : MyMessageBox
    {
        public override DialogResult Show(string text)
        {
            return MessageBox.Show(text);
        }
        public override DialogResult Show(string text, string caption)
        {
            return MessageBox.Show(text, caption);
        }
        public override DialogResult Show(string text, string caption, MessageBoxButtons buttons)
        {
            return MessageBox.Show(text, caption, buttons);
        }
    }
}
