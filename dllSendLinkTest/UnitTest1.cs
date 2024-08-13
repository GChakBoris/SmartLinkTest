using SmartLink;
namespace dllSendLinkTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestSeparatorZapyataya()
        {
            var mainer = new Mainer();
            int position = mainer.GetSeparator("a,d");
            Assert.AreEqual(1, position);
        }
        [TestMethod]
        public void TestSeparatorProbel()
        {
            var mainer = new Mainer();
            int position = mainer.GetSeparator("a d");
            Assert.AreEqual(1, position);
        }
        [TestMethod]
        public void TestSeparatorSkobka()
        {
            var mainer = new Mainer();
            int position = mainer.GetSeparator("a}d");
            Assert.AreEqual(1, position);
        }
        [TestMethod]
        public void TestSeparatorPustaya()
        {
            var mainer = new Mainer();
            int position = mainer.GetSeparator("");
            Assert.AreEqual(-1, position);
        }
        [TestMethod]
        public void TestSeparatorNichego()
        {
            var mainer = new Mainer();
            int position = mainer.GetSeparator(null);
            Assert.AreEqual(-1, position);
        }
        [TestMethod]
        public void TestGetLink11()
        {
            var mainer = new Mainer();
            string position = mainer.GetLink("{location=1, time=1}");
            Assert.AreEqual("First link", position);
        }
        [TestMethod]
        public void TestGetLink22()
        {
            var mainer = new Mainer();
            string position = mainer.GetLink("{location=2, time=2}");
            Assert.AreEqual("Standart link", position);
        }
    }
}