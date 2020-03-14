//___________________________________________________________________________________
//
//  Copyright (C) 2020, Mariusz Postol LODZ POLAND.
//
//  To be in touch join the community at GITTER: https://gitter.im/mpostol/OPC-UA-OOI
//___________________________________________________________________________________

using CAS.CommServer.ProtocolHub.Communication;
using CAS.CommServer.ProtocolHub.MonitorInterface;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using UAOOI.ProcessObserver.RealTime.Processes;

namespace CAS.CommServer.ProtocolHub.CommunicationUnitTests
{
  [TestClass()]
  public class CommServerComponentTest
  {
    /// <summary>
    /// CommServerComponent is singleton but not implemented using this pattern because the development environment fails to handle it
    /// </summary>
    [TestMethod]
    public void CommServerComponentExistTest()
    {
      Assert.IsNotNull(AssemblyInitializeClass.CommServerComponent);
      Assert.IsNotNull(CommServerComponent.Source);
      Assert.IsNotNull(CommServerComponent.Tracer);
    }
    /// <summary>
    ///A test for <see cref="CommServerComponent"/> Constructor - because it was created in assembly initialize class creation new instance must throw an exception.
    ///</summary>
    [TestMethod()]
    [ExpectedException(typeof(ApplicationException))]
    public void CommServerComponentConstructorExistTest()
    {
      CommServerComponent target = new CommServerComponent();
    }
    [TestMethod()]
    [ExpectedException(typeof(ApplicationException))]
    public void CommServerComponentInitializeReentryTest()
    {
      ISettingsBase _settings = new SettingsBaseFixture();
      AssemblyInitializeClass.CommServerComponent.Initialize("bleble.config", _settings);
    }
    [TestMethod]
    public void LogFileExistsTest()
    {
      TraceEvent _tracer = CommServerComponent.Tracer;
      FileInfo _logFileInfo = new FileInfo(@"CAS.CommServer.ProtocolHub.Communication.log");
      long _logFileLength = _logFileInfo.Length;
      Assert.IsTrue(500 < _logFileLength, $"The file length is {_logFileLength} but expected < 500");
      _tracer.TraceVerbose(0, "Unit Test", "LogFileExistsTest is executed");
      _logFileInfo.Refresh();
      Assert.IsTrue(_logFileInfo.Exists);
      Assert.IsTrue(_logFileInfo.Length > _logFileLength + 20);
    }

    #region instrumentation
    private class SettingsBaseFixture : ISettingsBase
    {
      public object this[string propertyName] { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
    #endregion

  }
}
