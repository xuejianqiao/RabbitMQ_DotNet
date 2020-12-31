using System;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

internal class Program
{
    public static async Task Main(string[] args)
    {
        //Console.WriteLine("RPC Client");
        //string n = args.Length > 0 ? args[0] : "30";

        //Task t = InvokeAsync(n);
        //t.Wait();

        //Console.WriteLine(" Press [enter] to exit.");
        //Console.ReadLine();

        var rpcClient = new RpcClient();

        //string input;
        //do
        //{
        //    input = Console.ReadLine();

        //    Console.WriteLine(" [x] Requesting fib({0})", input);

        //    //发布消息
        //    var response = await rpcClient.CallAsync(input);


        //    Console.WriteLine(" [.] Got '{0}'", response);

        //} while (input.Trim().ToLower() != "exit");


        for (var i = 0; i <= 100000; i++)
        {
            var response = await rpcClient.CallAsync("30");

            Console.WriteLine(" [.] Got '{0}'", response);
        }


        // Parallel.For(0, 100000, async (i) =>
        //{
        //    var response = await rpcClient.CallAsync("30");

        //    Console.WriteLine(" [.] Got '{0}'", response);
        //});



        rpcClient.Close();
    }

    //private static async Task InvokeAsync(string n)
    //{
    //    var rpcClient = new RpcClient();

    //    Console.WriteLine(" [x] Requesting fib({0})", n);
    //    var response = await rpcClient.CallAsync(n.ToString());
    //    Console.WriteLine(" [.] Got '{0}'", response);

    //    rpcClient.Close();
    //}
}
