//<summary>
//  Title   : MainForm for ItemDescriber
//  System  : Microsoft Visual C# .NET 2008
//  $LastChangedDate$
//  $Rev$
//  $LastChangedBy$
//  $URL$
//  $Id$
//
//  Copyright (C)2009, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//</summary>

using BaseStation.ItemDescriber;
using CAS.Lib.CodeProtect.Controls;
using CAS.Lib.ControlLibrary;
using Opc.Da;
using System;
using System.Reflection;
using System.Windows.Forms;

namespace CAS.CommServer.DA.ItemDescriberEditor
{

  /// <summary>
  /// Summary description for Form1.
  /// </summary>
  public partial class MainFormItemDescriber : System.Windows.Forms.Form
  {

    /// <summary>
    /// Initializes a new instance of the <see cref="MainFormItemDescriber"/> class.
    /// </summary>
    public MainFormItemDescriber()
    {
      InitializeComponent();
      ClearAndInitialize();
    }

    #region private
    private BaseStation.ItemDescriber.ItemDecriberDataSet m_ItemDescriberDataSet;
    private void CloseForm()
    {
      Close();
    }
    private void ClearAndInitialize()
    {
      m_ItemDescriberDataSet.ItemProperty.Clear();
      m_ItemDescriberDataSet.Items.Clear();
      m_ItemDescriberDataSet.Property.Clear();
      foreach (PropertyDescription property in PropertyDescription.Enumerate())
      {
        ItemDecriberDataSet.PropertyRow row = m_ItemDescriberDataSet.Property.NewPropertyRow();
        row.Code = property.ID.Code;
        row.Name = property.Name;
        m_ItemDescriberDataSet.Property.AddPropertyRow(row);
      }
    }
    private void SaveXML()
    {
      SaveFileDialog saveXMLFileDialog = new SaveFileDialog();
      saveXMLFileDialog.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory;
      switch (saveXMLFileDialog.ShowDialog())
      {
        case System.Windows.Forms.DialogResult.OK:
          XMLManagement myConfig = new XMLManagement();
          myConfig.writeXMLFile(m_ItemDescriberDataSet, saveXMLFileDialog.FileName);
          m_ItemDescriberDataSet.Items.AcceptChanges();
          m_ItemDescriberDataSet.ItemProperty.AcceptChanges();
          break;
        default:
          break;
      }
    }
    private void LoadXML()
    {
      if ((m_ItemDescriberDataSet.Items.GetChanges() != null)
        || (m_ItemDescriberDataSet.ItemProperty.GetChanges() != null))
      {
        if (MessageBox.Show(this, "Save current data?", "Data changed", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
        {
          SaveXML();
        }
      }
      System.Windows.Forms.OpenFileDialog openFileDialogXMLFile = new System.Windows.Forms.OpenFileDialog();
      openFileDialogXMLFile.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory;
      openFileDialogXMLFile.Filter = "XML files (*.XML)|*.XML";
      openFileDialogXMLFile.DefaultExt = ".XML";
      switch (openFileDialogXMLFile.ShowDialog())
      {
        case System.Windows.Forms.DialogResult.OK:
          m_ItemDescriberDataSet.ItemProperty.Clear();
          m_ItemDescriberDataSet.Items.Clear();
          m_ItemDescriberDataSet.Property.Clear();
          XMLManagement myConfig = new XMLManagement();
          myConfig.readXMLFile(m_ItemDescriberDataSet, openFileDialogXMLFile.FileName);
          //((Button)sender).Enabled = false;
          this.Text = "Item Describer: " + openFileDialogXMLFile.FileName;
          m_ItemDescriberDataSet.Items.AcceptChanges();
          m_ItemDescriberDataSet.ItemProperty.AcceptChanges();
          break;
        default:
          break;
      }
    }
    private void ExportCSV()
    {
      System.Windows.Forms.SaveFileDialog saveXMLFileDialog = new SaveFileDialog();
      saveXMLFileDialog.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory;
      saveXMLFileDialog.Filter = "CSV files (*.CSV)|*.CSV";
      saveXMLFileDialog.DefaultExt = ".CSV";
      switch (saveXMLFileDialog.ShowDialog())
      {
        case System.Windows.Forms.DialogResult.OK:
          CSVManagement myConfig = new CSVManagement();
          myConfig.SaveCSV(m_ItemDescriberDataSet, saveXMLFileDialog.FileName);
          //itemDecriberDataSet1.Items.AcceptChanges();
          //itemDecriberDataSet1.ItemProperty.AcceptChanges();
          break;
        default:
          break;
      }
    }
    private void ImportCSV()
    {
      if ((m_ItemDescriberDataSet.Items.GetChanges() != null) || (m_ItemDescriberDataSet.ItemProperty.GetChanges() != null))
      {
        if (MessageBox.Show(this, "Save current data?", "Data changed", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
          SaveXML();
      }
      OpenFileDialog openFileDialogXMLFile = new OpenFileDialog();
      openFileDialogXMLFile.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory;
      openFileDialogXMLFile.Filter = "CSV files (*.CSV)|*.CSV";
      openFileDialogXMLFile.DefaultExt = ".CSV";
      switch (openFileDialogXMLFile.ShowDialog())
      {
        case System.Windows.Forms.DialogResult.OK:
          CSVManagement _csvManagement = new CSVManagement();
          _csvManagement.LoadCSV(m_ItemDescriberDataSet, openFileDialogXMLFile.FileName);
          break;
        default:
          break;
      }
    }

    #region handlers
    private void menuItem10_Click(object sender, System.EventArgs e)
    {
      this.CloseForm();
    }

    private void menuItem3_Click(object sender, System.EventArgs e)
    {
      LoadXML();
    }

    private void menuItem7_Click(object sender, System.EventArgs e)
    {
      SaveXML();
    }

    private void menuItem2_Click(object sender, System.EventArgs e)
    {
      if (MessageBox.Show(this, "Clear all data grids???", "Clear", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
      {
        ClearAndInitialize();
      }
    }

    private void menuItem9_Click(object sender, System.EventArgs e)
    {
      ExportCSV();
    }
    private void ShowAboutDialog()
    {
      string usr = "";
      Assembly cMyAss = Assembly.GetEntryAssembly();
      using (AboutForm cAboutForm = new AboutForm(null, usr, cMyAss))
      {
        cAboutForm.ShowDialog();
      }
    }

    private void menuItem12_Click(object sender, System.EventArgs e)
    {
      ShowAboutDialog();
    }

    private void menuItem6_Click(object sender, System.EventArgs e)
    {
      ImportCSV();
    }

    private void menuItem14_Click(object sender, EventArgs e)
    {
      string usr = "";
      Assembly cMyAss = Assembly.GetEntryAssembly();
      using (LicenseForm cAboutForm = new LicenseForm(null, usr, cMyAss))
      {
        using (Licences cLicDial = new Licences())
        {
          cAboutForm.SetAdditionalControl = cLicDial;
          cAboutForm.LicenceRequestMessageProvider
            = new LicenseForm.LicenceRequestMessageProviderDelegate(
               delegate () { return cLicDial.GetLicenseMessageRequest(); });
          cAboutForm.ShowDialog();
        }
      }
    }
    #endregion    

    #endregion

  }
}
