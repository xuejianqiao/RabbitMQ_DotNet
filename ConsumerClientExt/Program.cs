using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace ConsumerClientExt
{
    class Program
    {
        public static string queueName = "kaolatest";
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };

            using (var connection = factory.CreateConnection())
            {

                using (var channel = connection.CreateModel())
                {

                    EventingBasicConsumer consumer = new EventingBasicConsumer(channel);

                    //consumer.Received += TestMethod;
                    //接收到消息事件
                    consumer.Received += (ch, ea) =>
                    {
                       
                        var message = Encoding.UTF8.GetString(ea.Body.ToArray());

                        Console.WriteLine($"Queue:{queueName}收到消息： {message}");
                        //手动确认该消息已被消费
                        //channel.BasicAck(ea.DeliveryTag, false);
                    };
                    //启动消费者 设置为手动应答消息
                    channel.BasicConsume(queueName, true, consumer);

                    Console.WriteLine($"Queue:{queueName}，消费者已启动");

                    Console.ReadKey();
                }

            }
        }



      


    }
}
