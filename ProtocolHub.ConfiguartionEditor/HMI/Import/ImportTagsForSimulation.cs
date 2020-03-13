//___________________________________________________________________________________
//
//  Copyright (C) 2020, Mariusz Postol LODZ POLAND.
//
//  To be in touch join the community at GITTER: https://gitter.im/mpostol/OPC-UA-OOI
//___________________________________________________________________________________

using CAS.NetworkConfigLib;
using System;
using System.ComponentModel;
using UAOOI.ProcessObserver.RealTime;
using UAOOI.Windows.Forms;

namespace CAS.CommServer.ProtocolHub.ConfigurationEditor.HMI.Import
{
  /// <summary>
  /// Summary description for ImportTagsForSimulation.
  /// </summary>
  internal class ImportTagsForSimulation : ImportFunctionRootClass
  {

    #region ImportTagsForSimulationInfo
    internal class ImportTagsForSimulationInfo : ImportFileControll.ImportInfo
    {

      #region private
      private uint m_stationID = 0;
      private uint m_address = 0;
      private uint m_datatype = 0;
      #endregion

      public override string ImportName => "Import Tags for simulation";
      public override string InitialDirectory => AppDomain.CurrentDomain.BaseDirectory;
      /// <summary>
      /// default browse filter for the dialog which is used for selecting a file
      /// </summary>
      public override string BrowseFilter => "Tags for simulation TXT file (*.TXT)|*.TXT";
      /// <summary>
      /// default extension for the dialog which is used for selecting a file
      /// </summary>
      public override string DefaultExt => ".TXT";
      /// <summary>
      /// text that is used to show the information about this importing function
      /// </summary>
      public override string InformationText => "This function import tags from file - each line in file is one tag name";
      [
      BrowsableAttribute(true),
      CategoryAttribute("Settings"),
      DescriptionAttribute("ID of the station")
      ]
      public uint StationID
      {
        get => m_stationID;
        set => m_stationID = value;
      }
      [
      BrowsableAttribute(true),
      CategoryAttribute("Settings"),
      DescriptionAttribute("Address of this DataBlock")
      ]
      public uint Address
      {
        get => m_address;
        set => m_address = value;
      }
      [
      BrowsableAttribute(true),
      CategoryAttribute("Settings"),
      DescriptionAttribute("Data type of this DataBlock")
      ]
      public uint DataType
      {
        get => m_datatype;
        set => m_datatype = value;
      }

    }
    #endregion

    #region private
    private readonly CAS.NetworkConfigLib.ComunicationNet m_database;
    private ImportTagsForSimulationInfo m_ImportTagsForSimulationInfo;
    #endregion

    #region ImportFunctionRootClass
    protected override void DoTheImport()
    {
      #region IMPORT
      //robimy jakies importowanie:
      long stationid = m_ImportTagsForSimulationInfo.StationID;
      ulong address = m_ImportTagsForSimulationInfo.Address;
      ulong datatype = m_ImportTagsForSimulationInfo.DataType;
      string filename = m_ImportTagsForSimulationInfo.Filename;
      int changes_number = 0;
      //dodajemy grupe:
      ComunicationNet.GroupsRow grow = HMI.ConfigurationManagement.ProtocolHubConfiguration.Groups.NewGroupsRow();
      //grow.GroupID = groupid;
      grow.Name = "SimulationGR_" + grow.GroupID.ToString();
      grow.StationID = stationid;
      grow.TimeScan = 1000;
      grow.TimeScanFast = 1000;
      grow.TimeOut = 10000;
      grow.TimeOutFast = 10000;
      ConfigurationManagement.ProtocolHubConfiguration.Groups.AddGroupsRow(grow);
      //dodajemy datablock
      ComunicationNet.DataBlocksRow drow = HMI.ConfigurationManagement.ProtocolHubConfiguration.DataBlocks.NewDataBlocksRow();
      drow.GroupID = grow.GroupID;
      //drow.SatationID = stationid;
      drow.Name = "SimulationDB_" + grow.GroupID.ToString();
      drow.Address = address;
      drow.DataType = datatype;
      ConfigurationManagement.ProtocolHubConfiguration.DataBlocks.AddDataBlocksRow(drow);
      //teraz otworzymy plik i dodamy wszystkie tagi:
      System.IO.StreamReader plik = new System.IO.StreamReader(filename, System.Text.Encoding.Default);
      string plikzawartosc = plik.ReadToEnd();
      plik.Close();
      string Tagname = "-- unknowname --";
      while (plikzawartosc.Length > 0)
      {
        try
        {
          int pos = plikzawartosc.IndexOf("\r\n");
          if (pos < 0)
            pos = plikzawartosc.Length;
          Tagname = plikzawartosc.Substring(0, pos);
          plikzawartosc = plikzawartosc.Remove(0, pos + 2);
          //dodajemy taga:
          ComunicationNet.TagsRow trow = HMI.ConfigurationManagement.ProtocolHubConfiguration.Tags.NewTagsRow();
          trow.Name = Tagname;
          trow.AccessRights = (sbyte)ItemAccessRights.ReadWrite;
          trow.DatBlockID = drow.DatBlockID;
          trow.AlarmMask = 0;
          trow.StateMask = 0;
          trow.StateTrigger = (sbyte)StateTrigger.None;
          trow.Alarm = false;
          ConfigurationManagement.ProtocolHubConfiguration.Tags.AddTagsRow(trow);
          changes_number++;
        }
        catch (
Exception
#if DEBUG
 ex
#endif
 )
        {
          AppendToLog("problem with tag:" + Tagname + " :"
#if DEBUG
 + ex.Message.ToString()
#endif
 );
        }
      }
      #endregion IMPORT
      AppendToLog("Number of changed tags: " + changes_number.ToString());
    }

    #endregion

    #region creator
    public ImportTagsForSimulation(CAS.NetworkConfigLib.ComunicationNet database, System.Windows.Forms.Form parrent_form)
      : base(parrent_form)
    {
      m_database = database;
      m_ImportTagsForSimulationInfo = new ImportTagsForSimulationInfo();
      SetImportInfo(m_ImportTagsForSimulationInfo);
    }
    #endregion

  }
}
