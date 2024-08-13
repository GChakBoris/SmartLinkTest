using SmartLink;
namespace LocationSend
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
        public void TestResolver12()
        {
            string information = Resolver.Resolve("{location=Russia}");
            Assert.AreEqual("{location=1}", information);
        }
        [TestMethod]
        public void TestResolver13()
        {
            string information = Resolver.Resolve("{location=ops}");
            Assert.AreEqual("{location=2}", information);
        }
        [TestMethod]
        public void TestResolverNichego()
        {
            string information = Resolver.Resolve(null);
            Assert.AreEqual(null, information);
        }
    }
}