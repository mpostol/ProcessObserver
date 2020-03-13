//___________________________________________________________________________________
//
//  Copyright (C) 2020, Mariusz Postol LODZ POLAND.
//
//  To be in touch join the community at GITTER: https://gitter.im/mpostol/OPC-UA-OOI
//___________________________________________________________________________________

using CAS.CommServer.ProtocolHub.MonitorInterface;
using CAS.Lib.CommonBus.ApplicationLayer;
using CAS.NetworkConfigLib;
using UAOOI.ProcessObserver.RealTime.Processes;

namespace CAS.CommServer.ProtocolHub.Communication.BaseStation
{

  /// <summary>
  /// Abstract class describing interface functionality of the station pipe
  /// </summary>
  public abstract class Interface : WaitTimeList<Interface>.TODescriptor
  {

    #region private
    private readonly CAS.CommServer.ProtocolHub.Communication.Diagnostic.Interface myStatistics;
    private readonly InterfaceParameters myParameters;
    #endregion

    #region PUBLIC
    internal ushort address { get { return myParameters.Address; } }
    /// <summary>
    /// Gets the interface number.
    /// </summary>
    /// <value>The interface number.</value>
    internal ushort InterfaceNumber
    {
      get { return (ushort)myStatistics.myID_InterfaceNum; }
    }
    /// <summary>
    /// Switches the Interface off after communication failure.
    /// </summary>
    internal virtual void SwitchIOffAfterFailure()
    {
      myStatistics.CurrentInterfaceState = Statistics.InterfaceStatistics.InterfaceState.Fail;
      Cycle = myParameters.InactivityAfterFailureTime;
      ResetCounter();
    }
    /// <summary>
    /// Switches the interface off.
    /// </summary>
    internal virtual void SwitchIOff()
    {
      myStatistics.CurrentInterfaceState = Statistics.InterfaceStatistics.InterfaceState.Standby;
      Cycle = myParameters.InactivityTime;
      ResetCounter();
    }
    /// <summary>
    /// Switches the interface on.
    /// </summary>
    internal virtual void SwitchIOn()
    {
      myStatistics.CurrentInterfaceState = Statistics.InterfaceStatistics.InterfaceState.Active;
      Remove();
    }
    /// <summary>
    /// Marks the end of RW operation.
    /// </summary>
    internal virtual void MarkEndOfRWOperation()
    {
      myStatistics.MarkRWOperationResult();
    }
    /// <summary>
    /// Gets the retries.
    /// </summary>
    /// <value>The get retries.</value>
    internal protected abstract RetryFilter Retries { get; }
    /// <summary>
    /// Writes the data.
    /// </summary>
    /// <param name="data">The data.</param>
    /// <param name="dataAddress">The data address.</param>
    /// <returns></returns>
    internal protected abstract bool WriteData(object data, IBlockDescription dataAddress);
    /// <summary>
    /// Reads the data.
    /// </summary>
    /// <param name="data">The data.</param>
    /// <param name="dataAddress">The data address.</param>
    /// <returns></returns>
    internal protected abstract bool ReadData(out object data, IBlockDescription dataAddress);
#if COMMSERVER
    /// <summary>
    /// Initializes a new instance of the <see cref="Interface"/> class.
    /// </summary>
    /// <param name="parameters">The parameters.</param>
    /// <param name="myWTimeList">wait time list</param>
    /// <param name="segmentStatistic">The segment statistic.</param>
    /// <param name="stationStatistic">The station statistic.</param>
    internal Interface(InterfaceParameters parameters, WaitTimeList<Interface> myWTimeList, IInterface2SegmentLink segmentStatistic, Diagnostic.Station stationStatistic)
#endif
#if SNIFFER
    internal Interface
      (
      InterfaceDsc interfaceDSC, WaitTimeList myWTimeList, Management.Statistics.Segment statSegment, 
      Management.Station statStation
      )
#endif
      : base(myWTimeList, parameters.InactivityTime)
    {
      myParameters = parameters;
      myStatistics = new Diagnostic.Interface(parameters.Name, parameters.InterfaceNumber, segmentStatistic, stationStatistic);
    }//Interface
    #endregion

  }
}
