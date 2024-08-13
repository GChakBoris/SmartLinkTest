using Microsoft.VisualBasic;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Reflection;
using System.Text;
namespace SmartLink
{
    public class Mainer
    {
        public static void Main()
        {
            // Принятие данных
            //var reciever = Factory.CreateReceiver();
            //var reciever = new Reciever();
            while (true)
            {
                var rabbit = Factory.CreateRabbit();
                string information = rabbit.Recieve("MainLine");
                if ($"{information}".Length != 0)
                {
                    // Обработка данных
                    //var resolver = Factory.CreateResolver(reciever.information);
                    //var resolver = new Resolver(reciever.information);
                    Task.Run(() =>
                    {
                        var rabbit = Factory.CreateRabbit();
                        try
                        {
                            var timeHandler = Factory.CreateTimeHandler(rabbit);
                            var locationHandler = Factory.CreateLocationHandler(timeHandler, rabbit);
                            locationHandler.information = information;
                            locationHandler.Handle();
                            var linkSender = Factory.CreateSendLink(timeHandler.information, information, rabbit);
                            linkSender.Execute();
                        }
                        catch
                        {
                            var linkSender = Factory.CreateSendLink(information, information, rabbit);
                            linkSender.Execute();
                        }
                    }
                    );
                }
            }
        }
    }
    interface ICommand
    {
        void Execute();
    }
    interface IFactory
    {
        //static abstract Reciever CreateReceiver();
        //static abstract Resolver CreateResolver(string information);
        //static abstract List<T> CreateList<T>();
        //static abstract string CreateString();
        abstract static HandleLocation CreateLocationHandler(IHandler handler, IRabbit rabbit);
        abstract static HandleTime CreateTimeHandler(IRabbit rabbit);
        abstract static SendLink CreateSendLink(string information, string original_information, IRabbit rabbit);
        abstract static IRabbit CreateRabbit();
    }
    public interface IHandler
    {
        IRabbit _rabbit { get; set; }
        string information { get; set; }
        IHandler? handler { get; set; }
        void Handle();
    }
    public class HandleLocation : IHandler
    {
        public IRabbit _rabbit { get; set; }
        public string information { get; set; }
        public IHandler? handler { get; set; }
        public HandleLocation(IHandler _handler, IRabbit rabbit) 
        {
            _rabbit = rabbit;
            handler = _handler;
        }
        public void Handle()
        {
            try
            {
                _rabbit.Send(information, "locationQueue");
                while (true)
                {
                    string changedInformation = _rabbit.Recieve($"{information}Queue");
                    if ($"{changedInformation}" != String.Empty)
                    {
                        handler.information = changedInformation;
                        handler.Handle();
                        break;
                    }
                }
            }
            catch
            {
                throw new Exception("Location error");
            }
        }
    }
    public class HandleTime : IHandler
    {
        public IRabbit _rabbit { get; set; }
        public string information { get; set; }
        public IHandler? handler { get; set; }
        public HandleTime(IHandler? _handler, IRabbit rabbit)
        {
            handler = _handler;
            _rabbit = rabbit;
            //information = _information;
        }
        public void Handle()
        {
            try
            {
                _rabbit.Send(information, "timeQueue");
                while (true)
                {
                    string changedInformation = _rabbit.Recieve($"{information}Queue");
                    if ($"{changedInformation}" != String.Empty)
                    {
                        information = changedInformation;
                        break;
                    }
                }
            }
            catch
            {
                throw new Exception("Time error");
            }
        }
    }
    public class Factory : IFactory
    {
        public static SendLink CreateSendLink(string information, string original_information, IRabbit rabbit)
        {
            return new SendLink(information, original_information, rabbit);
        }
        /*public static Resolver CreateResolver(string information)
        {
            return new Resolver(information);
        }*/
        /*public static List<T> CreateList<T>()
        {
            return new List<T>();
        }*/
        /*public static string CreateString()
        {
            return String.Empty;
        }*/
        public static HandleLocation CreateLocationHandler(IHandler handler, IRabbit rabbit)
        {
            return new HandleLocation(handler, rabbit);
        }
        public static HandleTime CreateTimeHandler(IRabbit rabbit)
        {
            return new HandleTime(null, rabbit);
        }
        public static IRabbit CreateRabbit()
        {
            return new Rabbit();
        }
    }
    /*class IoC
    {
        static Dictionary<string, Dictionary<object[], object>> data = new Dictionary<string, Dictionary<object[], object>>();
        public static T resolve<T>(string key, params object[] args)
        {
            return (T)data[key][args];
        }
        public static void AddResolve(string key, params object[] args)
        {

        }
    }*/
    /*class Reciever : ICommand
    {
        public string information = String.Empty;
        public void Execute()
        {*/
            /*var factory = new ConnectionFactory() { HostName = "host" };// TODO: получить откуда читать сообщения
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare(exchange: "direct_logs", type: ExchangeType.Direct);

                var queueName = channel.QueueDeclare().QueueName;

                channel.QueueBind(queue: queueName, exchange: "direct_logs", routingKey: "info");

                var consumer = new EventingBasicConsumer(channel);

                consumer.Received += (sender, e) =>
                {
                    var body = e.Body;
                    information = Encoding.UTF8.GetString(body.ToArray());
                };
                channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);

            }*/
            /*information = Rabbit.Recieve("MainLine");
        }
    }*/
    /*class Rabbit
    {
        public static string Recieve(string queueName)
        {
            var factory = new ConnectionFactory() { HostName = "host" };// TODO: получить откуда читать сообщения
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare(exchange: "direct_logs", type: ExchangeType.Direct);

                var queueName = channel.QueueDeclare().QueueName;

                channel.QueueBind(queue: queueName, exchange: "direct_logs", routingKey: "info");

                var consumer = new EventingBasicConsumer(channel);

                var information = Factory.CreateString();

                consumer.Received += (sender, e) =>
                {
                    var body = e.Body;
                    information = Encoding.UTF8.GetString(body.ToArray());
                };
                channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);
                return information;
            }
        }
        public static void Send(string message, string queueName)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare(exchange: "headers_ex", type: ExchangeType.Headers);
                Dictionary<string, object> headers = new Dictionary<string, object>();
                headers.Add("currency", "usd");
                headers.Add("transfer", "abroad");
                var properties = channel.CreateBasicProperties();
                properties.Headers = headers;
                //string message = $"{count} message sent";
                var body = Encoding.UTF8.GetBytes(message);
                channel.BasicPublish(exchange: "headers_ex", routingKey: "doesnt metter", basicProperties: properties, body: body);
            }
        }
    }*/
    /*class Resolver : ICommand
    {
        private string _information;
        public Resolver(string information) 
        {
            _information = information;
        }
        public void Execute() 
        {
            //var vars = Factory.CreateList<ICommand>();
            //var vars = new List<ICommand>();
            var timeHandler = Factory.CreateTimeHandler();
            var locationHandler = Factory.CreateLocationHandler(timeHandler);
            locationHandler.information = _information;
            locationHandler.Handle();
            //функция для обработки обработанной информации
            Rabbit.Send("Корректная ссылка", $"{_information}MainQueue");*/
            /*vars.Add(IoC.resolve<ICommand>("Локация", _information));
            foreach (var item in vars)
            {
                item.Execute();
            }
            sender = IoC.resolve<ICommand>("Ответ", vars);*/
        //}
    //}
    public class SendLink : ICommand
    {
        public IRabbit _rabbit;
        public string _information;
        public string _original_information;
        public SendLink(string information, string original_information, IRabbit rabbit)
        {
            _information = information;
            _original_information = original_information;
            _rabbit = rabbit;
        }
        public void Execute()
        {
            try
            {
                Assembly a = Assembly.Load("dllSendLink");
                Object o = a.CreateInstance("SmartLink");
                Type t = a.GetType("Mainer");
                MethodInfo mi = t.GetMethod("GetLink");
                string[] info = new string[1]
                {
                _information
                };
                var link = (string)mi.Invoke(o, info);
                _rabbit.Send(link, $"{_original_information}MainQueue");
            }
            catch
            {
                _rabbit.Send("Стандартная ссылка", $"{_original_information}MainQueue");
            }
        }
    }
}