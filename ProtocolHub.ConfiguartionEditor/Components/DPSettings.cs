//___________________________________________________________________________________
//
//  Copyright (C) 2020, Mariusz Postol LODZ POLAND.
//
//  To be in touch join the community at GITTER: https://gitter.im/mpostol/OPC-UA-OOI
//___________________________________________________________________________________

using CAS.Lib.CommonBus;
using System;
using System.Windows.Forms;

namespace CAS.CommServer.ProtocolHub.ConfigurationEditor.Components
{
  /// <summary>
  /// this UserControl allows user to set settings for the DPSettings
  /// </summary>
  //TODO CommServer.CommonBus - remove dependency on `System.Windows.Forms` #7
  public partial class DPSettings: UserControl
  {

    #region creator
      /// <summary>
      /// default creator of the DPSettings
      /// </summary>
    public DPSettings()
    {
      InitializeComponent();
    }
    #endregion

    #region public
      /// <summary>
      /// sets the source object for the property window
      /// </summary>
      /// <param name="pDPID"></param>
    public void SetObjects( IDataProviderID pDPID )
    {
      cn_PropertyGridApp.SelectedObject = pDPID;
      cn_PropertyGridComm.SelectedObject = pDPID.SelectedCommunicationLayer;
    }
    #endregion

    #region events handlers
    private void cn_ToolStripButtonApp_Click( object sender, EventArgs e )
    {
      cn_SplitContainer.Panel1Collapsed = !cn_ToolStripButtonApp.Checked;
      cn_ToolStripButtonComm.Checked = !cn_SplitContainer.Panel2Collapsed;
    }
    private void cn_ToolStripButtonComm_Click( object sender, EventArgs e )
    {
      cn_SplitContainer.Panel2Collapsed = !cn_ToolStripButtonComm.Checked;
      cn_ToolStripButtonApp.Checked = !cn_SplitContainer.Panel1Collapsed;
    }
    #endregion

  }
}
