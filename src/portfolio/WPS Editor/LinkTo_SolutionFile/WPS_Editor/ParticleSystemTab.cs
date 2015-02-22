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
    public class ParticleSystemTab : TabPage
    {
        #region Name
        Point namePoint;
        Label nameLabel;
        public TextBox nameTextBox;
        Label maxpLabel;
        public TextBox maxpTextbox;
        Label partlLabel;
        public TextBox partlTextbox;
        Label textureLabel;
        public TextBox textureTextbox;
        Label visLabel;
        public ComboBox visDropDown;
        #endregion

        #region Domains
        Label domain_Label;
        public ComboBox add_Domain_Dropdown;
        public Button add_Domain_Button;
        public Button remove_Domain_Button;
        public RefreshingListBox domain_List;
        Point domain_Point;
        int domain_Added = 0;
        #endregion

        #region Permanent
        Label perm_Label;
        public ComboBox add_Perm_Dropdown;
        public Button add_Perm_Button;
        public Button remove_Perm_Button;
        public RefreshingListBox perm_List;
        Point perm_Point;
        int perm_Added = 0;
        #endregion

        #region Temporary
        Label temp_Label;
        public ComboBox add_Temp_Dropdown;
        public Button add_Temp_Button;
        public Button remove_Temp_Button;
        public RefreshingListBox temp_List;
        Point temp_Point;
        Label totalframes_Label;
        public TextBox totalframes_Textbox;
        Label loop_Label;
        public RadioButton loop_YES_Radio;
        public RadioButton loop_NO_Radio;
        int temp_Added = 0;
        #endregion

        public ParticleSystemTab(string name)
        {
            #region Name
            this.Text = name;
            this.Name = name;
            namePoint = new Point(20, 7);
            nameLabel = new Label();
            nameLabel.Text = "Particle System Name: ";
            nameLabel.AutoSize = true;
            nameLabel.Location = namePoint;
            nameTextBox = new TextBox();
            nameTextBox.Text = name;
            nameTextBox.Width = 170;
            namePoint.X += nameLabel.Size.Width + 25;
            namePoint.Y -= 2;
            nameTextBox.Location = namePoint;
            nameTextBox.TextChanged += (s, e) => 
                {
                    this.Name = nameTextBox.Text;
                    this.Text = nameTextBox.Text;
                };

            this.Controls.Add(nameLabel);
            this.Controls.Add(nameTextBox);

            maxpLabel = new Label();
            maxpLabel.Location = new Point(20, 27);
            maxpLabel.Text = "MaxParticles:";
            maxpLabel.Height = 13;
            maxpTextbox = new TextBox();
            maxpTextbox.Location = new Point(20 + maxpLabel.Width + 25, maxpLabel.Location.Y - 2);
            maxpTextbox.Text = "100";
            this.Controls.Add(maxpLabel);
            this.Controls.Add(maxpTextbox);

            partlLabel = new Label();
            partlLabel.Location = new Point(20, 47);
            partlLabel.Text = "ParticleLife:";
            partlLabel.Height = 13;
            partlTextbox = new TextBox();
            partlTextbox.Location = new Point(20 + partlLabel.Width + 25, partlLabel.Location.Y - 2);
            partlTextbox.Text = "10";
            this.Controls.Add(partlLabel);
            this.Controls.Add(partlTextbox);

            textureLabel = new Label();
            textureLabel.Location = new Point(20, 67);
            textureLabel.Text = "TextureFilePath:";
            textureLabel.Height = 13;
            textureTextbox = new TextBox();
            textureTextbox.Location = new Point(20 + textureLabel.Width + 25, textureLabel.Location.Y - 2);
            textureTextbox.Text = "WPS_Content/SPRITES/fire";
            textureTextbox.Width = 170;
            this.Controls.Add(textureLabel);
            this.Controls.Add(textureTextbox);

            visLabel = new Label();
            visLabel.Location = new Point(20, 87);
            visLabel.Text = "Visability:";
            visLabel.Height = 13;
            visDropDown = new ComboBox();
            visDropDown.Location = new Point(20 + visLabel.Width + 25, visLabel.Location.Y - 2);
            visDropDown.Text = "ALPHA";
            visDropDown.Items.Add("ALPHA");
            visDropDown.Items.Add("OPAQUE");
            this.Controls.Add(visLabel);
            this.Controls.Add(visDropDown);
            #endregion

            #region Domains
            domain_Point = new Point(10, 115);

            domain_Label = new Label();
            domain_Label.Location = domain_Point;
            domain_Label.Height = 20;
            domain_Label.Text = "Domains:";
            domain_Point.Y += 20;

            add_Domain_Dropdown = new ComboBox();
            add_Domain_Dropdown.Location = domain_Point;
            add_Domain_Dropdown.Items.Add("Box");
            add_Domain_Dropdown.Items.Add("Cylinder");
            add_Domain_Dropdown.Items.Add("Disc");
            add_Domain_Dropdown.Items.Add("Line");
            add_Domain_Dropdown.Items.Add("Point");
            add_Domain_Dropdown.Items.Add("Ring");
            add_Domain_Dropdown.Items.Add("Sphere");
            add_Domain_Dropdown.Items.Add("Square");

            domain_List = new RefreshingListBox();
            domain_Point.Y += 30;
            domain_List.Location = domain_Point;
            domain_List.Width = 300;
            domain_List.Height = 75;
            domain_List.DisplayMember = "DisplayText";
            domain_List.ValueMember = "DisplayText";
            domain_List.DoubleClick += (s, e) =>
            {
                if (domain_List.SelectedItem != null)
                {
                    ((DomainTracker)domain_List.SelectedItem).ShowDialog();
                    domain_List.RefreshItems();
                }
            };

            add_Domain_Button = new Button();
            domain_Point.X += 130;
            domain_Point.Y -= 30;
            add_Domain_Button.Location = domain_Point;
            add_Domain_Button.Text = "Add Domain";
            add_Domain_Button.Click += (s, e) =>
            {
                if (add_Domain_Dropdown.SelectedItem != null)
                {
                    switch (add_Domain_Dropdown.SelectedItem.ToString())
                    {
                        case "Box":
                            domain_List.Items.Add(new BoxDomainTracker(add_Domain_Dropdown.SelectedItem.ToString() + domain_Added, domain_List));
                            domain_Added++;
                            break;

                        case "Cylinder":
                            domain_List.Items.Add(new CylinderDomainTracker(add_Domain_Dropdown.SelectedItem.ToString() + domain_Added, domain_List));
                            domain_Added++;
                            break;

                        case "Disc":
                            domain_List.Items.Add(new DiscDomainTracker(add_Domain_Dropdown.SelectedItem.ToString() + domain_Added, domain_List));
                            domain_Added++;
                            break;

                        case "Line":
                            domain_List.Items.Add(new LineDomainTracker(add_Domain_Dropdown.SelectedItem.ToString() + domain_Added, domain_List));
                            domain_Added++;
                            break;

                        case "Point":
                            domain_List.Items.Add(new PointDomainTracker(add_Domain_Dropdown.SelectedItem.ToString() + domain_Added, domain_List));
                            domain_Added++;
                            break;

                        case "Ring":
                            domain_List.Items.Add(new RingDomainTracker(add_Domain_Dropdown.SelectedItem.ToString() + domain_Added, domain_List));
                            domain_Added++;
                            break;

                        case "Sphere":
                            domain_List.Items.Add(new SphereDomainTracker(add_Domain_Dropdown.SelectedItem.ToString() + domain_Added, domain_List));
                            domain_Added++;
                            break;
                            
                        case "Square":
                            domain_List.Items.Add(new SquareDomainTracker(add_Domain_Dropdown.SelectedItem.ToString() + domain_Added, domain_List));
                            domain_Added++;
                            break;
                    }
                }
            };

            remove_Domain_Button = new Button();
            domain_Point.X += 95;
            remove_Domain_Button.Location = domain_Point;
            remove_Domain_Button.Text = "Remove Domain";
            remove_Domain_Button.Click += (s, e) =>
            {
                if (domain_List.SelectedItem != null)
                {
                    domain_List.Items.Remove(domain_List.SelectedItem);
                }
            };

            this.Controls.Add(domain_Label);
            this.Controls.Add(add_Domain_Dropdown);
            this.Controls.Add(add_Domain_Button);
            this.Controls.Add(remove_Domain_Button);
            this.Controls.Add(domain_List);
            #endregion

            #region Permanent
            perm_Point = new Point(10, 260);

            perm_Label = new Label();
            perm_Label.Location = perm_Point;
            perm_Label.Height = 20;
            perm_Label.Text = "Permanent Actions:";
            perm_Point.Y += 20;

            add_Perm_Dropdown = new ComboBox();
            add_Perm_Dropdown.Location = perm_Point;
            add_Perm_Dropdown.Items.Add("Age");
            add_Perm_Dropdown.Items.Add("DiscBounce");
            add_Perm_Dropdown.Items.Add("Fade");
            add_Perm_Dropdown.Items.Add("Gravity");
            add_Perm_Dropdown.Items.Add("KillAll");
            add_Perm_Dropdown.Items.Add("Move");
            add_Perm_Dropdown.Items.Add("OrbitAxis");
            add_Perm_Dropdown.Items.Add("OrbitLine");
            add_Perm_Dropdown.Items.Add("OrbitPoint");
            add_Perm_Dropdown.Items.Add("OrbitRing");
            add_Perm_Dropdown.Items.Add("PlaneBounce");
            add_Perm_Dropdown.Items.Add("Rotate");
            add_Perm_Dropdown.Items.Add("Scale");
            add_Perm_Dropdown.Items.Add("Source");
            add_Perm_Dropdown.Items.Add("SquareBounce");
            add_Perm_Dropdown.Items.Add("ZeroVel");
            
            perm_List = new RefreshingListBox();
            perm_Point.Y += 30;
            perm_List.Location = perm_Point;
            perm_List.Width = 300;
            perm_List.Height = 130;
            perm_List.DisplayMember = "DisplayText";
            perm_List.ValueMember = "DisplayText";
            perm_List.DoubleClick += (s, e) =>
                {
                    if (perm_List.SelectedItem != null)
                    {
                        ((WPSActionTracker)perm_List.SelectedItem).ShowDialog();
                        perm_List.RefreshItems();
                    }
                };
            
            add_Perm_Button = new Button();
            perm_Point.X += 130;
            perm_Point.Y -= 30;
            add_Perm_Button.Location = perm_Point;
            add_Perm_Button.Text = "Add Action";
            add_Perm_Button.Click += (s, e) =>
                {
                    if (add_Perm_Dropdown.SelectedItem != null)
                    {
                        switch (add_Perm_Dropdown.SelectedItem.ToString())
                        {
                            case "Age":
                                perm_List.Items.Add(new AgeActionTracker(add_Perm_Dropdown.SelectedItem.ToString() + perm_Added, perm_List, true));
                                perm_Added++;
                                break;
                            
                            case "DiscBounce":
                                perm_List.Items.Add(new DiscBounceActionTracker(add_Perm_Dropdown.SelectedItem.ToString() + perm_Added, perm_List, true));
                                perm_Added++;
                                break;

                            case "Fade":
                                perm_List.Items.Add(new FadeActionTracker(add_Perm_Dropdown.SelectedItem.ToString() + perm_Added, perm_List, true));
                                perm_Added++;
                                break;

                            case "Gravity":
                                perm_List.Items.Add(new GravityActionTracker(add_Perm_Dropdown.SelectedItem.ToString() + perm_Added, perm_List, true));
                                perm_Added++;
                                break;

                            case "KillAll":
                                perm_List.Items.Add(new KillAllActionTracker(add_Perm_Dropdown.SelectedItem.ToString() + perm_Added, perm_List, true));
                                perm_Added++;
                                break;

                            case "Move":
                                perm_List.Items.Add(new MoveActionTracker(add_Perm_Dropdown.SelectedItem.ToString() + perm_Added, perm_List, true));
                                perm_Added++;
                                break;

                            case "OrbitAxis":
                                perm_List.Items.Add(new OrbitAxisActionTracker(add_Perm_Dropdown.SelectedItem.ToString() + perm_Added, perm_List, true));
                                perm_Added++;
                                break;

                            case "OrbitLine":
                                perm_List.Items.Add(new OrbitLineActionTracker(add_Perm_Dropdown.SelectedItem.ToString() + perm_Added, perm_List, true));
                                perm_Added++;
                                break;

                            case "OrbitPoint":
                                perm_List.Items.Add(new OrbitPointActionTracker(add_Perm_Dropdown.SelectedItem.ToString() + perm_Added, perm_List, true));
                                perm_Added++;
                                break;

                            case "OrbitRing":
                                perm_List.Items.Add(new OrbitRingActionTracker(add_Perm_Dropdown.SelectedItem.ToString() + perm_Added, perm_List, true));
                                perm_Added++;
                                break;

                            case "PlaneBounce":
                                perm_List.Items.Add(new PlaneBounceActionTracker(add_Perm_Dropdown.SelectedItem.ToString() + perm_Added, perm_List, true));
                                perm_Added++;
                                break;

                            case "Rotate":
                                perm_List.Items.Add(new RotateActionTracker(add_Perm_Dropdown.SelectedItem.ToString() + perm_Added, perm_List, true));
                                perm_Added++;
                                break;

                            case "Scale":
                                perm_List.Items.Add(new ScaleActionTracker(add_Perm_Dropdown.SelectedItem.ToString() + perm_Added, perm_List, true));
                                perm_Added++;
                                break;

                            case "Source":
                                perm_List.Items.Add(new SourceActionTracker(add_Perm_Dropdown.SelectedItem.ToString() + perm_Added, perm_List, domain_List, true));
                                perm_Added++;
                                break;

                            case "SquareBounce":
                                perm_List.Items.Add(new SquareBounceActionTracker(add_Perm_Dropdown.SelectedItem.ToString() + perm_Added, perm_List, true));
                                perm_Added++;
                                break;

                            case "ZeroVel":
                                perm_List.Items.Add(new ZeroVelActionTracker(add_Perm_Dropdown.SelectedItem.ToString() + perm_Added, perm_List, true));
                                perm_Added++;
                                break;
                        }
                    }
                };

            remove_Perm_Button = new Button();
            perm_Point.X += 95;
            remove_Perm_Button.Location = perm_Point;
            remove_Perm_Button.Text = "Remove Action";
            remove_Perm_Button.Click += (s, e) => 
                {
                    if (perm_List.SelectedItem != null)
                    {
                        perm_List.Items.Remove(perm_List.SelectedItem);
                    }
                };

            this.Controls.Add(perm_Label);
            this.Controls.Add(add_Perm_Dropdown);
            this.Controls.Add(add_Perm_Button);
            this.Controls.Add(remove_Perm_Button);
            this.Controls.Add(perm_List);
            #endregion

            #region Temporary
            temp_Point = new Point(10, 460);

            temp_Label = new Label();
            temp_Label.Location = temp_Point;
            temp_Label.Height = 20;
            temp_Label.Text = "Temporary Actions:";
            temp_Point.Y += 20;

            add_Temp_Dropdown = new ComboBox();
            add_Temp_Dropdown.Location = temp_Point;
            add_Temp_Dropdown.Items.Add("Age");
            add_Temp_Dropdown.Items.Add("DiscBounce");
            add_Temp_Dropdown.Items.Add("Fade");
            add_Temp_Dropdown.Items.Add("Gravity");
            add_Temp_Dropdown.Items.Add("KillAll");
            add_Temp_Dropdown.Items.Add("Move");
            add_Temp_Dropdown.Items.Add("OrbitAxis");
            add_Temp_Dropdown.Items.Add("OrbitLine");
            add_Temp_Dropdown.Items.Add("OrbitPoint");
            add_Temp_Dropdown.Items.Add("OrbitRing");
            add_Temp_Dropdown.Items.Add("PlaneBounce");
            add_Temp_Dropdown.Items.Add("Rotate");
            add_Temp_Dropdown.Items.Add("Scale");
            add_Temp_Dropdown.Items.Add("Source");
            add_Temp_Dropdown.Items.Add("SquareBounce");
            add_Temp_Dropdown.Items.Add("ZeroVel");

            temp_List = new RefreshingListBox();
            temp_Point.Y += 30;
            temp_List.Location = temp_Point;
            temp_List.Width = 300;
            temp_List.Height = 130;
            temp_List.DisplayMember = "DisplayText";
            temp_List.ValueMember = "DisplayText";
            temp_List.DoubleClick += (s, e) =>
            {
                if (temp_List.SelectedItem != null)
                {
                    ((WPSActionTracker)temp_List.SelectedItem).ShowDialog();
                }
            };

            add_Temp_Button = new Button();
            temp_Point.X += 130;
            temp_Point.Y -= 30;
            add_Temp_Button.Location = temp_Point;
            add_Temp_Button.Text = "Add Action";
            add_Temp_Button.Click += (s, e) =>
            {
                if (add_Temp_Dropdown.SelectedItem != null)
                {
                    switch (add_Temp_Dropdown.SelectedItem.ToString())
                    {
                        case "Age":
                            temp_List.Items.Add(new AgeActionTracker(add_Temp_Dropdown.SelectedItem.ToString() + temp_Added, temp_List, false));
                            temp_Added++;
                            break;

                        case "DiscBounce":
                            temp_List.Items.Add(new DiscBounceActionTracker(add_Temp_Dropdown.SelectedItem.ToString() + temp_Added, temp_List, false));
                            temp_Added++;
                            break;

                        case "Fade":
                            temp_List.Items.Add(new FadeActionTracker(add_Temp_Dropdown.SelectedItem.ToString() + temp_Added, temp_List, false));
                            temp_Added++;
                            break;

                        case "Gravity":
                            temp_List.Items.Add(new GravityActionTracker(add_Temp_Dropdown.SelectedItem.ToString() + temp_Added, temp_List, false));
                            temp_Added++;
                            break;

                        case "KillAll":
                            temp_List.Items.Add(new KillAllActionTracker(add_Temp_Dropdown.SelectedItem.ToString() + temp_Added, temp_List, false));
                            temp_Added++;
                            break;

                        case "Move":
                            temp_List.Items.Add(new MoveActionTracker(add_Temp_Dropdown.SelectedItem.ToString() + temp_Added, temp_List, false));
                            temp_Added++;
                            break;

                        case "OrbitAxis":
                            temp_List.Items.Add(new OrbitAxisActionTracker(add_Temp_Dropdown.SelectedItem.ToString() + temp_Added, temp_List, false));
                            temp_Added++;
                            break;

                        case "OrbitLine":
                            temp_List.Items.Add(new OrbitLineActionTracker(add_Temp_Dropdown.SelectedItem.ToString() + temp_Added, temp_List, false));
                            temp_Added++;
                            break;

                        case "OrbitPoint":
                            temp_List.Items.Add(new OrbitPointActionTracker(add_Temp_Dropdown.SelectedItem.ToString() + temp_Added, temp_List, false));
                            temp_Added++;
                            break;

                        case "OrbitRing":
                            temp_List.Items.Add(new OrbitRingActionTracker(add_Temp_Dropdown.SelectedItem.ToString() + temp_Added, temp_List, false));
                            temp_Added++;
                            break;

                        case "PlaneBounce":
                            temp_List.Items.Add(new PlaneBounceActionTracker(add_Temp_Dropdown.SelectedItem.ToString() + temp_Added, temp_List, false));
                            temp_Added++;
                            break;

                        case "Rotate":
                            temp_List.Items.Add(new RotateActionTracker(add_Temp_Dropdown.SelectedItem.ToString() + temp_Added, temp_List, false));
                            temp_Added++;
                            break;

                        case "Scale":
                            temp_List.Items.Add(new ScaleActionTracker(add_Temp_Dropdown.SelectedItem.ToString() + temp_Added, temp_List, false));
                            temp_Added++;
                            break;

                        case "Source":
                            temp_List.Items.Add(new SourceActionTracker(add_Temp_Dropdown.SelectedItem.ToString() + temp_Added, temp_List, domain_List, false));
                            temp_Added++;
                            break;

                        case "SquareBounce":
                            temp_List.Items.Add(new SquareBounceActionTracker(add_Temp_Dropdown.SelectedItem.ToString() + temp_Added, temp_List, false));
                            temp_Added++;
                            break;

                        case "ZeroVel":
                            temp_List.Items.Add(new ZeroVelActionTracker(add_Temp_Dropdown.SelectedItem.ToString() + temp_Added, temp_List, false));
                            temp_Added++;
                            break;
                    }
                }
            };

            remove_Temp_Button = new Button();
            temp_Point.X += 95;
            remove_Temp_Button.Location = temp_Point;
            remove_Temp_Button.Text = "Remove Action";
            remove_Temp_Button.Click += (s, e) =>
            {
                if (temp_List.SelectedItem != null)
                {
                    temp_List.Items.Remove(temp_List.SelectedItem);
                }
            };

            this.Controls.Add(temp_Label);
            this.Controls.Add(add_Temp_Dropdown);
            this.Controls.Add(add_Temp_Button);
            this.Controls.Add(remove_Temp_Button);
            this.Controls.Add(temp_List);

            totalframes_Label = new Label();
            totalframes_Label.Location = new Point(10, 640);
            totalframes_Label.Text = "TotalFrames:";
            totalframes_Label.Height = 13;
            totalframes_Label.AutoSize = true;
            totalframes_Textbox = new TextBox();
            totalframes_Textbox.Location = new Point(totalframes_Label.Width - 20, totalframes_Label.Location.Y - 2);
            totalframes_Textbox.Text = "100";
            totalframes_Textbox.Width = 50;
            this.Controls.Add(totalframes_Label);
            this.Controls.Add(totalframes_Textbox);

            loop_Label = new Label();
            loop_Label.Location = new Point(150, 640);
            loop_Label.Text = "Loop: YES          NO";
            loop_Label.Height = 13;
            loop_Label.AutoSize = true;
            loop_YES_Radio = new RadioButton();
            loop_YES_Radio.Location = new Point(210, loop_Label.Location.Y - 5);
            loop_YES_Radio.Width = 13;
            loop_NO_Radio = new RadioButton();
            loop_NO_Radio.Location = new Point(255, loop_Label.Location.Y - 5);
            loop_NO_Radio.Width = 13;
            loop_NO_Radio.Checked = true;
            this.Controls.Add(loop_YES_Radio);
            this.Controls.Add(loop_NO_Radio);
            this.Controls.Add(loop_Label);
            
            #endregion
        }

        public string ToXML()
        {
            string xml = "";
            xml += "<particlesystem>\n";

            xml += "<property,name=Name,type=string>" + Name + "</property>\n" +
                "<property,name=MaxParticles,type=int>" + (int)SetTextBoxValueToFloat(ref maxpTextbox) + "</property>\n" +
                "<property,name=ParticleLife,type=float>" + SetTextBoxValueToFloat(ref partlTextbox) + "</property>\n" +
                "<property,name=TextureFilePath,type=string>" + textureTextbox.Text + "</property>\n" +
                "<property,name=Visability,type=string>" + visDropDown.Text + "</property>\n" +
                "<property,name=TotalFrames,type=int>" + (int)SetTextBoxValueToFloat(ref totalframes_Textbox) + "</property>\n" +
                "<property,name=Loop,type=bool>";
            if (loop_YES_Radio.Checked)
                xml += "true";
            else
                xml += "false";
            xml += "</property>\n" +
                "<domains>\n";
            for (int i = 0; i < domain_List.Items.Count; i++)
                xml += ((DomainTracker)domain_List.Items[i]).ToXML();
            xml += "</domains>\n" +
                "<permanentactions>\n";
            for (int i = 0; i < perm_List.Items.Count; i++)
                xml += ((WPSActionTracker)perm_List.Items[i]).ToXML();
            xml += "</permanentactions>\n" +
                "<temporaryactions>\n";
            for (int i = 0; i < temp_List.Items.Count; i++)
                xml += ((WPSActionTracker)temp_List.Items[i]).ToXML();
            xml += "</temporaryactions>\n";

            xml += "</particlesystem>\n";
            return xml;
        }

        private float SetTextBoxValueToFloat(ref TextBox t)
        {
            float data = 0;
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
    }
}
