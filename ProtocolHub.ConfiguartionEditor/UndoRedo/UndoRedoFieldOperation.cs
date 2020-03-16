//___________________________________________________________________________________
//
//  Copyright (C) 2020, Mariusz Postol LODZ POLAND.
//
//  To be in touch join the community at GITTER: https://gitter.im/mpostol/OPC-UA-OOI
//___________________________________________________________________________________

using System;
using System.Data;

namespace CAS.CommServer.ProtocolHub.ConfigurationEditor.UndoRedo
{

  /// <summary>
  /// Undo or redo operation data used when changing fields.
  /// </summary>
  public sealed class UndoRedoFieldOperation: UndoRedoOperationBase
  {

    #region Properties
    /// <summary>
    /// Gets or sets the new field value.
    /// </summary>
    public object NewValue { get; set; }

    /// <summary>
    /// Gets the old field value. 
    /// </summary>
    public object OldValue { get; private set; }

    /// <summary>
    /// Gets the name of the column associated with this operation.  
    /// </summary>
    public string ColumnName { get; private set; }
    #endregion

    #region Constructors
    /// <summary>
    /// Creates a new <see cref="UndoRedoFieldOperation"/> object.
    /// </summary>
    /// <param name="id">The transaction number.  This can be any integer value.</param>
    /// <param name="dt">The data table.</param>
    /// <param name="row">The row being added/deleted.</param>
    /// <param name="columnName">The column name of the field being changed.</param>
    /// <param name="oldValue">The old field's value.</param>
    /// <param name="newValue">The new field's value.</param>
    /// <exception cref="ArgumentNullException">If the data table, data row or column name is null</exception>
    public UndoRedoFieldOperation( int id, DataTable dt, DataRow row, string columnName, object oldValue, object newValue )
    {
      this.id = id;
      this.table = dt ?? throw new ArgumentNullException( "Data table cannot be null." );
      this.row = row ?? throw new ArgumentNullException( "DataRow cannot be null." );
      this.operationType = UndoRedoOperationBaseType.ChangeField;
      this.ColumnName = columnName ?? throw new ArgumentNullException( "Column name cannot be null." );
      this.OldValue = oldValue;
      this.NewValue = newValue;
      Initialize();
    }
    #endregion

  }
}
