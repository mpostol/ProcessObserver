//_______________________________________________________________
//  Title   : NetworkConfig - entry point class
//  System  : Microsoft VisualStudio 2015 / C#
//  $LastChangedDate$
//  $Rev$
//  $LastChangedBy$
//  $URL$
//  $Id$
//
//  Copyright (C) 2016, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//_______________________________________________________________


using CAS.CommServer.ProtocolHub.ConfiguartionEditor.Properties;
using CAS.Lib.CodeProtect;
using CAS.Lib.CodeProtect.LicenseDsc;
using NetworkConfig.HMI;
using System;
using System.Diagnostics;
using System.Windows.Forms;

[assembly: CLSCompliant(true)]

namespace CAS.CommServer.ProtocolHub.ConfiguartionEditor
{
  class MainClass
  {
    [STAThread]
    static void Main()
    {
      AssemblyTraceEvent.Tracer.TraceMessage(TraceEventType.Verbose, 32, "Starting application CAS.CommServer.ProtocolHub.ConfiguartionEditor");
      string m_CommandLine = Environment.CommandLine;
      if (m_CommandLine.ToLower().Contains("installic"))
        try
        {
          LibInstaller.InstallLicense(false);
        }
        catch (LicenseFileException ex)
        {
          MessageBox.Show("Cannot install license, error:" + ex.Message);
        }
      Application.Run(
          new ConfigTreeView(ConfigurationManagement.configDataBase,
          new ConfigIOHandler(ConfigurationManagement.ReadConfiguration),
          new ConfigIOHandler(ConfigurationManagement.SaveProc),
          new ConfigIOHandler(ConfigurationManagement.ClearConfig),
          new Settings().ToolsMenu
          ));
    }
  }
}
