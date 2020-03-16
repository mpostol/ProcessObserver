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
  /// Exception class for exceptions occurring in the UndoRedo module.
  /// </summary>
  public class UndoRedoException : ApplicationException
  {
    /// <summary>
    /// Creates a new <see cref="UndoRedoException"/> object.
    /// </summary>
    /// <param name="msg"></param>
    public UndoRedoException(string msg)
      : base(msg)
    { }
  }
}
