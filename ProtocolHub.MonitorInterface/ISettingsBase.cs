//___________________________________________________________________________________
//
//  Copyright (C) 2020, Mariusz Postol LODZ POLAND.
//
//  To be in touch join the community at GITTER: https://gitter.im/mpostol/OPC-UA-OOI
//___________________________________________________________________________________

namespace CAS.CommServer.ProtocolHub.MonitorInterface
{
  
  /// <summary>
  /// Interface ISettingsBase provides the base functionality used to support user property settings.
  /// </summary>
  public interface ISettingsBase
  {

    /// <summary>
    /// Gets or sets the value of the specified settings property.
    /// </summary>
    /// <param name="propertyName">A <see cref="System.String"/> containing the name of the property to access.</param>
    /// <exception cref="System.Configuration.SettingsPropertyNotFoundException">There are no properties associated with the current object, or the specified property could not be found.</exception>
    /// <exception cref="System.Configuration.SettingsPropertyIsReadOnlyException">An attempt was made to set a read-only property.</exception>
    /// <exception cref="System.Configuration.SettingsPropertyWrongTypeException">The value supplied is of a type incompatible with the settings property, during a set operation.</exception>
    /// <returns>If found, the value of the named settings property.</returns>
    object this[string propertyName] { get; set; }

  }
}
