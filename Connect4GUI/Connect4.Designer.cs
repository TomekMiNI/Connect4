namespace Connect4GUI
{
  partial class Connect4
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
      this.MainPanel = new System.Windows.Forms.TableLayoutPanel();
      this.StartButton = new System.Windows.Forms.Button();
      this.TurnLabel = new System.Windows.Forms.Label();
      this.vsGB = new System.Windows.Forms.GroupBox();
      this.abRB = new System.Windows.Forms.RadioButton();
      this.playerRB = new System.Windows.Forms.RadioButton();
      this.startBox = new System.Windows.Forms.GroupBox();
      this.playerStartRB = new System.Windows.Forms.RadioButton();
      this.youStartBox = new System.Windows.Forms.RadioButton();
      this.mctsRB = new System.Windows.Forms.RadioButton();
      this.puctRB = new System.Windows.Forms.RadioButton();
      this.tunedRB = new System.Windows.Forms.RadioButton();
      this.MainPanel.SuspendLayout();
      this.vsGB.SuspendLayout();
      this.startBox.SuspendLayout();
      this.SuspendLayout();
      // 
      // MainPanel
      // 
      this.MainPanel.ColumnCount = 8;
      this.MainPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
      this.MainPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
      this.MainPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
      this.MainPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
      this.MainPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
      this.MainPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
      this.MainPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
      this.MainPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
      this.MainPanel.Controls.Add(this.StartButton, 7, 0);
      this.MainPanel.Controls.Add(this.TurnLabel, 7, 1);
      this.MainPanel.Controls.Add(this.vsGB, 7, 2);
      this.MainPanel.Controls.Add(this.startBox, 7, 4);
      this.MainPanel.Dock = System.Windows.Forms.DockStyle.Fill;
      this.MainPanel.Location = new System.Drawing.Point(0, 0);
      this.MainPanel.Margin = new System.Windows.Forms.Padding(6);
      this.MainPanel.Name = "MainPanel";
      this.MainPanel.Padding = new System.Windows.Forms.Padding(2);
      this.MainPanel.RowCount = 6;
      this.MainPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
      this.MainPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
      this.MainPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
      this.MainPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
      this.MainPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
      this.MainPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
      this.MainPanel.Size = new System.Drawing.Size(800, 450);
      this.MainPanel.TabIndex = 0;
      this.MainPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.mainPanel_Paint);
      this.MainPanel.MouseClick += mainPanel_MouseClick;
      // 
      // StartButton
      // 
      this.StartButton.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.StartButton.Location = new System.Drawing.Point(698, 5);
      this.StartButton.Name = "StartButton";
      this.StartButton.Size = new System.Drawing.Size(97, 68);
      this.StartButton.TabIndex = 0;
      this.StartButton.Text = "START";
      this.StartButton.UseVisualStyleBackColor = true;
      this.StartButton.Click += new System.EventHandler(this.StartButton_Click);
      // 
      // TurnLabel
      // 
      this.TurnLabel.Anchor = System.Windows.Forms.AnchorStyles.None;
      this.TurnLabel.AutoSize = true;
      this.TurnLabel.Location = new System.Drawing.Point(732, 106);
      this.TurnLabel.Name = "TurnLabel";
      this.TurnLabel.Size = new System.Drawing.Size(29, 13);
      this.TurnLabel.TabIndex = 1;
      this.TurnLabel.Text = "Turn";
      // 
      // vsGB
      // 
      this.vsGB.Controls.Add(this.tunedRB);
      this.vsGB.Controls.Add(this.puctRB);
      this.vsGB.Controls.Add(this.mctsRB);
      this.vsGB.Controls.Add(this.abRB);
      this.vsGB.Controls.Add(this.playerRB);
      this.vsGB.Location = new System.Drawing.Point(698, 153);
      this.vsGB.Name = "vsGB";
      this.MainPanel.SetRowSpan(this.vsGB, 2);
      this.vsGB.Size = new System.Drawing.Size(97, 142);
      this.vsGB.TabIndex = 3;
      this.vsGB.TabStop = false;
      this.vsGB.Text = "VS";
      // 
      // abRB
      // 
      this.abRB.AutoSize = true;
      this.abRB.Location = new System.Drawing.Point(6, 45);
      this.abRB.Name = "abRB";
      this.abRB.Size = new System.Drawing.Size(74, 17);
      this.abRB.TabIndex = 3;
      this.abRB.Text = "AlphaBeta";
      this.abRB.UseVisualStyleBackColor = true;
      // 
      // playerRB
      // 
      this.playerRB.AutoSize = true;
      this.playerRB.Checked = true;
      this.playerRB.Location = new System.Drawing.Point(6, 19);
      this.playerRB.Name = "playerRB";
      this.playerRB.Size = new System.Drawing.Size(54, 17);
      this.playerRB.TabIndex = 2;
      this.playerRB.TabStop = true;
      this.playerRB.Text = "Player";
      this.playerRB.UseVisualStyleBackColor = true;
      // 
      // startBox
      // 
      this.startBox.Controls.Add(this.playerStartRB);
      this.startBox.Controls.Add(this.youStartBox);
      this.startBox.Location = new System.Drawing.Point(698, 301);
      this.startBox.Name = "startBox";
      this.startBox.Size = new System.Drawing.Size(97, 68);
      this.startBox.TabIndex = 4;
      this.startBox.TabStop = false;
      this.startBox.Text = "Starts";
      // 
      // playerStartRB
      // 
      this.playerStartRB.AutoSize = true;
      this.playerStartRB.Location = new System.Drawing.Point(6, 45);
      this.playerStartRB.Name = "playerStartRB";
      this.playerStartRB.Size = new System.Drawing.Size(72, 17);
      this.playerStartRB.TabIndex = 0;
      this.playerStartRB.Text = "Opponent";
      this.playerStartRB.UseVisualStyleBackColor = true;
      // 
      // youStartBox
      // 
      this.youStartBox.AutoSize = true;
      this.youStartBox.Checked = true;
      this.youStartBox.Location = new System.Drawing.Point(6, 20);
      this.youStartBox.Name = "youStartBox";
      this.youStartBox.Size = new System.Drawing.Size(44, 17);
      this.youStartBox.TabIndex = 0;
      this.youStartBox.TabStop = true;
      this.youStartBox.Text = "You";
      this.youStartBox.UseVisualStyleBackColor = true;
      // 
      // mctsRB
      // 
      this.mctsRB.AutoSize = true;
      this.mctsRB.Location = new System.Drawing.Point(6, 68);
      this.mctsRB.Name = "mctsRB";
      this.mctsRB.Size = new System.Drawing.Size(55, 17);
      this.mctsRB.TabIndex = 4;
      this.mctsRB.TabStop = true;
      this.mctsRB.Text = "MCTS";
      this.mctsRB.UseVisualStyleBackColor = true;
      // 
      // puctRB
      // 
      this.puctRB.AutoSize = true;
      this.puctRB.Location = new System.Drawing.Point(6, 91);
      this.puctRB.Name = "puctRB";
      this.puctRB.Size = new System.Drawing.Size(54, 17);
      this.puctRB.TabIndex = 4;
      this.puctRB.TabStop = true;
      this.puctRB.Text = "PUCT";
      this.puctRB.UseVisualStyleBackColor = true;
      // 
      // tunedRB
      // 
      this.tunedRB.AutoSize = true;
      this.tunedRB.Location = new System.Drawing.Point(6, 114);
      this.tunedRB.Name = "tunedRB";
      this.tunedRB.Size = new System.Drawing.Size(94, 17);
      this.tunedRB.TabIndex = 5;
      this.tunedRB.TabStop = true;
      this.tunedRB.Text = "UCB1-TUNED";
      this.tunedRB.UseVisualStyleBackColor = true;
      // 
      // Connect4
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(800, 450);
      this.Controls.Add(this.MainPanel);
      this.MaximizeBox = false;
      this.Name = "Connect4";
      this.Text = "Connect4";
      this.MainPanel.ResumeLayout(false);
      this.MainPanel.PerformLayout();
      this.vsGB.ResumeLayout(false);
      this.vsGB.PerformLayout();
      this.startBox.ResumeLayout(false);
      this.startBox.PerformLayout();
      this.ResumeLayout(false);

    }



    #endregion

    private System.Windows.Forms.TableLayoutPanel MainPanel;
    private System.Windows.Forms.Button StartButton;
    private System.Windows.Forms.Label TurnLabel;
    private System.Windows.Forms.GroupBox vsGB;
    private System.Windows.Forms.RadioButton abRB;
    private System.Windows.Forms.RadioButton playerRB;
    private System.Windows.Forms.GroupBox startBox;
    private System.Windows.Forms.RadioButton playerStartRB;
    private System.Windows.Forms.RadioButton youStartBox;
    private System.Windows.Forms.RadioButton tunedRB;
    private System.Windows.Forms.RadioButton puctRB;
    private System.Windows.Forms.RadioButton mctsRB;
  }
}

