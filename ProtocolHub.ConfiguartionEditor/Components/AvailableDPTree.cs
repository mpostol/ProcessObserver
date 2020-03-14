//___________________________________________________________________________________
//
//  Copyright (C) 2020, Mariusz Postol LODZ POLAND.
//
//  To be in touch join the community at GITTER: https://gitter.im/mpostol/OPC-UA-OOI
//___________________________________________________________________________________

using CAS.Lib.CommonBus;
using CAS.Lib.CommonBus.CommunicationLayer;
using CAS.Lib.CommonBus.Management;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using UAOOI.ASMD.GUIAbstractions;

namespace CAS.CommServer.ProtocolHub.ConfigurationEditor.Components
{
  /// <summary>
  /// The form that presents the available DataProviders on Tree
  /// </summary>
  //TODO CommServer.CommonBus - remove dependency on `System.Windows.Forms` #7
  public partial class AvailableDPTree: UserControl
  {

    #region private
    private ICanBeAccepted m_OKCnacelForm;
    private PluginCollection m_PluginCollection;
    private void FillTree( PluginCollection pPlugins )
    {
      foreach ( KeyValuePair<Guid, IDataProviderID> dp in pPlugins )
      {
        TreeNode nndp = new TreeNode( dp.Value.Title );
        nndp.Tag = dp.Value;
        c_TreeView.Nodes[ 0 ].Nodes.Add( nndp );
        foreach ( KeyValuePair<string, ICommunicationLayerId> cl in dp.Value )
        {
          TreeNode nncl = new TreeNode( cl.Value.Title );
          nncl.Tag = cl.Value;
          nndp.Nodes.Add( nncl );
        }
      }
    }
    #endregion

    #region public

    /// <summary>
    /// Gets the get selected Data-provider ID.
    /// </summary>
    /// <value>The get selected Data-provider ID.</value>
    public IDataProviderID GetSelectedDPID { get; private set; }
    #endregion

    #region creators
    private AvailableDPTree()
    {
      InitializeComponent();
    }
    /// <summary>
    /// Initializes a new instance of the <see cref="AvailableDPTree"/> class.
    /// </summary>
    /// <param name="pParent">The parent CommonBusControl.</param>
    /// <param name="pOKCnacelForm">The reference to parent form with interface ICanBeAccepted.</param>
    public AvailableDPTree( CommonBusControl pParent, ICanBeAccepted pOKCnacelForm )
      : this()
    {
      m_OKCnacelForm = pOKCnacelForm;
      //m_OKCnacelForm.SetUserControl = this;
      m_PluginCollection = new PluginCollection( pParent );
      FillTree( m_PluginCollection );
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="AvailableDPTree"/> class.
    /// </summary>
    /// <param name="pParent">The parent CommonBusControl.</param>
    /// <param name="pOKCnacelForm">The reference to parent form with interface ICanBeAccepted.</param>
    /// <param name="pSetting">The settings.</param>
    /// <param name="pGuid">The GUID.</param>
    public AvailableDPTree( CommonBusControl pParent, ICanBeAccepted pOKCnacelForm, string pSetting, Guid pGuid )
      :
      this( pParent, pOKCnacelForm )
    {
      m_PluginCollection.SetDefaultSsetting( pSetting, pGuid );
    }
    #endregion

    #region Events handlers
    private void c_TreeView_AfterSelect( object sender, TreeViewEventArgs e )
    {
      if ( e.Node.Tag is ICommunicationLayerId )
      {
        c_PropertyGrid.SelectedObject = ( (ICommunicationLayerId)e.Node.Tag ).GetCommunicationLayerDescription;
        GetSelectedDPID = (IDataProviderID)e.Node.Parent.Tag;
        GetSelectedDPID.SelectedCommunicationLayer = (ICommunicationLayerId)e.Node.Tag;
        m_OKCnacelForm.CanBeAccepted( true );
      }
      else
      {
        if ( e.Node.Tag is IDataProviderID )
          c_PropertyGrid.SelectedObject = ( (IDataProviderID)e.Node.Tag ).GetDataProviderDescription;
        GetSelectedDPID = null;
        m_OKCnacelForm.CanBeAccepted( false );
      }
    }
    private void c_PropertyGrid_PropertyValueChanged( object s, PropertyValueChangedEventArgs e )
    {
      ( (PropertyGrid)s ).Refresh();
    }
    #endregion

  }
}
