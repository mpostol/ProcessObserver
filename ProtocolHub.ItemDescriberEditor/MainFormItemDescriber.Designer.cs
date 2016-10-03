using System.Windows.Forms;

namespace CAS.CommServer.DA.ItemDescriberEditor
{
  partial class MainFormItemDescriber
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
    private System.Windows.Forms.DataGrid dataGrid_items;
    private System.Windows.Forms.MainMenu mainMenu_form;
    private System.Windows.Forms.MenuItem menuItem1;
    private System.Windows.Forms.MenuItem menuItem2;
    private System.Windows.Forms.MenuItem menuItem3;
    private System.Windows.Forms.MenuItem menuItem4;
    private System.Windows.Forms.MenuItem menuItem5;
    private System.Windows.Forms.MenuItem menuItem6;
    private System.Windows.Forms.MenuItem menuItem7;
    private System.Windows.Forms.MenuItem menuItem8;
    private System.Windows.Forms.MenuItem menuItem9;
    private System.Windows.Forms.MenuItem menuItem10;
    private System.Windows.Forms.DataGrid dataGrid_Property;
    private System.Windows.Forms.MenuItem menuItem11;
    private System.Windows.Forms.MenuItem menuItem12;
    private System.Windows.Forms.MenuItem menuItem13;
    private MenuItem menuItem14;

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      this.components = new System.ComponentModel.Container();
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainFormItemDescriber));
      this.m_ItemDescriberDataSet = new BaseStation.ItemDescriber.ItemDecriberDataSet();
      this.dataGrid_items = new System.Windows.Forms.DataGrid();
      this.mainMenu_form = new System.Windows.Forms.MainMenu(this.components);
      this.menuItem1 = new System.Windows.Forms.MenuItem();
      this.menuItem2 = new System.Windows.Forms.MenuItem();
      this.menuItem3 = new System.Windows.Forms.MenuItem();
      this.menuItem4 = new System.Windows.Forms.MenuItem();
      this.menuItem5 = new System.Windows.Forms.MenuItem();
      this.menuItem6 = new System.Windows.Forms.MenuItem();
      this.menuItem7 = new System.Windows.Forms.MenuItem();
      this.menuItem8 = new System.Windows.Forms.MenuItem();
      this.menuItem9 = new System.Windows.Forms.MenuItem();
      this.menuItem10 = new System.Windows.Forms.MenuItem();
      this.menuItem11 = new System.Windows.Forms.MenuItem();
      this.menuItem13 = new System.Windows.Forms.MenuItem();
      this.menuItem12 = new System.Windows.Forms.MenuItem();
      this.dataGrid_Property = new System.Windows.Forms.DataGrid();
      this.menuItem14 = new System.Windows.Forms.MenuItem();
      ((System.ComponentModel.ISupportInitialize)(this.m_ItemDescriberDataSet)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.dataGrid_items)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.dataGrid_Property)).BeginInit();
      this.SuspendLayout();
      // 
      // itemDecriberDataSet1
      // 
      this.m_ItemDescriberDataSet.DataSetName = "ItemDecriberDataSet";
      this.m_ItemDescriberDataSet.Locale = new System.Globalization.CultureInfo("en-US");
      this.m_ItemDescriberDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
      // 
      // dataGrid_items
      // 
      this.dataGrid_items.DataMember = "";
      this.dataGrid_items.DataSource = this.m_ItemDescriberDataSet.Items;
      this.dataGrid_items.HeaderForeColor = System.Drawing.SystemColors.ControlText;
      this.dataGrid_items.Location = new System.Drawing.Point(24, 16);
      this.dataGrid_items.Name = "dataGrid_items";
      this.dataGrid_items.Size = new System.Drawing.Size(448, 480);
      this.dataGrid_items.TabIndex = 0;
      // 
      // mainMenu_form
      // 
      this.mainMenu_form.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem1,
            this.menuItem11});
      // 
      // menuItem1
      // 
      this.menuItem1.Index = 0;
      this.menuItem1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem2,
            this.menuItem3,
            this.menuItem4,
            this.menuItem7,
            this.menuItem8,
            this.menuItem10});
      this.menuItem1.Text = "Menu";
      // 
      // menuItem2
      // 
      this.menuItem2.Index = 0;
      this.menuItem2.Text = "New";
      this.menuItem2.Click += new System.EventHandler(this.menuItem2_Click);
      // 
      // menuItem3
      // 
      this.menuItem3.Index = 1;
      this.menuItem3.Text = "Open...";
      this.menuItem3.Click += new System.EventHandler(this.menuItem3_Click);
      // 
      // menuItem4
      // 
      this.menuItem4.Index = 2;
      this.menuItem4.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem5,
            this.menuItem6});
      this.menuItem4.Text = "Import";
      // 
      // menuItem5
      // 
      this.menuItem5.Index = 0;
      this.menuItem5.Text = "NetworkConfig - file";
      // 
      // menuItem6
      // 
      this.menuItem6.Index = 1;
      this.menuItem6.Text = "CSV";
      this.menuItem6.Click += new System.EventHandler(this.menuItem6_Click);
      // 
      // menuItem7
      // 
      this.menuItem7.Index = 3;
      this.menuItem7.Text = "Save...";
      this.menuItem7.Click += new System.EventHandler(this.menuItem7_Click);
      // 
      // menuItem8
      // 
      this.menuItem8.Index = 4;
      this.menuItem8.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem9});
      this.menuItem8.Text = "Export";
      // 
      // menuItem9
      // 
      this.menuItem9.Index = 0;
      this.menuItem9.Text = "CSV";
      this.menuItem9.Click += new System.EventHandler(this.menuItem9_Click);
      // 
      // menuItem10
      // 
      this.menuItem10.Index = 5;
      this.menuItem10.Text = "Exit";
      this.menuItem10.Click += new System.EventHandler(this.menuItem10_Click);
      // 
      // menuItem11
      // 
      this.menuItem11.Index = 1;
      this.menuItem11.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem13,
            this.menuItem12,
            this.menuItem14});
      this.menuItem11.Text = "Help";
      // 
      // menuItem13
      // 
      this.menuItem13.Index = 0;
      this.menuItem13.Text = "Pomoc";
      // 
      // menuItem12
      // 
      this.menuItem12.Index = 1;
      this.menuItem12.Text = "About ItemDescriber";
      this.menuItem12.Click += new System.EventHandler(this.menuItem12_Click);
      // 
      // dataGrid_Property
      // 
      this.dataGrid_Property.AllowNavigation = false;
      this.dataGrid_Property.DataMember = "";
      this.dataGrid_Property.DataSource = this.m_ItemDescriberDataSet.Property;
      this.dataGrid_Property.HeaderForeColor = System.Drawing.SystemColors.ControlText;
      this.dataGrid_Property.Location = new System.Drawing.Point(488, 16);
      this.dataGrid_Property.Name = "dataGrid_Property";
      this.dataGrid_Property.ReadOnly = true;
      this.dataGrid_Property.Size = new System.Drawing.Size(264, 480);
      this.dataGrid_Property.TabIndex = 1;
      // 
      // menuItem14
      // 
      this.menuItem14.Index = 2;
      this.menuItem14.Text = "License information";
      this.menuItem14.Click += new System.EventHandler(this.menuItem14_Click);
      // 
      // MainFormItemDescriber
      // 
      this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
      this.ClientSize = new System.Drawing.Size(776, 509);
      this.Controls.Add(this.dataGrid_Property);
      this.Controls.Add(this.dataGrid_items);
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.Menu = this.mainMenu_form;
      this.Name = "MainFormItemDescriber";
      this.Text = "ItemDescriber";
      ((System.ComponentModel.ISupportInitialize)(this.m_ItemDescriberDataSet)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.dataGrid_items)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.dataGrid_Property)).EndInit();
      this.ResumeLayout(false);

    }
    #endregion

  }
}