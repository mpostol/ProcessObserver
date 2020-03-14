//___________________________________________________________________________________
//
//  Copyright (C) 2020, Mariusz Postol LODZ POLAND.
//
//  To be in touch join the community at GITTER: https://gitter.im/mpostol/OPC-UA-OOI
//___________________________________________________________________________________

using CAS.CommServer.ProtocolHub.Communication;
using CAS.CommServer.ProtocolHub.MonitorInterface;
using CAS.Lib.CodeProtect;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using System.IO;

namespace CAS.CommServer.ProtocolHub.CommunicationUnitTests
{

  [TestClass()]
  [DeploymentItem("CAS.License.lic")]
  [DeploymentItem("DefaultConfig.xml")]
  public class AssemblyInitializeClass
  {
    internal static CommServerComponent CommServerComponent { get; private set; }
    private static string m_InstallLicenseError = string.Empty;

    [AssemblyInitialize()]
    public static void InstallLicense(TestContext testContext)
    {
      try
      {
        LibInstaller.InstallLicense(false);
        CommServerComponent = new CommServerComponent();
        SettingsBaseFixture _settings = new SettingsBaseFixture();
        CommServerComponent.Initialize("DefaultConfig.xml", _settings);
      }
      catch (System.Exception _ex)
      {
        m_InstallLicenseError = _ex.ToString();
        Debug.WriteLine(m_InstallLicenseError);
        throw;
      }
    }
    [AssemblyCleanup()]
    public static void MyClassTestCleanup()
    {
      CommServerComponent.Dispose();
    }
    [TestMethod]
    public void LicenseExist()
    {
      FileInfo _licenseFile = new FileInfo("DefaultConfig.xml");
      Assert.IsTrue(_licenseFile.Exists, "DefaultConfig.xml doesn't exist in the working directory");
    }
    [TestMethod]
    public void AssemblyInitializeTestMethod()
    {
      Assert.IsTrue(string.IsNullOrEmpty(m_InstallLicenseError), m_InstallLicenseError);
    }
    private class SettingsBaseFixture : ISettingsBase
    {
      public object this[string propertyName] { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    }

  }
}
