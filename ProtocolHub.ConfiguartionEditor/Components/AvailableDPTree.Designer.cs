//___________________________________________________________________________________
//
//  Copyright (C) 2020, Mariusz Postol LODZ POLAND.
//
//  To be in touch join the community at GITTER: https://gitter.im/mpostol/OPC-UA-OOI
//___________________________________________________________________________________

namespace CAS.CommServer.ProtocolHub.ConfigurationEditor.Components
{
  partial class AvailableDPTree
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
      System.Windows.Forms.SplitContainer c_SplitContainer;
      System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode( "Data providers" );
      this.c_TreeView = new System.Windows.Forms.TreeView();
      this.c_PropertyGrid = new System.Windows.Forms.PropertyGrid();
      c_SplitContainer = new System.Windows.Forms.SplitContainer();
      c_SplitContainer.Panel1.SuspendLayout();
      c_SplitContainer.Panel2.SuspendLayout();
      c_SplitContainer.SuspendLayout();
      this.SuspendLayout();
      // 
      // c_SplitContainer
      // 
      c_SplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
      c_SplitContainer.Location = new System.Drawing.Point( 0, 0 );
      c_SplitContainer.Name = "c_SplitContainer";
      // 
      // c_SplitContainer.Panel1
      // 
      c_SplitContainer.Panel1.Controls.Add( this.c_TreeView );
      // 
      // c_SplitContainer.Panel2
      // 
      c_SplitContainer.Panel2.Controls.Add( this.c_PropertyGrid );
      c_SplitContainer.Size = new System.Drawing.Size( 522, 378 );
      c_SplitContainer.SplitterDistance = 169;
      c_SplitContainer.TabIndex = 0;
      // 
      // c_TreeView
      // 
      this.c_TreeView.Dock = System.Windows.Forms.DockStyle.Fill;
      this.c_TreeView.Location = new System.Drawing.Point( 0, 0 );
      this.c_TreeView.Name = "c_TreeView";
      treeNode1.Name = "c_DPTreeRoot";
      treeNode1.Text = "Data providers";
      treeNode1.ToolTipText = "Available  data providers";
      this.c_TreeView.Nodes.AddRange( new System.Windows.Forms.TreeNode[] {
            treeNode1} );
      this.c_TreeView.Size = new System.Drawing.Size( 169, 378 );
      this.c_TreeView.TabIndex = 0;
      this.c_TreeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler( this.c_TreeView_AfterSelect );
      // 
      // c_PropertyGrid
      // 
      this.c_PropertyGrid.Dock = System.Windows.Forms.DockStyle.Fill;
      this.c_PropertyGrid.Location = new System.Drawing.Point( 0, 0 );
      this.c_PropertyGrid.Name = "c_PropertyGrid";
      this.c_PropertyGrid.Size = new System.Drawing.Size( 349, 378 );
      this.c_PropertyGrid.TabIndex = 0;
      this.c_PropertyGrid.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler( this.c_PropertyGrid_PropertyValueChanged );
      // 
      // AvailableDPTree
      // 
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
      this.Controls.Add( c_SplitContainer );
      this.Name = "AvailableDPTree";
      this.Size = new System.Drawing.Size( 522, 378 );
      c_SplitContainer.Panel1.ResumeLayout( false );
      c_SplitContainer.Panel2.ResumeLayout( false );
      c_SplitContainer.ResumeLayout( false );
      this.ResumeLayout( false );

    }

    #endregion

    private System.Windows.Forms.TreeView c_TreeView;
    private System.Windows.Forms.PropertyGrid c_PropertyGrid;

  }
}
