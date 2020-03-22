//___________________________________________________________________________________
//
//  Copyright (C) 2020, Mariusz Postol LODZ POLAND.
//
//  To be in touch join the community at GITTER: https://gitter.im/mpostol/OPC-UA-OOI
//___________________________________________________________________________________

using CAS.Lib.CodeProtect;
using System.IO;
using UAOOI.ProcessObserver.Configuration.Settings;

namespace CAS.CommServer.ProtocolHub.Communication
{
  /// <summary>
  /// Reads application configuration file
  /// </summary>
  internal class AppConfigManagement
  {
    internal static readonly int RemotingHTTPport = 5000;
    internal static readonly int ConsoleRemotingHTTPport = 5757;
    internal static readonly string ItemDsc_configfile;

    static AppConfigManagement()
    {
      ItemDsc_configfile = ApplicationConfiguration.GetAppSetting("ItemDsc_configfile", "");
      FileInfo fi = RelativeFilePathsCalculator.GetAbsolutePathToFileInApplicationDataFolder(ItemDsc_configfile);
      if (ItemDsc_configfile == null || ItemDsc_configfile.Length < 3)
      {
        FileInfo fiIfNull = RelativeFilePathsCalculator.GetAbsolutePathToFileInApplicationDataFolder(@"\item_dsc.xml");
        ItemDsc_configfile = fiIfNull.FullName;
      }
      else
        ItemDsc_configfile = fi.FullName;
      RemotingHTTPport = ApplicationConfiguration.GetAppSetting("RemotingHTTPport", RemotingHTTPport);
      ConsoleRemotingHTTPport = ApplicationConfiguration.GetAppSetting("ConsoleRemotingHTTPport", ConsoleRemotingHTTPport);
    }
  }
}