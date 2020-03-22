//___________________________________________________________________________________
//
//  Copyright (C) 2020, Mariusz Postol LODZ POLAND.
//
//  To be in touch join the community at GITTER: https://gitter.im/mpostol/OPC-UA-OOI
//___________________________________________________________________________________

using CAS.Lib.CommonBus;
using System;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace CAS.CommServer.ProtocolHub.ConfigurationEditor.HMI
{
  internal class ActionBase
  {
    protected static CommonBusControl m_CommonBusControl;
    internal static CommonBusControl SetCommonBusControl { set => m_CommonBusControl = value; }
  }

  /// <summary>
  /// Abstract class to support operations on <see cref="TreeNode"/>
  /// </summary>
  /// <typeparam name="TRow">Object type</typeparam>
  internal abstract class Action<TRow> : ActionBase, IAction where TRow : DataRow
  {
    #region private

    private void Table_RowDeleted(object sender, System.Data.DataRowChangeEventArgs e)
    {
      System.Diagnostics.Debug.Assert((m_Node != null) && (m_Parent != null) && !m_disposed);
      if (e.Row != m_Parent)
        return;
      this.Dispose(true);
    }

    private void RemoveChildren()
    {
      foreach (TreeNode cn in m_Node.Nodes.Cast<TreeNode>().ToList<TreeNode>())
        ((IAction)cn.Tag).Dispose();
    }

    private void RemoveEventHandler()
    {
      if (m_Parent == null)
        return;
      m_Parent.Table.RowDeleted -= new DataRowChangeEventHandler(Table_RowDeleted);
    }

    #endregion private

    #region protected

    /// <summary>
    /// Data row associated to this action
    /// </summary>
    protected TRow m_Parent;

    protected TreeNode m_Node = new TreeNode();

    #endregion protected

    #region Constructor

    /// <summary>
    /// Associate specified data row to this action
    /// </summary>
    /// <param name="row"></param>
    protected Action(TRow row)
    {
      m_Parent = row;
    }

    /// <summary>
    /// Delated data row associaded to this action
    /// </summary>
    ~Action()
    {
      Dispose(false);
    }

    #endregion Constructor

    #region Properties

    /// <summary>
    /// Gets data row asscociated to this action
    /// </summary>
    [Browsable(false)]
    public TRow DataRow => m_Parent;

    #endregion Properties

    #region public

    internal TreeNode AddActionTreeNode(int pImageIndex, int pSelectedImageIndex)
    {
      m_Node.Tag = this;
      m_Node.ImageIndex = pImageIndex;
      m_Node.SelectedImageIndex = pSelectedImageIndex;
      m_Node.Text = this.ToString();
      return m_Node;
    }

    internal void AddActionTreeNode(TreeNode pParentNode, int pImageIndex, int pSelectedImageIndex)
    {
      m_Node = AddActionTreeNode(pImageIndex, pSelectedImageIndex);
      pParentNode.Nodes.Add(m_Node);
      m_Parent.Table.RowDeleted += new System.Data.DataRowChangeEventHandler(Table_RowDeleted);
    }

    #endregion public

    #region IAction Members

    #region Methods

    /// <summary>
    /// Adds new row to the table
    /// </summary>
    /// <remarks>If we perform this action for PrortocolAndSerialWrapper we adds protocol and serial rows</remarks>
    public void AddObjectToTable()
    {
      m_Parent.Table.Rows.Add(m_Parent);
    }

    /// <summary>
    /// cleanup after unfinished add operation
    /// by default it is empty function and cound be overriden if any cleanup is neccessary
    /// </summary>
    public virtual void AddUnfinishedCleanup()
    { }

    /// <summary>
    /// Creates new child row in dataset
    /// </summary>
    /// <returns>IAction interface</returns>
    public abstract IAction CreateNewChildObject();

    /// <summary>
    /// Creates nodes in TreeView from specifed TreeNode
    /// </summary>
    public virtual void CreateNodes()
    {
      RemoveChildren();
      m_Node.Nodes.Clear();
    }

    /// <summary>
    /// Deletes row from the table
    /// </summary>
    public virtual void DeleteObject()
    {
      m_Parent.Delete();
    }

    /// <summary>
    /// Pastes specified IAction object under this node
    /// </summary>
    /// <param name="objToPaste">Object to paste</param>
    public abstract void PasteChildObject(IAction objToPaste);

    /// <summary>
    /// Checks if specified object can be pasted under this object
    /// </summary>
    /// <param name="objToPaste">Object to paste</param>
    /// <returns>True if specified object can be pasted hare</returns>
    public abstract bool CanBePastedAsChild(IAction objToPaste);

    /// <summary>
    /// Checks if specified <see cref="IAction"/> object can be moved
    /// </summary>
    /// <returns>True if specified object can be moved</returns>
    public abstract bool CanBeMoved();

    /// <summary>
    /// Moves specified <see cref="IAction"/> object under this wrapper
    /// </summary>
    /// <param name="pObjToMove"><see cref="IAction"/> object to be moved</param>
    public abstract void MoveChildObject(IAction pObjToMove);

    /// <summary>
    /// Inform the object that some values have been changed.
    /// </summary>
    public virtual void HasChanged() { return; }

    #endregion Methods

    #region Properties

    /// <summary>
    /// Get the boolean value indicated that this object can be copy or not.
    /// By default it always returns true. If wrapper cannot be copy this property have to be overriten
    /// </summary>
    [BrowsableAttribute(false)]
    public virtual bool CanBeCopied => true;

    /// <summary>
    /// Get the boolean value indicated that this object can create child object or not.
    /// By default it always returns false. If wrapper cannot create child obj this property have to be overriten
    /// </summary>
    [BrowsableAttribute(false)]
    public virtual bool CanCreateChild => true;

    /// <summary>
    /// Get the boolean value indicated that this object can be deleted or not.
    /// By default it always returns true. If wrapper cannot be deleted this property have to be overriten
    /// </summary>
    [BrowsableAttribute(false)]
    public virtual bool CanBeDeleted => true;

    #endregion Properties

    #region IDisposable

    /// <summary>
    /// Implement IDisposable.
    /// </summary>
    public void Dispose()
    {
      Dispose(true);
      GC.SuppressFinalize(this);
    }

    // Track whether Dispose has been called.
    private bool m_disposed = false;

    /// <summary>
    /// Dispose(bool disposing) executes in two distinct scenarios. If disposing equals true,
    /// the method has been called directly or indirectly by a user's code. Managed and
    /// unmanaged resources can be disposed. If disposing equals false, the method has been
    /// called by the  runtime from inside the finalizer and you should not reference  other
    /// objects. Only unmanaged resources can be disposed.
    /// </summary>
    /// <param name="pDisposing">If dsposing equals true, the method has been called directly or indirectly by a user's code</param>
    protected virtual void Dispose(bool pDisposing)
    {
      if (m_disposed || !pDisposing)
        return;
      RemoveEventHandler();
      RemoveChildren();
      m_Node.Remove();
      m_Node = null;
      m_disposed = true;
    }

    #endregion IDisposable

    #endregion IAction Members
  }
}