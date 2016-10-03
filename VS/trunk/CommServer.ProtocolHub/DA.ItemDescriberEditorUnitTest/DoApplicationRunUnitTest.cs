using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CAS.CommServer.DA.ItemDescriberEditor.UnitTest
{
  [TestClass]
  public class DoApplicationRunUnitTest
  {
    [TestMethod]
    public void DoApplicationRunTestMethod()
    {
      bool _called = false;
      Program.DoApplicationRun(x => { Assert.IsInstanceOfType(x, typeof(MainFormItemDescriber)); _called = true; });
      Assert.IsTrue(_called);
    }
  }
}
