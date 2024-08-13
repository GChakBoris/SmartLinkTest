using Moq;
using SmartLink;
namespace SmartLinkTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestCheckEmptyLineZero()
        {
            bool check = Mainer.CheckEmptyLine("");
            Assert.IsTrue(check);
        }
        [TestMethod]
        public void TestCheckEmptyLineNull()
        {
            bool check = Mainer.CheckEmptyLine(null);
            Assert.IsTrue(check);
        }
        [TestMethod]
        public void TestCheckEmptyLineString()
        {
            bool check = Mainer.CheckEmptyLine("string");
            Assert.IsFalse(check);
        }
        [TestMethod]
        [ExpectedException(typeof(Exception), "Ошибка с получением локации")]
        public void TestHandleLocationReceiveError()
        {
            var mock = new Mock<IRabbit>();
            mock.Setup(d => d.Recieve(It.IsAny<string>())).Throws(new Exception ());
            mock.Setup(d => d.Send(It.IsAny<string>(), It.IsAny<string>()));
            var mock2 = new Mock<IHandler>();
            mock2.Setup(d => d.Handle());
            var handler = new HandleLocation(mock2.Object, mock.Object);
            handler.information = "aaaa";
            handler.Handle();
        }
        [TestMethod]
        public void TestHandleLocationReceiveSomething()
        {
            var mock = new Mock<IRabbit>();
            mock.Setup(d => d.Recieve(It.IsAny<string>())).Returns("bbbb");
            mock.Setup(d => d.Send(It.IsAny<string>(), It.IsAny<string>()));
            var mock2 = new Mock<IHandler>();
            mock2.Setup(d => d.Handle());
            var handler = new HandleLocation(mock2.Object, mock.Object);
            handler.information = "aaaa";
            handler.Handle();
            Assert.AreEqual("aaaa", handler.information);
        }
        [TestMethod]
        [ExpectedException(typeof(Exception), "Ошибка с получением локации")]
        public void TestHandleLocationHandleError()
        {
            var mock = new Mock<IRabbit>();
            mock.Setup(d => d.Recieve(It.IsAny<string>())).Returns("bbbb");
            mock.Setup(d => d.Send(It.IsAny<string>(), It.IsAny<string>()));
            var mock2 = new Mock<IHandler>();
            mock2.Setup(d => d.Handle()).Throws(new Exception() );
            var handler = new HandleLocation(mock2.Object, mock.Object);
            handler.information = "aaaa";
            handler.Handle();
        }
        [TestMethod]
        public void TestHandleTimeRecieveSomething()
        {
            var mock = new Mock<IRabbit>();
            mock.Setup(d => d.Recieve(It.IsAny<string>())).Returns("bbbb");
            mock.Setup(d => d.Send(It.IsAny<string>(), It.IsAny<string>()));
            var handler = new HandleTime(null, mock.Object);
            handler.information = "aaaa";
            handler.Handle();
            Assert.AreEqual("bbbb", handler.information);
        }
        [TestMethod]
        [ExpectedException(typeof(Exception), "Ошибка с получением времени")]
        public void TestHandleTimeRecieveError()
        {
            var mock = new Mock<IRabbit>();
            mock.Setup(d => d.Recieve(It.IsAny<string>())).Throws(new Exception() );
            mock.Setup(d => d.Send(It.IsAny<string>(), It.IsAny<string>()));
            var handler = new HandleTime(null, mock.Object);
            handler.information = "aaaa";
            handler.Handle();
        }
    }
}