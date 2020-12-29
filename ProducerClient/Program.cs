using RabbitMQ.Client;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace ProducerClient
{
    class Program
    {

        public static string ExchangeName = "DirectExchange";

        public static string queueName = "kaolatest";

        //public static string queueName2 = "kaolatesttwo";

        public static string RountingKey = "kaolaRounting";

        static void Main(string[] args)
        {

            var factory = new ConnectionFactory() { HostName = "localhost" };


            using (var connection = factory.CreateConnection())
            {

                using (var channel = connection.CreateModel())
                {

                    //定义交换机的类型
                    channel.ExchangeDeclare(ExchangeName, ExchangeType.Direct, false, autoDelete:true);

                    //var argss = new Dictionary<string,object>
                    //{
                    //   {"x-message-ttl",6000}
                    //};
                    //定义消息队列
                    channel.QueueDeclare(queueName, durable:false, exclusive:false,autoDelete:false/*, arguments: argss*/);

                    //channel.QueueDeclare(queueName2, durable: false, exclusive: false, autoDelete: false);

                    channel.QueueBind(queueName, ExchangeName, RountingKey);

                    
                    //channel.QueueBind(queueName2, ExchangeName, RountingKey);

                    Console.WriteLine($"\n RabbitMQ连接成功，\n\n请输入消息，输入exit退出！");

                    var properties = channel.CreateBasicProperties();
                    //properties.Persistent = true;
                    properties.DeliveryMode = 2;
                    properties.Expiration = "6000";

                    string input;
                    do
                    {
                        input = Console.ReadLine();

                        var sendBytes = Encoding.UTF8.GetBytes(input);
                        //发布消息
                        channel.BasicPublish(ExchangeName, RountingKey, null, sendBytes);

                    } while (input.Trim().ToLower() != "exit");

                    channel.Close();
                    connection.Close();

                }
            }
        }
    }
}
