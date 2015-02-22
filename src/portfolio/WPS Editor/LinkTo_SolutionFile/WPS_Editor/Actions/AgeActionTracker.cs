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
    class AgeActionTracker : WPSActionTracker
    {
        public float AgeStep { get; set; }

        private Label ageStep_Label;
        private TextBox ageStep_TextBox;

        public AgeActionTracker(string name, RefreshingListBox parent, bool isPerm)
        {
            perm = isPerm;
            base.Initialize(name, parent);

            form.Height = 180;

            SetUpSingleDataEntry(out ageStep_Label, "Age Step:", out ageStep_TextBox, "1", 20, 50, ref form);

            AgeStep = 1;

            AddButtons(50);
        }

        protected override void UpdateProperties()
        {
            AgeStep = SetTextBoxValueToFloat(ref ageStep_TextBox, AgeStep);

            base.UpdateProperties();
        }

        public override void ShowDialog()
        {
            base.ShowDialog();
            ageStep_TextBox.Text = "" + AgeStep;
        }

        public override string ToXML()
        {
            string xml = "";
            xml += "<action,type=AgeAction>\n" +
                "<property,name=Name,type=string>" + Name + "</property>\n" +
                "<property,name=AgeStep,type=float>" + AgeStep + "</property>\n" + AddTemporaryXML() +
                "</action>\n";
            return xml;
        }
    }
}
