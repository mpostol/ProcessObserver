//<summary>
//  Title   : Unit tests for CommServerComponent constructor
//  System  : Microsoft Visual C# .NET 2008
//  $LastChangedDate$
//  $Rev$
//  $LastChangedBy$
//  $URL$
//  $Id$
//
//  Copyright (C)2008, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//</summary>

using CAS.CommServer.ProtocolHub.Communication;
using CAS.Lib.RTLib.Processes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;

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
      AssemblyInitializeClass.CommServerComponent.Initialize("bleble.config");
    }
    [TestMethod]
    public void LogFileExistsTest()
    {
      TraceEvent _tracer = CommServerComponent.Tracer;
      FileInfo _logFileInfo = new FileInfo(@"CAS.CommServer.ProtocolHub.Communication.log");
      long _logFileLength = _logFileInfo.Length;
      Assert.IsTrue(500 < _logFileLength, $"The file lentht is {_logFileLength} but expected 500+");
      _tracer.TraceVerbose(0, "Unit Test", "LogFileExistsTest is executed");
      _logFileInfo.Refresh();
      Assert.IsTrue(_logFileInfo.Exists);
      Assert.IsTrue(_logFileInfo.Length > _logFileLength + 20);
    }

  }
}
