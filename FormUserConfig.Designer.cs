namespace NightscoutTrayIconMonitor
{
    partial class FormUserConfig
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
            this.lblStatusToken = new System.Windows.Forms.Label();
            this.lblStatusServer = new System.Windows.Forms.Label();
            this.rbUnitsMmol = new System.Windows.Forms.RadioButton();
            this.rbUnitsMg = new System.Windows.Forms.RadioButton();
            this.tbTokenServer = new System.Windows.Forms.TextBox();
            this.tbNameServer = new System.Windows.Forms.TextBox();
            this.lblUnits = new System.Windows.Forms.Label();
            this.lblToken = new System.Windows.Forms.Label();
            this.lblServer = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.lblTimer = new System.Windows.Forms.Label();
            this.tbTimer = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // lblStatusToken
            // 
            this.lblStatusToken.AutoSize = true;
            this.lblStatusToken.Location = new System.Drawing.Point(672, 128);
            this.lblStatusToken.Name = "lblStatusToken";
            this.lblStatusToken.Size = new System.Drawing.Size(33, 20);
            this.lblStatusToken.TabIndex = 23;
            this.lblStatusToken.Text = "Ok!";
            // 
            // lblStatusServer
            // 
            this.lblStatusServer.AutoSize = true;
            this.lblStatusServer.Location = new System.Drawing.Point(672, 79);
            this.lblStatusServer.Name = "lblStatusServer";
            this.lblStatusServer.Size = new System.Drawing.Size(33, 20);
            this.lblStatusServer.TabIndex = 22;
            this.lblStatusServer.Text = "Ok!";
            // 
            // rbUnitsMmol
            // 
            this.rbUnitsMmol.AutoSize = true;
            this.rbUnitsMmol.Location = new System.Drawing.Point(275, 226);
            this.rbUnitsMmol.Name = "rbUnitsMmol";
            this.rbUnitsMmol.Size = new System.Drawing.Size(78, 24);
            this.rbUnitsMmol.TabIndex = 21;
            this.rbUnitsMmol.TabStop = true;
            this.rbUnitsMmol.Text = "mmol/L";
            this.rbUnitsMmol.UseVisualStyleBackColor = true;
            // 
            // rbUnitsMg
            // 
            this.rbUnitsMg.AutoSize = true;
            this.rbUnitsMg.Location = new System.Drawing.Point(168, 226);
            this.rbUnitsMg.Name = "rbUnitsMg";
            this.rbUnitsMg.Size = new System.Drawing.Size(71, 24);
            this.rbUnitsMg.TabIndex = 20;
            this.rbUnitsMg.TabStop = true;
            this.rbUnitsMg.Text = "mg/dL";
            this.rbUnitsMg.UseVisualStyleBackColor = true;
            // 
            // tbTokenServer
            // 
            this.tbTokenServer.Location = new System.Drawing.Point(168, 125);
            this.tbTokenServer.Name = "tbTokenServer";
            this.tbTokenServer.Size = new System.Drawing.Size(483, 26);
            this.tbTokenServer.TabIndex = 19;
            // 
            // tbNameServer
            // 
            this.tbNameServer.Location = new System.Drawing.Point(168, 76);
            this.tbNameServer.Name = "tbNameServer";
            this.tbNameServer.Size = new System.Drawing.Size(483, 26);
            this.tbNameServer.TabIndex = 18;
            // 
            // lblUnits
            // 
            this.lblUnits.AutoSize = true;
            this.lblUnits.Location = new System.Drawing.Point(49, 228);
            this.lblUnits.Name = "lblUnits";
            this.lblUnits.Size = new System.Drawing.Size(46, 20);
            this.lblUnits.TabIndex = 17;
            this.lblUnits.Text = "Units";
            // 
            // lblToken
            // 
            this.lblToken.AutoSize = true;
            this.lblToken.Location = new System.Drawing.Point(49, 128);
            this.lblToken.Name = "lblToken";
            this.lblToken.Size = new System.Drawing.Size(53, 20);
            this.lblToken.TabIndex = 16;
            this.lblToken.Text = "Token";
            // 
            // lblServer
            // 
            this.lblServer.AutoSize = true;
            this.lblServer.Location = new System.Drawing.Point(49, 79);
            this.lblServer.Name = "lblServer";
            this.lblServer.Size = new System.Drawing.Size(55, 20);
            this.lblServer.TabIndex = 15;
            this.lblServer.Text = "Server";
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(765, 30);
            this.label1.TabIndex = 14;
            this.label1.Text = "Nightscout Tray Icon Monitor";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(657, 269);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(105, 35);
            this.btnCancel.TabIndex = 13;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(546, 269);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(105, 35);
            this.btnSave.TabIndex = 12;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // lblTimer
            // 
            this.lblTimer.AutoSize = true;
            this.lblTimer.Location = new System.Drawing.Point(49, 178);
            this.lblTimer.Name = "lblTimer";
            this.lblTimer.Size = new System.Drawing.Size(70, 20);
            this.lblTimer.TabIndex = 24;
            this.lblTimer.Text = "Timer (s)";
            // 
            // tbTimer
            // 
            this.tbTimer.Location = new System.Drawing.Point(168, 175);
            this.tbTimer.Name = "tbTimer";
            this.tbTimer.Size = new System.Drawing.Size(104, 26);
            this.tbTimer.TabIndex = 25;
            // 
            // FormUserConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(789, 329);
            this.Controls.Add(this.tbTimer);
            this.Controls.Add(this.lblTimer);
            this.Controls.Add(this.lblStatusToken);
            this.Controls.Add(this.lblStatusServer);
            this.Controls.Add(this.rbUnitsMmol);
            this.Controls.Add(this.rbUnitsMg);
            this.Controls.Add(this.tbTokenServer);
            this.Controls.Add(this.tbNameServer);
            this.Controls.Add(this.lblUnits);
            this.Controls.Add(this.lblToken);
            this.Controls.Add(this.lblServer);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Name = "FormUserConfig";
            this.Text = "FormUserConfig";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblStatusToken;
        private System.Windows.Forms.Label lblStatusServer;
        private System.Windows.Forms.RadioButton rbUnitsMmol;
        private System.Windows.Forms.RadioButton rbUnitsMg;
        private System.Windows.Forms.TextBox tbTokenServer;
        private System.Windows.Forms.TextBox tbNameServer;
        private System.Windows.Forms.Label lblUnits;
        private System.Windows.Forms.Label lblToken;
        private System.Windows.Forms.Label lblServer;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Label lblTimer;
        private System.Windows.Forms.TextBox tbTimer;
    }
}