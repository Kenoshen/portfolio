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
    class PointDomainTracker : DomainTracker
    {
        private Label point_Label;
        private TextBox pointx_TextBox;
        private TextBox pointy_TextBox;
        private TextBox pointz_TextBox;

        public PointDomainTracker(string name, RefreshingListBox parent)
        {
            base.Initialize(name, parent);

            form.Height = 210;

            SetUpVector3DataEntry(out point_Label, "StartPoint (x, y, z):", out pointx_TextBox, "0", out pointy_TextBox, "0", out pointz_TextBox, "0", 20, 50, ref form);
        }

        protected override void UpdateProperties()
        {
            First = SetTextBoxValueToVector3(ref pointx_TextBox, ref pointy_TextBox, ref pointz_TextBox, First);

            base.UpdateProperties();
        }

        public override void ShowDialog()
        {
            base.ShowDialog();
            pointx_TextBox.Text = "" + First.X;
            pointy_TextBox.Text = "" + First.Y;
            pointz_TextBox.Text = "" + First.Z;
        }

        public override string ToXML()
        {
            string xml = "";
            xml += "<domain,type=PointDomain>\n" +
                "<property,name=Name,type=string>" + Name + "</property>\n" +
                "<property,name=Point,type=Vector3>" + First + "</property>\n" +
                "</domain>\n";
            return xml;
        }
    }
}
