using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Xna.Framework;

namespace WPS_Editor
{
    class RotateActionTracker : WPSActionTracker
    {
        public float MinRotRevs { get; set; }
        public float MaxRotRevs { get; set; }

        private Label minrotrevs_Label;
        private TextBox minrotrevs_TextBox;
        private Label maxrotrevs_Label;
        private TextBox maxrotrevs_TextBox;

        public RotateActionTracker(string name, RefreshingListBox parent, bool isPerm)
        {
            perm = isPerm;
            base.Initialize(name, parent);

            form.Height = 210;

            SetUpSingleDataEntry(out minrotrevs_Label, "MinRotRevs:", out minrotrevs_TextBox, "0", 20, 50, ref form);
            SetUpSingleDataEntry(out maxrotrevs_Label, "MaxRotRevs:", out maxrotrevs_TextBox, "0", 20, 80, ref form);

            AddButtons(80);
        }

        protected override void UpdateProperties()
        {
            MinRotRevs = SetTextBoxValueToFloat(ref minrotrevs_TextBox, MinRotRevs);
            MaxRotRevs = SetTextBoxValueToFloat(ref maxrotrevs_TextBox, MaxRotRevs);

            base.UpdateProperties();
        }

        public override void ShowDialog()
        {
            base.ShowDialog();
            minrotrevs_TextBox.Text = "" + MinRotRevs;
            maxrotrevs_TextBox.Text = "" + MaxRotRevs;
        }

        public override string ToXML()
        {
            string xml = "";
            xml += "<action,type=RotateAction>\n" +
                "<property,name=Name,type=string>" + Name + "</property>\n" +
                "<property,name=MinRotRevs,type=float>" + MinRotRevs + "</property>\n" +
                "<property,name=MaxRotRevs,type=float>" + MaxRotRevs + "</property>\n" + AddTemporaryXML() +
                "</action>\n";
            return xml;
        }
    }
}
