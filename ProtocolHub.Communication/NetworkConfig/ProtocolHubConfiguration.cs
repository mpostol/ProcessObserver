//___________________________________________________________________________________
//
//  Copyright (C) 2020, Mariusz Postol LODZ POLAND.
//
//  To be in touch join the community at GITTER: https://gitter.im/mpostol/OPC-UA-OOI
//___________________________________________________________________________________

using System;
using UAOOI.ProcessObserver.Configuration;
using UAOOI.ProcessObserver.RealTime.Processes;

namespace CAS.CommServer.ProtocolHub.Communication.NetworkConfig
{
  /// <summary>
  /// Opens and reads OPC server configuration from XML file.
  /// </summary>
  internal class ProtocolHubConfiguration
  {
    #region private

    private readonly string filename;

    #endregion private

    #region public

    /// <summary>
    /// configuration
    /// </summary>
    public readonly ComunicationNet configuration;

    /// <summary>
    /// Initializes a new instance of the <see cref="ProtocolHubConfiguration"/> class.
    /// </summary>
    /// <param name="ConfigurationFilename">The configuration filename.</param>
    public ProtocolHubConfiguration(string ConfigurationFilename)
      : this(ConfigurationFilename, false)
    { }

    /// <summary>
    /// Initializes a new instance of the <see cref="ProtocolHubConfiguration"/> class.
    /// </summary>
    /// <param name="ConfigurationFilename">The configuration filename.</param>
    /// <param name="open_readonly">if set to <c>true</c> file is opened as read only.</param>
    public ProtocolHubConfiguration(string ConfigurationFilename, bool open_readonly)
    {
      filename = ConfigurationFilename;
      try
      {
        configuration = new ComunicationNet();
        this.configuration.BeginInit();
        this.configuration.DataSetName = "ComunicationNet";
        this.configuration.Locale = new System.Globalization.CultureInfo("en-US");
        this.configuration.EndInit();
        XML2DataSetIO.readXMLFile(configuration, filename, open_readonly);
      }
      catch (System.IO.IOException ioex)
      {
        EventLogMonitor.WriteToEventLogError("Problem with CommServer configuration file:" + ioex.Message, (int)Error.CommServer_Configuration);
      }
      catch (Exception ex)
      {
        string _path = Environment.CurrentDirectory;
        EventLogMonitor.WriteToEventLogError($"Problem with CommServer XML configuration file : {filename} - in directory: {_path } error: {ex.ToString()}", (int)Error.CommServer_Configuration);
      }
    }

    #endregion public
  }
}