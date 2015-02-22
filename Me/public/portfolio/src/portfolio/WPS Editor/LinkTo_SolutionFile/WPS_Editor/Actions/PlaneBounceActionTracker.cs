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
    class PlaneBounceActionTracker : WPSActionTracker
    {
        public Vector3 PlanePoint { get; set; }
        public Vector3 Normal { get; set; }
        public float Dampening { get; set; }
        public float TimeStep { get; set; }

        private Label planepoint_Label;
        private TextBox planepointx_TextBox;
        private TextBox planepointy_TextBox;
        private TextBox planepointz_TextBox;
        private Label normal_Label;
        private TextBox normalx_TextBox;
        private TextBox normaly_TextBox;
        private TextBox normalz_TextBox;
        private Label dampening_Label;
        private TextBox dampening_TextBox;
        private Label timestep_Label;
        private TextBox timestep_TextBox;

        public PlaneBounceActionTracker(string name, RefreshingListBox parent, bool isPerm)
        {
            perm = isPerm;
            base.Initialize(name, parent);

            form.Height = 300;

            SetUpVector3DataEntry(out planepoint_Label, "Center (x,y,z):", out planepointx_TextBox, "0", out planepointy_TextBox, "0", out planepointz_TextBox, "0", 20, 50, ref form);
            SetUpVector3DataEntry(out normal_Label, "Normal (x,y,z):", out normalx_TextBox, "0", out normaly_TextBox, "0", out normalz_TextBox, "0", 20, 80, ref form); 
            SetUpSingleDataEntry(out dampening_Label, "Dampening:", out dampening_TextBox, "0", 20, 110, ref form);
            SetUpSingleDataEntry(out timestep_Label, "TimeStep:", out timestep_TextBox, "0", 20, 140, ref form);

            Dampening = 1;
            TimeStep = 1;

            AddButtons(140);
        }

        protected override void UpdateProperties()
        {
            PlanePoint = SetTextBoxValueToVector3(ref planepointx_TextBox, ref planepointy_TextBox, ref planepointz_TextBox, PlanePoint);
            Normal = SetTextBoxValueToVector3(ref normalx_TextBox, ref normaly_TextBox, ref normalz_TextBox, PlanePoint);
            Dampening = SetTextBoxValueToFloat(ref dampening_TextBox, Dampening);
            TimeStep = SetTextBoxValueToFloat(ref timestep_TextBox, TimeStep);

            base.UpdateProperties();
        }

        public override void ShowDialog()
        {
            base.ShowDialog();
            planepointx_TextBox.Text = "" + PlanePoint.X;
            planepointy_TextBox.Text = "" + PlanePoint.Y;
            planepointz_TextBox.Text = "" + PlanePoint.Z;
            normalx_TextBox.Text = "" + Normal.X;
            normaly_TextBox.Text = "" + Normal.Y;
            normalz_TextBox.Text = "" + Normal.Z;
            dampening_TextBox.Text = "" + Dampening;
            timestep_TextBox.Text = "" + TimeStep;
        }

        public override string ToXML()
        {
            string xml = "";
            xml += "<action,type=PlaneBounceAction>\n" +
                "<property,name=Name,type=string>" + Name + "</property>\n" +
                "<property,name=PlanePoint,type=Vector3>" + PlanePoint + "</property>\n" +
                "<property,name=Normal,type=Vector3>" + Normal + "</property>\n" +
                "<property,name=Dampening,type=float>" + Dampening + "</property>\n" +
                "<property,name=TimeStep,type=float>" + TimeStep + "</property>\n" + AddTemporaryXML() +
                "</action>\n";
            return xml;
        }
    }
}
