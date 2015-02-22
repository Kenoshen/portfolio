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
    using Point = System.Drawing.Point;
    class SourceActionTracker : WPSActionTracker
    {
        public string PositionDomain { get; set; }
        public string VelocityDomain { get; set; }
        public float Size { get; set; }
        public float Particle_Rate { get; set; }
        public float TimeStep { get; set; }

        private Label pos_Label;
        private ComboBox pos_DropDown;
        private Label vel_Label;
        private ComboBox vel_DropDown;
        private Label size_Label;
        private TextBox size_TextBox;
        private Label particlerate_Label;
        private TextBox particlerate_TextBox;
        private Label timestep_Label;
        private TextBox timestep_TextBox;

        private RefreshingListBox Domains;

        public SourceActionTracker(string name, RefreshingListBox parent, RefreshingListBox domains, bool isPerm)
        {
            perm = isPerm;
            base.Initialize(name, parent);
            Domains = domains;

            form.Height = 300;

            string[] options = GetDropDownOptions();
            SetUpDropDown(out pos_Label, "PositionDomain:", out pos_DropDown, options, 20, 50, ref form);
            SetUpDropDown(out vel_Label, "VelocityDomain:", out vel_DropDown, options, 20, 80, ref form);
            SetUpSingleDataEntry(out size_Label, "Size:", out size_TextBox, "0", 20, 110, ref form);
            SetUpSingleDataEntry(out particlerate_Label, "Particle_Rate:", out particlerate_TextBox, "0", 20, 140, ref form);
            SetUpSingleDataEntry(out timestep_Label, "TimeStep:", out timestep_TextBox, "0", 20, 170, ref form);

            Size = 1;
            Particle_Rate = 1;
            TimeStep = 1;

            AddButtons(170);
        }

        protected override void UpdateProperties()
        {
            PositionDomain = SetDropDownValueToString(ref pos_DropDown, PositionDomain);
            VelocityDomain = SetDropDownValueToString(ref vel_DropDown, VelocityDomain);
            Size = SetTextBoxValueToFloat(ref size_TextBox, Size);
            Particle_Rate = SetTextBoxValueToFloat(ref particlerate_TextBox, Particle_Rate);
            TimeStep = SetTextBoxValueToFloat(ref timestep_TextBox, TimeStep);

            base.UpdateProperties();
        }

        public override void ShowDialog()
        {
            base.ShowDialog();
            string[] options = GetDropDownOptions();
            UpdateDropDown(ref pos_DropDown, options);
            UpdateDropDown(ref vel_DropDown, options);
            size_TextBox.Text = "" + Size;
            particlerate_TextBox.Text = "" + Particle_Rate;
            timestep_TextBox.Text = "" + TimeStep;
        }

        public override string ToXML()
        {
            string xml = "";
            xml += "<action,type=SourceAction>\n" +
                "<property,name=Name,type=string>" + Name + "</property>\n" +
                "<property,name=PositionDomain,type=string>" + PositionDomain + "</property>\n" +
                "<property,name=VelocityDomain,type=string>" + VelocityDomain + "</property>\n" +
                "<property,name=Size,type=float>" + Size + "</property>\n" +
                "<property,name=Particle_Rate,type=float>" + Particle_Rate + "</property>\n" +
                "<property,name=TimeStep,type=float>" + TimeStep + "</property>\n" + AddTemporaryXML() +
                "</action>\n";
            return xml;
        }

        private string[] GetDropDownOptions()
        {
            string[] options = new string[Domains.Items.Count];

            for (int i = 0; i < options.Length; i++)
                options[i] = Domains.Items[i].ToString();

            return options;
        }

        private void UpdateDropDown(ref ComboBox c, string[] ct)
        {
            for (int i = 0; i < c.Items.Count; i++)
            {
                c.Items.RemoveAt(i);
                i--;
            }

            if (ct.Length > 0)
                for (int i = 0; i < ct.Length; i++)
                    c.Items.Add(ct[i]);
            else
                c.Items.Add("You must add a domain first");
        }

        private void SetUpDropDown(out Label l, string lt, out ComboBox c, string[] ct, int x, int y, ref SuperForm f)
        {
            l = new Label();
            l.Location = new Point(x, y);
            l.Text = lt;

            c = new ComboBox();
            c.Location = new Point(l.Width + 25, l.Location.Y - 2);
            if (ct.Length > 0)
                for (int i = 0; i < ct.Length; i++)
                    c.Items.Add(ct[i]);
            else
                c.Items.Add("You must add a domain first");

            f.Controls.Add(l);
            f.Controls.Add(c);
        }

        private string SetDropDownValueToString(ref ComboBox c, string v)
        {
            try
            {
                v = c.SelectedItem.ToString();
            }
            catch (Exception)
            {
                // do nothing
            }
            return v;
        }
    }
}
