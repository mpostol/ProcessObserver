//___________________________________________________________________________________
//
//  Copyright (C) 2020, Mariusz Postol LODZ POLAND.
//
//  To be in touch join the community at GITTER: https://gitter.im/mpostol/OPC-UA-OOI
//___________________________________________________________________________________

using CAS.CommServer.ProtocolHub.Communication.BaseStation;
using System.Collections;
using UAOOI.ProcessObserver.Configuration;

namespace CAS.CommServer.ProtocolHub.CommunicationUnitTests.Instrumentation
{
  /// <summary>
  /// Facade implementation of Pipe
  /// </summary>
  internal class FacadePipe : Pipe
  {
    private readonly ArrayList myDataDescriptionsList = new ArrayList();
    protected override IEnumerable GetDataDescriptionList => myDataDescriptionsList;

    internal FacadePipe(ComunicationNet.StationRow currSDsc)
    {
      myStatistics = new CAS.CommServer.ProtocolHub.Communication.Diagnostic.Station(currSDsc);
    }
  }
}