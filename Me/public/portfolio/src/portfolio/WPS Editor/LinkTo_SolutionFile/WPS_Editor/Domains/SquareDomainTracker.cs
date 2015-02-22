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
    class SquareDomainTracker : DomainTracker
    {
        private Label center_Label;
        private TextBox centerx_TextBox;
        private TextBox centery_TextBox;
        private TextBox centerz_TextBox;
        private Label normal_Label;
        private TextBox normalx_TextBox;
        private TextBox normaly_TextBox;
        private TextBox normalz_TextBox;
        private Label width_Label;
        private TextBox width_TextBox;
        private Label height_Label;
        private TextBox height_TextBox;

        public SquareDomainTracker(string name, RefreshingListBox parent)
        {
            base.Initialize(name, parent);

            form.Height = 300;

            SetUpVector3DataEntry(out center_Label, "Center (x, y, z):", out centerx_TextBox, "0", out centery_TextBox, "0", out centerz_TextBox, "0", 20, 50, ref form);
            SetUpVector3DataEntry(out normal_Label, "Normal (x, y, z):", out normalx_TextBox, "0", out normaly_TextBox, "0", out normalz_TextBox, "0", 20, 80, ref form);
            SetUpSingleDataEntry(out width_Label, "Width:", out width_TextBox, "0", 20, 110, ref form);
            SetUpSingleDataEntry(out height_Label, "Height:", out height_TextBox, "0", 20, 140, ref form);
        }

        protected override void UpdateProperties()
        {
            First = SetTextBoxValueToVector3(ref centerx_TextBox, ref centery_TextBox, ref centerz_TextBox, First);
            Second = SetTextBoxValueToVector3(ref normalx_TextBox, ref normaly_TextBox, ref normalz_TextBox, Second);
            Third = new Microsoft.Xna.Framework.Vector3(SetTextBoxValueToFloat(ref width_TextBox, Third.X), SetTextBoxValueToFloat(ref width_TextBox, Third.X), 0);

            base.UpdateProperties();
        }

        public override void ShowDialog()
        {
            base.ShowDialog();
            centerx_TextBox.Text = "" + First.X;
            centery_TextBox.Text = "" + First.Y;
            centerz_TextBox.Text = "" + First.Z;
            normalx_TextBox.Text = "" + Second.X;
            normaly_TextBox.Text = "" + Second.Y;
            normalz_TextBox.Text = "" + Second.Z;
            width_TextBox.Text = "" + Third.X;
            height_TextBox.Text = "" + Third.Y;
        }

        public override string ToXML()
        {
            string xml = "";
            xml += "<domain,type=SquareDomain>\n" +
                "<property,name=Name,type=string>" + Name + "</property>\n" +
                "<property,name=Center,type=Vector3>" + First + "</property>\n" +
                "<property,name=Normal,type=Vector3>" + Second + "</property>\n" +
                "<property,name=Width,type=float>" + Third.X + "</property>\n" +
                "<property,name=Height,type=float>" + Third.Y + "</property>\n" +
                "</domain>\n";
            return xml;
        }
    }
}
