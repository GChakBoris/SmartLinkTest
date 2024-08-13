using Moq;
using SmartLink;
namespace SmartLinkTest
{
    [TestClass]
    public class UnitTest1
    {
        /*[TestMethod]
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
        }*/
        [TestMethod]
        [ExpectedException(typeof(Exception), "Location error")]
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
        [ExpectedException(typeof(Exception), "Location error")]
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
        [ExpectedException(typeof(Exception), "Time error")]
        public void TestHandleTimeRecieveError()
        {
            var mock = new Mock<IRabbit>();
            mock.Setup(d => d.Recieve(It.IsAny<string>())).Throws(new Exception() );
            mock.Setup(d => d.Send(It.IsAny<string>(), It.IsAny<string>()));
            var handler = new HandleTime(null, mock.Object);
            handler.information = "aaaa";
            handler.Handle();
        }
        [TestMethod]
        public void TestCreateLocationHandler()
        {
            var mock = new Mock<IHandler>();
            var mock1 = new Mock<IRabbit>();
            var create = Factory.CreateLocationHandler(mock.Object, mock1.Object);
            var locationHandler = new HandleLocation(null, null);
            Assert.AreEqual(create.GetType(), locationHandler.GetType());
            Assert.AreEqual(create._rabbit, mock1.Object);
            Assert.AreEqual(create.handler, mock.Object);
        }
        [TestMethod]
        public void TestCreateSendLink()
        {
            var mock1 = new Mock<IRabbit>();
            var create = Factory.CreateSendLink("1", "2", mock1.Object);
            var locationHandler = new SendLink(null, null, null);
            Assert.AreEqual(create.GetType(), locationHandler.GetType());
            Assert.AreEqual(create._original_information, "2");
            Assert.AreEqual(create._information, "1");
            Assert.AreEqual(create._rabbit, mock1.Object);
        }
        [TestMethod]
        public void TestCreateTimeHandler()
        {
            var mock1 = new Mock<IRabbit>();
            var create = Factory.CreateTimeHandler(mock1.Object);
            var locationHandler = new HandleTime(null, null);
            Assert.AreEqual(create.GetType(), locationHandler.GetType());
            Assert.AreEqual(create._rabbit, mock1.Object);
            Assert.AreEqual(create.handler, null);
        }
        [TestMethod]
        public void TestCreateRabbit()
        {
            var mock = new Mock<IHandler>();
            var mock1 = new Mock<IRabbit>();
            var create = Factory.CreateRabbit();
            var locationHandler = new Rabbit();
            Assert.AreEqual(create.GetType(), locationHandler.GetType());
        }
        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void TestMain()
        {
            Mainer.Main();
        }
    }
}