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
    class SphereDomainTracker : DomainTracker
    {
        private Label center_Label;
        private TextBox centerx_TextBox;
        private TextBox centery_TextBox;
        private TextBox centerz_TextBox;
        private Label radius_Label;
        private TextBox radius_TextBox;

        public SphereDomainTracker(string name, RefreshingListBox parent)
        {
            base.Initialize(name, parent);

            form.Height = 240;

            SetUpVector3DataEntry(out center_Label, "Center (x, y, z):", out centerx_TextBox, "0", out centery_TextBox, "0", out centerz_TextBox, "0", 20, 50, ref form);
            SetUpSingleDataEntry(out radius_Label, "Radius:", out radius_TextBox, "0", 20, 80, ref form);
        }

        protected override void UpdateProperties()
        {
            First = SetTextBoxValueToVector3(ref centerx_TextBox, ref centery_TextBox, ref centerz_TextBox, First);
            Second = new Microsoft.Xna.Framework.Vector3(SetTextBoxValueToFloat(ref radius_TextBox, Second.X), 0, 0);

            base.UpdateProperties();
        }

        public override void ShowDialog()
        {
            base.ShowDialog();
            centerx_TextBox.Text = "" + First.X;
            centery_TextBox.Text = "" + First.Y;
            centerz_TextBox.Text = "" + First.Z;
            radius_TextBox.Text = "" + Second.X;
        }

        public override string ToXML()
        {
            string xml = "";
            xml += "<domain,type=SphereDomain>\n" +
                "<property,name=Name,type=string>" + Name + "</property>\n" +
                "<property,name=Center,type=Vector3>" + First + "</property>\n" +
                "<property,name=Radius,type=float>" + Second.X + "</property>\n" +
                "</domain>\n";
            return xml;
        }
    }
}
