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

using System;
using System.ComponentModel;
using System.Reflection;
using System.Windows.Forms;
using CAS.Lib.CodeProtect.Controls;
using CAS.Lib.ControlLibrary;
using Opc.Da;
using BaseStation.ItemDescriber;

namespace CAS.CommServer.DA.ItemDescriberEditor
{
  /// <summary>
  /// Summary description for Form1.
  /// </summary>
  public partial class MainFormItemDescriber: System.Windows.Forms.Form
  {
    private BaseStation.ItemDescriber.ItemDecriberDataSet itemDecriberDataSet1;

    /// <summary>
    /// constructor for main form for itemdescriber
    /// </summary>
    public MainFormItemDescriber()
    {
      //
      // Required for Windows Form Designer support
      //
      InitializeComponent();
      ClearAndInit();

      //
      // TODO: Add any constructor code after InitializeComponent call
      //
    }

    #region dodane funkcje
    private void CloseForm()
    {
      Close();
    }
    private void ClearAndInit()
    {
      itemDecriberDataSet1.ItemProperty.Clear();
      itemDecriberDataSet1.Items.Clear();
      itemDecriberDataSet1.Property.Clear();
      foreach ( PropertyDescription property in PropertyDescription.Enumerate() )
      {
        ItemDecriberDataSet.PropertyRow row = itemDecriberDataSet1.Property.NewPropertyRow();
        row.Code = property.ID.Code;
        row.Name = property.Name;
        itemDecriberDataSet1.Property.AddPropertyRow( row );
      }

    }
    private void SaveProc()
    {
      System.Windows.Forms.SaveFileDialog saveXMLFileDialog = new SaveFileDialog();
      saveXMLFileDialog.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory;
      switch ( saveXMLFileDialog.ShowDialog() )
      {
        case System.Windows.Forms.DialogResult.OK:
          XMLManagement myConfig = new XMLManagement();
          myConfig.writeXMLFile( itemDecriberDataSet1, saveXMLFileDialog.FileName );
          itemDecriberDataSet1.Items.AcceptChanges();
          itemDecriberDataSet1.ItemProperty.AcceptChanges();
          break;
        default:
          break;
      }

    }
    private void LoadProc()
    {
      if ( ( itemDecriberDataSet1.Items.GetChanges() != null )
        || ( itemDecriberDataSet1.ItemProperty.GetChanges() != null ) )
      {
        if ( MessageBox.Show( this, "Save current data?", "Data changed", MessageBoxButtons.YesNo, MessageBoxIcon.Question ) == DialogResult.Yes )
        {
          SaveProc();
        }
      }
      System.Windows.Forms.OpenFileDialog openFileDialogXMLFile = new System.Windows.Forms.OpenFileDialog();
      openFileDialogXMLFile.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory;
      openFileDialogXMLFile.Filter = "XML files (*.XML)|*.XML";
      openFileDialogXMLFile.DefaultExt = ".XML";
      switch ( openFileDialogXMLFile.ShowDialog() )
      {
        case System.Windows.Forms.DialogResult.OK:
          itemDecriberDataSet1.ItemProperty.Clear();
          itemDecriberDataSet1.Items.Clear();
          itemDecriberDataSet1.Property.Clear();
          XMLManagement myConfig = new XMLManagement();
          myConfig.readXMLFile( itemDecriberDataSet1, openFileDialogXMLFile.FileName );
          //((Button)sender).Enabled = false;
          this.Text = "Item Describer: " + openFileDialogXMLFile.FileName;
          itemDecriberDataSet1.Items.AcceptChanges();
          itemDecriberDataSet1.ItemProperty.AcceptChanges();
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
      switch ( saveXMLFileDialog.ShowDialog() )
      {
        case System.Windows.Forms.DialogResult.OK:
          CSVManagement myConfig = new CSVManagement();
          myConfig.SaveCSV( itemDecriberDataSet1, saveXMLFileDialog.FileName );
          //itemDecriberDataSet1.Items.AcceptChanges();
          //itemDecriberDataSet1.ItemProperty.AcceptChanges();
          break;
        default:
          break;
      }
    }
    private void ImportCSV()
    {
      if ( ( itemDecriberDataSet1.Items.GetChanges() != null )
        || ( itemDecriberDataSet1.ItemProperty.GetChanges() != null ) )
      {
        if ( MessageBox.Show( this, "Save current data?", "Data changed", MessageBoxButtons.YesNo, MessageBoxIcon.Question ) == DialogResult.Yes )
        {
          SaveProc();
        }
      }
      System.Windows.Forms.OpenFileDialog openFileDialogXMLFile = new System.Windows.Forms.OpenFileDialog();
      openFileDialogXMLFile.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory;
      openFileDialogXMLFile.Filter = "CSV files (*.CSV)|*.CSV";
      openFileDialogXMLFile.DefaultExt = ".CSV";
      switch ( openFileDialogXMLFile.ShowDialog() )
      {
        case System.Windows.Forms.DialogResult.OK:
          CSVManagement myConfig = new CSVManagement();
          myConfig.LoadCSV( itemDecriberDataSet1, openFileDialogXMLFile.FileName );
          break;
        default:
          break;
      }
    }
    #endregion

    private void menuItem10_Click( object sender, System.EventArgs e )
    {
      this.CloseForm();
    }

    private void menuItem3_Click( object sender, System.EventArgs e )
    {
      LoadProc();
    }

    private void menuItem7_Click( object sender, System.EventArgs e )
    {
      SaveProc();
    }

    private void menuItem2_Click( object sender, System.EventArgs e )
    {
      if ( MessageBox.Show( this, "Clear all data grids???", "Clear", MessageBoxButtons.YesNo, MessageBoxIcon.Question ) == DialogResult.Yes )
      {
        ClearAndInit();
      }
    }

    private void menuItem9_Click( object sender, System.EventArgs e )
    {
      ExportCSV();
    }
    private void ShowAboutDialog()
    {
      string usr = "";
      Assembly cMyAss = Assembly.GetEntryAssembly();
      using ( AboutForm cAboutForm = new AboutForm( null, usr, cMyAss ) )
      {
        cAboutForm.ShowDialog();
      }
    }

    private void menuItem12_Click( object sender, System.EventArgs e )
    {
      ShowAboutDialog();
    }

    private void menuItem6_Click( object sender, System.EventArgs e )
    {
      ImportCSV();
    }

    private void menuItem14_Click( object sender, EventArgs e )
    {
      string usr = "";
      Assembly cMyAss = Assembly.GetEntryAssembly();
      using ( LicenseForm cAboutForm = new LicenseForm( null, usr, cMyAss ) )
      {
        using ( Licences cLicDial = new Licences() )
        {
          cAboutForm.SetAdditionalControl = cLicDial;
          cAboutForm.LicenceRequestMessageProvider
            = new LicenseForm.LicenceRequestMessageProviderDelegate(
               delegate() { return cLicDial.GetLicenseMessageRequest(); } );
          cAboutForm.ShowDialog();
        }
      }
    }
  }
}
