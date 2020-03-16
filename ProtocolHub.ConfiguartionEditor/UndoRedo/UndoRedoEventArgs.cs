//___________________________________________________________________________________
//
//  Copyright (C) 2020, Mariusz Postol LODZ POLAND.
//
//  To be in touch join the community at GITTER: https://gitter.im/mpostol/OPC-UA-OOI
//___________________________________________________________________________________

using System;

namespace CAS.CommServer.ProtocolHub.ConfigurationEditor.UndoRedo
{

  /// <summary>
  /// Encapsulates the operation record associated with a operation event.
  /// </summary>
  public class UndoRedoEventArgs : EventArgs
  {

    #region private
    /// <summary>
    /// The transaction record.
    /// </summary>
    private readonly UndoRedoOperationBase record;
    #endregion

    #region Properties
    /// <summary>
    /// Gets the transaction record associated with the event.
    /// </summary>
    public UndoRedoOperationBase Record => record;
    #endregion

    #region Constructor
    /// <summary>
    /// Creates a new <see cref="UndoRedoEventArgs"/> object.
    /// </summary>
    /// <param name="record">The transaction record.</param>
    public UndoRedoEventArgs(UndoRedoOperationBase record)
    {
      this.record = record;
    }
    #endregion

  }
}