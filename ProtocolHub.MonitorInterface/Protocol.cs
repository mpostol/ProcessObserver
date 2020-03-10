//___________________________________________________________________________________
//
//  Copyright (C) 2020, Mariusz Postol LODZ POLAND.
//
//  To be in touch join the community at GITTER: https://gitter.im/mpostol/OPC-UA-OOI
//___________________________________________________________________________________

using CAS.CommServer.ProtocolHub.MonitorInterface;
using CAS.Lib.RTLib.Utils;
using System;
using System.Collections.Generic;

namespace CAS.Lib.RTLib.Management
{
  /// <summary>
  /// Communication statistics management class for protocol
  /// Provides statistic information to management level - human machine interface (HMI)
  /// </summary>
  [Serializable]
  public class Protocol: IProtocolParent
  {

    #region PRIVATE
    private Processes.Stopwatch TimeSlaveResponseDelayStopwatch = new Processes.Stopwatch();
    private MinMaxAvr m_TimeSlaveResponseDelay;
    private ProtocolInternal m_StatisticData = new ProtocolInternal();
    private ProtocolDsc m_Descriptor;
    private static SortedList<long, Protocol> ProtocolList = new SortedList<long, Protocol>();
    private MinMaxAvr m_TimeMaxResponseDelay; //BaseStation.Management.AppConfigManagement.MinAvgMax_FrameResponse_management);
    private MinMaxAvr m_TimeCharGap;
    private ISettingsBase m_SettingsBase;

    /// <summary>
    /// Initializes a new instance of the <see cref="Protocol" /> class.
    /// </summary>
    /// <param name="protocolPar">The protocol parameters.</param>
    /// <param name="Name">The name.</param>
    /// <param name="ID">The ID.</param>
    /// <param name="protocolPar_humanreadable">The protocol human readable description.</param>
    /// <param name="settingsBase">The settings base.</param>
    protected Protocol( string protocolPar, string Name, long ID, string protocolPar_humanreadable, ISettingsBase settingsBase)
    {
      m_SettingsBase = settingsBase ?? throw new ArgumentNullException("Settings provider must not be empty");
      m_Descriptor = new ProtocolDsc( protocolPar, Name, ID, protocolPar_humanreadable );
      ProtocolList.Add( ID, this );
      m_TimeSlaveResponseDelay = new MinMaxAvr((ushort)m_SettingsBase["MinAvgMax_FrameResponse_management"]);
      m_TimeMaxResponseDelay = new MinMaxAvr((ushort)m_SettingsBase["MinAvgMax_FrameResponse_management"]);
      m_TimeCharGap = new MinMaxAvr((ushort)m_SettingsBase["MinAvgMax_CharacterGap_management"]);
      m_TimeMaxResponseDelay.MarkNewVal += new MinMaxAvr.newVal(TimeMaxResponseDelayEvHndlr);
      m_TimeCharGap.MarkNewVal += new MinMaxAvr.newVal(TimeCharGapEvHndlr);
    }
    /// <summary>
    /// Gets the statistic data.
    /// </summary>
    /// <value>The statistic data.</value>
    protected IProtocol StatisticData
    {
      get
      {
        return m_StatisticData;
      }
    }
    /// <summary>
    /// Gets minimal and maximal of theProtocolParameters description values as a string. 
    /// </summary>
    /// <param name="min">minimal value</param>
    /// <param name="max">maximal value</param>
    /// <param name="avr">average value</param>
    /// <returns></returns>
    private static string MinMaxToString( long min, long max, long avr )
    {
      return min.ToString() + "\\" + avr.ToString() + "\\" + max.ToString() + "(Mn\\Av\\Mx)";
    }
    /// <summary>
    /// Maximal waiting time (in milliseconds) for the first character in response.
    /// </summary>
    /// <param name="min">minimal value</param>
    /// <param name="max">maximal value</param>
    /// <param name="avr">average value</param>
    private void TimeMaxResponseDelayEvHndlr( long min, long max, long avr )
    {
      m_StatisticData.TimeMaxResponseDelayStr = MinMaxToString( min, max, avr );
    }
    /// <summary>
    /// Statistic of waiting interval (in microseconds) between characters in response.
    /// </summary>
    /// <param name="min">minimal value</param>
    /// <param name="max">maximal value</param>
    /// <param name="avr">average value</param>
    private void TimeCharGapEvHndlr( long min, long max, long avr )
    {
      m_StatisticData.TimeCharGapStr = MinMaxToString( min, max, avr );
    }

    [Serializable]
    private class ProtocolInternal: IProtocol
    {

      #region private
      internal ulong rxDBSucc = 0;
      internal ulong rxDBFail = 0;
      internal ulong txDBSucc = 0;
      internal ulong txDBFail = 0;
      internal uint StTxFrameCounter = 0, StRxFrameCounter = 0, StRxFragmentedCounter = 0,
      StRxNoResponseCounter = 0, StRxCRCErrorCounter = 0, StRxInvalid = 0, StRxSynchError = 0,
      StRxNAKCounter = 0, StTxNAKCounter = 0, StTxACKCounter = 0, StTxDATACounter = 0;
      internal string TimeMaxResponseDelayStr = String.Empty;
      internal string TimeCharGapStr = String.Empty;
      internal ulong RxBytesTransferred = 0;
      internal ulong TxBytesTransferred = 0;
      #endregion

      #region IProtocol
      ulong IProtocol.GetRXDBSucc { get { return rxDBSucc; } }
      ulong IProtocol.GetRXDBFail { get { return rxDBFail; } }
      ulong IProtocol.GetTXDBSucc { get { return txDBSucc; } }
      ulong IProtocol.GetTXDBFail { get { return txDBFail; } }
      uint IProtocol.GetStTxFrameCounter { get { return StTxFrameCounter; } }
      uint IProtocol.GetStRxFrameCounter { get { return StRxFrameCounter; } }
      uint IProtocol.GetStRxCRCErrorCounter { get { return StRxCRCErrorCounter; } }
      uint IProtocol.GetStRxFragmentedCounter { get { return StRxFragmentedCounter; } }
      uint IProtocol.GetStRxNoResponseCounter { get { return StRxNoResponseCounter; } }
      uint IProtocol.GetStRxInvalid { get { return StRxInvalid; } }
      uint IProtocol.GetStRxSynchError { get { return StRxSynchError; } }
      uint IProtocol.GetStRxNAKCounter { get { return StRxNAKCounter; } }
      string IProtocol.GetTimeMaxResponseDelay { get { return TimeMaxResponseDelayStr; } }
      string IProtocol.GetTimeCharGap { get { return TimeCharGapStr; } }
      ulong IProtocol.GetRxBytesTransferred { get { return RxBytesTransferred; } }
      ulong IProtocol.GetTxBytesTransferred { get { return TxBytesTransferred; } }
      void IProtocol.ResetStatistics()
      {
        rxDBSucc = 0;
        rxDBFail = 0;
        txDBSucc = 0;
        txDBFail = 0;
        StTxFrameCounter = 0;
        StRxFrameCounter = 0;
        StRxFragmentedCounter = 0;
        StRxNoResponseCounter = 0;
        StRxCRCErrorCounter = 0;
        StRxInvalid = 0;
        StRxSynchError = 0;
        StRxNAKCounter = 0;
        StTxNAKCounter = 0;
        StTxACKCounter = 0;
        StTxDATACounter = 0;
        TimeMaxResponseDelayStr = String.Empty;
        TimeCharGapStr = String.Empty;
        RxBytesTransferred = 0;
        TxBytesTransferred = 0;
      }
      #endregion

      internal ProtocolInternal() { }
    }
    #endregion

    #region IProtocolParent
    /// <summary>
    /// Incrementing number of complete transmited frames.
    /// </summary>
    public void IncStTxFrameCounter() { m_StatisticData.StTxFrameCounter++; }
    /// <summary>
    /// Incrementing number of complete received frames.
    /// </summary>
    public void IncStRxFrameCounter() { m_StatisticData.StRxFrameCounter++; }
    /// <summary>
    ///Incrementing number of synchronization errors.
    /// </summary>
    public void IncStRxSynchError() { m_StatisticData.StRxSynchError++; }
    /// <summary>
    /// Incrementing number of no response (there was no response for server request).
    /// </summary>
    public void IncStRxNoResponseCounter() { m_StatisticData.StRxNoResponseCounter++; }
    /// <summary>
    /// Incrementing number of incomplete frames.
    /// </summary>
    public void IncStRxFragmentedCounter() { m_StatisticData.StRxFragmentedCounter++; }
    /// <summary>
    /// Incrementing number of CRC errors.
    /// </summary>
    public void IncStRxCRCErrorCounter() { m_StatisticData.StRxCRCErrorCounter++; }
    /// <summary>
    /// Incrementing number of invalid frames.
    /// </summary>
    public void IncStRxInvalid() { m_StatisticData.StRxInvalid++; }
    /// <summary>
    /// Incrementing number of transmited NAK (negative acknowledge).
    /// </summary>
    public void IncStRxNAKCounter() { m_StatisticData.StRxNAKCounter++; }
    /// <summary>
    /// Incrementing number of received NAK (negative acknowledge).
    /// </summary>
    public void IncStTxNAKCounter() { m_StatisticData.StTxNAKCounter++; }
    /// <summary>
    /// Incrementing number of successfully wrote operations.
    /// </summary>
    public void IncStTxACKCounter() { m_StatisticData.StTxACKCounter++; }
    /// <summary>
    /// Incrementing number of sent bytes.
    /// </summary>
    public void IncStTxDATACounter() { m_StatisticData.StTxDATACounter++; }
    /// <summary>
    /// Incrementing number of received data blocks.
    /// </summary>
    /// <param name="succ">true - if received frame is good, false otherwise </param>
    public void RxDataBlock( bool succ )
    {
      if ( succ )
        m_StatisticData.rxDBSucc++;
      else
        m_StatisticData.rxDBFail++;
    }
    /// <summary>
    /// Incrementing number of transmited data blocks.
    /// </summary>
    /// <param name="succ">true - if frame is trensmitted successfully, false otherwise </param>
    public void TxDataBlock( bool succ )
    {
      if ( succ )
        m_StatisticData.txDBSucc++;
      else
        m_StatisticData.txDBFail++;
    }
    /// <summary>
    /// Waiting time (in milliseconds) for the first character in response.
    /// </summary>
    /// <param name="val">value of last measurement</param>
    public void TimeMaxResponseDelayAdd( long val ) { m_TimeMaxResponseDelay.Add = val; }
    /// <summary>
    /// Waiting interval (in microseconds) between characters in response.
    /// </summary>
    /// <param name="val">value of last measurement</param>
    public void TimeCharGapAdd( long val ) { m_TimeCharGap.Add = val; }
    #endregion

    #region PUBLIC
    /// <summary>
    /// Gets the protocol descriptions.
    /// </summary>
    /// <value>protocol description.</value>
    public static ProtocolDsc[] GetProtDescriptions
    {
      get
      {
        ProtocolDsc[] myList = new ProtocolDsc[ ProtocolList.Count ];
        ushort idx = 0;
        foreach ( Protocol prot in ProtocolList.Values )
        {
          myList[ idx++ ] = prot.m_Descriptor;
        }
        return myList;
      }
    }
    /// <summary>
    /// Getting the statistics of the protocol.
    /// </summary>
    /// <param name="ID">id of protocol that we are interesting</param>
    /// <returns>protocol statistics</returns>
    public static IProtocol GetProtocolStatistics( long ID ) { return ProtocolList[ ID ].m_StatisticData; }
    /// <summary>
    /// Creating new protocol.
    /// </summary>
    /// <param name="protocolPar">The protocol description containing parameter of the protocol in XML Format.</param>
    /// <param name="Name">The name of the protocol.</param>
    /// <param name="ID">The identifier of the protocol.</param>
    /// <param name="protocolPar_HumanReadable">The protocol parameters as human readable string.</param>
    /// <param name="settingsBase">The settings base.</param>
    /// <returns>Returns <see cref="IProtocolParent" /></returns>
    public static IProtocolParent CreateNewProtocol(string protocolPar, string Name, long ID, string protocolPar_HumanReadable, ISettingsBase settingsBase)
    {
      return new Protocol(protocolPar, Name, ID, protocolPar_HumanReadable, settingsBase);
    }
    /// <summary>
    /// This method is responsible for showing waiting interval (in microseconds) waiting time (in milliseconds) for the first character in response.
    /// </summary>
    public string TimeSlaveResponseDelayMsValue
    {
      get
      {
        return m_TimeSlaveResponseDelay.ToString();
      }
    }
    /// <summary>
    /// This method is responsible for reseting and starting waiting time (in milliseconds) for the first character in response.
    /// </summary>
    public void TimeSlaveResponseDelayResetAndStart()
    {
      ulong lastSWReading = 0;
      lastSWReading = TimeSlaveResponseDelayStopwatch.Stop;
      lastSWReading = TimeSlaveResponseDelayStopwatch.Reset;
      lastSWReading = TimeSlaveResponseDelayStopwatch.Start;
    }
    /// <summary>
    /// This method is responsible for stooping waiting time (in milliseconds) for the first character in response.
    /// </summary>
    public void TimeSlaveResponseDelayStop()
    {
      ulong lastSWReading = 0;
      lastSWReading = TimeSlaveResponseDelayStopwatch.Stop;
      m_TimeSlaveResponseDelay.Add =
        (long)( CAS.Lib.RTLib.Processes.Stopwatch.ConvertTo_ms( lastSWReading ) );
    }
    /// <summary>
    /// Gets the ProtocolParameters description as a string. 
    /// </summary>
    /// <returns>Description in form: [Name][ID]</returns>
    public override string ToString()
    {
      return m_Descriptor.m_Name + "(" + m_Descriptor.m_ID.ToString() + ")";
    }
    #endregion

  }
}
