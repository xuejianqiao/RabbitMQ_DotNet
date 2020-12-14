using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace ConsumerClient
{
    class Program
    {


        //public static string queueName2 = "kaolatesttwo";
        public static string queueName2 = "kaolatest";

        static void Main(string[] args)
        {

            var factory = new ConnectionFactory() { HostName = "localhost" };

            using (var connection = factory.CreateConnection())
            {

                using (var channel = connection.CreateModel())
                {

                    ////定义交换机的类型
                    //channel.ExchangeDeclare(ExchangeName, ExchangeType.Direct, false, false);

                    //channel.QueueDeclare(queueName);

                    EventingBasicConsumer consumer = new EventingBasicConsumer(channel);

                    //接收到消息事件
                    consumer.Received += (ch, ea) =>
                    {
                        var message = Encoding.UTF8.GetString(ea.Body.ToArray());
                       
                        Console.WriteLine($"Queue:{queueName2}收到消息： {message}");
                        //确认该消息已被消费
                        //channel.BasicAck(ea.DeliveryTag, false);
                    };
                    //启动消费者 设置为手动应答消息
                    channel.BasicConsume(queueName2, true, consumer);
                    Console.WriteLine($"Queue:{queueName2}，消费者已启动");

                    Console.ReadKey();
                }

            }
        }
    }
}
