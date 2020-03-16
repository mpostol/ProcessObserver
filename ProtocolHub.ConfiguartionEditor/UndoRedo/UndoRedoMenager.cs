//___________________________________________________________________________________
//
//  Copyright (C) 2020, Mariusz Postol LODZ POLAND.
//
//  To be in touch join the community at GITTER: https://gitter.im/mpostol/OPC-UA-OOI
//___________________________________________________________________________________

using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Text;

namespace CAS.CommServer.ProtocolHub.ConfigurationEditor.UndoRedo
{
  /// <summary>
  /// Manages row and field changes for a specific DataTable.
  /// </summary>
  /// <remarks>Based on http://www.codeproject.com/cs/database/dtt1.asp</remarks>
  [Obsolete()]
  public sealed class UndoRedoMenager
  {

    #region Debug
#if DEBUG
    /// <summary>
    /// Writes operations list when debugging
    /// </summary>
    /// <param name="param"></param>
    private void ShowOperations(string param)
    {
      StringBuilder sb = new StringBuilder();
      foreach (UndoRedoOperationBase opb in m_operations)
      {
        sb.AppendFormat("{0}   {1} Transaction {2} Deleted {3}", opb.ID, opb.OperationType.ToString(), opb.TransactionId.ToString(), opb.WasDeleted.ToString());
        sb.AppendLine();
      }
      sb.AppendFormat("Param {0}", param);
      Debug.Write(sb.ToString());
    }
#endif
    #endregion

    #region private fields
    /// <summary>
    /// The log of operations.
    /// </summary>
    private static List<UndoRedoOperationBase> m_operations;
    /// <summary>
    /// The dataset on which we are tracking operations.
    /// </summary>
    private DataSet m_sourceDataSet;
    /// <summary>
    /// Enables (the default) or disables logging.  Logging is disabled
    /// during Revert and Apply.
    /// </summary>
    private static bool m_doLogging;
    /// <summary>
    /// Begins or ends the transaction. By default transactions are disabled
    /// </summary>
    private static bool m_trasaction;
    /// <summary>
    /// The last operation record index on which table operations were accepted.
    /// </summary>
    private static int m_lastAcceptedChangeIndex;
    /// <summary>
    /// The list of uncommitted rows
    /// </summary>
    private static Dictionary<DataRow, List<int>> m_uncomittedRows;
    /// <summary>
    /// 
    /// </summary>
    private static List<UndoRedoOperationBase> waitingForChangedEventList;
    /// <summary>
    /// Current transaction identifier if its null the transaction is disabled
    /// </summary>
    private static int? curentTransactionID;
    #endregion

    #region private methods
    /// <summary>
    /// Applies all given transactions
    /// </summary>
    /// <param name="ops">Operation list</param>
    private void ApplyTransaction(List<UndoRedoOperationBase> ops)
    {
      int i = 0;
      while (i < ops.Count)
      {
        UndoRedoOperationBase r = ops[i];
        DataRow row = r.Row;

        switch (r.OperationType)
        {
          case UndoRedoOperationBaseType.NewRow:
            UndoRedoRowOperation rowOp = (UndoRedoRowOperation)r;
            DataTable sourceTable = rowOp.Table;
            DataRow newRow = sourceTable.NewRow();

            if (rowOp.WasDeleted)
            {
              RestoreRowFields(rowOp, newRow);
            }
            bool flag = true;
            int j = i + 1;
            while (flag & j < ops.Count)
            {
              if (ops[j].OperationType == UndoRedoOperationBaseType.ChangeField)
              {
                UndoRedoFieldOperation f = (UndoRedoFieldOperation)ops[j];
                newRow[f.ColumnName] = f.NewValue;
                i = j - 1;
              }
              else
                flag = false;
              j++;
            }
            sourceTable.Rows.Add(newRow);

            for (int n = r.ID; n < m_operations.Count; ++n)
            {
              if (m_operations[n].Row == row)
              {
                m_operations[n].Row = newRow;
              }
            }
            break;

          case UndoRedoOperationBaseType.DeleteRow:
            //((UndoRedoRowOperation)r).Table.Rows.Remove(row);
            ((UndoRedoRowOperation)r).Row.Delete();
            break;

          case UndoRedoOperationBaseType.ChangeField:
            UndoRedoFieldOperation fieldOp = (UndoRedoFieldOperation)r;
            row[fieldOp.ColumnName] = fieldOp.NewValue;
            break;
        }
        i++;
      }
    }
    /// <summary>
    /// Wire up all the events foreach data table in source dataset used by the operation logger.
    /// </summary>
    private void Hook()
    {
      //foreach ( DataTable sourceTable in SourceDataSet.Tables )
      //{
      //  sourceTable.ColumnChanging += new DataColumnChangeEventHandler( OnColumnChanging );
      //  sourceTable.ColumnChanged += new DataColumnChangeEventHandler( OnColumnChanged );
      //  sourceTable.RowDeleting += new DataRowChangeEventHandler( OnRowDeleting );
      //  sourceTable.RowChanged += new DataRowChangeEventHandler( OnRowChanged );
      //  sourceTable.TableNewRow += new DataTableNewRowEventHandler( OnTableNewRow );
      //  sourceTable.TableCleared += new DataTableClearEventHandler( OnTableCleared );
      //}
    }
    /// <summary>
    /// Unhook our event handlers from the source dataset.
    /// </summary>
    private void Unhook()
    {
      //foreach ( DataTable sourceTable in SourceDataSet.Tables )
      //{
      //  sourceTable.ColumnChanging -= new DataColumnChangeEventHandler( OnColumnChanging );
      //  sourceTable.ColumnChanged -= new DataColumnChangeEventHandler( OnColumnChanged );
      //  sourceTable.RowDeleting -= new DataRowChangeEventHandler( OnRowDeleting );
      //  sourceTable.RowChanged -= new DataRowChangeEventHandler( OnRowChanged );
      //  sourceTable.TableNewRow -= new DataTableNewRowEventHandler( OnTableNewRow );
      //  sourceTable.TableCleared -= new DataTableClearEventHandler( OnTableCleared );
      //}
    }
    /// <summary>
    /// Restores all row fields saved in the transaction record's field-value collection.
    /// </summary>
    /// <param name="record"></param>
    /// <param name="row"></param>
    private void RestoreRowFields(UndoRedoRowOperation record, DataRow row)
    {
      foreach (DataColumn dc in row.Table.Columns)
      {
        row[dc] = record.GetValue(dc.ColumnName);
      }
    }
    /// <summary>
    /// Saves all row fields to the transaction record's field-value collection.
    /// </summary>
    /// <param name="record"></param>
    /// <param name="row"></param>
    private void SaveRowFields(UndoRedoRowOperation record, DataRow row)
    {
      foreach (DataColumn dc in row.Table.Columns)
      {
        try
        {
          record.AddColumnNameValuePair(dc.ColumnName, row[dc]);
        }
        catch
        { }
      }
    }
    #region Event handlers
    /// <summary>
    /// We do not support undoing a Clear action.  This simply clears the internal collections and state.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void OnTableCleared(object sender, DataTableClearEventArgs e)
    {
      ClearLog();
    }
    /// <summary>
    /// Log the new row and add it to the uncommitted row collection.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void OnTableNewRow(object sender, DataTableNewRowEventArgs e)
    {
      if (m_doLogging)
      {
        int idx = m_operations.Count;
        UndoRedoRowOperation record = new UndoRedoRowOperation
          (idx, e.Row.Table, e.Row, UndoRedoOperationBaseType.NewRow);
        if (m_trasaction)
          record.TransactionId = curentTransactionID;
        OnRedoUndoOperationAdding(new UndoRedoEventArgs(record));
        m_operations.Add(record);
        OnRedoUndoOperationAdded(new UndoRedoEventArgs(record));
        List<int> rowIndices = new List<int>
        {
          idx
        };
        m_uncomittedRows.Add(e.Row, rowIndices);
      }
    }
    /// <summary>
    /// Handler for when the row is actually added to the DataTable's row collection.
    /// The row is now committed, so it is removed from the uncommitted map.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <exception cref="UndoRedoException">If row that doesn't exist in the uncommitted row collection</exception>
    private void OnRowChanged(object sender, DataRowChangeEventArgs e)
    {
      if (m_doLogging)
      {
        if (e.Action == DataRowAction.Add)
        {
          if (!m_uncomittedRows.ContainsKey(e.Row))
            throw new UndoRedoException("Attempting to commit a row that doesn't exist in the uncommitted row collection.");
          m_uncomittedRows.Remove(e.Row);
        }
      }
    }
    /// <summary>
    /// Handler for when a field changes.  This records only the current (old) field value.
    /// The OnColumnChanged handler records the new value, giving the application a chance
    /// to change the proposed value.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void OnColumnChanging(object sender, DataColumnChangeEventArgs e)
    {
      if (m_doLogging)
      {
        object oldVal = e.Row[e.Column];
        int trnIdx = m_operations.Count;
        UndoRedoFieldOperation record = new UndoRedoFieldOperation
          (trnIdx, e.Row.Table, e.Row, e.Column.ColumnName, oldVal, e.ProposedValue);
        if (m_trasaction)
          record.TransactionId = curentTransactionID;
        OnRedoUndoOperationAdding(new UndoRedoEventArgs(record));
        m_operations.Add(record);
        OnRedoUndoOperationAdded(new UndoRedoEventArgs(record));
        waitingForChangedEventList.Add(record);
        if (m_uncomittedRows.ContainsKey(e.Row))
          m_uncomittedRows[e.Row].Add(trnIdx);
      }
    }
    /// <summary>
    /// Handler for when the field value actually changes.  The application has now
    /// had a chance to set the proposed value, so we can record it as the new value.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void OnColumnChanged(object sender, DataColumnChangeEventArgs e)
    {
      if (m_doLogging)
      {
        for (int i = 0; i < waitingForChangedEventList.Count; i++)
        {
          UndoRedoFieldOperation r = (UndoRedoFieldOperation)waitingForChangedEventList[i];
          if ((r.Row == e.Row) && (r.ColumnName == e.Column.ColumnName))
          {
            r.NewValue = e.ProposedValue;
            waitingForChangedEventList.RemoveAt(i);
            break;
          }
        }
      }
    }
    /// <summary>
    /// The row deleting event fires when the row has being removed fro the collection.
    /// We can't use the row deleted event to record the row field values because the row
    /// has been then marked as deleted and accessing the fields throws an exception.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void OnRowDeleting(object sender, DataRowChangeEventArgs e)
    {
      if (m_doLogging)
      {
        UndoRedoRowOperation record = new UndoRedoRowOperation
          (m_operations.Count, e.Row.Table, e.Row, UndoRedoOperationBaseType.DeleteRow);
        if (m_trasaction)
          record.TransactionId = curentTransactionID;
        m_operations.Add(record);
        SaveRowFields(record, e.Row);
        OnRedoUndoOperationAdded(new UndoRedoEventArgs(record));
      }
    }
    /// <summary>
    /// Fires the RedoUndoOperationAdding event.
    /// </summary>
    /// <param name="e"></param>
    private void OnRedoUndoOperationAdding(UndoRedoEventArgs e)
    {
      RedoUndoOperationAdding?.Invoke(this, e);
    }
    /// <summary>
    /// Fires the RedoUndoOperationAdded event.
    /// </summary>
    /// <param name="e"></param>
    private void OnRedoUndoOperationAdded(UndoRedoEventArgs e)
    {
      RedoUndoOperationAdded?.Invoke(this, e);
    }
    #endregion
    #endregion

    #region public Properties
    /// <summary>
    /// Gets or sets the source dataset.
    /// </summary>
    /// <exception cref="ArgumentNullException">If data set is null</exception>
    public DataSet SourceDataSet
    {
      get => m_sourceDataSet;
      set
      {
        if (m_sourceDataSet != null)
        {
          Unhook();
        }
        m_sourceDataSet = value ?? throw new ArgumentNullException("The source dataset cannot be null.");
        Hook();
      }
    }
    /// <summary>
    /// Gets the operation log for the associated dataset.
    /// </summary>
    public List<UndoRedoOperationBase> Log => m_operations;
    #endregion

    #region public methods
    /// <summary>
    /// Clears log data
    /// </summary>
    public static void ClearLog()
    {
      m_lastAcceptedChangeIndex = 0;
      m_operations.Clear();
      m_uncomittedRows.Clear();
      waitingForChangedEventList.Clear();
      m_operations.TrimExcess();
      m_doLogging = true;
    }
    /// <summary>
    /// Suspends logging.  Used during Revert and Apply to prevent logging of already
    /// logged operations.
    /// </summary>
    public static void SuspendLogging()
    {
      m_doLogging = false;
    }
    /// <summary>
    /// Resumes logging.
    /// </summary>
    public static void ResumeLogging()
    {
      m_doLogging = true;
    }
    /// <summary>
    /// Begins the transaction.
    /// </summary>
    public static void BeginTransaction()
    {
      m_trasaction = true;
      if (!curentTransactionID.HasValue)
        curentTransactionID = 0;
      else
        curentTransactionID++;
    }
    /// <summary>
    /// Ends current transaction.
    /// </summary>
    public static void EndTransaction()
    {
      m_trasaction = false;
    }
    /// <summary>
    /// Accepts the changes.
    /// </summary>
    public void AcceptChanges()
    {
      m_lastAcceptedChangeIndex = m_operations.Count;
      m_sourceDataSet.AcceptChanges();
    }
    /// <summary>
    /// Rejects the changes.
    /// </summary>
    public void RejectChanges()
    {
      int numTran = m_operations.Count - m_lastAcceptedChangeIndex;
      m_operations.RemoveRange(m_lastAcceptedChangeIndex, numTran);
      m_sourceDataSet.RejectChanges();
    }
    /// <summary>
    /// Collects the uncommitted rows.
    /// </summary>
    public void CollectUncommittedRows()
    {
      List<int> allIndices = new List<int>();
      foreach (List<int> indices in m_uncomittedRows.Values)
        allIndices.AddRange(indices);
      allIndices.Sort();
      for (int n = allIndices.Count - 1; n >= 0; --n)
        m_operations.RemoveAt(allIndices[n]);
      m_uncomittedRows.Clear();
    }
    /// <summary>
    /// Reverts the operation or transaction occurring at the specified index.
    /// </summary>
    /// <param name="idx">The operation index to roll back.</param>
    /// <exception cref="ArgumentOutOfRangeException">If identifier is negative or greater than the number of operations</exception>
    public int Revert(int idx)
    {
      if ((idx < 0) || (idx >= m_operations.Count))
      {
        throw new ArgumentOutOfRangeException("Idx cannot be negative or greater than the number of operations.");
      }

      int? result = null;
      UndoRedoOperationBase r = m_operations[idx];
      SuspendLogging();
      List<UndoRedoOperationBase> tempOpList = new List<UndoRedoOperationBase>();
      for (int i = 0; i < m_operations.Count; i++)
      {
        UndoRedoOperationBase op = m_operations[i];
        if (op.TransactionId == m_operations[idx].TransactionId)
        {
          if (result == null)
          {
            result = i;
          }
          tempOpList.Add(op);
        }
      }
      RevertTransaction(tempOpList);
      ResumeLogging();
#if Debug
      ShowOperations( "Revert result " + result.ToString() );
#endif
      return (result.Value - 1);
    }
    /// <summary>
    /// Reverts all given operations
    /// </summary>
    /// <param name="ops"></param>
    /// <returns></returns>
    public int RevertTransaction(List<UndoRedoOperationBase> ops)
    {
      int result = m_operations[0].ID;
      //foreach (UndoRedoOperationBase r in ops)
      //{
      for (int i = 0; i < ops.Count; i++)
      {
        DataRow row = ops[i].Row;

        switch (ops[i].OperationType)
        {
          case UndoRedoOperationBaseType.NewRow:
            if (!ops[i].WasDeleted)
            {
              SaveRowFields((UndoRedoRowOperation)ops[i], row);
            }
            row.Delete();
            break;

          case UndoRedoOperationBaseType.DeleteRow:
            UndoRedoRowOperation rowOp = (UndoRedoRowOperation)ops[i];
            DataTable sourceTable = rowOp.Table;
            DataRow newRow = sourceTable.NewRow();
            RestoreRowFields(rowOp, newRow);
            sourceTable.Rows.Add(newRow);
            for (int n = ops[i].ID; n >= 0; --n)
            //for ( int n = 0; n > ops[i].ID; n++ )
            {
              if (m_operations[n].Row == row)
              {
                m_operations[n].Row = newRow;
              }
            }
            break;

          case UndoRedoOperationBaseType.ChangeField:
            UndoRedoFieldOperation fieldOp = (UndoRedoFieldOperation)ops[i];
            try
            {
              row[fieldOp.ColumnName] = fieldOp.OldValue;
            }
            catch (Exception)
            {

            }
            break;
        }
      }
      return result;
    }
    /// <summary>
    /// Applies the operation or transaction occurring at the specified index.
    /// </summary>
    /// <param name="idx">The operation index to apply.</param>
    /// <exception cref="ArgumentOutOfRangeException">If identifier is negative or greater than the number of operations</exception>
    public int Apply(int idx)
    {
      if ((idx < 0) || (idx >= m_operations.Count))
        throw new ArgumentOutOfRangeException("Idx cannot be negative or greater than the number of operations.");
      SuspendLogging();
      int result = idx;
      UndoRedoOperationBase r = m_operations[idx];
      List<UndoRedoOperationBase> tempOpList = new List<UndoRedoOperationBase>();
      for (int i = 0; i < m_operations.Count; i++)
      {
        UndoRedoOperationBase op = m_operations[i];
        if (op.TransactionId == m_operations[idx].TransactionId)
        {
          tempOpList.Add(op);
          result = i;
        }
      }
      ApplyTransaction(tempOpList);
      ResumeLogging();
#if Debug
      ShowOperations( "Appply result " + result.ToString() );
#endif
      return (result);
    }
    /// <summary>
    /// Deletes the specified idx operation from the rever log.
    /// </summary>
    /// <param name="idx">The idx.</param>
    /// <returns></returns>
    public int Delete(int idx)
    {
      if ((idx < 0) || (idx >= m_operations.Count))
      {
        throw new ArgumentOutOfRangeException("Idx cannot be negative or greater than the number of operations.");
      }
      int? result = null;
      UndoRedoOperationBase r = m_operations[idx];
      SuspendLogging();
      List<UndoRedoOperationBase> tempOpList = new List<UndoRedoOperationBase>();
      for (int i = 0; i < m_operations.Count; i++)
      {
        //foreach(UndoRedoOperationBase op in operations)
        //{
        UndoRedoOperationBase op = m_operations[i];
        if (op.TransactionId == m_operations[idx].TransactionId)
        {
          if (result == null)
          {
            tempOpList.Add(op);
            result = i;
          }
          //operations.RemoveAt(i);
        }
      }
      foreach (UndoRedoOperationBase op in tempOpList)
      {
        m_operations.Remove(op);
      }
      ResumeLogging();

#if Debug
      ShowOperations( "Delete result " + result.ToString() );
#endif

      return (result.Value - 1);
    }
    #endregion

    #region Constructors
    /// <summary>
    /// Creates a new <see cref="UndoRedoMenager"/> object.
    /// </summary>
    public UndoRedoMenager()
    {
      m_operations = new List<UndoRedoOperationBase>();
      m_uncomittedRows = new Dictionary<DataRow, List<int>>();
      waitingForChangedEventList = new List<UndoRedoOperationBase>();
      m_doLogging = true;
    }
    #endregion

    #region Delegates end events
    /// <summary>
    /// Delegate used with the RedoUndoOperationAdding and RedoUndoOperationAdded events.
    /// </summary>
    /// <param name="sender">The instance of this class.</param>
    /// <param name="e">The UndoRedoEventArgs instance.</param>
    public delegate void OperationDlgt(object sender, UndoRedoEventArgs e);
    /// <summary>
    /// Triggered before the operation is added to the log.
    /// </summary>
    public event OperationDlgt RedoUndoOperationAdding;
    /// <summary>
    /// Triggered after the operation is added to the log.
    /// </summary>
    public event OperationDlgt RedoUndoOperationAdded;
    #endregion

  }
}
