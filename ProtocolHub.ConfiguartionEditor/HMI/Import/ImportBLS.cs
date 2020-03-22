//___________________________________________________________________________________
//
//  Copyright (C) 2020, Mariusz Postol LODZ POLAND.
//
//  To be in touch join the community at GITTER: https://gitter.im/mpostol/OPC-UA-OOI
//___________________________________________________________________________________

using System;
using System.Collections;
using System.ComponentModel;
using System.IO;
using System.Text.RegularExpressions;
using UAOOI.ProcessObserver.Configuration;
using UAOOI.ProcessObserver.RealTime;
using UAOOI.Windows.Forms;

namespace CAS.CommServer.ProtocolHub.ConfigurationEditor.HMI.Import
{
  /// <summary>
  /// Summary description for ImportBLS.
  /// </summary>
  internal class ImportBLS : ImportFunctionRootClass
  {
    internal enum PROTOCOL { SBUS, MODBUS };

    #region ImportBLSInfo

    internal class ImportBLSInfo : ImportFileControll.ImportInfo
    {
      #region private

      private uint tout2tscan = 10;
      private uint tf2tn = 1;
      private PROTOCOL m_protocol;

      #endregion private

      public override string ImportName => "Import BLS";
      public override string InitialDirectory => AppDomain.CurrentDomain.BaseDirectory;

      /// <summary>
      /// default browse filter for the dialog which is used for selecting a file
      /// </summary>
      public override string BrowseFilter => "WIZCON BLS files (*.BLS)|*.BLS";

      /// <summary>
      /// default extension for the dialog which is used for selecting a file
      /// </summary>
      public override string DefaultExt => ".BLS";

      /// <summary>
      /// text that is used to show the information about this importing function
      /// </summary>
      public override string InformationText => "Importing Wizcon BLS files";

      [
      BrowsableAttribute(true),
      CategoryAttribute("Settings"),
      DescriptionAttribute("Timeout2ScanTimeCoefiecent")
      ]
      public uint Timeout2ScanTimeCoefiecent
      {
        get => tout2tscan;
        set => tout2tscan = value;
      }

      [
      BrowsableAttribute(true),
      CategoryAttribute("Settings"),
      DescriptionAttribute("ScanTimeNormal2ScanTimeFaseCoef")
      ]
      public uint ScanTimeNormal2ScanTimeFaseCoef
      {
        get => tf2tn;
        set => tf2tn = value;
      }

      [
      BrowsableAttribute(true),
      CategoryAttribute("Settings"),
      DescriptionAttribute("PROTOCOL")
      ]
      public PROTOCOL Protocol
      {
        get => m_protocol;
        set => m_protocol = value;
      }
    }

    #endregion ImportBLSInfo

    #region private

    private ComunicationNet m_database;
    private ImportBLSInfo m_ImportBLSInfo;
    private int TagsAdded_number = 0;

    #endregion private

    #region ImportFunctionRootClass

    protected override void DoTheImport()
    {
      TagsAdded_number = 0;

      #region FileRead

      PROTOCOL p = m_ImportBLSInfo.Protocol;
      //stream creation
      StreamReader srpom = new StreamReader(m_ImportBLSInfo.Filename);
      // in this variable - read data will be stored
      string line;
      //first two lines are comment
      line = srpom.ReadLine();
      line = srpom.ReadLine();
      //variable used to store part of the string
      string elements;
      while ((line = srpom.ReadLine()) != null)
      {
        try
        {
          //record creation
          RecordBLS curr = new RecordBLS();
          if (line[2] == 'F' || line[2] == 'R' || line[2] == 'E')
            p = PROTOCOL.SBUS;
          else
            p = PROTOCOL.MODBUS;
          curr.proto = p;
          //station id:
          elements = line.Substring(0, 2);
          if (p == PROTOCOL.MODBUS)
            curr.stationId = Convert.ToUInt16(elements);
          if (p == PROTOCOL.SBUS)
            curr.stationId = Convert.ToUInt16(elements, 16);
          //datatype
          curr.DataType = null;//null DataType means that the data type is not set
          if (p == PROTOCOL.MODBUS)
          {
            if (line[2] == '0')
              curr.ProtocolDataType = 0;//DataType.Flag;
            else
            {
              curr.ProtocolDataType = 10;// DataType.Register;
            }
          }
          else if (p == PROTOCOL.SBUS)
          {
            switch (line[2])
            {
              case 'F':
                curr.ProtocolDataType = 0;//DataType.Flag;
                break;

              case 'R':
                curr.ProtocolDataType = 10;// DataType.Register;
                break;

              case 'E':
                curr.ProtocolDataType = 10; //DataType.Register;
                curr.DataType = typeof(float).ToString();
                break;

              default:
                throw new Exception(line[2] + " DataType not Supported in line" + line);
            }
          }
          else
            throw new Exception("Protocol not supported: " + line);
          //address
          elements = line.Substring(3, 4);
          curr.address = (ushort)(Convert.ToUInt16(elements));
          if (p == PROTOCOL.MODBUS)
            curr.address = (ushort)(curr.address - 1);
          //length
          elements = line.Substring(7, 5);
          curr.length = Convert.ToUInt16(elements);
          //timeSCan
          Match m = Regex.Match(line, @"([0-9RFEDCBA]*)\s*([0-9]*)\s*([0-9]*)\s*([0-9]*)");
          elements = m.Groups[3].ToString();
          curr.timeScan = Convert.ToUInt32(elements);
          if (m.Groups[4].ToString() != "")
          {
            curr.timeFast = Convert.ToUInt32(m.Groups[4].ToString());
          }
          else
          {
            curr.timeFast = 0;
          }
          //System.Windows.Forms.MessageBox.Show("address: "+m.Groups[1].ToString()+"\r\nlength: "+m.Groups[2].ToString()+"\r\nscan: "+m.Groups[3].ToString()+"\r\nfast: "+m.Groups[4].ToString());
          DataBloksColl.Add(curr);
        }
        catch (Exception ex)
        {
          AppendToLog("error: " + ex.Message + "at:" + line);
        }
      }
      srpom.Close();

      #endregion FileRead

      #region adding groups

      uint tout2tscan = m_ImportBLSInfo.Timeout2ScanTimeCoefiecent;
      uint tf2tn = m_ImportBLSInfo.ScanTimeNormal2ScanTimeFaseCoef;
      long grIdx = m_database.Groups.GetNextGroupID();
      foreach (ImportBLS.RecordBLS curr in this.DataBloksColl)
      {
        if (curr.timeFast == 0)
        {
          NewGroup(this.m_database.Groups,
            "GR." + curr.ProtocolDataType.ToString() + "." + curr.address.ToString(), grIdx, curr.stationId,
            curr.timeScan, curr.timeScan * tout2tscan,
            curr.timeScan / tf2tn, curr.timeScan * tout2tscan / tf2tn);
        }
        else
        {
          NewGroup(this.m_database.Groups,
            "GR." + curr.ProtocolDataType.ToString() + "." + curr.address.ToString(), grIdx, curr.stationId,
            curr.timeScan, curr.timeScan * tout2tscan,
            curr.timeFast, curr.timeFast * tout2tscan);
        }
        NewDataBlock
          (this.m_database, "DB."
          + curr.stationId.ToString()
          + "." + curr.ProtocolDataType.ToString()
          + "." + curr.address.ToString(), curr.stationId,
          curr.address, grIdx, curr.ProtocolDataType, curr.length, curr.proto, curr.DataType);
        grIdx++;
      }

      #endregion adding groups

      AppendToLog("Number of Tags added: " + TagsAdded_number.ToString());
    }

    #endregion ImportFunctionRootClass

    #region constructor

    public ImportBLS(ComunicationNet database, System.Windows.Forms.Form parrent_form)
      :
      base(parrent_form)
    {
      m_database = database;
      m_ImportBLSInfo = new ImportBLSInfo();
      SetImportInfo(m_ImportBLSInfo);
    }

    #endregion constructor

    //enum that indicate the protocol type for conversion
    internal ArrayList DataBloksColl = new ArrayList(); //array to store read data

    /// <summary>
    /// Class that contains information about data blocks
    /// </summary>
    private class RecordBLS
    {
      internal ushort stationId;
      internal short ProtocolDataType;
      internal ushort address;
      internal ushort length;
      internal uint timeScan;
      internal PROTOCOL proto;
      internal uint timeFast = 0;
      internal string DataType;
    }

    private static string WizOPCName(ushort stationId, short ProtocolDataType, ushort address, PROTOCOL prot, string DataType)
    {
      char dtChar;
      string name = "";
      switch (prot)
      {
        case PROTOCOL.MODBUS:
          dtChar = '0';
          if (ProtocolDataType == 10/*DataType.Register*/)
            dtChar = '4';
          name = stationId.ToString("00") + dtChar + address.ToString("0000");
          break;

        case PROTOCOL.SBUS:
          dtChar = 'F';
          if (ProtocolDataType == 10/*DataType.Register*/)
            if (DataType == typeof(float).ToString())
              dtChar = 'E';
            else
              dtChar = 'R';
          string station_id_hex = "";
          station_id_hex = string.Format("{0:X}", stationId);
          if (stationId < 16)
            station_id_hex = "0" + station_id_hex;
          name = station_id_hex + dtChar + address.ToString("0000");
          break;
      }
      return name;
    }

    private void removeGroups(ComunicationNet configDataBase)
    {
      foreach (ComunicationNet.StationRow st in configDataBase.Station)
      {
        foreach (ComunicationNet.GroupsRow gr in st.GetGroupsRows())
          configDataBase.Groups.RemoveGroupsRow(gr);
      }
    }

    #region new elements

    private void NewTag(ComunicationNet.TagsDataTable currTable, string nm, ushort tid, ushort sid,
      ushort add, short ProtocolDataType, int datablockid, string DataType)
    {
      ComunicationNet.TagsRow curr = currTable.NewTagsRow();
      curr.Name = nm;
      curr.TagID = tid;
      curr.AccessRights = (sbyte)ItemAccessRights.ReadWrite;
      curr.StateTrigger = (sbyte)StateTrigger.None;
      curr.Alarm = false;
      curr.AlarmMask = 0;
      curr.StateMask = 0;
      curr.DatBlockID = datablockid;
      if (DataType != null && DataType != "")
        curr.DataTypeConversion = DataType;
      currTable.AddTagsRow(curr);
      TagsAdded_number++;
    }

    private void NewDataBlock
      (ComunicationNet configTabele, string nm, ushort sid, ushort add, long gid, short ProtocolDataType, ushort length, PROTOCOL p, string DataType)
    {
      ComunicationNet.DataBlocksRow curr = configTabele.DataBlocks.NewDataBlocksRow();
      curr.Name = nm;
      //curr.SatationID = sid;
      curr.Address = add;
      //po znalezieniu grupy zastapic jej identyfikatorem
      curr.GroupID = gid;
      curr.DataType = (ulong)ProtocolDataType;
      configTabele.DataBlocks.AddDataBlocksRow(curr);
      int proto_offset = 0;
      if (p == PROTOCOL.MODBUS)
        proto_offset = 1;
      for (ushort idx = 0; idx < length; idx++)
      {
        string name = ImportBLS.WizOPCName(sid, ProtocolDataType, (ushort)(add + idx + proto_offset), p, DataType);
        try
        {
          NewTag
            (
            configTabele.Tags, name, (ushort)(gid * 100 + idx),
            sid, add, ProtocolDataType, curr.DatBlockID, DataType
            );
        }
        catch (System.Data.ConstraintException)
        {
          this.AppendToLog("tag :" + name + " cannot be added");
        }
      }
    }

    private void NewGroup
      (ComunicationNet.GroupsDataTable currTable, string name, long gid, long stid, ulong tscan, ulong tout,
      ulong ftscan, ulong ftout)
    {
      ComunicationNet.GroupsRow curr = currTable.NewGroupsRow();
      curr.Name = name;
      curr.GroupID = gid;
      curr.StationID = stid;
      curr.TimeScan = tscan;
      curr.TimeOut = tout;
      curr.TimeScanFast = ftscan;
      curr.TimeOutFast = ftout;
      currTable.AddGroupsRow(curr);
    }

    #endregion new elements
  }
}