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
    class BoxDomainTracker : DomainTracker
    {
        private Label center_Label;
        private TextBox centerx_TextBox;
        private TextBox centery_TextBox;
        private TextBox centerz_TextBox;
        private Label width_Label;
        private TextBox width_TextBox;
        private Label height_Label;
        private TextBox height_TextBox;
        private Label depth_Label;
        private TextBox depth_TextBox;

        public BoxDomainTracker(string name, RefreshingListBox parent)
        {
            base.Initialize(name, parent);

            form.Height = 300;

            SetUpVector3DataEntry(out center_Label, "Center (x, y, z):", out centerx_TextBox, "0", out centery_TextBox, "0", out centerz_TextBox, "0", 20, 50, ref form);
            SetUpSingleDataEntry(out width_Label, "Width:", out width_TextBox, "0", 20, 80, ref form);
            SetUpSingleDataEntry(out height_Label, "Height:", out height_TextBox, "0", 20, 110, ref form);
            SetUpSingleDataEntry(out depth_Label, "Depth:", out depth_TextBox, "0", 20, 140, ref form);
        }

        protected override void UpdateProperties()
        {
            First = SetTextBoxValueToVector3(ref centerx_TextBox, ref centery_TextBox, ref centerz_TextBox, First);
            Second = SetTextBoxValueToVector3(ref width_TextBox, ref height_TextBox, ref depth_TextBox, Second);

            base.UpdateProperties();
        }

        public override void ShowDialog()
        {
            base.ShowDialog();
            centerx_TextBox.Text = "" + First.X;
            centery_TextBox.Text = "" + First.Y;
            centerz_TextBox.Text = "" + First.Z;
            width_TextBox.Text = "" + Second.X;
            height_TextBox.Text = "" + Second.Y;
            depth_TextBox.Text = "" + Second.Z;
        }

        public override string ToXML()
        {
            string xml = "";
            xml += "<domain,type=BoxDomain>\n" +
                "<property,name=Name,type=string>" + Name + "</property>\n" +
                "<property,name=Center,type=Vector3>" + First + "</property>\n" +
                "<property,name=Width,type=float>" + Second.X + "</property>\n" +
                "<property,name=Height,type=float>" + Second.Y + "</property>\n" +
                "<property,name=Depth,type=float>" + Second.Z + "</property>\n" +
                "</domain>\n";
            return xml;
        }
    }
}
