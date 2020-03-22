//___________________________________________________________________________________
//
//  Copyright (C) 2020, Mariusz Postol LODZ POLAND.
//
//  To be in touch join the community at GITTER: https://gitter.im/mpostol/OPC-UA-OOI
//___________________________________________________________________________________

using CAS.Lib.CommonBus;
using System;
using UAOOI.ProcessObserver.Configuration;

namespace CAS.CommServer.ProtocolHub.Communication
{
  /// <summary>
  /// Structure converting configuration descriptor to a structure of parameters.
  /// </summary>
  public class SegmentParameters
  {
    #region private

    private TimeSpan CheckedValue(long value)
    {
      return value < 0 ? TimeSpan.MaxValue : TimeSpan.FromMilliseconds(value);
    }

    #endregion private

    #region public

    /// <summary>
    /// Time keep connection
    /// </summary>
    public readonly TimeSpan TimeKeepConnrction;

    /// <summary>
    /// Time idle keep connection.
    /// </summary>
    public readonly TimeSpan TimeIdleKeepConnection;

    /// <summary>
    /// Time reconnect segment
    /// </summary>
    public readonly TimeSpan TimeReconnect;

    /// <summary>
    /// Time Reconnect after failure
    /// </summary>
    public readonly TimeSpan TimeReconnectAfterFailure;

    /// <summary>
    /// True if to pickup the incoming connection
    /// </summary>
    public readonly bool AllowPickupConnection;

    /// <summary>
    /// Segment address
    /// </summary>
    public IAddress SegmentAddress;

    /// <summary>
    /// Segment Name
    /// </summary>
    public string Name;

    #endregion public

    #region creator

    /// <summary>
    /// Initializes a new instance of the <see cref="SegmentParameters"/> structure.
    /// </summary>
    /// <param name="segmentRow">The segment row.</param>
    public SegmentParameters(ComunicationNet.SegmentsRow segmentRow)
    {
      Name = segmentRow.Name;
      SegmentAddress = new StringAddress(segmentRow.Address);
      AllowPickupConnection = segmentRow.PickupConn;
      TimeKeepConnrction = CheckedValue(segmentRow.timeKeepConn);
      if (segmentRow.KeepConnect)
        TimeIdleKeepConnection = TimeSpan.FromMilliseconds(uint.MaxValue - 1);
      else
        TimeIdleKeepConnection = CheckedValue(segmentRow.TimeIdleKeepConn);
      TimeReconnect = CheckedValue(segmentRow.TimeScan);
      TimeReconnectAfterFailure = CheckedValue(segmentRow.TimeReconnect);
    }

    #endregion creator
  }
}