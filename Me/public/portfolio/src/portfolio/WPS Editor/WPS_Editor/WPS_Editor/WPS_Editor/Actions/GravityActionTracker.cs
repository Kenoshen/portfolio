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
    class GravityActionTracker : WPSActionTracker
    {
        public Vector3 Gravity { get; set; }
        public float TimeStep { get; set; }

        private Label gravity_Label;
        private TextBox gravityx_TextBox;
        private TextBox gravityy_TextBox;
        private TextBox gravityz_TextBox;
        private Label timestep_Label;
        private TextBox timestep_TextBox;

        public GravityActionTracker(string name, RefreshingListBox parent, bool isPerm)
        {
            perm = isPerm;
            base.Initialize(name, parent);

            form.Height = 230;

            SetUpVector3DataEntry(out gravity_Label, "Gravity (x,y,z):", out gravityx_TextBox, "0", out gravityy_TextBox, "0", out gravityz_TextBox, "0", 20, 50, ref form); 
            SetUpSingleDataEntry(out timestep_Label, "TimeStep:", out timestep_TextBox, "0", 20, 80, ref form);

            TimeStep = 1;

            AddButtons(80);
        }

        protected override void UpdateProperties()
        {
            Gravity = SetTextBoxValueToVector3(ref gravityx_TextBox, ref gravityy_TextBox, ref gravityz_TextBox, Gravity);
            TimeStep = SetTextBoxValueToFloat(ref timestep_TextBox, TimeStep);

            base.UpdateProperties();
        }

        public override void ShowDialog()
        {
            base.ShowDialog();
            gravityx_TextBox.Text = "" + Gravity.X;
            gravityy_TextBox.Text = "" + Gravity.Y;
            gravityz_TextBox.Text = "" + Gravity.Z;
            timestep_TextBox.Text = "" + TimeStep;
        }

        public override string ToXML()
        {
            string xml = "";
            xml += "<action,type=GravityAction>\n" +
                "<property,name=Name,type=string>" + Name + "</property>\n" +
                "<property,name=Gravity,type=Vector3>" + Gravity + "</property>\n" +
                "<property,name=TimeStep,type=float>" + TimeStep + "</property>\n" + AddTemporaryXML() +
                "</action>\n";
            return xml;
        }
    }
}
