//___________________________________________________________________________________
//
//  Copyright (C) 2020, Mariusz Postol LODZ POLAND.
//
//  To be in touch join the community at GITTER: https://gitter.im/mpostol/OPC-UA-OOI
//___________________________________________________________________________________

using CAS.CommServer.ProtocolHub.MonitorInterface;
using CAS.Lib.RTLib;
using CAS.Lib.RTLib.Management;
using CAS.NetworkConfigLib;
using System;

namespace CAS.CommServer.ProtocolHub.Communication.Diagnostic
{
  /// <summary>
  /// Summary description for Interface.
  /// </summary>
  [Serializable]
  internal class OPCValTag : CAS.Lib.DeviceSimulator.Device.TagInDevice
  {
    protected override bool UpdateRemote(object val)
    {
      return false;
    }
    protected override bool ReadRemote(out object data)
    {
      data = null;
      return false;
    }
    public object Val
    {
      set => base.UpdateTag(value);
    }
    public OPCValTag(string Name, Type datatype)
      : base(Name, null, Opc.Da.qualityBits.bad, ItemAccessRights.ReadOnly, datatype)
    {
      this.EuType = Opc.Da.euType.noEnum;
    }
  }
  /// <summary>
  /// Interface
  /// </summary>
  [Serializable]
  public class Interface : Statistics.InterfaceStatistics
  {

    #region private
    private OPCValTag ActiveTimeTag;
    private OPCValTag FailTimeTag;
    private OPCValTag StandbyTag;
    private OPCValTag StateTag;
    #endregion private

    #region internal
    /// <summary>
    /// Refreshes this instance.
    /// </summary>
    protected override void refresh()
    {
      StateTag.Val = (ushort)myStat.MyState;
      ActiveTimeTag.Val = (int)(System.Math.Min(ActiveTime, int.MaxValue));
      FailTimeTag.Val = (int)(System.Math.Min(FailTime, int.MaxValue));
      StandbyTag.Val = (int)(System.Math.Min(StandbyTime, int.MaxValue));
    }
    /// <summary>
    /// Initializes a new instance of the <see cref="Interface"/> class.
    /// </summary>
    /// <param name="name">The name.</param>
    /// <param name="interfaceNumber">The interface number.</param>
    /// <param name="seg">The segment .</param>
    /// <param name="stt">The station.</param>
    public Interface(string name, ulong interfaceNumber, IInterface2SegmentLink seg, Station stt)
      :
      base(name, interfaceNumber, seg, stt)
    {
      string prefix = seg.GetOPCPrefix + "/" + ToString();
      ActiveTimeTag = new OPCValTag(prefix + "/ActiveTime", typeof(int));
      FailTimeTag = new OPCValTag(prefix + "/FailTime", typeof(int));
      StandbyTag = new OPCValTag(prefix + "/Standby", typeof(int));
      StateTag = new OPCValTag(prefix + "/State", typeof(bool));
    }
    #endregion internal

  }//Interface
  /// <summary>
  /// Publishes statistics as OPC tags.
  /// </summary>
  [Serializable]
  public class Segment : Statistics.SegmentStatistics
  {
    #region private
    private OPCValTag Tag_WriteDelayMin;
    private OPCValTag Tag_WriteDelayAvr;
    private OPCValTag Tag_WriteDelayMax;
    private OPCValTag Tag_ReadDelayMin;
    private OPCValTag Tag_ReadDelayAvr;
    private OPCValTag Tag_ReadDelayMax;
    private OPCValTag Tag_ConnectTimeMin;
    private OPCValTag Tag_ConnectTimeAvr;
    private OPCValTag Tag_ConnectTimeMax;
    private OPCValTag Tag_OvertimeCffMin;
    private OPCValTag Tag_OvertimeCffAvr;
    private OPCValTag Tag_OvertimeCffMax;
    private OPCValTag Tag_connFailCount;
    private OPCValTag Tag_connMadeCount;
    private OPCValTag Tag_State;
    private OPCValTag Tag_TotalConnTime;
    private readonly string m_prefix;
    #endregion
    #region public
    /// <summary>
    /// Refreshes this instance.
    /// </summary>
    protected override void refresh()
    {
      Tag_WriteDelayMin.Val = this.WriteDelayMin;
      Tag_WriteDelayAvr.Val = this.WriteDelayAvr;
      Tag_WriteDelayMax.Val = this.WriteDelayMax;
      Tag_ReadDelayMin.Val = this.ReadDelayMin;
      Tag_ReadDelayAvr.Val = this.ReadDelayAvr;
      Tag_ReadDelayMax.Val = this.ReadDelayMax;
      Tag_ConnectTimeMin.Val = this.ConnectTimeMin;
      Tag_ConnectTimeAvr.Val = this.ConnectTimeAvr;
      Tag_ConnectTimeMax.Val = this.ConnectTimeMax;
      Tag_OvertimeCffMin.Val = this.myStat.OvertimeCoefficient_Min;
      Tag_OvertimeCffAvr.Val = this.myStat.OvertimeCoefficient_Avr;
      Tag_OvertimeCffMax.Val = this.myStat.OvertimeCoefficient_Max;
      Tag_connFailCount.Val = this.myStat.ConnFailCount;
      Tag_connMadeCount.Val = this.myStat.ConnMadeCount;
      Tag_State.Val = (ushort)this.myStat.CurrentState;
      Tag_TotalConnTime.Val = ConnectTime;
    }
    /// <summary>
    /// Gets the get OPC prefix.
    /// </summary>
    /// <value>The get OPC prefix.</value>
    public override string GetOPCPrefix => m_prefix;
    /// <summary>
    /// Initializes a new instance of the <see cref="Segment" /> class.
    /// </summary>
    /// <param name="segments">The segments descriptor <see cref="ComunicationNet.SegmentsRow" /></param>
    /// <param name="channelStatistics">The channel statistics.</param>
    internal Segment(ComunicationNet.SegmentsRow segments, Statistics.ChannelStatistics channelStatistics)
      : base(segments, channelStatistics)
    {
      m_prefix = "Internal/Channels/" + channelStatistics.ToString() + "/" + ToString();
      Tag_WriteDelayMin = new OPCValTag(m_prefix + "/WriteDelayMin", typeof(long));
      Tag_WriteDelayAvr = new OPCValTag(m_prefix + "/WriteDelayAvr", typeof(long));
      Tag_WriteDelayMax = new OPCValTag(m_prefix + "/WriteDelayMax", typeof(long));
      Tag_ReadDelayMin = new OPCValTag(m_prefix + "/ReadDelayMin", typeof(long));
      Tag_ReadDelayAvr = new OPCValTag(m_prefix + "/ReadDelayAvr", typeof(long));
      Tag_ReadDelayMax = new OPCValTag(m_prefix + "/ReadDelayMax", typeof(long));
      Tag_ConnectTimeMin = new OPCValTag(m_prefix + "/ConnectTimeMin", typeof(long));
      Tag_ConnectTimeAvr = new OPCValTag(m_prefix + "/ConnectTimeAvr", typeof(long));
      Tag_ConnectTimeMax = new OPCValTag(m_prefix + "/ConnectTimeMax", typeof(long));
      Tag_OvertimeCffMin = new OPCValTag(m_prefix + "/OvertimeCffMin", typeof(uint));
      Tag_OvertimeCffAvr = new OPCValTag(m_prefix + "/OvertimeCffAvr", typeof(uint));
      Tag_OvertimeCffMax = new OPCValTag(m_prefix + "/OvertimeCffMax", typeof(uint));
      Tag_connFailCount = new OPCValTag(m_prefix + "/ConnFailCount", typeof(uint));
      Tag_connMadeCount = new OPCValTag(m_prefix + "/ConnMadeCount", typeof(uint));
      Tag_State = new OPCValTag(m_prefix + "/SegmentState", typeof(ushort));
      Tag_TotalConnTime = new OPCValTag(m_prefix + "/TotalConnectionTime", typeof(ulong));
    }
    #endregion
  }
  /// <summary>
  /// Station
  /// </summary>
  [Serializable]
  public class Station : Statistics.StationStatistics
  {
    private OPCValTag StateTag;
    /// <summary>
    /// Refreshes this instance.
    /// </summary>
    protected override void refresh() { StateTag.Val = myOn; }
    /// <summary>
    /// Initializes a new instance of the <see cref="Station"/> class.
    /// </summary>
    /// <param name="currDsc">The station row descriptor.</param>
    public Station(ComunicationNet.StationRow currDsc)
      : base(currDsc)
    {
      StateTag = new OPCValTag("STT/" + myName + "/St", typeof(bool));
    }
  }
  /// <summary>
  /// Protocol
  /// </summary>
  [Serializable]
  public class CommServerProtocol : Statistics.ProtocolStatisticsUpdater
  {
    #region private
    private readonly string prefix;
    private OPCValTag Tag_RXDBSucc;
    private OPCValTag Tag_RXDBFail;
    private OPCValTag Tag_TXDBSucc;
    private OPCValTag Tag_TXDBFail;
    private OPCValTag Tag_StTxFrameCounter;
    private OPCValTag Tag_StRxFrameCounter;
    private OPCValTag Tag_StRxCRCErrorCounter;
    private OPCValTag Tag_StRxFragmentedCounter;
    private OPCValTag Tag_StRxNoResponseCounter;
    private OPCValTag Tag_StRxInvalid;
    private OPCValTag Tag_StRxSynchError;
    private OPCValTag Tag_StRxNAKCounter;
    private OPCValTag Tag_TimeMaxResponseDelay;
    private OPCValTag Tag_TimeCharGap;
    private OPCValTag Tag_RxBytesTransferred;
    private OPCValTag Tag_TxBytesTransferred;
    private CommServerProtocol(string protocolPar, string Name, long ID, string protocolPar_humanreadable, Statistics.ChannelStatistics MyChannelStatistics)
      : base(protocolPar, Name, ID, protocolPar_humanreadable)
    {
      prefix = Cnf.OPCPathChannels + MyChannelStatistics.ToString() + "/" + Name;
      Tag_RXDBSucc = new OPCValTag(prefix + "/RX+", typeof(ulong));
      Tag_RXDBFail = new OPCValTag(prefix + "/RX-", typeof(ulong));
      Tag_TXDBSucc = new OPCValTag(prefix + "/TX+", typeof(ulong));
      Tag_TXDBFail = new OPCValTag(prefix + "/TX-", typeof(ulong));
      Tag_StTxFrameCounter = new OPCValTag(prefix + "/Counters/Tx", typeof(uint));
      Tag_StRxFrameCounter = new OPCValTag(prefix + "/Counters/Rx", typeof(uint));
      Tag_StRxCRCErrorCounter = new OPCValTag(prefix + "/Counters/CRCErrors", typeof(uint));
      Tag_StRxFragmentedCounter = new OPCValTag(prefix + "/Counters/FragmentedFrames", typeof(uint));
      Tag_StRxNoResponseCounter = new OPCValTag(prefix + "/Counters/NoResponses", typeof(uint));
      Tag_StRxInvalid = new OPCValTag(prefix + "/Counters/InvalidFrames", typeof(uint));
      Tag_StRxSynchError = new OPCValTag(prefix + "/Counters/SynchronizationErrors", typeof(uint));
      Tag_StRxNAKCounter = new OPCValTag(prefix + "/Counters/NAK", typeof(uint));
      Tag_TimeMaxResponseDelay = new OPCValTag(prefix + "/Time/MaxResponseDelay", typeof(string));
      Tag_TimeCharGap = new OPCValTag(prefix + "/Time/CharacterGap", typeof(string));
      Tag_RxBytesTransferred = new OPCValTag(prefix + "/BytesTransferred/RX", typeof(ulong));
      Tag_TxBytesTransferred = new OPCValTag(prefix + "/BytesTransferred/TX", typeof(ulong));
    }
    #endregion private
    /// <summary>
    /// Creating new protocol.
    /// </summary>
    /// <param name="protocolPar">The protocol description containing parameter of the protocol in XML Format.</param>
    /// <param name="Name">The name of the protocol.</param>
    /// <param name="ID">The identifier of the protocol.</param>
    /// <param name="protocolPar_HumanReadable">The protocol parameters as human readable string.</param>
    /// <param name="MyChannelStatistics">My channel statistics.</param>
    /// <returns>Returns <see cref="IProtocolParent"/></returns>
    public static IProtocolParent CreateNewProtocol(string protocolPar, string Name, long ID, string protocolPar_HumanReadable, Statistics.ChannelStatistics MyChannelStatistics)
    {
      return new CommServerProtocol(protocolPar, Name, ID, protocolPar_HumanReadable, MyChannelStatistics);
    }
    /// <summary>
    /// Refreshes this instance.
    /// </summary>
    protected override void refresh()
    {
      Tag_RXDBSucc.Val = this.StatisticData.GetRXDBSucc;
      Tag_RXDBFail.Val = this.StatisticData.GetRXDBFail;
      Tag_TXDBSucc.Val = this.StatisticData.GetTXDBSucc;
      Tag_TXDBFail.Val = this.StatisticData.GetTXDBFail;
      Tag_StTxFrameCounter.Val = this.StatisticData.GetStTxFrameCounter;
      Tag_StRxFrameCounter.Val = this.StatisticData.GetStRxFrameCounter;
      Tag_StRxCRCErrorCounter.Val = this.StatisticData.GetStRxCRCErrorCounter;
      Tag_StRxFragmentedCounter.Val = this.StatisticData.GetStRxFragmentedCounter;
      Tag_StRxNoResponseCounter.Val = this.StatisticData.GetStRxFragmentedCounter;
      Tag_StRxInvalid.Val = this.StatisticData.GetStRxInvalid;
      Tag_StRxSynchError.Val = this.StatisticData.GetStRxSynchError;
      Tag_StRxNAKCounter.Val = this.StatisticData.GetStRxNAKCounter;
      Tag_TimeMaxResponseDelay.Val = this.StatisticData.GetTimeMaxResponseDelay;
      Tag_TimeCharGap.Val = this.StatisticData.GetTimeCharGap;
      Tag_RxBytesTransferred.Val = this.StatisticData.GetRxBytesTransferred;
      Tag_TxBytesTransferred.Val = this.StatisticData.GetTxBytesTransferred;
    }
  }
}
