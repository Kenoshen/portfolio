namespace WPS_Editor
{
    partial class EditorForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.editor_panel = new System.Windows.Forms.Panel();
            this.save_button = new System.Windows.Forms.Button();
            this.load_button = new System.Windows.Forms.Button();
            this.start = new System.Windows.Forms.Button();
            this.remove_ps = new System.Windows.Forms.Button();
            this.add_ps = new System.Windows.Forms.Button();
            this.ps_tab_control = new System.Windows.Forms.TabControl();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.numericUpDown2 = new System.Windows.Forms.NumericUpDown();
            this.numericUpDown3 = new System.Windows.Forms.NumericUpDown();
            this.game_control = new WPS_Editor.GameControl();
            this.editor_panel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown3)).BeginInit();
            this.SuspendLayout();
            // 
            // editor_panel
            // 
            this.editor_panel.Controls.Add(this.save_button);
            this.editor_panel.Controls.Add(this.load_button);
            this.editor_panel.Controls.Add(this.start);
            this.editor_panel.Controls.Add(this.remove_ps);
            this.editor_panel.Controls.Add(this.add_ps);
            this.editor_panel.Controls.Add(this.ps_tab_control);
            this.editor_panel.Dock = System.Windows.Forms.DockStyle.Right;
            this.editor_panel.Location = new System.Drawing.Point(685, 0);
            this.editor_panel.Name = "editor_panel";
            this.editor_panel.Size = new System.Drawing.Size(331, 752);
            this.editor_panel.TabIndex = 2;
            // 
            // save_button
            // 
            this.save_button.Location = new System.Drawing.Point(89, 708);
            this.save_button.Name = "save_button";
            this.save_button.Size = new System.Drawing.Size(105, 23);
            this.save_button.TabIndex = 5;
            this.save_button.Text = "Save As";
            this.save_button.UseVisualStyleBackColor = true;
            this.save_button.Click += new System.EventHandler(this.save_button_Click);
            // 
            // load_button
            // 
            this.load_button.Location = new System.Drawing.Point(89, 736);
            this.load_button.Name = "load_button";
            this.load_button.Size = new System.Drawing.Size(105, 23);
            this.load_button.TabIndex = 4;
            this.load_button.Text = "Load";
            this.load_button.UseVisualStyleBackColor = true;
            this.load_button.Click += new System.EventHandler(this.load_button_Click);
            // 
            // start
            // 
            this.start.Location = new System.Drawing.Point(3, 708);
            this.start.Name = "start";
            this.start.Size = new System.Drawing.Size(65, 51);
            this.start.TabIndex = 3;
            this.start.Text = "Start";
            this.start.UseVisualStyleBackColor = true;
            this.start.Click += new System.EventHandler(this.start_Click);
            // 
            // remove_ps
            // 
            this.remove_ps.Location = new System.Drawing.Point(216, 736);
            this.remove_ps.Name = "remove_ps";
            this.remove_ps.Size = new System.Drawing.Size(112, 23);
            this.remove_ps.TabIndex = 2;
            this.remove_ps.Text = "Remove Current PS";
            this.remove_ps.UseVisualStyleBackColor = true;
            this.remove_ps.Click += new System.EventHandler(this.remove_ps_Click);
            // 
            // add_ps
            // 
            this.add_ps.Location = new System.Drawing.Point(216, 708);
            this.add_ps.Name = "add_ps";
            this.add_ps.Size = new System.Drawing.Size(112, 22);
            this.add_ps.TabIndex = 1;
            this.add_ps.Text = "Add New PS";
            this.add_ps.UseVisualStyleBackColor = true;
            this.add_ps.Click += new System.EventHandler(this.add_ps_Click);
            // 
            // ps_tab_control
            // 
            this.ps_tab_control.Location = new System.Drawing.Point(0, 0);
            this.ps_tab_control.Name = "ps_tab_control";
            this.ps_tab_control.SelectedIndex = 0;
            this.ps_tab_control.Size = new System.Drawing.Size(328, 687);
            this.ps_tab_control.TabIndex = 0;
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "Black",
            "CornflowerBlue",
            "White",
            "Red",
            "Yellow",
            "Green",
            "Gray"});
            this.comboBox1.Location = new System.Drawing.Point(0, 745);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(121, 21);
            this.comboBox1.TabIndex = 6;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.DecimalPlaces = 2;
            this.numericUpDown1.Location = new System.Drawing.Point(538, 746);
            this.numericUpDown1.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            -2147483648});
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(41, 20);
            this.numericUpDown1.TabIndex = 6;
            this.numericUpDown1.ValueChanged += new System.EventHandler(this.numericUpDown1_ValueChanged);
            // 
            // numericUpDown2
            // 
            this.numericUpDown2.DecimalPlaces = 2;
            this.numericUpDown2.Location = new System.Drawing.Point(585, 746);
            this.numericUpDown2.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            -2147483648});
            this.numericUpDown2.Name = "numericUpDown2";
            this.numericUpDown2.Size = new System.Drawing.Size(41, 20);
            this.numericUpDown2.TabIndex = 7;
            this.numericUpDown2.ValueChanged += new System.EventHandler(this.numericUpDown2_ValueChanged);
            // 
            // numericUpDown3
            // 
            this.numericUpDown3.DecimalPlaces = 2;
            this.numericUpDown3.Location = new System.Drawing.Point(632, 746);
            this.numericUpDown3.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            -2147483648});
            this.numericUpDown3.Name = "numericUpDown3";
            this.numericUpDown3.Size = new System.Drawing.Size(41, 20);
            this.numericUpDown3.TabIndex = 8;
            this.numericUpDown3.ValueChanged += new System.EventHandler(this.numericUpDown3_ValueChanged);
            // 
            // game_control
            // 
            this.game_control.Dock = System.Windows.Forms.DockStyle.Left;
            this.game_control.Location = new System.Drawing.Point(0, 0);
            this.game_control.Name = "game_control";
            this.game_control.Size = new System.Drawing.Size(679, 752);
            this.game_control.TabIndex = 1;
            this.game_control.Text = "game_control";
            // 
            // EditorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1016, 752);
            this.Controls.Add(this.numericUpDown3);
            this.Controls.Add(this.numericUpDown2);
            this.Controls.Add(this.numericUpDown1);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.editor_panel);
            this.Controls.Add(this.game_control);
            this.Name = "EditorForm";
            this.Text = "EditorForm";
            this.editor_panel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown3)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private GameControl game_control;
        private System.Windows.Forms.Panel editor_panel;
        private System.Windows.Forms.TabControl ps_tab_control;
        private System.Windows.Forms.Button remove_ps;
        private System.Windows.Forms.Button add_ps;
        private System.Windows.Forms.Button start;
        private System.Windows.Forms.Button save_button;
        private System.Windows.Forms.Button load_button;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.NumericUpDown numericUpDown2;
        private System.Windows.Forms.NumericUpDown numericUpDown3;
    }
}