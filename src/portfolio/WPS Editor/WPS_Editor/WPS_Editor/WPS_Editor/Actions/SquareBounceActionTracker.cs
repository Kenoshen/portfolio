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
    class SquareBounceActionTracker : WPSActionTracker
    {
        public Vector3 PlanePoint { get; set; }
        public Vector3 Normal { get; set; }
        public float Width { get; set; }
        public float Height { get; set; }
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
        private Label width_Label;
        private TextBox width_TextBox;
        private Label height_Label;
        private TextBox height_TextBox;
        private Label dampening_Label;
        private TextBox dampening_TextBox;
        private Label timestep_Label;
        private TextBox timestep_TextBox;

        public SquareBounceActionTracker(string name, RefreshingListBox parent, bool isPerm)
        {
            perm = isPerm;
            base.Initialize(name, parent);

            form.Height = 360;

            SetUpVector3DataEntry(out planepoint_Label, "Center (x,y,z):", out planepointx_TextBox, "0", out planepointy_TextBox, "0", out planepointz_TextBox, "0", 20, 50, ref form);
            SetUpVector3DataEntry(out normal_Label, "Normal (x,y,z):", out normalx_TextBox, "0", out normaly_TextBox, "0", out normalz_TextBox, "0", 20, 80, ref form);
            SetUpSingleDataEntry(out width_Label, "Width:", out width_TextBox, "0", 20, 110, ref form);
            SetUpSingleDataEntry(out height_Label, "Height:", out height_TextBox, "0", 20, 140, ref form);
            SetUpSingleDataEntry(out dampening_Label, "Dampening:", out dampening_TextBox, "0", 20, 170, ref form);
            SetUpSingleDataEntry(out timestep_Label, "TimeStep:", out timestep_TextBox, "0", 20, 200, ref form);

            Dampening = 1;
            TimeStep = 1;

            AddButtons(200);
        }

        protected override void UpdateProperties()
        {
            PlanePoint = SetTextBoxValueToVector3(ref planepointx_TextBox, ref planepointy_TextBox, ref planepointz_TextBox, PlanePoint);
            Normal = SetTextBoxValueToVector3(ref normalx_TextBox, ref normaly_TextBox, ref normalz_TextBox, PlanePoint);
            Width = SetTextBoxValueToFloat(ref width_TextBox, Width);
            Height = SetTextBoxValueToFloat(ref height_TextBox, Height);
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
            width_TextBox.Text = "" + Width;
            height_TextBox.Text = "" + Height;
            dampening_TextBox.Text = "" + Dampening;
            timestep_TextBox.Text = "" + TimeStep;
        }

        public override string ToXML()
        {
            string xml = "";
            xml += "<action,type=SquareBounceAction>\n" +
                "<property,name=Name,type=string>" + Name + "</property>\n" +
                "<property,name=PlanePoint,type=Vector3>" + PlanePoint + "</property>\n" +
                "<property,name=Normal,type=Vector3>" + Normal + "</property>\n" +
                "<property,name=Width,type=float>" + Width + "</property>\n" +
                "<property,name=Height,type=float>" + Height + "</property>\n" +
                "<property,name=Dampening,type=float>" + Dampening + "</property>\n" +
                "<property,name=TimeStep,type=float>" + TimeStep + "</property>\n" + AddTemporaryXML() +
                "</action>\n";
            return xml;
        }
    }
}
