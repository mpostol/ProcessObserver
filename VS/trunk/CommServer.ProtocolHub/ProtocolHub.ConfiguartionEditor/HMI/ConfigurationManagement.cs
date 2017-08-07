//<summary>
//  Title   : Configuration Management
//  System  : Microsoft Visual C# .NET 
//  $LastChangedDate$
//  $Rev$
//  $LastChangedBy$
//  $URL$
//  $Id$
//  History :
//    20081203: mzbrzezny: undoredomanager is switched off
//    20081022: mzbrzezny: modification to clear function
//    Tomek Siwecki - 2007-2-19
//    Refomated and some code changes
//    Mariusz Postol - 2006-10-19
//      reformated
//    Maciej Zbrzezny - 2006-09-19
//    created
//
//  Copyright (C)2008, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto:techsupp@cas.eu
//  http://www.cas.eu
//</summary>

using CAS.CommServer.ProtocolHub.ConfigurationEditor.Properties;
using CAS.Lib.RTLib.Database;
using CAS.NetworkConfigLib;
using System;
using System.Data;
using System.Globalization;
using System.Windows.Forms;

namespace CAS.CommServer.ProtocolHub.ConfigurationEditor.HMI
{

  public delegate void ConfigIOHandler(Form form);
  public static class ConfigurationManagement
  {

    #region Properties
    public static string ConfigFileName
    {
      get { return m_ProtocolHubConfigurationSIngleton; }
      set { m_ProtocolHubConfigurationSIngleton = value; }
    }
    #endregion

    #region Constructors
    static ConfigurationManagement()
    {
      ProtocolHubConfiguration = new ComunicationNet();
      ProtocolHubConfiguration.DataSetName = "ComunicationNet";
      ProtocolHubConfiguration.Locale = new CultureInfo("en-US");
      ProtocolHubConfiguration.SchemaSerializationMode = SchemaSerializationMode.IncludeSchema;
    }
    #endregion

    #region public
    public static ComunicationNet ProtocolHubConfiguration { get; private set; }
    public static void ClearConfig(Form form)
    {
      if (MessageBox.Show(form, "Clear all data grids???", "Clear", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
        ClearProtocolHubConfiguration(form);
    }
    internal static void SaveDemoProc(Form form)
    {
      MessageBox.Show(Resources.tx_DemoWriteErr, Resources.tx_licenseCap, MessageBoxButtons.OK, MessageBoxIcon.Hand);
    }
    public static void SaveProc(Form form)
    {
      SaveFileDialog saveXMLFileDialog = new SaveFileDialog();
      saveXMLFileDialog.OverwritePrompt = true;
      saveXMLFileDialog.Filter = "XML files|*.xml";
      saveXMLFileDialog.DefaultExt = "xml";
      saveXMLFileDialog.InitialDirectory = CAS.Lib.CodeProtect.InstallContextNames.ApplicationDataPath;
      if (!string.IsNullOrEmpty(ConfigurationManagement.ConfigFileName))
        saveXMLFileDialog.FileName = ConfigurationManagement.ConfigFileName;
      switch (saveXMLFileDialog.ShowDialog())
      {
        case DialogResult.OK:
          try
          {
            XML2DataSetIO.writeXMLFile(ProtocolHubConfiguration, saveXMLFileDialog.FileName);
            UpdateFormName(form, saveXMLFileDialog.FileName);
            ConfigFileName = saveXMLFileDialog.FileName;
            ProtocolHubConfiguration.Channels.AcceptChanges();
            ProtocolHubConfiguration.Protocol.AcceptChanges();
            ProtocolHubConfiguration.SerialSetings.AcceptChanges();
            ProtocolHubConfiguration.Station.AcceptChanges();
            ProtocolHubConfiguration.Segments.AcceptChanges();
            ProtocolHubConfiguration.Interfaces.AcceptChanges();
            ProtocolHubConfiguration.Groups.AcceptChanges();
            ProtocolHubConfiguration.Tags.AcceptChanges();
            ProtocolHubConfiguration.TagBit.AcceptChanges();
            ProtocolHubConfiguration.DataBlocks.AcceptChanges();
          }
          catch (Exception e)
          {
            MessageBox.Show("Error", "I cant save file to this location because: " + e.Message, MessageBoxButtons.OK, MessageBoxIcon.Error);
          }
          break;
        default:
          break;
      }
    }
    public static void ReadConfiguration(Form form)
    {
      OpenFileDialog openFileDialogXMLFile = new OpenFileDialog();
      if ((ProtocolHubConfiguration.Channels.GetChanges() != null)
        || (ProtocolHubConfiguration.Protocol.GetChanges() != null)
        || (ProtocolHubConfiguration.SerialSetings.GetChanges() != null)
        || (ProtocolHubConfiguration.Segments.GetChanges() != null)
        || (ProtocolHubConfiguration.Station.GetChanges() != null)
        || (ProtocolHubConfiguration.Interfaces.GetChanges() != null)
        || (ProtocolHubConfiguration.Groups.GetChanges() != null)
        || (ProtocolHubConfiguration.Tags.GetChanges() != null)
        || (ProtocolHubConfiguration.TagBit.GetChanges() != null)
        || (ProtocolHubConfiguration.DataBlocks.GetChanges() != null))
      {
        if (MessageBox.Show(form, "Save current data?", "Data changed", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
          SaveProc(form);
      }
      openFileDialogXMLFile.InitialDirectory = CAS.Lib.CodeProtect.InstallContextNames.ApplicationDataPath;
      openFileDialogXMLFile.Filter = "XML files|*.xml";
      openFileDialogXMLFile.DefaultExt = ".XML";
      switch (openFileDialogXMLFile.ShowDialog())
      {
        case DialogResult.OK:
          ClearProtocolHubConfiguration(form);
#if UNDOREDO
          RTLib.DataBase.UndoRedo.UndoRedoMenager.SuspendLogging();
#endif
          try
          {
            XML2DataSetIO.readXMLFile(ProtocolHubConfiguration, openFileDialogXMLFile.FileName);
            //int idx = 0;
            //foreach ( ComunicationNet.DataBlocksRow cr in configDataBase.DataBlocks )
            //{
            //  //cr.DatBlockID = idx++;
            //  foreach ( ComunicationNet.TagsRow ctg in cr.GetTagsRows() )
            //    ctg.DatBlockID = cr.DatBlockID;
            //}
          }
          catch (Exception e)
          {
            MessageBox.Show("Error", "I cant load file from this location because: " + e.Message, MessageBoxButtons.OK, MessageBoxIcon.Error);
          }
          //((Button)sender).Enabled = false;
          ConfigFileName = openFileDialogXMLFile.FileName;
          UpdateFormName(form, openFileDialogXMLFile.FileName);
          ProtocolHubConfiguration.Channels.AcceptChanges();
          ProtocolHubConfiguration.Protocol.AcceptChanges();
          ProtocolHubConfiguration.SerialSetings.AcceptChanges();
          ProtocolHubConfiguration.Station.AcceptChanges();
          ProtocolHubConfiguration.Segments.AcceptChanges();
          ProtocolHubConfiguration.Interfaces.AcceptChanges();
          ProtocolHubConfiguration.Groups.AcceptChanges();
          ProtocolHubConfiguration.Tags.AcceptChanges();
          ProtocolHubConfiguration.TagBit.AcceptChanges();
          ProtocolHubConfiguration.DataBlocks.AcceptChanges();
#if UNDOREDO
          RTLib.DataBase.UndoRedo.UndoRedoMenager.ClearLog();
#endif
          break;
        default:
          break;
      }
    }
    #endregion //public

    #region private
    private static string m_ProtocolHubConfigurationSIngleton;
    private static void UpdateFormName(Form form, string filename)
    {
      form.Text = "Network configuration ";
      if (!string.IsNullOrEmpty(filename))
        form.Text += filename;
    }
    /// <summary>
    /// Clears curent configuration dataset
    /// </summary>
    private static void ClearProtocolHubConfiguration(Form form)
    {
      ProtocolHubConfiguration.Clear();
      UpdateFormName(form, null);
      ConfigFileName = null;
    }
    #endregion

  }

}
