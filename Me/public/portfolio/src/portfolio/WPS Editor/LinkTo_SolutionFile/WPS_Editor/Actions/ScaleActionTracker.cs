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
    class ScaleActionTracker : WPSActionTracker
    {
        public float EndScale { get; set; }

        private Label endscale_Label;
        private TextBox endscale_TextBox;

        public ScaleActionTracker(string name, RefreshingListBox parent, bool isPerm)
        {
            perm = isPerm;
            base.Initialize(name, parent);

            form.Height = 180;

            SetUpSingleDataEntry(out endscale_Label, "EndScale:", out endscale_TextBox, "0", 20, 50, ref form);

            AddButtons(50);
        }

        protected override void UpdateProperties()
        {
            EndScale = SetTextBoxValueToFloat(ref endscale_TextBox, EndScale);
            base.UpdateProperties();
        }

        public override void ShowDialog()
        {
            base.ShowDialog();
            endscale_TextBox.Text = "" + EndScale;
        }

        public override string ToXML()
        {
            string xml = "";
            xml += "<action,type=ScaleAction>\n" +
                "<property,name=Name,type=string>" + Name + "</property>\n" +
                "<property,name=EndScale,type=float>" + EndScale + "</property>\n" + AddTemporaryXML() +
                "</action>\n";
            return xml;
        }
    }
}
