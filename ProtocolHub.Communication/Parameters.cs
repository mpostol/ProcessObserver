//_______________________________________________________________
//  Title   : Parameters od the Interface
//  System  : Microsoft VisualStudio 2015 / C#
//  $LastChangedDate:  $
//  $Rev: $
//  $LastChangedBy: $
//  $URL: $
//  $Id:  $
//
//  Copyright (C) 2017, CAS LODZ POLAND.
//  TEL: +48 608 61 98 99 
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//_______________________________________________________________

using System;
using static CAS.NetworkConfigLib.ComunicationNet;

namespace CAS.CommServer.ProtocolHub.Communication
{
  /// <summary>
  /// Parameters of the interface
  /// </summary>
  public struct InterfaceParameters
  {
    internal readonly TimeSpan InactivityTime;
    internal readonly TimeSpan InactivityAfterFailureTime;
    internal string Name;
    internal ushort Address;
    internal byte InterfaceNumber;
    /// <summary>
    /// Gets the interface number max value.
    /// </summary>
    /// <value>The interface number max value.</value>
    static public byte InterfaceNumberMaxValue { get { return 1; } }
    /// <summary>
    /// Initializes a new instance of the <see cref="InterfaceParameters"/> struct.
    /// </summary>
    /// <param name="interfacesRow">The interfaces row.</param>
    public InterfaceParameters(InterfacesRow interfacesRow)
    {
      InactivityTime = TimeSpan.FromMilliseconds(interfacesRow.InactTime);
      InactivityAfterFailureTime = TimeSpan.FromMilliseconds(interfacesRow.InactTimeAFailure);
      Name = interfacesRow.Name;
      Address = (ushort)Math.Min(interfacesRow.Address, ushort.MaxValue);
      InterfaceNumber = (byte)Math.Min(interfacesRow.InterfaceNum, byte.MaxValue);
      if (InterfaceNumber > InterfaceNumberMaxValue)
        throw new ArgumentOutOfRangeException("InterfaceNumber > InterfaceNumberMaxValue");
    }
  }
}
