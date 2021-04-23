using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PostService.MessageQueue
{
    public class RabbitMQHandler
    {
        IConnectionFactory factory;
        IConnection connection;
        IModel channel;
        string _queueName;


        public RabbitMQHandler(string queueName)
        {
            _queueName = queueName;
            SetupMQ();
        }

        private void SetupMQ()
        {
            factory = new ConnectionFactory
            {
                // Username and password are hardcoded
                Uri = new Uri("amqp://guest:freaks@localhost:5672")
            };

            connection = factory.CreateConnection();
            channel = connection.CreateModel();

            channel.QueueDeclare(_queueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);
        }

        public void SendMessage(object message)
        {
            var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));

            channel.BasicPublish("", _queueName, null, body);
        }
    }
}
