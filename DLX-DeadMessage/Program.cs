using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace DLX_DeadMessage
{
    class Program
    {

        static void Main(string[] args)
        {

            var factory = new ConnectionFactory() { HostName = "localhost" };


            using (var connection = factory.CreateConnection())
            {

                using (var channel = connection.CreateModel())
                {

                    //定义交换机的类型
                    channel.ExchangeDeclare("exchange.dlx", ExchangeType.Direct, durable:false, autoDelete: true);
                    channel.ExchangeDeclare("exchange.normal", ExchangeType.Direct, durable: false, autoDelete: false);

                    var dic = new Dictionary<string, object>() {

                        { "x-message-ttl",1000 },
                        { "x-dead-letter-exchange","exchange.dlx" },
                        { "x-dead-letter-routing-key","dlxkey" },
                    };
                    //定义消息队列
                    channel.QueueDeclare("queue.normal", durable: true, exclusive: false, autoDelete: false, dic);
                    channel.QueueBind("queue.normal", "exchange.normal", "normalkey");

                    //死信队列
                    channel.QueueDeclare("queue.dlx", durable: true, exclusive: false, autoDelete: false);
                    channel.QueueBind("queue.dlx", "exchange.dlx", "dlxkey");

                    var properties = channel.CreateBasicProperties();
                    properties.Priority = 5;
                    channel.BasicPublish("exchange.normal", "normalkey", properties, Encoding.UTF8.GetBytes("消息") );


                    //properties.DeliveryMode = 2;
                    //properties.Expiration = "6000";

                    Console.WriteLine($"\n RabbitMQ连接成功，\n\n请输入消息，输入exit退出！");

                    string input;
                    do
                    {
                        input = Console.ReadLine();

                        var sendBytes = Encoding.UTF8.GetBytes(input);
                        //发布消息
                        channel.BasicPublish("exchange.normal", "normalkey", null, sendBytes);

                    } while (input.Trim().ToLower() != "exit");

                    channel.Close();
                    connection.Close();


                   

                }
            }
        }
    }
}
