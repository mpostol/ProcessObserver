//___________________________________________________________________________________
//
//  Copyright (C) 2020, Mariusz Postol LODZ POLAND.
//
//  To be in touch join the community at GITTER: https://gitter.im/mpostol/OPC-UA-OOI
//___________________________________________________________________________________

using System.ComponentModel;
using UAOOI.ProcessObserver.Configuration;

namespace CAS.CommServer.ProtocolHub.ConfigurationEditor.HMI
{
  /// <summary>
  /// Wrapper classes that wrap <see cref="ComunicationNet.ChannelsRow"/>. It contains getters and setters to support property grid.
  /// </summary>
  /// <seealso cref="ProtocolAndSerialRowWrapper">ProtocolAndSerialRowWrapper is the child object in TreeView</seealso>
  /// <remarks>
  /// Attributes for property grid:
  /// DescriptionAttribute. Sets the text for the property that is displayed in the description help pane below the properties. This is a useful way to provide help text for the active property (the property that has focus). Apply this attribute to the MaxRepeatRate property.
  /// CategoryAttribute. Sets the category that the property is under in the grid. This is useful when you want a property grouped by a category name. If a property does not have a category specified, then it will be assigned to the Misc category. Apply this attribute to all properties.
  /// BrowsableAttribute – Indicates whether the property is shown in the grid. This is useful when you want to hide a property from the grid. By default, a public property is always shown in the grid. Apply this attribute to the SettingsChanged property.
  /// ReadOnlyAttribute – Indicates whether the property is read-only. This is useful when you want to keep a property from being editable in the grid. By default, a public property with get and set accessor functions is editable in the grid. Apply this attribute to the AppVersion property.
  /// DefaultValueAttribute – Identifies the property's default value. This is useful when you want to provide a default value for a property and later determine if the property's value is different than the default. Apply this attribute to all properties.
  /// DefaultPropertyAttribute – Identifies the default property for the class. The default property for a class gets the focus first when the class is selected in the grid. Apply this attribute to the AppSettings class.
  /// </remarks>
  [DefaultProperty("Name")]
  internal partial class ChannelsRowWrapper : Action<ComunicationNet.ChannelsRow>
  {
    #region Constructor

    /// <summary>
    /// Creates new <see cref="ChannelsRowWrapper"/> object based on specified channel row
    /// </summary>
    /// <param name="parent">Channel row</param>
    public ChannelsRowWrapper(ComunicationNet.ChannelsRow parent)
      : base(parent)
    { }

    #endregion Constructor

    #region Properties for PropertyGrid

    /// <summary>
    /// Gets or sets Channel identifier - must be unique
    /// </summary>
    [
      BrowsableAttribute(false),
      CategoryAttribute("Global Settings"),
      DescriptionAttribute("Defines Channel identifier - must be unique")
    ]
    public long ChannelID
    {
      get => m_Parent.ChannelID;
      set => m_Parent.ChannelID = value;
    }

    /// <summary>
    /// Gets or sets channel name
    /// </summary>
    [
      BrowsableAttribute(true),
      CategoryAttribute("Global Settings"),
      DefaultValueAttribute("Channel name"),
      DescriptionAttribute("Defines Channel name")
    ]
    public string Name
    {
      get => m_Parent.IsNameNull() ? "" : m_Parent.Name;
      set => m_Parent.Name = value;
    }

    #endregion Properties for PropertyGrid

    #region Override methods

    #region Object overrides

    /// <summary>
    /// Returns channel identifier adn channel name in specified format
    /// </summary>
    /// <returns></returns>
    public override string ToString() { return "Channel: " + Name; }

    #endregion Object overrides

    #region Action overrides

    /// <summary>
    /// This property indicates that the channel row can be deleted
    /// </summary>
    public override bool CanBeDeleted => true;

    /// <summary>
    /// Creates new protocol row
    /// </summary>
    /// <returns>Returns new <see cref="ProtocolAndSerialRowWrapper"/> object</returns>
    public override IAction CreateNewChildObject()
    {
      ComunicationNet.ProtocolDataTable protocolTable = ((ComunicationNet)(m_Parent.Table.DataSet)).Protocol;
      ComunicationNet.ProtocolRow protocolRow = protocolTable.NewProtocolRow(ChannelID, m_Parent.Name);
      return new ProtocolAndSerialRowWrapper(protocolRow);
    }

    /// <summary>
    /// Creates ProtocolAndSerial rows in tree view
    /// </summary>
    public override void CreateNodes()
    {
      base.CreateNodes();
      foreach (ComunicationNet.ProtocolRow curr in m_Parent.GetProtocolRows())
        if (curr.RowState != System.Data.DataRowState.Deleted)
        {
          ProtocolAndSerialRowWrapper newWrapper = new ProtocolAndSerialRowWrapper(curr);
          newWrapper.AddActionTreeNode(m_Node, 11, 11);
          newWrapper.CreateNodes();
        }
    }

    /// <summary>
    /// Pastes specified <see cref="ProtocolAndSerialRowWrapper"/> object under this wrapper
    /// </summary>
    /// <param name="objToPaste"><see cref="ProtocolAndSerialRowWrapper"/> object to be pasted</param>
    public override void PasteChildObject(IAction objToPaste)
    {
      if (objToPaste is ProtocolAndSerialRowWrapper)
      {
        ProtocolAndSerialRowWrapper wrapperToPaste = objToPaste as ProtocolAndSerialRowWrapper;
        ((ComunicationNet)(m_Parent.Table.DataSet)).Protocol.NewProtocolRow(ChannelID, wrapperToPaste.DataRow, false, Name);
      }
    }

    /// <summary>
    /// Checks if specified <see cref="IAction"></see> interface is <see cref="ProtocolAndSerialRowWrapper"></see>
    /// </summary>
    /// <param name="objToPaste">Object to check</param>
    /// <returns>True if specified object is <see cref="ProtocolAndSerialRowWrapper"/></returns>
    public override bool CanBePastedAsChild(IAction objToPaste)
    {
      return objToPaste is ProtocolAndSerialRowWrapper;
    }

    /// <summary>
    /// Checks if specified <see cref="IAction"/> object can be moved
    /// </summary>
    /// <returns>True if specified object can be moved</returns>
    public override bool CanBeMoved() { return false; }

    /// <summary>
    /// Moves specified <see cref="ProtocolAndSerialRowWrapper"/> object under this wrapper
    /// </summary>
    /// <param name="objToPaste"><see cref="ProtocolAndSerialRowWrapper"/> object to be pasted</param>
    public override void MoveChildObject(IAction objToPaste)
    {
      Action<ComunicationNet.ProtocolRow> wrapperToPaste = objToPaste as Action<ComunicationNet.ProtocolRow>;
      if (wrapperToPaste == null)
        return;
      ComunicationNet.ProtocolRow cRowToPaste = wrapperToPaste.DataRow;
      cRowToPaste.BeginEdit();
      cRowToPaste.ChannelID = m_Parent.ChannelID;
      cRowToPaste.EndEdit();
    }

    //#region IDisposable
    ///// <summary>
    ///// Implement IDisposable.
    ///// </summary>
    //protected override void Dispose( bool pDisposing )
    //{
    //  if ( !pDisposing )
    //    return;
    //}
    //#endregion

    #endregion Action overrides

    #endregion Override methods
  }
}