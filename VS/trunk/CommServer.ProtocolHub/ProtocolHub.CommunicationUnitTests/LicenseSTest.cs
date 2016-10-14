﻿//_______________________________________________________________
//  Title   : This is a test class for OTALicenseTest and is intended to contain all OTALicenseTest Unit Tests
//  System  : Microsoft VisualStudio 2015 / C#
//  $LastChangedDate$
//  $Rev$
//  $LastChangedBy$
//  $URL$
//  $Id$
//
//  Copyright (C) 2016, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//_______________________________________________________________

using CAS.CommServer.ProtocolHub.Communication.LicenseControl;
using CAS.CommServer.ProtocolHub.CommunicationUnitTests.Instrumentation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace CAS.CommServer.ProtocolHub.CommunicationUnitTests
{
  [TestClass()]
  public class LicensesTest
  {

    /// <summary>
    ///A test for OTALicense Constructor
    ///</summary>
    [TestMethod()]
    public void OTALicenseConstructorTest()
    {
      OTALicense _ota = OTALicense.License;
      Assert.IsNotNull(_ota);
      Assert.IsTrue(OTALicense.License.Licensed, "License error - see log");
    }
    [TestMethod()]
    [ExpectedException(typeof(System.ComponentModel.LicenseException))]
    public void MultichannelTest()
    {
      Assert.AreEqual<int?>(5, Multichannel.License.Volume);
      Assert.AreEqual<double>(TimeSpan.MaxValue.TotalHours, Multichannel.License.RunTime.Value.TotalHours);
      for (int i = 0; i < 5; i++)
        Multichannel.NextChannnel();
      Multichannel.NextChannnel();
    }
    [TestMethod()]
    [ExpectedException(typeof(System.ComponentModel.LicenseException))]
    public void RedundancyTest()
    {
      Assert.IsTrue(Redundancy.License.Licensed);
      Assert.AreEqual(Redundancy.License.Volume, 2);
      Redundancy.CheckIfAllowed(2, "test name");
    }
    [TestMethod()]
    public void ASALicenseTest()
    {
      ASALicense _ASALicense = new ASALicense();
      Assert.IsTrue(_ASALicense.Licensed);
      Assert.IsNotNull(_ASALicense.Warning);
      Assert.AreEqual<int>(1, _ASALicense.Warning.Count);
      Assert.IsTrue(_ASALicense.Warning[0].Contains("It allows only to be used for non commercial purpose"));
    }
  }
}


