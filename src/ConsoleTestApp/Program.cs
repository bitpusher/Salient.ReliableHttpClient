using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Salient.ReliableHttpClient;
using Salient.ReliableHttpClient.Serialization.Newtonsoft;
 

namespace ConsoleTestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var gate = new AutoResetEvent(false);
            var testWeb = "Http://localhost:21266";
            var client = new ClientBase(new Serializer());

            client.BeginRequest(RequestMethod.GET, testWeb, "/DelayHandler.ashx", null, null, ContentType.TEXT, ContentType.TEXT,
                TimeSpan.FromSeconds(1), 3000, 0, ar =>
                {
                    Console.WriteLine("we are in the callback about to end the request");

                    // any time your code utilizes network resources you need to be ready to handle exceptions.

                    try
                    {
                        var response = client.EndRequest(ar);

                        Console.WriteLine("you will not see this line output, exception had already been triggered by .EndRequest()");
                    }
                    catch (Salient.ReliableHttpClient.TimeoutException ex)
                    {

                        Console.Write("our code properly caught a timeout exception:\r\n" + ex.ToString());
                    }

                    gate.Set();

                }, null);


            gate.WaitOne(20000);
            client.Dispose();
            Console.WriteLine("\r\nPress enter....");
            Console.ReadLine();
        }
    }
}
