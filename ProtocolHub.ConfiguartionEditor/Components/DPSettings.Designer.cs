namespace CAS.CommServer.ProtocolHub.ConfigurationEditor.Components
{
  partial class DPSettings
  {
    /// <summary> 
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary> 
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose( bool disposing )
    {
      if ( disposing && ( components != null ) )
      {
        components.Dispose();
      }
      base.Dispose( disposing );
    }

    #region Component Designer generated code

    /// <summary> 
    /// Required method for Designer support - do not modify 
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager( typeof( DPSettings ) );
      this.cn_PropertyGridApp = new System.Windows.Forms.PropertyGrid();
      this.cn_PropertyGridComm = new System.Windows.Forms.PropertyGrid();
      this.cn_SplitContainer = new System.Windows.Forms.SplitContainer();
      this.toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
      this.toolStrip1 = new System.Windows.Forms.ToolStrip();
      this.cn_ToolStripButtonApp = new System.Windows.Forms.ToolStripButton();
      this.cn_ToolStripButtonComm = new System.Windows.Forms.ToolStripButton();
      this.cn_SplitContainer.Panel1.SuspendLayout();
      this.cn_SplitContainer.Panel2.SuspendLayout();
      this.cn_SplitContainer.SuspendLayout();
      this.toolStripContainer1.ContentPanel.SuspendLayout();
      this.toolStripContainer1.TopToolStripPanel.SuspendLayout();
      this.toolStripContainer1.SuspendLayout();
      this.toolStrip1.SuspendLayout();
      this.SuspendLayout();
      // 
      // cn_PropertyGridApp
      // 
      this.cn_PropertyGridApp.Dock = System.Windows.Forms.DockStyle.Fill;
      this.cn_PropertyGridApp.Location = new System.Drawing.Point( 0, 0 );
      this.cn_PropertyGridApp.Name = "cn_PropertyGridApp";
      this.cn_PropertyGridApp.Size = new System.Drawing.Size( 494, 190 );
      this.cn_PropertyGridApp.TabIndex = 0;
      // 
      // cn_PropertyGridComm
      // 
      this.cn_PropertyGridComm.Dock = System.Windows.Forms.DockStyle.Fill;
      this.cn_PropertyGridComm.Location = new System.Drawing.Point( 0, 0 );
      this.cn_PropertyGridComm.Name = "cn_PropertyGridComm";
      this.cn_PropertyGridComm.Size = new System.Drawing.Size( 494, 186 );
      this.cn_PropertyGridComm.TabIndex = 1;
      // 
      // cn_SplitContainer
      // 
      this.cn_SplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
      this.cn_SplitContainer.Location = new System.Drawing.Point( 0, 0 );
      this.cn_SplitContainer.Name = "cn_SplitContainer";
      this.cn_SplitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
      // 
      // cn_SplitContainer.Panel1
      // 
      this.cn_SplitContainer.Panel1.Controls.Add( this.cn_PropertyGridApp );
      // 
      // cn_SplitContainer.Panel2
      // 
      this.cn_SplitContainer.Panel2.Controls.Add( this.cn_PropertyGridComm );
      this.cn_SplitContainer.Size = new System.Drawing.Size( 494, 380 );
      this.cn_SplitContainer.SplitterDistance = 190;
      this.cn_SplitContainer.TabIndex = 2;
      // 
      // toolStripContainer1
      // 
      this.toolStripContainer1.BottomToolStripPanelVisible = false;
      // 
      // toolStripContainer1.ContentPanel
      // 
      this.toolStripContainer1.ContentPanel.Controls.Add( this.cn_SplitContainer );
      this.toolStripContainer1.ContentPanel.Size = new System.Drawing.Size( 494, 380 );
      this.toolStripContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.toolStripContainer1.LeftToolStripPanelVisible = false;
      this.toolStripContainer1.Location = new System.Drawing.Point( 0, 0 );
      this.toolStripContainer1.Name = "toolStripContainer1";
      this.toolStripContainer1.RightToolStripPanelVisible = false;
      this.toolStripContainer1.Size = new System.Drawing.Size( 494, 405 );
      this.toolStripContainer1.TabIndex = 5;
      this.toolStripContainer1.Text = "toolStripContainer1";
      // 
      // toolStripContainer1.TopToolStripPanel
      // 
      this.toolStripContainer1.TopToolStripPanel.Controls.Add( this.toolStrip1 );
      // 
      // toolStrip1
      // 
      this.toolStrip1.Dock = System.Windows.Forms.DockStyle.None;
      this.toolStrip1.Items.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.cn_ToolStripButtonApp,
            this.cn_ToolStripButtonComm} );
      this.toolStrip1.Location = new System.Drawing.Point( 3, 0 );
      this.toolStrip1.Name = "toolStrip1";
      this.toolStrip1.Size = new System.Drawing.Size( 214, 25 );
      this.toolStrip1.TabIndex = 0;
      // 
      // cn_ToolStripButtonApp
      // 
      this.cn_ToolStripButtonApp.Checked = true;
      this.cn_ToolStripButtonApp.CheckOnClick = true;
      this.cn_ToolStripButtonApp.CheckState = System.Windows.Forms.CheckState.Checked;
      this.cn_ToolStripButtonApp.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
      this.cn_ToolStripButtonApp.Image = ( (System.Drawing.Image)( resources.GetObject( "cn_ToolStripButtonApp.Image" ) ) );
      this.cn_ToolStripButtonApp.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.cn_ToolStripButtonApp.Name = "cn_ToolStripButtonApp";
      this.cn_ToolStripButtonApp.Size = new System.Drawing.Size( 83, 22 );
      this.cn_ToolStripButtonApp.Text = "Protocol on/off";
      this.cn_ToolStripButtonApp.Click += new System.EventHandler( this.cn_ToolStripButtonApp_Click );
      // 
      // cn_ToolStripButtonComm
      // 
      this.cn_ToolStripButtonComm.Checked = true;
      this.cn_ToolStripButtonComm.CheckOnClick = true;
      this.cn_ToolStripButtonComm.CheckState = System.Windows.Forms.CheckState.Checked;
      this.cn_ToolStripButtonComm.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
      this.cn_ToolStripButtonComm.Image = ( (System.Drawing.Image)( resources.GetObject( "cn_ToolStripButtonComm.Image" ) ) );
      this.cn_ToolStripButtonComm.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.cn_ToolStripButtonComm.Name = "cn_ToolStripButtonComm";
      this.cn_ToolStripButtonComm.Size = new System.Drawing.Size( 119, 22 );
      this.cn_ToolStripButtonComm.Text = "Communication  on/off";
      this.cn_ToolStripButtonComm.Click += new System.EventHandler( this.cn_ToolStripButtonComm_Click );
      // 
      // DPSettings
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add( this.toolStripContainer1 );
      this.Name = "DPSettings";
      this.Size = new System.Drawing.Size( 494, 405 );
      this.cn_SplitContainer.Panel1.ResumeLayout( false );
      this.cn_SplitContainer.Panel2.ResumeLayout( false );
      this.cn_SplitContainer.ResumeLayout( false );
      this.toolStripContainer1.ContentPanel.ResumeLayout( false );
      this.toolStripContainer1.TopToolStripPanel.ResumeLayout( false );
      this.toolStripContainer1.TopToolStripPanel.PerformLayout();
      this.toolStripContainer1.ResumeLayout( false );
      this.toolStripContainer1.PerformLayout();
      this.toolStrip1.ResumeLayout( false );
      this.toolStrip1.PerformLayout();
      this.ResumeLayout( false );

    }

    #endregion

    private System.Windows.Forms.PropertyGrid cn_PropertyGridApp;
    private System.Windows.Forms.PropertyGrid cn_PropertyGridComm;
    private System.Windows.Forms.SplitContainer cn_SplitContainer;
    private System.Windows.Forms.ToolStripContainer toolStripContainer1;
    private System.Windows.Forms.ToolStrip toolStrip1;
    private System.Windows.Forms.ToolStripButton cn_ToolStripButtonApp;
    private System.Windows.Forms.ToolStripButton cn_ToolStripButtonComm;
  }
}
