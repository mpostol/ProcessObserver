
//<summary>
//  Title   : Interlink between the Interface and Segment statistic object
//  System  : Microsoft Visual C# .NET 2005
//  $LastChangedDate$
//  $Rev$
//  $LastChangedBy$
//  $URL$
//  $Id$

//
//  Copyright (C)2006, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto:techsupp@cas.com.pl
//  http:\\www.cas.eu
//</summary>

namespace CAS.CommServer.ProtocolHub.MonitorInterface
{

  /// <summary>
  /// Interlink between the Interface and Segment statistic object
  /// </summary>
  public interface IInterface2SegmentLink
  {

    /// <summary>
    /// Adds the interface.
    /// </summary>
    /// <param name="interfaceStatistics">The interface statistics.</param>
    void AddInterface(Statistics.InterfaceStatistics interfaceStatistics);
    /// <summary>
    /// Gets the protocol statistics.
    /// </summary>
    /// <param name="counters">The counters.</param>
    /// <param name="isAnySuccess">if set to <c>true</c> means any success.</param>
    void GetProtocolStatistics(ref uint[] counters, out bool isAnySuccess);
    /// <summary>
    /// Gets the get OPC prefix.
    /// </summary>
    /// <value>The get OPC prefix.</value>
    string GetOPCPrefix { get; }

  }
}
