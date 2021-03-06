//___________________________________________________________________________________
//
//  Copyright (C) 2020, Mariusz Postol LODZ POLAND.
//
//  To be in touch join the community at GITTER: https://gitter.im/mpostol/OPC-UA-OOI
//___________________________________________________________________________________

using CAS.Lib.CommonBus.ApplicationLayer;
using CAS.Lib.DeviceSimulator;
using Opc.Da;
using System;
using System.Collections;
using System.Xml;
using UAOOI.ProcessObserver.Configuration;
using UAOOI.ProcessObserver.RealTime;
using UAOOI.ProcessObserver.RealTime.Processes;

namespace CAS.CommServer.ProtocolHub.Communication.BaseStation
{
  internal interface IDataWrite
  {
    bool WriteData(object data, IBlockDescription address);

    bool ReadData(out object data, IBlockDescription address);
  }

  internal interface IStationState
  {
    void ChangeToHighPriority();

    void ChangeToLowPriority();
  }

  internal abstract class DataBlock : WaitTimeList<DataBlock>.TODescriptor, IBlockDescription
  {
    #region private

    private readonly int myStartAddress;
    private readonly int mylength;
    private readonly short myDataType;
    private Tag[] tagHendlers;

    protected abstract Tag newTag(ComunicationNet.TagsRow currRow, IStationState myStationState, int myAddress, short myDataType, IDataWrite myWriteInt);

    private void CreateAllTags(ComunicationNet.TagsRow[] tagsDsc, out int length, IDataWrite myWriteInt, int myAddress, short myDataType, IStationState myStationState, ref int cVConstrain)
    {
      SortedList sortedTgs = new SortedList();
      foreach (ComunicationNet.TagsRow curr in tagsDsc)
      {
        if (cVConstrain > 0)
        {
          sortedTgs.Add(curr.TagID, curr);
          cVConstrain--;
        }
      }
      length = sortedTgs.Count;
      tagHendlers = new Tag[length];
      for (ushort idx = 0; (idx < length); idx++)
      {
        ComunicationNet.TagsRow currRow =
          (ComunicationNet.TagsRow)sortedTgs.GetByIndex(idx);
        tagHendlers[idx] = newTag(currRow, myStationState, myAddress + idx, myDataType, myWriteInt);
      }
    }//CreateAllTags

    protected Tag[] createdTags => tagHendlers;

    #endregion private

    #region IBlockDescription

    int IBlockDescription.startAddress => myStartAddress;
    int IBlockDescription.length => mylength;
    short IBlockDescription.dataType => myDataType;

    #endregion IBlockDescription

    #region public

    internal abstract class Tag : Device.TagInDevice
    {
      #region PRIVATE

      private IStationState myStation;
      private readonly bool stateHihgTriger = false;
      private readonly bool stateLowTriger = false;
      private readonly int stateMask;

      //      private System.Type destination type;
      private bool initState = true; // prevents the situation that prevVal is not properly initialized in the ChechTagAlarm function

      private object prevVal;

      private void CheckTagAlarm(object val)
      {
        if (stateHihgTriger | stateLowTriger)
        {
          if (!initState)
          {
            if (val is int)
            {
              //            if      (stateHihgTriger && (( ((int)val ^ (int)prevVal ^ ~(int)prevVal) & stateMask ) != 0 ))
              //              myStation.ChangeToHihgPriority();
              //            else if (stateLowTriger  && (( ((int)val ^ (int)prevVal ^  (int)prevVal) & stateMask ) != 0 ))
              //              myStation.ChangeToLowPriority();
              //           Condition was modified to change the prioryty only when 0 change to 1
              if (stateHihgTriger && (((int)val ^ (int)prevVal) & ((int)val & stateMask)) != 0)
                myStation.ChangeToHighPriority();
              if (stateLowTriger && (((int)val ^ (int)prevVal) & ((int)val & stateMask)) != 0)
                myStation.ChangeToLowPriority();
            }
            else if (val is bool)
            {
              if (stateHihgTriger && ((bool)val ^ (bool)prevVal & (bool)val))
                myStation.ChangeToHighPriority();
              else if (stateLowTriger && ((bool)val ^ (bool)prevVal & (bool)val))
                myStation.ChangeToLowPriority();
            }
          }
          else
          {
            initState = false;
            if (val is int)
            {
              if (stateHihgTriger && (((int)val & stateMask)) != 0)
                myStation.ChangeToHighPriority();
              if (stateLowTriger && ((int)val & stateMask) != 0)
                myStation.ChangeToLowPriority();
            }
            else if (val is bool)
            {
              if (stateHihgTriger && (bool)val)
                myStation.ChangeToHighPriority();
              else if (stateLowTriger && (bool)val)
                myStation.ChangeToLowPriority();
            }
          }
          prevVal = val;
        }
      }

      #endregion PRIVATE

      #region public

      /// <summary>
      /// Gets the data type from configuration row.
      /// </summary>
      /// <param name="myDSC">Tag description as row from configuration.</param>
      /// <returns></returns>
      internal static System.Type GetDataTypeFromConfig(ComunicationNet.TagsRow myDSC)
      {
        System.Type typetobereturned = null;
        if (!myDSC.IsDataTypeConversionNull())
        {
          try
          {
            typetobereturned = System.Type.GetType(myDSC.DataTypeConversion);
          }
          catch (Exception ex)
          {
            EventLogMonitor.WriteToEventLogInfo("Unknown type:" + myDSC.DataTypeConversion + "; conversion exception info:" + ex.Message, 128);
          }
          if (typetobereturned == null)
            EventLogMonitor.WriteToEventLogInfo("Unknown type:" + myDSC.DataTypeConversion, 166);
        }
        if (typetobereturned != null)
          return typetobereturned;
        else
          return typeof(object);
      }

      public override void UpdateTag(object Val)
      {
        base.UpdateTag(Val);
        CheckTagAlarm(Val);
      }

      public override bool GetVal(ref object Val)
      {
        if (!base.GetVal(ref Val))
          return false;
        CheckTagAlarm(Val);
        return true;
      }

      /// <summary>
      /// Tag constructor
      /// </summary>
      /// <param name="myDSC">parameters from Tags table</param>
      /// <param name="myStation">pointer to interface that allow to change priority of the station</param>
      internal Tag(ComunicationNet.TagsRow myDSC, IStationState myStation)
          : base(myDSC.Name, null, qualityBits.badNotConnected, (ItemAccessRights)myDSC.AccessRights, GetDataTypeFromConfig(myDSC))
      {
        switch ((StateTrigger)myDSC.StateTrigger)
        {
          case StateTrigger.StateHigh:
            stateHihgTriger = true;
            break;

          case StateTrigger.StateLow:
            stateLowTriger = true;
            break;

          default:
            break;
        }
        stateMask = (int)myDSC.StateMask;
        this.myStation = myStation;
        this.EuType = Opc.Da.euType.noEnum;
        ItemPropertyCollection itemPropertyCollection = new ItemPropertyCollection();
        foreach (ComunicationNet.ItemPropertiesTableRow row_property in myDSC.GetItemPropertiesTableRows())
        {
          try
          {
            PropertyDescription prop_dsc =
                PropertyDescription.Find(
                new PropertyID(
                    new XmlQualifiedName(row_property.ID_Name_Name, row_property.ID_Name_Namespace)
                    ));
            ItemProperty _itemProperty = new ItemProperty
            {
              ID = prop_dsc.ID,
              Value = row_property.Value
            };
            if (prop_dsc.ID != Opc.Da.Property.DATATYPE) //this property is managed differently
                                                         // as GetDataTypeFromConfig( myDSC )
                                                         // at the constructor
            {
              if (prop_dsc.ID == Opc.Da.Property.HI_LIMIT ||
                 prop_dsc.ID == Opc.Da.Property.LO_LIMIT ||
                  prop_dsc.ID == Opc.Da.Property.HIHI_LIMIT ||
                 prop_dsc.ID == Opc.Da.Property.LOLO_LIMIT ||
                 prop_dsc.ID == Opc.Da.Property.LOWEU ||
                  prop_dsc.ID == Opc.Da.Property.HIGHEU ||
                 prop_dsc.ID == Opc.Da.Property.SCANRATE
                )
              {
                // this property contains double value
                if (double.TryParse(row_property.Value, out double prop_value))
                  _itemProperty.Value = prop_value;
              }
              if (prop_dsc.ID == Opc.Da.Property.EUTYPE)
              {
                _itemProperty.Value = Opc.Da.euType.noEnum;
                // this property contains vale from enum: Opc.Da.euType
                foreach (Opc.Da.euType NEWeuType in Enum.GetValues(typeof(Opc.Da.euType)))
                {
                  if (NEWeuType.ToString() == row_property.Value)
                    _itemProperty.Value = NEWeuType;
                }
              }
              if (prop_dsc.ID == Opc.Da.Property.EUINFO)
              {
                //I assume that this is table of strings spited by ;
                _itemProperty.Value = row_property.Value.Split(';');
              }
              itemPropertyCollection.Add(_itemProperty);
            }
          }
          catch (Exception ex)
          {
            CommServerComponent.Tracer.TraceInformation(290, "DataBlock.Tag",
            "Problem with property for item : " + myDSC.Name + ": " +
              TraceEvent.GetMessageWithExceptionNameFromExceptionIncludingInnerException(ex));
          }
        }
        try
        {
          this.AddProperties(itemPropertyCollection);
        }
        catch (Exception ex)
        {
          CommServerComponent.Tracer.TraceInformation(290, "DataBlock.Tag",
          "Problem with many properties for item : " + myDSC.Name + ": " +
            TraceEvent.GetMessageWithExceptionNameFromExceptionIncludingInnerException(ex));
        }
      }

      #endregion public
    }//Tag

    internal virtual void UpdateAllTags(IReadValue val)
    {
      for (ushort idx = 0; (idx < mylength); idx++)
      {
        try
        {
          tagHendlers[idx].UpdateTag(val.ReadValue(idx, tagHendlers[idx].TagCanonicalType));
        }
        catch (Exception ex)
        {
          tagHendlers[idx].MarkTagQuality(Opc.Da.qualityBits.badCommFailure);
          CommServerComponent.Tracer.TraceWarning(187, "DataBlock.UpdateAllTags",
            "An exception was thrown during data read:  " + ex.Message);
        }
      }
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DataBlock"/> class.
    /// </summary>
    /// <param name="myRow">My row.</param>
    /// <param name="myWriteInt">My write interface.</param>
    /// <param name="myStationState">State of my station.</param>
    /// <param name="myQueue">My queue.</param>
    /// <param name="cycle">The cycle.</param>
    /// <param name="cVConstrain">The down counter of max number of tags allowed according to license.</param>
    internal DataBlock(ComunicationNet.DataBlocksRow myRow, IDataWrite myWriteInt, IStationState myStationState, WaitTimeList<DataBlock> myQueue, TimeSpan cycle, ref int cVConstrain)
        : base(myQueue, cycle)
    {
      myStartAddress = (int)myRow.Address;
      myDataType = (short)myRow.DataType;
      CreateAllTags
        (myRow.GetTagsRows(), out mylength, myWriteInt, myStartAddress, myDataType, myStationState, ref cVConstrain);
    }

    /// <summary>
    /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
    /// </summary>
    /// <returns>
    /// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
    /// </returns>
    public override string ToString()
    {
      return string.Format("{0}: start address={1}; data type={2}; length={3};", base.ToString(),
        myStartAddress, myDataType, mylength);
    }

    #endregion public
  }// class DataBlock
}//namespace BaseStation