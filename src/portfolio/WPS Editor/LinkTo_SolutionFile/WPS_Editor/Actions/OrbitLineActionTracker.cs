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
    class OrbitLineActionTracker : WPSActionTracker
    {
        public Vector3 StartPoint { get; set; }
        public Vector3 EndPoint { get; set; }
        public float MaxRange { get; set; }
        public float Magnitude { get; set; }
        public float Epsilon { get; set; }
        public float TimeStep { get; set; }

        private Label startpoint_Label;
        private TextBox startpointx_TextBox;
        private TextBox startpointy_TextBox;
        private TextBox startpointz_TextBox;
        private Label endpoint_Label;
        private TextBox endpointx_TextBox;
        private TextBox endpointy_TextBox;
        private TextBox endpointz_TextBox;
        private Label maxrange_Label;
        private TextBox maxrange_TextBox;
        private Label magnitude_Label;
        private TextBox magnitude_TextBox;
        private Label epsilon_Label;
        private TextBox epsilon_TextBox;
        private Label timestep_Label;
        private TextBox timestep_TextBox;

        public OrbitLineActionTracker(string name, RefreshingListBox parent, bool isPerm)
        {
            perm = isPerm;
            base.Initialize(name, parent);

            form.Height = 330;

            SetUpVector3DataEntry(out startpoint_Label, "StartPoint (x,y,z):", out startpointx_TextBox, "0", out startpointy_TextBox, "0", out startpointz_TextBox, "0", 20, 50, ref form);
            SetUpVector3DataEntry(out endpoint_Label, "EndPoint (x,y,z):", out endpointx_TextBox, "0", out endpointy_TextBox, "0", out endpointz_TextBox, "0", 20, 80, ref form); 
            SetUpSingleDataEntry(out maxrange_Label, "MaxRange:", out maxrange_TextBox, "0", 20, 110, ref form);
            SetUpSingleDataEntry(out magnitude_Label, "Magnitude:", out magnitude_TextBox, "0", 20, 140, ref form);
            SetUpSingleDataEntry(out epsilon_Label, "Epsilon:", out epsilon_TextBox, "0", 20, 170, ref form);
            SetUpSingleDataEntry(out timestep_Label, "TimeStep:", out timestep_TextBox, "0", 20, 200, ref form);

            MaxRange = 5;
            Magnitude = 1;
            Epsilon = 1;
            TimeStep = 1;

            AddButtons(200);
        }

        protected override void UpdateProperties()
        {
            StartPoint = SetTextBoxValueToVector3(ref startpointx_TextBox, ref startpointy_TextBox, ref startpointz_TextBox, StartPoint);
            EndPoint = SetTextBoxValueToVector3(ref endpointx_TextBox, ref endpointy_TextBox, ref endpointz_TextBox, StartPoint);
            MaxRange = SetTextBoxValueToFloat(ref maxrange_TextBox, MaxRange);
            Magnitude = SetTextBoxValueToFloat(ref magnitude_TextBox, Magnitude);
            Epsilon = SetTextBoxValueToFloat(ref epsilon_TextBox, Epsilon);
            TimeStep = SetTextBoxValueToFloat(ref timestep_TextBox, TimeStep);

            base.UpdateProperties();
        }

        public override void ShowDialog()
        {
            base.ShowDialog();
            startpointx_TextBox.Text = "" + StartPoint.X;
            startpointy_TextBox.Text = "" + StartPoint.Y;
            startpointz_TextBox.Text = "" + StartPoint.Z;
            endpointx_TextBox.Text = "" + EndPoint.X;
            endpointy_TextBox.Text = "" + EndPoint.Y;
            endpointz_TextBox.Text = "" + EndPoint.Z;
            maxrange_TextBox.Text = "" + MaxRange;
            magnitude_TextBox.Text = "" + Magnitude;
            epsilon_TextBox.Text = "" + Epsilon;
            timestep_TextBox.Text = "" + TimeStep;
        }

        public override string ToXML()
        {
            string xml = "";
            xml += "<action,type=OrbitLineAction>\n" +
                "<property,name=Name,type=string>" + Name + "</property>\n" +
                "<property,name=StartPoint,type=Vector3>" + StartPoint + "</property>\n" +
                "<property,name=EndPoint,type=Vector3>" + EndPoint + "</property>\n" +
                "<property,name=MaxRange,type=float>" + MaxRange + "</property>\n" +
                "<property,name=Magnitude,type=float>" + Magnitude + "</property>\n" +
                "<property,name=Epsilon,type=float>" + Epsilon + "</property>\n" +
                "<property,name=TimeStep,type=float>" + TimeStep + "</property>\n" + AddTemporaryXML() +
                "</action>\n";
            return xml;
        }
    }
}
