namespace LPR381_Project
{
    partial class MainMenu
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
            metroSetPanel1 = new MetroSet_UI.Controls.MetroSetPanel();
            lblImport = new MetroSet_UI.Controls.MetroSetLabel();
            btnFile = new MetroSet_UI.Controls.MetroSetButton();
            pnlDragnDrop = new MetroSet_UI.Controls.MetroSetPanel();
            lblDrop = new MetroSet_UI.Controls.MetroSetLabel();
            metroSetPanel4 = new MetroSet_UI.Controls.MetroSetPanel();
            btnCAOutputClear = new MetroSet_UI.Controls.MetroSetButton();
            lblCASolution = new MetroSet_UI.Controls.MetroSetLabel();
            rtbOutput = new MetroSet_UI.Controls.MetroSetRichTextBox();
            metroSetPanel5 = new MetroSet_UI.Controls.MetroSetPanel();
            rtbFileOutput = new MetroSet_UI.Controls.MetroSetRichTextBox();
            lblFileOutput = new MetroSet_UI.Controls.MetroSetLabel();
            metroSetPanel6 = new MetroSet_UI.Controls.MetroSetPanel();
            btnSolve = new MetroSet_UI.Controls.MetroSetButton();
            cboMethod = new MetroSet_UI.Controls.MetroSetComboBox();
            metroSetLabel9 = new MetroSet_UI.Controls.MetroSetLabel();
            lblSolve = new MetroSet_UI.Controls.MetroSetLabel();
            cbForm = new MetroSet_UI.Controls.MetroSetControlBox();
            btnClose = new MetroSet_UI.Controls.MetroSetButton();
            metroSetPanel1.SuspendLayout();
            pnlDragnDrop.SuspendLayout();
            metroSetPanel4.SuspendLayout();
            metroSetPanel5.SuspendLayout();
            metroSetPanel6.SuspendLayout();
            SuspendLayout();
            // 
            // metroSetPanel1
            // 
            metroSetPanel1.BackgroundColor = Color.FromArgb(30, 30, 30);
            metroSetPanel1.BorderColor = Color.FromArgb(110, 110, 110);
            metroSetPanel1.BorderThickness = 1;
            metroSetPanel1.Controls.Add(lblImport);
            metroSetPanel1.Controls.Add(btnFile);
            metroSetPanel1.Controls.Add(pnlDragnDrop);
            metroSetPanel1.IsDerivedStyle = true;
            metroSetPanel1.Location = new Point(15, 50);
            metroSetPanel1.Name = "metroSetPanel1";
            metroSetPanel1.Size = new Size(267, 175);
            metroSetPanel1.Style = MetroSet_UI.Enums.Style.Dark;
            metroSetPanel1.StyleManager = null;
            metroSetPanel1.TabIndex = 8;
            metroSetPanel1.ThemeAuthor = "Narwin";
            metroSetPanel1.ThemeName = "MetroDark";
            // 
            // lblImport
            // 
            lblImport.Font = new Font("Microsoft Sans Serif", 13F, FontStyle.Regular, GraphicsUnit.Point);
            lblImport.IsDerivedStyle = true;
            lblImport.Location = new Point(7, 4);
            lblImport.Margin = new Padding(4, 0, 4, 0);
            lblImport.Name = "lblImport";
            lblImport.Size = new Size(217, 22);
            lblImport.Style = MetroSet_UI.Enums.Style.Light;
            lblImport.StyleManager = null;
            lblImport.TabIndex = 8;
            lblImport.Text = "Import File:";
            lblImport.ThemeAuthor = "Narwin";
            lblImport.ThemeName = "Custom";
            // 
            // btnFile
            // 
            btnFile.DisabledBackColor = Color.FromArgb(120, 65, 177, 225);
            btnFile.DisabledBorderColor = Color.FromArgb(120, 65, 177, 225);
            btnFile.DisabledForeColor = Color.Gray;
            btnFile.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Regular, GraphicsUnit.Point);
            btnFile.HoverBorderColor = Color.FromArgb(144, 228, 193);
            btnFile.HoverColor = Color.FromArgb(144, 228, 193);
            btnFile.HoverTextColor = Color.White;
            btnFile.IsDerivedStyle = true;
            btnFile.Location = new Point(74, 133);
            btnFile.Name = "btnFile";
            btnFile.NormalBorderColor = Color.Teal;
            btnFile.NormalColor = Color.Teal;
            btnFile.NormalTextColor = Color.White;
            btnFile.PressBorderColor = Color.FromArgb(144, 228, 193);
            btnFile.PressColor = Color.FromArgb(144, 228, 193);
            btnFile.PressTextColor = Color.White;
            btnFile.Size = new Size(117, 34);
            btnFile.Style = MetroSet_UI.Enums.Style.Custom;
            btnFile.StyleManager = null;
            btnFile.TabIndex = 8;
            btnFile.Text = "Browse for File";
            btnFile.ThemeAuthor = "Narwin";
            btnFile.ThemeName = "MetroLite";
            btnFile.Click += btnFile_Click;
            // 
            // pnlDragnDrop
            // 
            pnlDragnDrop.AllowDrop = true;
            pnlDragnDrop.BackgroundColor = Color.FromArgb(30, 30, 30);
            pnlDragnDrop.BorderColor = Color.FromArgb(110, 110, 110);
            pnlDragnDrop.BorderThickness = 1;
            pnlDragnDrop.Controls.Add(lblDrop);
            pnlDragnDrop.IsDerivedStyle = true;
            pnlDragnDrop.Location = new Point(36, 38);
            pnlDragnDrop.Name = "pnlDragnDrop";
            pnlDragnDrop.Size = new Size(213, 89);
            pnlDragnDrop.Style = MetroSet_UI.Enums.Style.Dark;
            pnlDragnDrop.StyleManager = null;
            pnlDragnDrop.TabIndex = 8;
            pnlDragnDrop.ThemeAuthor = "Narwin";
            pnlDragnDrop.ThemeName = "MetroDark";
            pnlDragnDrop.DragDrop += pnlDragnDrop_DragDrop;
            pnlDragnDrop.DragEnter += pnlDragnDrop_DragEnter;
            // 
            // lblDrop
            // 
            lblDrop.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Regular, GraphicsUnit.Point);
            lblDrop.IsDerivedStyle = true;
            lblDrop.Location = new Point(27, 25);
            lblDrop.Name = "lblDrop";
            lblDrop.Size = new Size(157, 42);
            lblDrop.Style = MetroSet_UI.Enums.Style.Dark;
            lblDrop.StyleManager = null;
            lblDrop.TabIndex = 0;
            lblDrop.Text = "Drop file here\r\nOR";
            lblDrop.TextAlign = ContentAlignment.TopCenter;
            lblDrop.ThemeAuthor = "Narwin";
            lblDrop.ThemeName = "MetroDark";
            // 
            // metroSetPanel4
            // 
            metroSetPanel4.BackgroundColor = Color.FromArgb(30, 30, 30);
            metroSetPanel4.BorderColor = Color.FromArgb(110, 110, 110);
            metroSetPanel4.BorderThickness = 1;
            metroSetPanel4.Controls.Add(btnCAOutputClear);
            metroSetPanel4.Controls.Add(lblCASolution);
            metroSetPanel4.Controls.Add(rtbOutput);
            metroSetPanel4.IsDerivedStyle = true;
            metroSetPanel4.Location = new Point(559, 50);
            metroSetPanel4.Name = "metroSetPanel4";
            metroSetPanel4.Size = new Size(538, 423);
            metroSetPanel4.Style = MetroSet_UI.Enums.Style.Dark;
            metroSetPanel4.StyleManager = null;
            metroSetPanel4.TabIndex = 10;
            metroSetPanel4.ThemeAuthor = "Narwin";
            metroSetPanel4.ThemeName = "MetroDark";
            // 
            // btnCAOutputClear
            // 
            btnCAOutputClear.DisabledBackColor = Color.Maroon;
            btnCAOutputClear.DisabledBorderColor = Color.Maroon;
            btnCAOutputClear.DisabledForeColor = Color.Gray;
            btnCAOutputClear.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Regular, GraphicsUnit.Point);
            btnCAOutputClear.HoverBorderColor = Color.FromArgb(252, 61, 57);
            btnCAOutputClear.HoverColor = Color.FromArgb(252, 61, 57);
            btnCAOutputClear.HoverTextColor = Color.White;
            btnCAOutputClear.IsDerivedStyle = true;
            btnCAOutputClear.Location = new Point(413, 4);
            btnCAOutputClear.Name = "btnCAOutputClear";
            btnCAOutputClear.NormalBorderColor = Color.FromArgb(227, 52, 55);
            btnCAOutputClear.NormalColor = Color.FromArgb(227, 52, 55);
            btnCAOutputClear.NormalTextColor = Color.White;
            btnCAOutputClear.PressBorderColor = Color.FromArgb(252, 61, 57);
            btnCAOutputClear.PressColor = Color.FromArgb(252, 61, 57);
            btnCAOutputClear.PressTextColor = Color.White;
            btnCAOutputClear.Size = new Size(117, 32);
            btnCAOutputClear.Style = MetroSet_UI.Enums.Style.Custom;
            btnCAOutputClear.StyleManager = null;
            btnCAOutputClear.TabIndex = 10;
            btnCAOutputClear.Text = "Clear";
            btnCAOutputClear.ThemeAuthor = "Narwin";
            btnCAOutputClear.ThemeName = "MetroLite";
            btnCAOutputClear.Click += btnCAOutputClear_Click;
            // 
            // lblCASolution
            // 
            lblCASolution.Font = new Font("Microsoft Sans Serif", 13F, FontStyle.Regular, GraphicsUnit.Point);
            lblCASolution.IsDerivedStyle = true;
            lblCASolution.Location = new Point(3, 3);
            lblCASolution.Name = "lblCASolution";
            lblCASolution.Size = new Size(205, 23);
            lblCASolution.Style = MetroSet_UI.Enums.Style.Dark;
            lblCASolution.StyleManager = null;
            lblCASolution.TabIndex = 7;
            lblCASolution.Text = "Critical Analysis Output:";
            lblCASolution.ThemeAuthor = "Narwin";
            lblCASolution.ThemeName = "MetroDark";
            // 
            // rtbOutput
            // 
            rtbOutput.AutoWordSelection = false;
            rtbOutput.BorderColor = Color.FromArgb(110, 110, 110);
            rtbOutput.DisabledBackColor = Color.FromArgb(80, 80, 80);
            rtbOutput.DisabledBorderColor = Color.FromArgb(109, 109, 109);
            rtbOutput.DisabledForeColor = Color.FromArgb(109, 109, 109);
            rtbOutput.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Regular, GraphicsUnit.Point);
            rtbOutput.HoverColor = Color.FromArgb(170, 170, 170);
            rtbOutput.IsDerivedStyle = true;
            rtbOutput.Lines = (new string[] { "", "", "", "\t\t\t", "", "", "", "", "", "\t\t\t\tFind a solution first..." });
            rtbOutput.Location = new Point(3, 38);
            rtbOutput.MaxLength = 32767;
            rtbOutput.Name = "rtbOutput";
            rtbOutput.ReadOnly = false;
            rtbOutput.Size = new Size(527, 382);
            rtbOutput.Style = MetroSet_UI.Enums.Style.Dark;
            rtbOutput.StyleManager = null;
            rtbOutput.TabIndex = 6;
            rtbOutput.Text = "\n\n\n\t\t\t\n\n\n\n\n\n\t\t\t\tFind a solution first...";
            rtbOutput.ThemeAuthor = "Narwin";
            rtbOutput.ThemeName = "MetroDark";
            rtbOutput.WordWrap = true;
            // 
            // metroSetPanel5
            // 
            metroSetPanel5.BackgroundColor = Color.FromArgb(30, 30, 30);
            metroSetPanel5.BorderColor = Color.FromArgb(110, 110, 110);
            metroSetPanel5.BorderThickness = 1;
            metroSetPanel5.Controls.Add(rtbFileOutput);
            metroSetPanel5.Controls.Add(lblFileOutput);
            metroSetPanel5.IsDerivedStyle = true;
            metroSetPanel5.Location = new Point(286, 50);
            metroSetPanel5.Name = "metroSetPanel5";
            metroSetPanel5.Size = new Size(267, 423);
            metroSetPanel5.Style = MetroSet_UI.Enums.Style.Dark;
            metroSetPanel5.StyleManager = null;
            metroSetPanel5.TabIndex = 9;
            metroSetPanel5.ThemeAuthor = "Narwin";
            metroSetPanel5.ThemeName = "MetroDark";
            // 
            // rtbFileOutput
            // 
            rtbFileOutput.AutoWordSelection = false;
            rtbFileOutput.BorderColor = Color.FromArgb(110, 110, 110);
            rtbFileOutput.DisabledBackColor = Color.FromArgb(80, 80, 80);
            rtbFileOutput.DisabledBorderColor = Color.FromArgb(109, 109, 109);
            rtbFileOutput.DisabledForeColor = Color.FromArgb(109, 109, 109);
            rtbFileOutput.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Regular, GraphicsUnit.Point);
            rtbFileOutput.HoverColor = Color.FromArgb(170, 170, 170);
            rtbFileOutput.IsDerivedStyle = true;
            rtbFileOutput.Lines = null;
            rtbFileOutput.Location = new Point(7, 38);
            rtbFileOutput.MaxLength = 32767;
            rtbFileOutput.Name = "rtbFileOutput";
            rtbFileOutput.ReadOnly = false;
            rtbFileOutput.Size = new Size(252, 382);
            rtbFileOutput.Style = MetroSet_UI.Enums.Style.Dark;
            rtbFileOutput.StyleManager = null;
            rtbFileOutput.TabIndex = 9;
            rtbFileOutput.Text = "Please add a file first...";
            rtbFileOutput.ThemeAuthor = "Narwin";
            rtbFileOutput.ThemeName = "MetroDark";
            rtbFileOutput.WordWrap = true;
            // 
            // lblFileOutput
            // 
            lblFileOutput.Font = new Font("Microsoft Sans Serif", 13F, FontStyle.Regular, GraphicsUnit.Point);
            lblFileOutput.IsDerivedStyle = true;
            lblFileOutput.Location = new Point(7, 4);
            lblFileOutput.Margin = new Padding(4, 0, 4, 0);
            lblFileOutput.Name = "lblFileOutput";
            lblFileOutput.Size = new Size(184, 22);
            lblFileOutput.Style = MetroSet_UI.Enums.Style.Dark;
            lblFileOutput.StyleManager = null;
            lblFileOutput.TabIndex = 8;
            lblFileOutput.Text = "Your file:";
            lblFileOutput.ThemeAuthor = "Narwin";
            lblFileOutput.ThemeName = "MetroDark";
            // 
            // metroSetPanel6
            // 
            metroSetPanel6.BackgroundColor = Color.FromArgb(30, 30, 30);
            metroSetPanel6.BorderColor = Color.FromArgb(110, 110, 110);
            metroSetPanel6.BorderThickness = 1;
            metroSetPanel6.Controls.Add(btnSolve);
            metroSetPanel6.Controls.Add(cboMethod);
            metroSetPanel6.Controls.Add(metroSetLabel9);
            metroSetPanel6.Controls.Add(lblSolve);
            metroSetPanel6.IsDerivedStyle = true;
            metroSetPanel6.Location = new Point(15, 229);
            metroSetPanel6.Name = "metroSetPanel6";
            metroSetPanel6.Size = new Size(267, 114);
            metroSetPanel6.Style = MetroSet_UI.Enums.Style.Dark;
            metroSetPanel6.StyleManager = null;
            metroSetPanel6.TabIndex = 12;
            metroSetPanel6.ThemeAuthor = "Narwin";
            metroSetPanel6.ThemeName = "MetroDark";
            // 
            // btnSolve
            // 
            btnSolve.DisabledBackColor = Color.DarkGreen;
            btnSolve.DisabledBorderColor = Color.DarkGreen;
            btnSolve.DisabledForeColor = Color.Gray;
            btnSolve.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Regular, GraphicsUnit.Point);
            btnSolve.HoverBorderColor = Color.FromArgb(83, 215, 105);
            btnSolve.HoverColor = Color.FromArgb(83, 215, 105);
            btnSolve.HoverTextColor = Color.White;
            btnSolve.IsDerivedStyle = true;
            btnSolve.Location = new Point(74, 73);
            btnSolve.Name = "btnSolve";
            btnSolve.NormalBorderColor = Color.FromArgb(70, 194, 99);
            btnSolve.NormalColor = Color.FromArgb(70, 194, 99);
            btnSolve.NormalTextColor = Color.White;
            btnSolve.PressBorderColor = Color.FromArgb(83, 215, 105);
            btnSolve.PressColor = Color.FromArgb(83, 215, 105);
            btnSolve.PressTextColor = Color.White;
            btnSolve.Size = new Size(117, 34);
            btnSolve.Style = MetroSet_UI.Enums.Style.Custom;
            btnSolve.StyleManager = null;
            btnSolve.TabIndex = 9;
            btnSolve.Text = "Solve";
            btnSolve.ThemeAuthor = "Narwin";
            btnSolve.ThemeName = "MetroLite";
            btnSolve.Click += btnSolve_Click;
            // 
            // cboMethod
            // 
            cboMethod.AllowDrop = true;
            cboMethod.ArrowColor = Color.FromArgb(110, 110, 110);
            cboMethod.BackColor = Color.Transparent;
            cboMethod.BackgroundColor = Color.FromArgb(34, 34, 34);
            cboMethod.BorderColor = Color.FromArgb(110, 110, 110);
            cboMethod.CausesValidation = false;
            cboMethod.DisabledBackColor = Color.FromArgb(80, 80, 80);
            cboMethod.DisabledBorderColor = Color.FromArgb(109, 109, 109);
            cboMethod.DisabledForeColor = Color.FromArgb(109, 109, 109);
            cboMethod.DrawMode = DrawMode.OwnerDrawFixed;
            cboMethod.DropDownStyle = ComboBoxStyle.DropDownList;
            cboMethod.Font = new Font("Microsoft Sans Serif", 11F, FontStyle.Regular, GraphicsUnit.Point);
            cboMethod.FormattingEnabled = true;
            cboMethod.IsDerivedStyle = true;
            cboMethod.ItemHeight = 20;
            cboMethod.Items.AddRange(new object[] { "Primal Simplex", "Dual Simplex", "Branch and Bound", "Cutting Plane" });
            cboMethod.Location = new Point(77, 36);
            cboMethod.Margin = new Padding(4);
            cboMethod.Name = "cboMethod";
            cboMethod.SelectedItemBackColor = Color.FromArgb(65, 177, 225);
            cboMethod.SelectedItemForeColor = Color.White;
            cboMethod.Size = new Size(172, 26);
            cboMethod.Style = MetroSet_UI.Enums.Style.Dark;
            cboMethod.StyleManager = null;
            cboMethod.TabIndex = 8;
            cboMethod.ThemeAuthor = "Narwin";
            cboMethod.ThemeName = "MetroDark";
            // 
            // metroSetLabel9
            // 
            metroSetLabel9.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Regular, GraphicsUnit.Point);
            metroSetLabel9.IsDerivedStyle = true;
            metroSetLabel9.Location = new Point(7, 40);
            metroSetLabel9.Margin = new Padding(4, 0, 4, 0);
            metroSetLabel9.Name = "metroSetLabel9";
            metroSetLabel9.Size = new Size(62, 22);
            metroSetLabel9.Style = MetroSet_UI.Enums.Style.Dark;
            metroSetLabel9.StyleManager = null;
            metroSetLabel9.TabIndex = 8;
            metroSetLabel9.Text = "Method:";
            metroSetLabel9.ThemeAuthor = "Narwin";
            metroSetLabel9.ThemeName = "MetroDark";
            // 
            // lblSolve
            // 
            lblSolve.Font = new Font("Microsoft Sans Serif", 13F, FontStyle.Regular, GraphicsUnit.Point);
            lblSolve.IsDerivedStyle = true;
            lblSolve.Location = new Point(7, 0);
            lblSolve.Margin = new Padding(4, 0, 4, 0);
            lblSolve.Name = "lblSolve";
            lblSolve.Size = new Size(184, 22);
            lblSolve.Style = MetroSet_UI.Enums.Style.Dark;
            lblSolve.StyleManager = null;
            lblSolve.TabIndex = 8;
            lblSolve.Text = "Solver:";
            lblSolve.ThemeAuthor = "Narwin";
            lblSolve.ThemeName = "MetroDark";
            // 
            // cbForm
            // 
            cbForm.Anchor = AnchorStyles.None;
            cbForm.CloseHoverBackColor = Color.FromArgb(183, 40, 40);
            cbForm.CloseHoverForeColor = Color.White;
            cbForm.CloseNormalForeColor = Color.Gray;
            cbForm.DisabledForeColor = Color.DimGray;
            cbForm.IsDerivedStyle = true;
            cbForm.Location = new Point(994, -279);
            cbForm.MaximizeBox = true;
            cbForm.MaximizeHoverBackColor = Color.FromArgb(238, 238, 238);
            cbForm.MaximizeHoverForeColor = Color.Gray;
            cbForm.MaximizeNormalForeColor = Color.Gray;
            cbForm.MinimizeBox = true;
            cbForm.MinimizeHoverBackColor = Color.FromArgb(238, 238, 238);
            cbForm.MinimizeHoverForeColor = Color.Gray;
            cbForm.MinimizeNormalForeColor = Color.Gray;
            cbForm.Name = "cbForm";
            cbForm.Size = new Size(100, 25);
            cbForm.Style = MetroSet_UI.Enums.Style.Light;
            cbForm.StyleManager = null;
            cbForm.TabIndex = 16;
            cbForm.Text = "metroSetControlBox1";
            cbForm.ThemeAuthor = "Narwin";
            cbForm.ThemeName = "MetroLite";
            // 
            // btnClose
            // 
            btnClose.DisabledBackColor = Color.Maroon;
            btnClose.DisabledBorderColor = Color.Maroon;
            btnClose.DisabledForeColor = Color.Gray;
            btnClose.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Regular, GraphicsUnit.Point);
            btnClose.HoverBorderColor = Color.Red;
            btnClose.HoverColor = Color.Red;
            btnClose.HoverTextColor = Color.White;
            btnClose.IsDerivedStyle = true;
            btnClose.Location = new Point(1079, 0);
            btnClose.Name = "btnClose";
            btnClose.NormalBorderColor = Color.Black;
            btnClose.NormalColor = Color.FromArgb(192, 0, 0);
            btnClose.NormalTextColor = Color.White;
            btnClose.PressBorderColor = Color.Red;
            btnClose.PressColor = Color.Red;
            btnClose.PressTextColor = Color.White;
            btnClose.Size = new Size(18, 18);
            btnClose.Style = MetroSet_UI.Enums.Style.Custom;
            btnClose.StyleManager = null;
            btnClose.TabIndex = 11;
            btnClose.Text = "X";
            btnClose.ThemeAuthor = "Narwin";
            btnClose.ThemeName = "MetroLite";
            btnClose.Click += metroSetButton1_Click;
            // 
            // MainMenu
            // 
            AutoScaleDimensions = new SizeF(14F, 29F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoSize = true;
            BackColor = SystemColors.ButtonFace;
            BackgroundColor = Color.FromArgb(109, 152, 145);
            BackgroundImageLayout = ImageLayout.Center;
            ClientSize = new Size(1098, 485);
            Controls.Add(btnClose);
            Controls.Add(cbForm);
            Controls.Add(metroSetPanel6);
            Controls.Add(metroSetPanel5);
            Controls.Add(metroSetPanel4);
            Controls.Add(metroSetPanel1);
            Font = new Font("Microsoft Sans Serif", 18F, FontStyle.Regular, GraphicsUnit.Point);
            Margin = new Padding(4);
            Name = "MainMenu";
            Padding = new Padding(12, 90, 12, 12);
            ShowLeftRect = false;
            Style = MetroSet_UI.Enums.Style.Custom;
            Text = "LP Solver";
            TextColor = Color.FromArgb(227, 235, 138);
            ThemeName = "MetroDark";
            Load += MainMenu_Load;
            metroSetPanel1.ResumeLayout(false);
            pnlDragnDrop.ResumeLayout(false);
            metroSetPanel4.ResumeLayout(false);
            metroSetPanel5.ResumeLayout(false);
            metroSetPanel6.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion
        private MetroSet_UI.Controls.MetroSetPanel metroSetPanel1;
        private MetroSet_UI.Controls.MetroSetLabel lblImport;
        private MetroSet_UI.Controls.MetroSetButton btnFile;
        private MetroSet_UI.Controls.MetroSetPanel pnlDragnDrop;
        private MetroSet_UI.Controls.MetroSetLabel lblDrop;
        private MetroSet_UI.Controls.MetroSetPanel metroSetPanel4;
        private MetroSet_UI.Controls.MetroSetLabel lblCASolution;
        private MetroSet_UI.Controls.MetroSetRichTextBox rtbOutput;
        private MetroSet_UI.Controls.MetroSetPanel metroSetPanel5;
        private MetroSet_UI.Controls.MetroSetRichTextBox rtbFileOutput;
        private MetroSet_UI.Controls.MetroSetLabel lblFileOutput;
        private MetroSet_UI.Controls.MetroSetPanel metroSetPanel6;
        private MetroSet_UI.Controls.MetroSetButton btnSolve;
        private MetroSet_UI.Controls.MetroSetComboBox cboMethod;
        private MetroSet_UI.Controls.MetroSetLabel metroSetLabel9;
        private MetroSet_UI.Controls.MetroSetLabel lblSolve;
        private MetroSet_UI.Controls.MetroSetControlBox cbForm;
        private MetroSet_UI.Controls.MetroSetButton btnCAOutputClear;
        private MetroSet_UI.Controls.MetroSetButton btnClose;
    }
}