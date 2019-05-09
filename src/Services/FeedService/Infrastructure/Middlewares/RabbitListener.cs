using System;
using System.Text;
using FeedService.Infrastructure.CQRS;
using FlueShared.CQRS;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace FeedService.Infrastructure.Middlewares
{
    public class RabbitListener
    {
        private readonly IMediatorHandler _mediatorHandler;
        ConnectionFactory factory { get; set; }
        IConnection connection { get; set; }
        IModel channel { get; set; }

        public RabbitListener(IMediatorHandler mediatorHandler)
        {
            _mediatorHandler = mediatorHandler;
            this.factory = new ConnectionFactory() { HostName = "localhost" };
            this.connection = factory.CreateConnection();
            this.channel = connection.CreateModel();
        }
            
        public void Register()
        {
            
            channel.QueueDeclare(queue: "rpc_queue", durable: false,
                exclusive: false, autoDelete: false, arguments: null);
            channel.BasicQos(0, 1, false);
            var consumer = new EventingBasicConsumer(channel);
            Console.WriteLine(" [x] Awaiting RPC requests");

            consumer.Received += (model, ea) =>
            {
                string response = null;

                var body = ea.Body;
                var props = ea.BasicProperties;
                var replyProps = channel.CreateBasicProperties();
                replyProps.CorrelationId = props.CorrelationId;

                try
                {
                    var message = Encoding.UTF8.GetString(body);
                    var m = JObject.Parse(message);
                    var t = m["MessageType"].ToString();

                    var tf = Type.GetType(t);
                    
                    
                    var cmdJson = JsonConvert.DeserializeObject(message, tf);
                    
                    
                    _mediatorHandler.SendCommand(cmdJson);
//                    int n = int.Parse(message);
                    Console.WriteLine(" [.] fib({0})", cmdJson);
                    response = cmdJson.ToString();
                }
                catch (Exception e)
                {
                    Console.WriteLine(" [.] " + e.Message);
                    response = "";
                }
                finally
                {
                    var responseBytes = Encoding.UTF8.GetBytes(response);
                    channel.BasicPublish(exchange: "", routingKey: props.ReplyTo,
                        basicProperties: replyProps, body: responseBytes);
                    channel.BasicAck(deliveryTag: ea.DeliveryTag,
                        multiple: false);
                }
            };

            channel.BasicConsume(queue: "rpc_queue",
                autoAck: false, consumer: consumer);
        }

        public void Deregister()
        {
            this.connection.Close();
        }
    }
}