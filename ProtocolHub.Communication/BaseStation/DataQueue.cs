//___________________________________________________________________________________
//
//  Copyright (C) 2020, Mariusz Postol LODZ POLAND.
//
//  To be in touch join the community at GITTER: https://gitter.im/mpostol/OPC-UA-OOI
//___________________________________________________________________________________

using CAS.Lib.CommonBus.ApplicationLayer;
using Opc.Da;
using System;
using System.Collections;
using UAOOI.ProcessObserver.Configuration;
using UAOOI.ProcessObserver.Configuration.ItemDescriber;
using UAOOI.ProcessObserver.RealTime;
using UAOOI.ProcessObserver.RealTime.Processes;
using NetConfig = UAOOI.ProcessObserver.Configuration.ComunicationNet;

namespace CAS.CommServer.ProtocolHub.Communication.BaseStation
{
  /// <summary>
  /// OPC tags data queue handling class.
  /// </summary>
  internal sealed class DataQueue : HandlerWaitTimeList<DataBlock>
  {
    #region private

    private static DataQueue myDataQueue;

    protected override void Handler(DataBlock currDatBlock)
    {
      ((DataDescription)currDatBlock).InvalidateTag();
    }

    #endregion private

    #region PUBLIC

    internal abstract class DataDescription : DataBlock
    {
      #region PRIVATE

      protected class TagDataDescription : DataBlock.Tag
      {
        #region private

        /// <summary>
        ///  Title   : class that describe Bit Tags
        /// </summary>
        private class TagBit : CAS.Lib.DeviceSimulator.Device.TagInDevice
        {
          #region public

          //          public override void UpdateTag(object val)
          //          {
          //            bool res = base.UpdateTag(val);
          //            return res;
          //          }
          protected override bool UpdateRemote(object data)
          {
            return false;
          }

          protected override bool ReadRemote(out object data)
          {
            data = null;
            return false;
          }

          internal int getBitNumber { get; }

          internal TagBit(string myName, int myBitNumber)
            :
            base(myName, null, qualityBits.bad, ItemAccessRights.ReadOnly, typeof(bool))
          {
            this.getBitNumber = myBitNumber;
          }

          #endregion public
        }// TagBit

        /// <summary>
        ///  Title   : Class that allow to write to station
        /// </summary>
        private class DataWriteDescription : IBlockDescription
        {
          #region private

          private readonly int myAddress;
          private readonly short myDataType;
          private IDataWrite m_WriteToStation;

          #endregion private

          #region IBlockDescription

          int IBlockDescription.length => 1;
          int IBlockDescription.startAddress => myAddress;
          short IBlockDescription.dataType => myDataType;

          #endregion IBlockDescription

          #region public

          internal bool WriteData(object data)
          {
            return m_WriteToStation.WriteData(data, this);
          }

          internal bool ReadData(out object data)
          {
            return m_WriteToStation.ReadData(out data, this);
          }

          internal DataWriteDescription
            (int myAddress, short myDataType, IDataWrite writeToStation)
          {
            this.myAddress = myAddress;
            this.myDataType = myDataType;
            this.m_WriteToStation = writeToStation;
            //this.myStation=0; //MZTD: ew. do zaimplementowania
          }

          #endregion public
        }

        private DataWriteDescription myDWD = null;
        private readonly bool writableTag = false;

        //MZ: Bit Tags:
        private readonly System.Collections.ArrayList TagBitList;

        #endregion private

        #region PUBLIC

        public override void UpdateTag(object val)
        {
          base.UpdateTag(val);
          //MZ: UpdateAllBitTags
          if (this.TagBitList != null)
          {
            int val_int = (int)val;
            foreach (TagBit t in this.TagBitList)
            {
              int mask = 1;
              mask = mask << (int)t.getBitNumber;
              //MP wywalilem if i przerywam po bledzie
              t.UpdateTag((mask & val_int) > 0);
              //              if ( ! res ) return false;
            }
          }
          //          return res;
        } //UpdateTag

        protected override bool UpdateRemote(object data)
        {
          if (myDWD.WriteData(data))
          {
            UpdateTag(data);
            return true;
          }
          else
          {
            MarkTagQuality(Opc.Da.qualityBits.bad);
            return false;
          }
        }

        protected override bool ReadRemote(out object data)
        {
          //MPTD brak konwersji na postac kanoniczna
          if (myDWD.ReadData(out data))
          {
            UpdateTag(data);
            return true;
          }
          else
          {
            MarkTagQuality(Opc.Da.qualityBits.bad);
            return false;
          }
        }

        /// <summary>
        /// Tag constructor
        /// </summary>
        /// <param name="myDSC">params from Tags table</param>
        /// <param name="myStation">pointer to interface that allow to change priority of the station</param>
        /// <param name="myAddress">address</param>
        /// <param name="myDataType">Data type</param>
        /// <param name="writeToStation">pointer to interface that allow to write to station</param>
        /// <param name="property_colllection">collection of properties for this tag see: <see cref="Opc.Da.ItemPropertyCollection"/></param>
        internal TagDataDescription
          (
          NetConfig.TagsRow myDSC, IStationState myStation, int myAddress,
          short myDataType, IDataWrite writeToStation, Opc.Da.ItemPropertyCollection property_colllection
          )
          : base(myDSC, myStation)
        {
          if (property_colllection != null)
            this.AddProperties(property_colllection);
          writableTag = (myDSC.AccessRights == (sbyte)ItemAccessRights.ReadWrite || myDSC.AccessRights == (sbyte)ItemAccessRights.WriteOnly);
          myDWD = new DataWriteDescription(myAddress, myDataType, writeToStation);
          ComunicationNet.TagBitRow[] tagbitsDsc = myDSC.GetTagBitRows();
          if (tagbitsDsc.Length != 0)
          {
            this.TagBitList = new ArrayList();
            foreach (ComunicationNet.TagBitRow curr in tagbitsDsc)
            {
              TagBit newTagBit = new TagBit(myDSC.Name + "_" + curr.Name, (int)curr.BitNumber);
              this.TagBitList.Add(newTagBit);
            }
          }
          else
            this.TagBitList = null;
        }

        #endregion PUBLIC
      }// Tag

      internal override void UpdateAllTags(IReadValue val)
      {
        base.UpdateAllTags(val);
        ResetCounter();
      }

      protected abstract TimeSpan TimeOut { get; }

      internal void ChangeTimeout()
      {
        Cycle = TimeOut;
        ResetCounter();
        NotifyNewTimeScan?.Invoke(this, EventArgs.Empty);
      }

      protected override DataBlock.Tag newTag(ComunicationNet.TagsRow currRow, IStationState myStationState, int myAddress, short myDataType, IDataWrite myWriteInt)
      {
#if COMMSERVER
        Opc.Da.ItemPropertyCollection prop_coll = ItemDescriber2OpcDA.GetItemPropertiesCollection(currRow.Name, Initialization.m_ds_dsc);
#else
        Opc.Da.ItemPropertyCollection prop_coll=null;
#endif
        return new TagDataDescription(currRow, myStationState, myAddress, myDataType, myWriteInt, prop_coll);
      }

      #endregion PRIVATE

      #region public

      internal event EventHandler NotifyNewTimeScan;

      internal abstract TimeSpan TimeScann { get; }

      internal void InvalidateTag()
      {
        //MPTD brak invalidacji tagow bitowych - trzeba to zrobiæ
        DataBlock.Tag[] tagHendlers = createdTags;
        System.Diagnostics.Debug.Assert(createdTags != null, "DataQueue.InvalidateTag: createdTags!= null");
        System.Diagnostics.Debug.Assert(tagHendlers != null, "DataQueue.InvalidateTag: tagHendlers != null");
        for (ushort idx = 0; (idx < tagHendlers.Length); idx++)
          tagHendlers[idx].MarkTagQuality(Opc.Da.qualityBits.bad);
      }

      internal DataDescription
        (
        ComunicationNet.DataBlocksRow myRow, TimeSpan timeOut, IDataWrite myWriteInt,
        IStationState myStationState, ref int cVConstrain
        )
        : base(myRow, myWriteInt, myStationState, myDataQueue, timeOut, ref cVConstrain)
      { }

      #endregion public
    }// class DataDescription

    #endregion PUBLIC

    #region INIT

    private DataQueue()
      : base(false, "DataQueueHandler")
    { }//DataQueue

    static DataQueue()
    {
      myDataQueue = new DataQueue();
    }

    #endregion INIT
  }//class DataQueue
}//namespace BaseStation