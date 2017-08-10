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


using CAS.CommServer.ProtocolHub.ConfigurationEditor.HMI;
using CAS.CommServer.ProtocolHub.ConfigurationEditor.Properties;
using CAS.Lib.CodeProtect;
using CAS.Lib.CodeProtect.LicenseDsc;
using System;
using System.Diagnostics;
using System.Windows.Forms;

[assembly: CLSCompliant(true)]

namespace CAS.CommServer.ProtocolHub.ConfigurationEditor
{
  class MainClass
  {
    [STAThread]
    static void Main()
    {
      try
      {
        AssemblyTraceEvent.Tracer.TraceMessage(TraceEventType.Verbose, 32, "Starting application CAS.CommServer.ProtocolHub.ConfigurationEditor");
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
        Application.Run
          (
            new ConfigTreeView(ConfigurationManagement.ProtocolHubConfiguration,
                               new ConfigIOHandler(ConfigurationManagement.ReadConfiguration),
                               new ConfigIOHandler(ConfigurationManagement.SaveProc),
                               new ConfigIOHandler(ConfigurationManagement.ClearConfig),
                               Settings.Default.ToolsMenu)
           );
        AssemblyTraceEvent.Tracer.TraceMessage(TraceEventType.Verbose, 32, "Finishing application CAS.CommServer.ProtocolHub.ConfigurationEditor");
      }
      catch (Exception _ex)
      {
        string _message = $"The application has been finished by the exception {_ex.Message} call the vendor for assistance";
        AssemblyTraceEvent.Tracer.TraceMessage(TraceEventType.Error, 36, _message);
        AssemblyTraceEvent.Tracer.TraceMessage(TraceEventType.Error, 36, $"Stock for the exception {_ex.StackTrace}");
        MessageBox.Show(_message, "Application error", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
      finally
      {
        AssemblyTraceEvent.Tracer.TraceSource.Flush();
      }
    }
  }
}
