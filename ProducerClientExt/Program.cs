using RabbitMQ.Client;
using System;
using System.Text;

namespace ProducerClientExt
{
    class Program
    {
        public static string ExchangeName = "DirectExchange";

        //public static string queueName = "kaolatest";

        public static string queueName2 = "kaolatestthree";

        public static string RountingKey = "kaolaRounting";

        static void Main(string[] args)
        {

            var factory = new ConnectionFactory() { HostName = "localhost" };


            using (var connection = factory.CreateConnection())
            {

                using (var channel = connection.CreateModel())
                {

                    //定义交换机的类型
                    channel.ExchangeDeclare(ExchangeName, ExchangeType.Direct, false, autoDelete: true);

                    //定义消息队列
                    //channel.QueueDeclare(queueName, durable: false, exclusive: false, autoDelete: false);

                    channel.QueueDeclare(queueName2, durable: false, exclusive: false, autoDelete: false);

                    //channel.QueueBind(queueName, ExchangeName, RountingKey);

                    channel.QueueBind(queueName2, ExchangeName, RountingKey);


                    Console.WriteLine($"\n RabbitMQ连接成功，\n\n请输入消息，输入exit退出！");

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
