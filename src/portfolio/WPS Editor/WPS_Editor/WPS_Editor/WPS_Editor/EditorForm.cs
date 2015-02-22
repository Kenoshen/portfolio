using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WPS;
using Microsoft.Xna.Framework;

namespace WPS_Editor
{
    public partial class EditorForm : Form
    {
        private int ps_tabs_added = 0;
        private string filePath = "";

        public EditorForm()
        {
            InitializeComponent();
        }

        private void add_ps_Click(object sender, EventArgs e)
        {
            ps_tab_control.Controls.Add(new ParticleSystemTab("particle system " + ps_tabs_added));
            ps_tabs_added++;
            filePath = "";
        }

        private void remove_ps_Click(object sender, EventArgs e)
        {
            if (ps_tab_control.Controls.Count > 0)
            {
                DialogResult popupResult = MessageBox.Show("Are you sure you want to delete the particle system object?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                if (popupResult == System.Windows.Forms.DialogResult.Yes)
                {
                    TabPage selectedTab = ps_tab_control.SelectedTab;
                    ps_tab_control.Controls.Remove(selectedTab);
                }
            }
            filePath = "";
        }

        private void start_Click(object sender, EventArgs e)
        {
            string xml = "WPS Editor File\n";
            for (int i = 0; i < ps_tab_control.TabCount; i++)
                xml += ((ParticleSystemTab)ps_tab_control.TabPages[i]).ToXML();
            xml += "End";

            if (filePath == "")
                filePath = Save(xml);
            else
                SaveNoDialog(filePath, xml);

            
        }

        private string Save(string data)
        {
            StringBuilder sb = new StringBuilder();
            SetStringBuilder(ref sb, data);
            string filepath = "";

            using (SaveFileDialog dialog = new SaveFileDialog())
            {
                dialog.Filter = "wpsxml (*.wpsxml)|*.wpsxml";
                dialog.FilterIndex = 2;
                if (dialog.ShowDialog(this) == DialogResult.OK)
                {
                    filepath = dialog.FileName;
                    File.WriteAllText(dialog.FileName, sb.ToString());

                    game_control.LoadNewAnimator(filepath);
                }
            }
            return filepath;
        }

        private void SaveNoDialog(string filename, string data)
        {
            StringBuilder sb = new StringBuilder();
            SetStringBuilder(ref sb, data);
            File.WriteAllText(filename, sb.ToString());

            game_control.LoadNewAnimator(filename);
        }

        private void SetStringBuilder(ref StringBuilder sb, string data)
        {
            string[] arrayData = data.Split(new string[1] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < arrayData.Length; i++)
                sb.AppendLine(arrayData[i]);
        }

        private void save_button_Click(object sender, EventArgs e)
        {
            string xml = "WPS Editor File\n";
            for (int i = 0; i < ps_tab_control.TabCount; i++)
                xml += ((ParticleSystemTab)ps_tab_control.TabPages[i]).ToXML();
            xml += "End";

            Save(xml);
        }

        private void load_button_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.Filter = "wpsxml (*.wpsxml)|*.wpsxml";
                dialog.FilterIndex = 2;
                if (dialog.ShowDialog(this) == DialogResult.OK)
                {
                    filePath = dialog.FileName;
                    Load(WPSXMLParser.ParseWPSXMLFile(dialog.FileName));
                }
            }
        }

        private void Load(List<WPSXMLParticleSystem> tree)
        {
            Dictionary<string, string> properties = new Dictionary<string,string>();
            foreach (WPSXMLParticleSystem ps in tree)
            {
                foreach(WPSXMLProperty property in ps.Properties)
                    InsertObject(properties, property.Name, property.Value);
                ParticleSystemTab psTab = new ParticleSystemTab(properties["Name"]);
                psTab.nameTextBox.Text = properties["Name"];
                psTab.maxpTextbox.Text = properties["MaxParticles"];
                psTab.partlTextbox.Text = properties["ParticleLife"];
                psTab.textureTextbox.Text = properties["TextureFilePath"];
                psTab.visDropDown.Text = properties["Visability"];
                psTab.totalframes_Textbox.Text = properties["TotalFrames"];
                if (properties["Loop"] == "true")
                    psTab.loop_YES_Radio.Checked = true;
                else
                    psTab.loop_NO_Radio.Checked = true;

                foreach (WPSXMLDomain domain in ps.Domains)
                {
                    for (int i = 0; i < psTab.add_Domain_Dropdown.Items.Count; i++)
                    {
                        string dm = domain.Type.Remove(domain.Type.Length - 6);
                        string ty = psTab.add_Domain_Dropdown.Items[i].ToString();
                        if (dm == ty)
                        {
                            psTab.add_Domain_Dropdown.SelectedItem = psTab.add_Domain_Dropdown.Items[i];
                            break;
                        }
                    }
                    psTab.add_Domain_Button.PerformClick();
                }
                psTab.add_Domain_Dropdown.SelectedItem = null;

                foreach (WPSXMLAction action in ps.PermanentActions)
                {
                    for (int i = 0; i < psTab.add_Perm_Dropdown.Items.Count; i++)
                    {
                        string dm = action.Type.Remove(action.Type.Length - 6);
                        string ty = psTab.add_Perm_Dropdown.Items[i].ToString();
                        if (dm == ty)
                        {
                            psTab.add_Perm_Dropdown.SelectedItem = psTab.add_Perm_Dropdown.Items[i];
                            break;
                        }
                    }
                    psTab.add_Perm_Button.PerformClick();
                }
                psTab.add_Perm_Dropdown.SelectedItem = null;

                foreach (WPSXMLAction action in ps.TemporaryActions)
                {
                    for (int i = 0; i < psTab.add_Temp_Dropdown.Items.Count; i++)
                    {
                        string dm = action.Type.Remove(action.Type.Length - 6);
                        string ty = psTab.add_Temp_Dropdown.Items[i].ToString();
                        if (dm == ty)
                        {
                            psTab.add_Temp_Dropdown.SelectedItem = psTab.add_Temp_Dropdown.Items[i];
                            break;
                        }
                    }
                    psTab.add_Temp_Button.PerformClick();
                }
                psTab.add_Temp_Dropdown.SelectedItem = null;

                for (int i = 0; i < psTab.domain_List.Items.Count; i++)
                {
                    properties = new Dictionary<string, string>();
                    foreach (WPSXMLProperty property in ps.Domains[i].Properties)
                        InsertObject(properties, property.Name, property.Value);
                    BuildDomainTracker((DomainTracker)psTab.domain_List.Items[i], properties);
                }

                for (int i = 0; i < psTab.perm_List.Items.Count; i++)
                {
                    properties = new Dictionary<string, string>();
                    foreach (WPSXMLProperty property in ps.PermanentActions[i].Properties)
                        InsertObject(properties, property.Name, property.Value);
                    BuildWPSActionTracker((WPSActionTracker)psTab.perm_List.Items[i], properties);
                }

                ps_tab_control.Controls.Add(psTab);
            }
        }

        private void InsertObject(Dictionary<string, string> dictionary, string key, string value)
        {
            try
            {
                object o = dictionary[key];
                InsertObject(dictionary, key + "0", value);
            }
            catch (Exception e)
            {
                dictionary.Add(key, value);
            }
        }

        private void BuildDomainTracker(DomainTracker dt, Dictionary<string, string> properties)
        {
            dt.Name = properties["Name"];

            if (dt is BoxDomainTracker)
            {
                dt.First = (Vector3)ConvertFromString(properties["Center"], "Vector3");
                dt.Second = new Vector3(
                    (float)ConvertFromString(properties["Width"], "float"),
                    (float)ConvertFromString(properties["Height"], "float"),
                    (float)ConvertFromString(properties["Depth"], "float"));
            }
            else if (dt is CylinderDomainTracker)
            {
                dt.First = (Vector3)ConvertFromString(properties["Center"], "Vector3");
                dt.Second = (Vector3)ConvertFromString(properties["Normal"], "Vector3");
                dt.Third = new Vector3(
                    (float)ConvertFromString(properties["Length"], "float"),
                    (float)ConvertFromString(properties["OuterRadius"], "float"),
                    (float)ConvertFromString(properties["InnerRadius"], "float"));
            }
            else if (dt is DiscDomainTracker)
            {
                dt.First = (Vector3)ConvertFromString(properties["Center"], "Vector3");
                dt.Second = (Vector3)ConvertFromString(properties["Normal"], "Vector3");
                dt.Third = new Vector3(
                    (float)ConvertFromString(properties["Radius"], "float"),
                    0,
                    0);
            }
            else if (dt is LineDomainTracker)
            {
                dt.First = (Vector3)ConvertFromString(properties["StartPoint"], "Vector3");
                dt.Second = (Vector3)ConvertFromString(properties["EndPoint"], "Vector3");
            }
            else if (dt is PointDomainTracker)
            {
                dt.First = (Vector3)ConvertFromString(properties["Point"], "Vector3");
            }
            else if (dt is RingDomainTracker)
            {
                dt.First = (Vector3)ConvertFromString(properties["Center"], "Vector3");
                dt.Second = (Vector3)ConvertFromString(properties["Normal"], "Vector3");
                dt.Third = new Vector3(
                    (float)ConvertFromString(properties["Radius"], "float"),
                    0,
                    0);
            }
            else if (dt is SphereDomainTracker)
            {
                dt.First = (Vector3)ConvertFromString(properties["Center"], "Vector3");
                dt.Second = new Vector3(
                    (float)ConvertFromString(properties["Radius"], "float"),
                    0,
                    0);
            }
            else if (dt is SquareDomainTracker)
            {
                dt.First = (Vector3)ConvertFromString(properties["Center"], "Vector3");
                dt.Second = (Vector3)ConvertFromString(properties["Normal"], "Vector3");
                dt.Third = new Vector3(
                    (float)ConvertFromString(properties["Width"], "float"),
                    (float)ConvertFromString(properties["Height"], "float"),
                    0);
            }
        }
        
        private void BuildWPSActionTracker(WPSActionTracker dt, Dictionary<string, string> properties)
        {
            dt.Name = properties["Name"];

            if (dt is AgeActionTracker)
            {
                ((AgeActionTracker)dt).AgeStep = (float)ConvertFromString(properties["AgeStep"], "float");
            }
            else if (dt is DiscBounceActionTracker)
            {
                ((DiscBounceActionTracker)dt).Center = (Vector3)ConvertFromString(properties["Center"], "Vector3");
                ((DiscBounceActionTracker)dt).Normal = (Vector3)ConvertFromString(properties["Normal"], "Vector3");
                ((DiscBounceActionTracker)dt).Radius = (float)ConvertFromString(properties["Radius"], "float");
                ((DiscBounceActionTracker)dt).Dampening = (float)ConvertFromString(properties["Dampening"], "float");
                ((DiscBounceActionTracker)dt).TimeStep = (float)ConvertFromString(properties["TimeStep"], "float");
            }
            else if (dt is FadeActionTracker)
            {
                // no properties to configure
            }
            else if (dt is GravityActionTracker)
            {
                ((GravityActionTracker)dt).Gravity = (Vector3)ConvertFromString(properties["Gravity"], "Vector3");
                ((GravityActionTracker)dt).TimeStep = (float)ConvertFromString(properties["TimeStep"], "float");
            }
            else if (dt is KillAllActionTracker)
            {
                // no properties to configure
            }
            else if (dt is MoveActionTracker)
            {
                ((MoveActionTracker)dt).TimeStep = (float)ConvertFromString(properties["TimeStep"], "float");
            }
            else if (dt is OrbitAxisActionTracker)
            {
                ((OrbitAxisActionTracker)dt).StartPoint = (Vector3)ConvertFromString(properties["StartPoint"], "Vector3");
                ((OrbitAxisActionTracker)dt).EndPoint = (Vector3)ConvertFromString(properties["EndPoint"], "Vector3");
                ((OrbitAxisActionTracker)dt).MaxRange = (float)ConvertFromString(properties["MaxRange"], "float");
                ((OrbitAxisActionTracker)dt).Magnitude = (float)ConvertFromString(properties["Magnitude"], "float");
                ((OrbitAxisActionTracker)dt).Epsilon = (float)ConvertFromString(properties["Epsilon"], "float");
                ((OrbitAxisActionTracker)dt).TimeStep = (float)ConvertFromString(properties["TimeStep"], "float");
            }
            else if (dt is OrbitLineActionTracker)
            {
                ((OrbitLineActionTracker)dt).StartPoint = (Vector3)ConvertFromString(properties["StartPoint"], "Vector3");
                ((OrbitLineActionTracker)dt).EndPoint = (Vector3)ConvertFromString(properties["EndPoint"], "Vector3");
                ((OrbitLineActionTracker)dt).MaxRange = (float)ConvertFromString(properties["MaxRange"], "float");
                ((OrbitLineActionTracker)dt).Magnitude = (float)ConvertFromString(properties["Magnitude"], "float");
                ((OrbitLineActionTracker)dt).Epsilon = (float)ConvertFromString(properties["Epsilon"], "float");
                ((OrbitLineActionTracker)dt).TimeStep = (float)ConvertFromString(properties["TimeStep"], "float");
            }
            else if (dt is OrbitPointActionTracker)
            {
                ((OrbitPointActionTracker)dt).OrbitPoint = (Vector3)ConvertFromString(properties["OrbitPoint"], "Vector3");
                ((OrbitPointActionTracker)dt).MaxRange = (float)ConvertFromString(properties["MaxRange"], "float");
                ((OrbitPointActionTracker)dt).Magnitude = (float)ConvertFromString(properties["Magnitude"], "float");
                ((OrbitPointActionTracker)dt).Epsilon = (float)ConvertFromString(properties["Epsilon"], "float");
                ((OrbitPointActionTracker)dt).TimeStep = (float)ConvertFromString(properties["TimeStep"], "float");
            }
            else if (dt is OrbitRingActionTracker)
            {
                ((OrbitRingActionTracker)dt).Center = (Vector3)ConvertFromString(properties["Center"], "Vector3");
                ((OrbitRingActionTracker)dt).Normal = (Vector3)ConvertFromString(properties["Normal"], "Vector3");
                ((OrbitRingActionTracker)dt).Radius = (float)ConvertFromString(properties["Radius"], "float");
                ((OrbitRingActionTracker)dt).MaxRange = (float)ConvertFromString(properties["MaxRange"], "float");
                ((OrbitRingActionTracker)dt).Magnitude = (float)ConvertFromString(properties["Magnitude"], "float");
                ((OrbitRingActionTracker)dt).Epsilon = (float)ConvertFromString(properties["Epsilon"], "float");
                ((OrbitRingActionTracker)dt).TimeStep = (float)ConvertFromString(properties["TimeStep"], "float");
            }
            else if (dt is PlaneBounceActionTracker)
            {
                ((PlaneBounceActionTracker)dt).PlanePoint = (Vector3)ConvertFromString(properties["PlanePoint"], "Vector3");
                ((PlaneBounceActionTracker)dt).Normal = (Vector3)ConvertFromString(properties["Normal"], "Vector3");
                ((PlaneBounceActionTracker)dt).Dampening = (float)ConvertFromString(properties["Dampening"], "float");
                ((PlaneBounceActionTracker)dt).TimeStep = (float)ConvertFromString(properties["TimeStep"], "float");
            }
            else if (dt is RotateActionTracker)
            {
                ((RotateActionTracker)dt).MinRotRevs = (float)ConvertFromString(properties["MinRotRevs"], "float");
                ((RotateActionTracker)dt).MaxRotRevs = (float)ConvertFromString(properties["MaxRotRevs"], "float");
            }
            else if (dt is ScaleActionTracker)
            {
                ((ScaleActionTracker)dt).EndScale = (float)ConvertFromString(properties["EndScale"], "float");
            }
            else if (dt is SourceActionTracker)
            {
                ((SourceActionTracker)dt).PositionDomain = properties["PositionDomain"];
                ((SourceActionTracker)dt).VelocityDomain = properties["VelocityDomain"];
                ((SourceActionTracker)dt).Size = (float)ConvertFromString(properties["Size"], "float");
                ((SourceActionTracker)dt).Particle_Rate = (float)ConvertFromString(properties["Particle_Rate"], "float");
                ((SourceActionTracker)dt).TimeStep = (float)ConvertFromString(properties["TimeStep"], "float");
            }
            else if (dt is SquareBounceActionTracker)
            {
                ((SquareBounceActionTracker)dt).PlanePoint = (Vector3)ConvertFromString(properties["PlanePoint"], "Vector3");
                ((SquareBounceActionTracker)dt).Normal = (Vector3)ConvertFromString(properties["Normal"], "Vector3");
                ((SquareBounceActionTracker)dt).Width = (float)ConvertFromString(properties["Width"], "float");
                ((SquareBounceActionTracker)dt).Height = (float)ConvertFromString(properties["Height"], "float");
                ((SquareBounceActionTracker)dt).Dampening = (float)ConvertFromString(properties["Dampening"], "float");
                ((SquareBounceActionTracker)dt).TimeStep = (float)ConvertFromString(properties["TimeStep"], "float");
            }
            else if (dt is ZeroVelActionTracker)
            {
                // no properties to configure
            }
        }

        private object ConvertFromString(string value, string type)
        {
            switch (type)
            {
                case "string":
                    return value;

                case "float":
                    try
                    {
                        return (float)Decimal.Parse(value);
                    }
                    catch (Exception e)
                    {
                        return 0;
                    }

                case "int":
                    try
                    {
                        return (int)Int32.Parse(value);
                    }
                    catch (Exception e)
                    {
                        return 0;
                    }

                case "bool":
                    if (value == "true")
                        return true;
                    else
                        return false;

                case "Vector3":
                    try
                    {
                        string[] parsed = value.Split(new char[3] { '{', ' ', '}' }, StringSplitOptions.RemoveEmptyEntries);
                        if (parsed.Length != 3)
                            throw new Exception("Not a valid Vector3 string");
                        float[] values = new float[3];
                        for (int i = 0; i < parsed.Length; i++)
                            values[i] = (float)Decimal.Parse(parsed[i].Split(new char[1] { ':' }, StringSplitOptions.RemoveEmptyEntries)[1]);
                        return new Vector3(values[0], values[1], values[2]);
                    }
                    catch (Exception e)
                    {
                        return Vector3.Zero;
                    }

                default:
                    return null;
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboBox1.SelectedItem.ToString())
            {
                case "Black":
                    game_control.GD_Clear_Color = Microsoft.Xna.Framework.Color.Black;
                    break;
                case "CornflowerBlue":
                    game_control.GD_Clear_Color = Microsoft.Xna.Framework.Color.CornflowerBlue;
                    break;
                case "White":
                    game_control.GD_Clear_Color = Microsoft.Xna.Framework.Color.White;
                    break;
                case "Red":
                    game_control.GD_Clear_Color = Microsoft.Xna.Framework.Color.Red;
                    break;
                case "Yellow":
                    game_control.GD_Clear_Color = Microsoft.Xna.Framework.Color.Yellow;
                    break;
                case "Green":
                    game_control.GD_Clear_Color = Microsoft.Xna.Framework.Color.Green;
                    break;
                case "Gray":
                    game_control.GD_Clear_Color = Microsoft.Xna.Framework.Color.Gray;
                    break;
            }
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            game_control.cam.Target = new Vector3((float)numericUpDown1.Value, game_control.cam.Target.Y, game_control.cam.Target.Z);
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            game_control.cam.Target = new Vector3(game_control.cam.Target.X, (float)numericUpDown2.Value, game_control.cam.Target.Z);
        }

        private void numericUpDown3_ValueChanged(object sender, EventArgs e)
        {
            game_control.cam.Target = new Vector3(game_control.cam.Target.X, game_control.cam.Target.Y, (float)numericUpDown3.Value);
        }
    }
}
