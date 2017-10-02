using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestFx.Testcases
{
    [TestClass]
    public class SampleTest
    {
        [TestMethod]
        public void TestUserName()
        {
            Assert.IsTrue(SignUpDriver.UserName is "MyUserName");
        }

        [TestMethod]
        public void TestPageTitle()
        {
            Assert.IsTrue(SignUpDriver.Title is "MyAppTitle");
        }

        [TestMethod]
        public void TestSignOut()
        {
	bool result = SignUpDriver.SignOut();
            Assert.IsTrue(result is true);
        }
    }
}
