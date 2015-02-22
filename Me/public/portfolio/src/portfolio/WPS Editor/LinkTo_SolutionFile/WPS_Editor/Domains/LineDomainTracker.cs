using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WPS_Editor
{
    class LineDomainTracker : DomainTracker
    {
        private Label startpoint_Label;
        private TextBox startpointx_TextBox;
        private TextBox startpointy_TextBox;
        private TextBox startpointz_TextBox;
        private Label endpoint_Label;
        private TextBox endpointx_TextBox;
        private TextBox endpointy_TextBox;
        private TextBox endpointz_TextBox;

        public LineDomainTracker(string name, RefreshingListBox parent)
        {
            base.Initialize(name, parent);

            form.Height = 240;

            SetUpVector3DataEntry(out startpoint_Label, "StartPoint (x, y, z):", out startpointx_TextBox, "0", out startpointy_TextBox, "0", out startpointz_TextBox, "0", 20, 50, ref form);
            SetUpVector3DataEntry(out endpoint_Label, "EndPoint (x, y, z):", out endpointx_TextBox, "0", out endpointy_TextBox, "0", out endpointz_TextBox, "0", 20, 80, ref form);
        }

        protected override void UpdateProperties()
        {
            First = SetTextBoxValueToVector3(ref startpointx_TextBox, ref startpointy_TextBox, ref startpointz_TextBox, First);
            Second = SetTextBoxValueToVector3(ref endpointx_TextBox, ref endpointy_TextBox, ref endpointz_TextBox, Second);

            base.UpdateProperties();
        }

        public override void ShowDialog()
        {
            base.ShowDialog();
            startpointx_TextBox.Text = "" + First.X;
            startpointy_TextBox.Text = "" + First.Y;
            startpointz_TextBox.Text = "" + First.Z;
            endpointx_TextBox.Text = "" + Second.X;
            endpointy_TextBox.Text = "" + Second.Y;
            endpointz_TextBox.Text = "" + Second.Z;
        }

        public override string ToXML()
        {
            string xml = "";
            xml += "<domain,type=LineDomain>\n" +
                "<property,name=Name,type=string>" + Name + "</property>\n" +
                "<property,name=StartPoint,type=Vector3>" + First + "</property>\n" +
                "<property,name=EndPoint,type=Vector3>" + Second + "</property>\n" +
                "</domain>\n";
            return xml;
        }
    }
}
