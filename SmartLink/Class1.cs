using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLink
{
    public interface IRabbit
    {
        public string Recieve(string queueName);
        public void Send(string message, string queueName);
    }
    public class Rabbit : IRabbit
    {
        public string Recieve(string queueName)
        {
            //var factory = new ConnectionFactory() { HostName = "host" };// TODO: получить откуда читать сообщения
            using (var connection = GetRabbitConnection())
            //using (var channel = connection.CreateModel())
            using (var channel = GetRabbitChannel("handling", queueName, "info", connection))
            {
                /*channel.ExchangeDeclare(exchange: "direct_logs", type: ExchangeType.Direct);

                var queueName = channel.QueueDeclare().QueueName;

                channel.QueueBind(queue: queueName, exchange: "direct_logs", routingKey: "info");*/

                var consumer = new EventingBasicConsumer(channel);

                var information = String.Empty;

                consumer.Received += (sender, e) =>
                {
                    var body = e.Body;
                    information = Encoding.UTF8.GetString(body.ToArray());
                };
                channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);
                return information;
            }
        }
        public void Send(string message, string queueName)
        {
            //var factory = new ConnectionFactory() { HostName = "localhost" };
            //using (var connection = factory.CreateConnection())
            using (var connection = GetRabbitConnection())
            //using (var channel = connection.CreateModel())
            using (var channel = GetRabbitChannel("handling", queueName, "info", connection))
            {
                //channel.ExchangeDeclare(exchange: "headers_ex", type: ExchangeType.Headers);
                //Dictionary<string, object> headers = new Dictionary<string, object>();
                //headers.Add("currency", "usd");
                //headers.Add("transfer", "abroad");
                //var properties = channel.CreateBasicProperties();
                //properties.Headers = headers;
                //string message = $"{count} message sent";
                var body = Encoding.UTF8.GetBytes(message);
                channel.BasicPublish(queueName, "info", null, body);
            }
        }
        private static IConnection GetRabbitConnection()
        {
            try
            {
                ConnectionFactory factory = new ConnectionFactory
                {
                    UserName = "guest",
                    Password = "guest",
                    HostName = "localhost"
                };
                IConnection conn = factory.CreateConnection();
                return conn;
            }
            catch
            {
                throw new Exception("Ошибка с получением подключения");
            }
        }
        private static IModel GetRabbitChannel(string exchangeName, string queueName, string routingKey, IConnection conn)
        {
            IModel model = conn.CreateModel();
            model.ExchangeDeclare(exchangeName, ExchangeType.Direct);
            model.QueueDeclare(queueName, false, false, false, null);
            model.QueueBind(queueName, exchangeName, routingKey, null);
            return model;
        }
    }
}
