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
    public abstract class WPSActionTracker
    {
        public string Name { get; set; }
        protected SuperForm form;
        protected Label nameLabel;
        protected TextBox nameTextBox;
        protected Button ok_Bttn;
        protected Button apply_Bttn;
        protected Button cancel_Bttn;
        private RefreshingListBox Parent;
        protected bool perm = true;
        public int FrameAdded { get; set; }
        public int FrameRemoved { get; set; }
        protected Label frameadded_Label;
        protected TextBox frameadded_TextBox;
        protected Label frameremoved_Label;
        protected TextBox frameremoved_TextBox;

        protected void Initialize(string name, RefreshingListBox parent)
        {
            Name = name;
            Parent = parent;

            form = new SuperForm();
            form.MinimizeBox = true;
            form.MaximizeBox = false;
            form.DisableCloseButton();
            form.FormBorderStyle = FormBorderStyle.FixedDialog;
            form.Visible = false;
            form.Width = 300;
            form.Height = 400;

            nameLabel = new Label();
            nameLabel.Text = "Action Name:";
            nameLabel.Location = new Point(20, 20);

            nameTextBox = new TextBox();
            nameTextBox.Text = name;
            nameTextBox.Location = new Point(nameLabel.Size.Width + 25, 18);
            nameTextBox.ReadOnly = false;

            ok_Bttn = new Button();
            ok_Bttn.Text = "Ok";
            ok_Bttn.Dock = DockStyle.Bottom;
            ok_Bttn.Location = new Point(20, form.Height - 70);
            ok_Bttn.Click += (s, e) =>
            {
                Ok_Clicked();
            };
            apply_Bttn = new Button();
            apply_Bttn.Text = "Apply";
            apply_Bttn.Dock = DockStyle.Bottom;
            apply_Bttn.Location = new Point(form.Width / 2 - apply_Bttn.Width / 2, form.Height - 70);
            apply_Bttn.Click += (s, e) =>
                {
                    Apply_Clicked();
                };
            cancel_Bttn = new Button();
            cancel_Bttn.Text = "Cancel";
            cancel_Bttn.Dock = DockStyle.Bottom;
            cancel_Bttn.Location = new Point(form.Width - 20 - cancel_Bttn.Width, form.Height - 70);
            cancel_Bttn.Click += (s, e) =>
            {
                Cancel_Clicked();
            };

            form.Controls.Add(nameLabel);
            form.Controls.Add(nameTextBox);
        }

        protected void AddButtons(int lastY)
        {
            if (!perm)
            {
                form.Height += 60;
                SetUpSingleDataEntry(out frameadded_Label, "FrameAdded:", out frameadded_TextBox, "0", 20, lastY + 30, ref form);
                SetUpSingleDataEntry(out frameremoved_Label, "FrameRemoved:", out frameremoved_TextBox, "0", 20, lastY + 60, ref form);
            }

            form.Controls.Add(ok_Bttn);
            form.Controls.Add(apply_Bttn);
            form.Controls.Add(cancel_Bttn);
        }

        protected virtual void Ok_Clicked()
        {
            UpdateProperties();
            HideDialog();

            Parent.RefreshItems();
        }

        protected virtual void Apply_Clicked()
        {
            UpdateProperties();
            form.Text = Name;

            Parent.RefreshItems();
        }

        protected virtual void UpdateProperties()
        {
            Name = nameTextBox.Text;

            if (!perm)
            {
                FrameAdded = (int)SetTextBoxValueToFloat(ref frameadded_TextBox, FrameAdded);
                FrameRemoved = (int)SetTextBoxValueToFloat(ref frameremoved_TextBox, FrameRemoved);
            }
        }

        protected virtual void Cancel_Clicked()
        {
            HideDialog();
        }

        private void HideDialog()
        {
            form.Visible = false;
        }

        public virtual void ShowDialog()
        {
            form.Visible = true;
            form.Focus();
            form.Text = Name;
            nameTextBox.Text = Name;
        }

        public override string ToString()
        {
            return Name;
        }

        public virtual string ToXML()
        {
            return "< Not implemented />";
        }

        protected string AddTemporaryXML()
        {
            if (!perm)
                return "<property,name=FrameAdded,type=int>" + FrameAdded + "</property>\n" + "<property,name=FrameRemoved,type=int>" + FrameRemoved + "</property>\n";
            return "";
        }

        protected void SetUpSingleDataEntry(out Label l, string lText, out TextBox t, string tText, int x, int y, ref SuperForm f)
        {
            l = new Label();
            l.Location = new Point(x, y);
            l.Text = lText;

            t = new TextBox();
            t.Location = new Point(l.Width + 25, l.Location.Y - 2);
            t.Text = tText;

            f.Controls.Add(l);
            f.Controls.Add(t);
        }

        protected void SetUpVector3DataEntry(out Label l, string lText, out TextBox tx, string txText, out TextBox ty, string tyText, out TextBox tz, string tzText, int x, int y, ref SuperForm f)
        {
            l = new Label();
            l.Location = new Point(x, y);
            l.Text = lText;

            tx = new TextBox();
            tx.Location = new Point(l.Width + 25, l.Location.Y - 2);
            tx.Text = txText;
            tx.Width = 50;

            ty = new TextBox();
            ty.Location = new Point(tx.Width + 5 + l.Width + 25, l.Location.Y - 2);
            ty.Text = tyText;
            ty.Width = 50;

            tz = new TextBox();
            tz.Location = new Point(tx.Width + 5 + ty.Width + 5 + l.Width + 25, l.Location.Y - 2);
            tz.Text = tzText;
            tz.Width = 50;

            f.Controls.Add(l);
            f.Controls.Add(tx);
            f.Controls.Add(ty);
            f.Controls.Add(tz);
        }

        protected float SetTextBoxValueToFloat(ref TextBox t, float data)
        {
            try
            {
                data = (float)Decimal.Parse(t.Text);
            }
            catch (Exception)
            {
                // If it gets here the value stays the same
                //data = data;
            }

            return data;
        }

        protected Vector3 SetTextBoxValueToVector3(ref TextBox tx, ref TextBox ty, ref TextBox tz, Vector3 data)
        {
            try
            {
                data.X = (float)Decimal.Parse(tx.Text);
            }
            catch (Exception)
            {
                // If it gets here the value stays the same
            }
            try
            {
                data.Y = (float)Decimal.Parse(ty.Text);
            }
            catch (Exception)
            {
                // If it gets here the value stays the same
            }
            try
            {
                data.Z = (float)Decimal.Parse(tz.Text);
            }
            catch (Exception)
            {
                // If it gets here the value stays the same
            }

            return data;
        }
    }
}
