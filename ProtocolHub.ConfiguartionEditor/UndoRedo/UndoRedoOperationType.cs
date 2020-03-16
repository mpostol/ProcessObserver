//___________________________________________________________________________________
//
//  Copyright (C) 2020, Mariusz Postol LODZ POLAND.
//
//  To be in touch join the community at GITTER: https://gitter.im/mpostol/OPC-UA-OOI
//___________________________________________________________________________________

namespace CAS.CommServer.ProtocolHub.ConfigurationEditor.UndoRedo
{
  /// <summary>
  /// Operation types
  /// </summary>
  public enum UndoRedoOperationBaseType
  {
    /// <summary>
    /// Adding new row
    /// </summary>
    NewRow,
    /// <summary>
    /// Deleting a row
    /// </summary>
    DeleteRow,
    /// <summary>
    /// Changing fields
    /// </summary>
    ChangeField
  }
}
