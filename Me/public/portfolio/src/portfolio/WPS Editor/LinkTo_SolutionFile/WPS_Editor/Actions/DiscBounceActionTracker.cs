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
    class DiscBounceActionTracker : WPSActionTracker
    {
        public Vector3 Center { get; set; }
        public Vector3 Normal { get; set; }
        public float Radius { get; set; }
        public float Dampening { get; set; }
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
        private Label dampening_Label;
        private TextBox dampening_TextBox;
        private Label timestep_Label;
        private TextBox timestep_TextBox;

        public DiscBounceActionTracker(string name, RefreshingListBox parent, bool isPerm)
        {
            perm = isPerm;
            base.Initialize(name, parent);

            form.Height = 300;

            SetUpVector3DataEntry(out center_Label, "Center (x,y,z):", out centerx_TextBox, "0", out centery_TextBox, "0", out centerz_TextBox, "0", 20, 50, ref form);
            SetUpVector3DataEntry(out normal_Label, "Normal (x,y,z):", out normalx_TextBox, "0", out normaly_TextBox, "0", out normalz_TextBox, "0", 20, 80, ref form); 
            SetUpSingleDataEntry(out radius_Label, "Radius:", out radius_TextBox, "0", 20, 110, ref form);
            SetUpSingleDataEntry(out dampening_Label, "Dampening:", out dampening_TextBox, "0", 20, 140, ref form);
            SetUpSingleDataEntry(out timestep_Label, "TimeStep:", out timestep_TextBox, "0", 20, 170, ref form);

            Dampening = 1;
            TimeStep = 1;

            AddButtons(170);
        }

        protected override void UpdateProperties()
        {
            Center = SetTextBoxValueToVector3(ref centerx_TextBox, ref centery_TextBox, ref centerz_TextBox, Center);
            Normal = SetTextBoxValueToVector3(ref normalx_TextBox, ref normaly_TextBox, ref normalz_TextBox, Center);
            Radius = SetTextBoxValueToFloat(ref radius_TextBox, Radius);
            Dampening = SetTextBoxValueToFloat(ref dampening_TextBox, Dampening);
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
            dampening_TextBox.Text = "" + Dampening;
            timestep_TextBox.Text = "" + TimeStep;
        }

        public override string ToXML()
        {
            string xml = "";
            xml += "<action,type=DiscBounceAction>\n" +
                "<property,name=Name,type=string>" + Name + "</property>\n" +
                "<property,name=Center,type=Vector3>" + Center + "</property>\n" +
                "<property,name=Normal,type=Vector3>" + Normal + "</property>\n" +
                "<property,name=Radius,type=float>" + Radius + "</property>\n" +
                "<property,name=Dampening,type=float>" + Dampening + "</property>\n" +
                "<property,name=TimeStep,type=float>" + TimeStep + "</property>\n" + AddTemporaryXML() +
                "</action>\n";
            return xml;
        }
    }
}
