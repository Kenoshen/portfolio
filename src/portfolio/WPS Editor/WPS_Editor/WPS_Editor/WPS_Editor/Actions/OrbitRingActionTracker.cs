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
    class OrbitRingActionTracker : WPSActionTracker
    {
        public Vector3 Center { get; set; }
        public Vector3 Normal { get; set; }
        public float Radius { get; set; }
        public float MaxRange { get; set; }
        public float Magnitude { get; set; }
        public float Epsilon { get; set; }
        public float TimeStep { get; set; }

        private Label center_Label;
        private TextBox centerx_TextBox;
        private TextBox centery_TextBox;
        private TextBox centerz_TextBox;
        private Label normal_Label;
        private TextBox normalx_TextBox;
        private TextBox normaly_TextBox;
        private TextBox normalz_TextBox;
        private Label radius_Label;
        private TextBox radius_TextBox;
        private Label maxrange_Label;
        private TextBox maxrange_TextBox;
        private Label magnitude_Label;
        private TextBox magnitude_TextBox;
        private Label epsilon_Label;
        private TextBox epsilon_TextBox;
        private Label timestep_Label;
        private TextBox timestep_TextBox;

        public OrbitRingActionTracker(string name, RefreshingListBox parent, bool isPerm)
        {
            perm = isPerm;
            base.Initialize(name, parent);

            form.Height = 360;

            SetUpVector3DataEntry(out center_Label, "Center (x,y,z):", out centerx_TextBox, "0", out centery_TextBox, "0", out centerz_TextBox, "0", 20, 50, ref form);
            SetUpVector3DataEntry(out normal_Label, "Normal (x,y,z):", out normalx_TextBox, "0", out normaly_TextBox, "0", out normalz_TextBox, "0", 20, 80, ref form); 
            SetUpSingleDataEntry(out radius_Label, "Radius:", out radius_TextBox, "0", 20, 110, ref form);
            SetUpSingleDataEntry(out maxrange_Label, "MaxRange:", out maxrange_TextBox, "0", 20, 140, ref form);
            SetUpSingleDataEntry(out magnitude_Label, "Magnitude:", out magnitude_TextBox, "0", 20, 170, ref form);
            SetUpSingleDataEntry(out epsilon_Label, "Epsilon:", out epsilon_TextBox, "0", 20, 200, ref form);
            SetUpSingleDataEntry(out timestep_Label, "TimeStep:", out timestep_TextBox, "0", 20, 230, ref form);

            MaxRange = 5;
            Magnitude = 1;
            Epsilon = 1;
            TimeStep = 1;

            AddButtons(230);
        }

        protected override void UpdateProperties()
        {
            Center = SetTextBoxValueToVector3(ref centerx_TextBox, ref centery_TextBox, ref centerz_TextBox, Center);
            Normal = SetTextBoxValueToVector3(ref normalx_TextBox, ref normaly_TextBox, ref normalz_TextBox, Center);
            Radius = SetTextBoxValueToFloat(ref radius_TextBox, Radius);
            MaxRange = SetTextBoxValueToFloat(ref maxrange_TextBox, MaxRange);
            Magnitude = SetTextBoxValueToFloat(ref magnitude_TextBox, Magnitude);
            Epsilon = SetTextBoxValueToFloat(ref epsilon_TextBox, Epsilon);
            TimeStep = SetTextBoxValueToFloat(ref timestep_TextBox, TimeStep);

            base.UpdateProperties();
        }

        public override void ShowDialog()
        {
            base.ShowDialog();
            centerx_TextBox.Text = "" + Center.X;
            centery_TextBox.Text = "" + Center.Y;
            centerz_TextBox.Text = "" + Center.Z;
            normalx_TextBox.Text = "" + Normal.X;
            normaly_TextBox.Text = "" + Normal.Y;
            normalz_TextBox.Text = "" + Normal.Z;
            radius_TextBox.Text = "" + Radius;
            maxrange_TextBox.Text = "" + MaxRange;
            magnitude_TextBox.Text = "" + Magnitude;
            epsilon_TextBox.Text = "" + Epsilon;
            timestep_TextBox.Text = "" + TimeStep;
        }

        public override string ToXML()
        {
            string xml = "";
            xml += "<action,type=OrbitRingAction>\n" +
                "<property,name=Name,type=string>" + Name + "</property>\n" +
                "<property,name=Center,type=Vector3>" + Center + "</property>\n" +
                "<property,name=Normal,type=Vector3>" + Normal + "</property>\n" +
                "<property,name=Radius,type=float>" + Radius + "</property>\n" +
                "<property,name=MaxRange,type=float>" + MaxRange + "</property>\n" +
                "<property,name=Magnitude,type=float>" + Magnitude + "</property>\n" +
                "<property,name=Epsilon,type=float>" + Epsilon + "</property>\n" +
                "<property,name=TimeStep,type=float>" + TimeStep + "</property>\n" + AddTemporaryXML() +
                "</action>\n";
            return xml;
        }
    }
}
