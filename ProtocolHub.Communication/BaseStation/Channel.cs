//___________________________________________________________________________________
//
//  Copyright (C) 2020, Mariusz Postol LODZ POLAND.
//
//  To be in touch join the community at GITTER: https://gitter.im/mpostol/OPC-UA-OOI
//___________________________________________________________________________________

using CAS.CommServer.ProtocolHub.Communication.LicenseControl;
using CAS.CommServer.ProtocolHub.MonitorInterface;
using CAS.Lib.CommonBus;
using CAS.Lib.CommonBus.ApplicationLayer;
using CAS.Lib.CommonBus.Management;
using System;
using System.Collections;
using System.ComponentModel;
using System.Xml;
using UAOOI.ProcessObserver.Configuration;
using UAOOI.ProcessObserver.RealTime.Management;
using UAOOI.ProcessObserver.RealTime.Processes;

namespace CAS.CommServer.ProtocolHub.Communication.BaseStation
{
  /// <summary>
  /// Channel implementation
  /// </summary>
  internal class Channel : HandlerWaitTimeList<SegmentStateMachine>
  {
    #region private

    private const string m_src = "CAS.Lib.CommServer.Channel";
    private readonly Statistics.ChannelStatistics myStatistics;
    private static ArrayList myChannels = new ArrayList();

    private IApplicationLayerMaster CreateApplicationProtocol(ComunicationNet.ProtocolRow protocol, CommServerComponent parent, PluginCollection plugins, ISettingsBase settings)
    {
      CommServerComponent.Tracer.TraceVerbose(60, m_src, "Creating protocol: " + protocol.Name);
      if (protocol.IsDPIdentifierNull())
      {
        //the protocol is not set so it cannot be created
        CommServerComponent.Tracer.TraceWarning(65, m_src, "The protocol is not set so it cannot be created, channel = " + this.myStatistics.myName);
        return null;
      }
      CommServerComponent.Tracer.TraceVerbose(69, m_src, string.Format("Trying to find data provider: {0}", protocol.DPIdentifier.ToString()));
      try
      {
        IDataProviderID _DataProviderID = plugins[protocol.DPIdentifier];
        if (_DataProviderID != null)
        {
          string _format = "OK I have got DataProvider. Name = {0} [{1}]";
          CommServerComponent.Tracer.TraceVerbose(77, m_src, string.Format(_format, _DataProviderID.Title, _DataProviderID.GetDataProviderDescription.FullName));
          try
          {
            _DataProviderID.SetSettings(protocol.DPConfig);
          }
          catch (XmlException xe)
          {
            _format = "Problem with: {0} because of Xml content: {1}.";
            CommServerComponent.Tracer.TraceWarning(85, m_src, string.Format(_format, protocol.DPConfig, xe.ToString()));
          }
          catch (Exception e)
          {
            _format = "Problem with: {0} because of general failure: {1}.";
            CommServerComponent.Tracer.TraceWarning(90, m_src, string.Format(_format, protocol.DPConfig, e.ToString()));
          }
          IProtocolParent cStatistic = Diagnostic.CommServerProtocol.CreateNewProtocol(protocol.DPConfig, protocol.Name, protocol.ProtocolID, _DataProviderID.GetSettingsHumanReadableFormat(), myStatistics, settings);
          IApplicationLayerMaster chnProtocol = _DataProviderID.GetApplicationLayerMaster(cStatistic, parent.m_CommonBusControl);
          CommServerComponent.Tracer.TraceVerbose(95, m_src, "I have created the DataProvider helper object.");
          return chnProtocol;
        }
        else
        {
          string _message = "The data provider you are looking for is not available – check your configuration and execution path of the product.";
          CommServerComponent.Tracer.TraceInformation(102, m_src, _message);
        }
      }//try
      catch (System.ComponentModel.LicenseException ex)
      {
        string _format = "The component cannot be granted a license: {0}.";
        CommServerComponent.Tracer.TraceWarning(108, m_src, string.Format(_format, ex.LicensedType.ToString()));
      }
      catch (Exception _ex)
      {
        CommServerComponent.Tracer.TraceWarning(112, m_src, string.Format("Some problem encountered while trying to get a DataProvider. Exception {0}.", _ex.Message));
      }
      return null;
    }

    private SegmentStateMachine CurrSegment = null;

    #region

    public override void NewOvertimeCoefficient(long min, long max, long avr)
    {
      //      base.NewOvertimeCoefficient (min, max, avr);
    }

    protected override void Handler(SegmentStateMachine myDsc)
    {
      //if ( !myDsc.NeedsChannelAccess )
      //  return;
      if (CurrSegment != null)
        CurrSegment.DisconnectRequest();
      CurrSegment = myDsc;
      CurrSegment.ConnectRequest();
    }//Scanner

    #endregion private

    #region creator

    private Channel(ComunicationNet.ChannelsRow myCDsc, CommServerComponent parent, bool demoVersion, ISettingsBase settings)
      : base(false, "ChannelSegTOL_" + myCDsc.Name)
    {
      CommServerComponent.Tracer.TraceVerbose(150, m_src, "Creating channel: " + myCDsc.Name);
      Multichannel.NextChannnel();
      myStatistics = new Statistics.ChannelStatistics(myCDsc);
      PluginCollection m_Plugins = new PluginCollection(parent.m_CommonBusControl);
      foreach (ComunicationNet.ProtocolRow proto in myCDsc.GetProtocolRows())
      {
        IApplicationLayerMaster chnProtocol = CreateApplicationProtocol(proto, parent, m_Plugins, settings);
        if (chnProtocol != null)
        {
          foreach (ComunicationNet.SegmentsRow currDSC in proto.GetSegmentsRows())
          {
            SegmentParameters parameters = new SegmentParameters(currDSC);
            Diagnostic.Segment segmentStatistic = new Diagnostic.Segment(currDSC, myStatistics);
            Segment segment = new Segment
              (currDSC, (byte)proto.MaxNumberOfRetries, chnProtocol, parameters, demoVersion, segmentStatistic, this)
            {
              Cycle = parameters.TimeReconnect
            };
            segment.ResetCounter();
          }
        }
        else
        {
          string name = proto != null ? proto.Name : "---not set---";
          CommServerComponent.Tracer.TraceWarning(199, m_src, "Cannot find component implementing the required protocol: " + name);
        }
      }//foreach (NetworkConfig.ComunicationNet.ProtocolRow proto in myCDsc.GetProtocolRows)
      CommServerComponent.Tracer.TraceVerbose(203, m_src, "Channel: " + myCDsc.Name + " has been created.");
    }//Channel

    #endregion creator

    #endregion

    #region API

    internal static void InitializeChannels(ComunicationNet.ChannelsDataTable channelsConfigTable, CommServerComponent parent, bool demoVersion, ISettingsBase settings)
    {
      Segment.DemoMode = demoVersion;
      foreach (ComunicationNet.ChannelsRow currRow in channelsConfigTable)
        try
        {
          myChannels.Add(new Channel(currRow, parent, demoVersion, settings));
        }
        catch (LicenseException ex)
        {
          string msg = "Cannot create channel {0} because of {1}";
          CommServerComponent.Tracer.TraceVerbose(220, m_src, string.Format(msg, currRow.Name, ex.Message));
        }
    }

    #endregion
  }
}