using SmartLink;
using System.Reflection.Metadata;
namespace TimeTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestSeparatorZapyataya()
        {
            int position = Separator.GetSeparator("a,d");
            Assert.AreEqual(1, position);
        }
        [TestMethod]
        public void TestSeparatorProbel()
        {
            int position = Separator.GetSeparator("a d");
            Assert.AreEqual(1, position);
        }
        [TestMethod]
        public void TestSeparatorSkobka()
        {
            int position = Separator.GetSeparator("a}d");
            Assert.AreEqual(1, position);
        }
        [TestMethod]
        public void TestSeparatorPustaya()
        {
            int position = Separator.GetSeparator("");
            Assert.AreEqual(-1, position);
        }
        [TestMethod]
        public void TestSeparatorNichego()
        {
            int position = Separator.GetSeparator(null);
            Assert.AreEqual(-1, position);
        }
        [TestMethod]
        public void TestResolverPustaya()
        {
            string information = Resolver.Resolve("");
            Assert.AreEqual("", information);
        }
        [TestMethod]
        public void TestResolverError()
        {
            string information = Resolver.Resolve("time=ops");
            Assert.AreEqual("time=ops", information);
        }
        [TestMethod]
        public void TestResolver12()
        {
            string information = Resolver.Resolve("{time=12:00:00}");
            Assert.AreEqual("{time=2}", information);
        }
        [TestMethod]
        public void TestResolver13()
        {
            string information = Resolver.Resolve("{time=13:00:00}");
            Assert.AreEqual("{time=1}", information);
        }
        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void TestMain()
        {
            Mainer.Main();
        }
    }
}