// <summary>
//  Title   : Part of advanced Network config form
//  Author  : Maciej Zbrzezny
//  System  : Microsoft Visual C# .NET
//  History :
//    Maciej Zbrzezny - 2006-09-19
//    created
//    <Author> - <date>:
//    <description>
//
//  Copyright (C)2003, CAS LODZ POLAND.
//  TEL: 42' 686 25 47
//  mailto:techsupp@cas.com.pl
//  http://www.cas.com.pl
// </summary>

namespace CAS.CommServer.ProtocolHub.ConfigurationEditor.HMI
{
  /// <summary>
  /// Summary description for Form AdvancedFormNetworkConfig.
  /// </summary>
  public partial class AdvancedFormNetworkConfig : System.Windows.Forms.Form
  {

    #region private
    ConfigIOHandler fileread;
    ConfigIOHandler filesave;
    ConfigIOHandler fileclear;
    #endregion //private

    #region handlers
    private void ReadXML_Click(object sender, System.EventArgs e)
    {
      fileread(this);
    }
    private void Button_SaveXML_Click(object sender, System.EventArgs e)
    {
      filesave(this);
    }

    #endregion //handlers

    #region constructor
    internal AdvancedFormNetworkConfig(CAS.NetworkConfigLib.ComunicationNet _configmanagemet, ConfigIOHandler _fileread, ConfigIOHandler _filesave, ConfigIOHandler _fileclear)
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
    #endregion

  }
}
