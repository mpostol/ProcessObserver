//___________________________________________________________________________________
//
//  Copyright (C) 2020, Mariusz Postol LODZ POLAND.
//
//  To be in touch join the community at GITTER: https://gitter.im/mpostol/OPC-UA-OOI
//___________________________________________________________________________________

using CAS.CommServer.ProtocolHub.Communication.NetworkConfig;
using CAS.CommServer.ProtocolHub.MonitorInterface;
using System;
using UAOOI.ProcessObserver.Configuration.ItemDescriber;

namespace CAS.CommServer.ProtocolHub.Communication.BaseStation
{
  /// <summary>
  /// COMMUNICATION LIBRARY - Data Base of Process Values
  /// </summary>
  internal static class Initialization
  {
    private static void InitializeItemDescriber()
    {
      XMLManagement xml_desc = new XMLManagement();
      m_ds_dsc = new ItemDecriberDataSet();
      xml_desc.readXMLFile(m_ds_dsc, AppConfigManagement.ItemDsc_configfile);
    }

    internal static ItemDecriberDataSet m_ds_dsc;

    /// <summary>
    /// Initialize the communication server
    /// </summary>
    /// <param name="parent">Parent control hosting some common resources</param>
    /// <param name="demoVer">true if there is no valid license, false otherwise</param>
    /// <param name="valueConstrain">Number of item I can instantiate according of the license. �1 if unlimited. Valid if cDemoVer is false.</param>
    /// <param name="configurationFileName">Name of the configuration file.</param>
    internal static void InitializeServer(CommServerComponent parent, bool demoVer, ref int valueConstrain, string configurationFileName, ISettingsBase settings)
    {
      ProtocolHubConfiguration xml = new ProtocolHubConfiguration(configurationFileName, true);
      try
      {
        InitializeItemDescriber();
      }
      catch (Exception ex)
      {
        CommServerComponent.Tracer.TraceWarning(98, typeof(Initialization).FullName + ".Initialization", ex.Message);
      }
      //CAS.OpcSvr.Da.NETServer.Initialization.InitComponent();
      Station.InitStations(xml.configuration.Station, ref valueConstrain);
      Channel.InitializeChannels(xml.configuration.Channels, parent, demoVer, settings);
      Station.SwitchOnDataScanning();
    }
  }
}