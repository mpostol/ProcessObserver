//_______________________________________________________________
//  Title   : ISegmentStatistics
//  System  : Microsoft VisualStudio 2015 / C#
//  $LastChangedDate$
//  $Rev$
//  $LastChangedBy$
//  $URL$
//  $Id$
//
//  Copyright (C) 2017, CAS LODZ POLAND.
//  TEL: +48 608 61 98 99 
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//_______________________________________________________________

namespace CAS.CommServer.ProtocolHub.MonitorInterface
{

  /// <summary>
  /// Interface of the Segment Statistics
  /// </summary>
  public interface ISegmentStatistics: IInterface2SegmentLink
  {
    
    /// <summary>
    /// Marks the Segment connection failure.
    /// </summary>
    void MarkConnFail();
    /// <summary>
    /// Sets the new state.
    /// </summary>
    /// <value>The new state.</value>
    Statistics.SegmentStatistics.States NewState { set; }
    /// <summary>
    /// Sets the overtime coefficients.
    /// </summary>
    /// <param name="min">The min. value</param>
    /// <param name="max">The max. value</param>
    /// <param name="avr">The average value</param>
    void SetOvertimeCoefficient( long min, long max, long avr );

  }
}
