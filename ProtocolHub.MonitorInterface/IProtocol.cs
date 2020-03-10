//<summary>
//  Title   : IProtocols
//  System  : Microsoft Visual C# .NET 2005
//  $LastChangedDate$
//  $Rev$
//  $LastChangedBy$
//  $URL$
//  $Id$
//  History :
//    <Author> - <date>:
//    <description>
//
//  Copyright (C)2006, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto:techsupp@cas.com.pl
//  http:\\www.cas.eu
//</summary>

using System;

namespace CAS.Lib.RTLib.Management
{
  /// <summary>
  /// Interface is to be used by the CommServerConsole application to get statistic data from a protocol. 
  /// It is implemented by internal class: BaseStation.Management.Protocol.ProtocolInternal in 
  /// the BaseStation.Management.Protocol. Implementation must be Serializable to be used by remoting.
  /// </summary>
  public interface IProtocol
  {
      /// <summary>
      /// Getting number of successful read operation.
      /// </summary>
    ulong GetRXDBSucc { get; }
    /// <summary>
    /// Getting number of unsuccessful read operation.
    /// </summary>
    ulong GetRXDBFail { get; }
    /// <summary>
    /// Getting number of successful write operation.
    /// </summary>
    ulong GetTXDBSucc { get; }
    /// <summary>
    /// Getting number of unsuccessful write operation.
    /// </summary>
    ulong GetTXDBFail { get; }
    /// <summary>
    /// Getting number of successfully sent frames.
    /// </summary>
    uint GetStTxFrameCounter { get; }
    /// <summary>
    /// Getting number of complete received frames.
    /// </summary>
    uint GetStRxFrameCounter { get; }
    /// <summary>
    /// Getting number of CRC errors.
    /// </summary>
    uint GetStRxCRCErrorCounter { get; }
    /// <summary>
    /// Getting number of incomplete frames.
    /// </summary>
    uint GetStRxFragmentedCounter { get; }
    /// <summary>
    /// Getting number of timeouts.
    /// </summary>
    uint GetStRxNoResponseCounter { get; }
    /// <summary>
    /// Getting number of invalid frames.
    /// </summary>
    uint GetStRxInvalid { get; }
    /// <summary>
    /// Getting number of synchronization errors.
    /// </summary>
    uint GetStRxSynchError { get; }
    /// <summary>
    /// Getting number of received NAK (negative acknowledge).
    /// </summary>
    uint GetStRxNAKCounter { get; }
    /// <summary>
    /// Getting waiting time (in milliseconds) for the first character in response (minimum/average/maximum).
    /// </summary>
    string GetTimeMaxResponseDelay { get; }
    /// <summary>
    /// Getting waiting interval (in microseconds) between characters in response (minimum/average/maximum).
    /// </summary>
    string GetTimeCharGap { get; }
    /// <summary>
    /// Getting number of received bytes.
    /// </summary>
    ulong GetRxBytesTransferred { get;}
    /// <summary>
    /// Getting number of sent bytes.
    /// </summary>
    ulong GetTxBytesTransferred { get;}
    /// <summary>
    /// Reseting all the data.
    /// </summary>
    void ResetStatistics();
  }
  /// <summary>
  /// Structure describing a protocol. It provides basic information for CommServerConsole application. 
  /// The information is constant along the lifecycle of the Protocol.
  /// </summary>
  [Serializable]
  public struct ProtocolDsc
  {
    /// <summary>
    /// Field containing parameter of the protocol in XML Format.
    /// </summary>
    public readonly string m_protocolPar;
    /// <summary>
    /// Field containing parameter of the protocol in human readable format.
    /// </summary>
    public readonly string m_protocolPar_humanreadable;
    /// <summary>
    /// Field containing name.
    /// </summary>
    public readonly string m_Name;
    /// <summary>
    /// Field containing ID number.
    /// </summary>
    public readonly long m_ID;
    /// <summary>
    /// Gets the ProtocolParameters description as a string.
    /// </summary>
    /// <returns>Description in form: [ResponseTimeOut][InterframeGap][Timeout35][Timeout15]</returns>
    public override string ToString(){return m_Name;}
    /// <summary>
    /// Initializes a new instance of the <see cref="ProtocolDsc"/> struct.
    /// </summary>
    /// <param name="protocolPar">The protocol description containing parameter of the protocol in XML Format.</param>
    /// <param name="Name">The name of the protocol.</param>
    /// <param name="ID">The identifier of the protocol.</param>
    /// <param name="protocolPar_humanreadable">The protocol parameters as human readable string.</param>
    internal ProtocolDsc( string protocolPar, string Name, long ID, string protocolPar_humanreadable )
    {
      m_protocolPar = protocolPar;
      m_Name = Name;
      m_ID = ID;
      m_protocolPar_humanreadable = protocolPar_humanreadable;
    }
  }
}
