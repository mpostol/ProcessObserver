//___________________________________________________________________________________
//
//  Copyright (C) 2020, Mariusz Postol LODZ POLAND.
//
//  To be in touch join the community at GITTER: https://gitter.im/mpostol/OPC-UA-OOI
//___________________________________________________________________________________

using UAOOI.ProcessObserver.Configuration;

namespace CAS.CommServer.ProtocolHub.ConfigurationEditor.HMI
{
  /// <summary>
  /// Summary description for Form AdvancedFormNetworkConfig.
  /// </summary>
  public partial class AdvancedFormNetworkConfig : System.Windows.Forms.Form
  {
    #region private

    private ConfigIOHandler fileread;
    private ConfigIOHandler filesave;
    private ConfigIOHandler fileclear;

    #endregion private

    #region handlers

    private void ReadXML_Click(object sender, System.EventArgs e)
    {
      fileread(this);
    }

    private void Button_SaveXML_Click(object sender, System.EventArgs e)
    {
      filesave(this);
    }

    #endregion handlers

    #region constructor

    internal AdvancedFormNetworkConfig(ComunicationNet _configmanagemet, ConfigIOHandler _fileread, ConfigIOHandler _filesave, ConfigIOHandler _fileclear)
    {
      InitializeComponent();
      //this.configDataBase = new NetworkConfig.ComunicationNet();
      this.configDataBase = _configmanagemet;
      InitConfigDataBase(false);
      InitializeAdvanceComponent();
      this.fileread = _fileread;
      this.filesave = _filesave;
      this.fileclear = _fileclear;
      //this.configDataBase = NetworkConfig.HMI.ConfigurationManagement.configDataBase;
    }

    #endregion constructor
  }
}