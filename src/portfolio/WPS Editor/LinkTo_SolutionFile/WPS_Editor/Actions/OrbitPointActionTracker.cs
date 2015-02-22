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
    class OrbitPointActionTracker : WPSActionTracker
    {
        public Vector3 OrbitPoint { get; set; }
        public float MaxRange { get; set; }
        public float Magnitude { get; set; }
        public float Epsilon { get; set; }
        public float TimeStep { get; set; }

        private Label orbitpoint_Label;
        private TextBox orbitpointx_TextBox;
        private TextBox orbitpointy_TextBox;
        private TextBox orbitpointz_TextBox;
        private Label maxrange_Label;
        private TextBox maxrange_TextBox;
        private Label magnitude_Label;
        private TextBox magnitude_TextBox;
        private Label epsilon_Label;
        private TextBox epsilon_TextBox;
        private Label timestep_Label;
        private TextBox timestep_TextBox;

        public OrbitPointActionTracker(string name, RefreshingListBox parent, bool isPerm)
        {
            perm = isPerm;
            base.Initialize(name, parent);

            form.Height = 300;

            SetUpVector3DataEntry(out orbitpoint_Label, "OrbitPoint (x,y,z):", out orbitpointx_TextBox, "0", out orbitpointy_TextBox, "0", out orbitpointz_TextBox, "0", 20, 50, ref form);
            SetUpSingleDataEntry(out maxrange_Label, "MaxRange:", out maxrange_TextBox, "0", 20, 80, ref form);
            SetUpSingleDataEntry(out magnitude_Label, "Magnitude:", out magnitude_TextBox, "0", 20, 110, ref form);
            SetUpSingleDataEntry(out epsilon_Label, "Epsilon:", out epsilon_TextBox, "0", 20, 140, ref form);
            SetUpSingleDataEntry(out timestep_Label, "TimeStep:", out timestep_TextBox, "0", 20, 170, ref form);

            MaxRange = 5;
            Magnitude = 1;
            Epsilon = 1;
            TimeStep = 1;

            AddButtons(170);
        }

        protected override void UpdateProperties()
        {
            OrbitPoint = SetTextBoxValueToVector3(ref orbitpointx_TextBox, ref orbitpointy_TextBox, ref orbitpointz_TextBox, OrbitPoint);
            MaxRange = SetTextBoxValueToFloat(ref maxrange_TextBox, MaxRange);
            Magnitude = SetTextBoxValueToFloat(ref magnitude_TextBox, Magnitude);
            Epsilon = SetTextBoxValueToFloat(ref epsilon_TextBox, Epsilon);
            TimeStep = SetTextBoxValueToFloat(ref timestep_TextBox, TimeStep);

            base.UpdateProperties();
        }

        public override void ShowDialog()
        {
            base.ShowDialog();
            orbitpointx_TextBox.Text = "" + OrbitPoint.X;
            orbitpointy_TextBox.Text = "" + OrbitPoint.Y;
            orbitpointz_TextBox.Text = "" + OrbitPoint.Z;
            maxrange_TextBox.Text = "" + MaxRange;
            magnitude_TextBox.Text = "" + Magnitude;
            epsilon_TextBox.Text = "" + Epsilon;
            timestep_TextBox.Text = "" + TimeStep;
        }

        public override string ToXML()
        {
            string xml = "";
            xml += "<action,type=OrbitPointAction>\n" +
                "<property,name=Name,type=string>" + Name + "</property>\n" +
                "<property,name=OrbitPoint,type=Vector3>" + OrbitPoint + "</property>\n" +
                "<property,name=MaxRange,type=float>" + MaxRange + "</property>\n" +
                "<property,name=Magnitude,type=float>" + Magnitude + "</property>\n" +
                "<property,name=Epsilon,type=float>" + Epsilon + "</property>\n" +
                "<property,name=TimeStep,type=float>" + TimeStep + "</property>\n" + AddTemporaryXML() +
                "</action>\n";
            return xml;
        }
    }
}
