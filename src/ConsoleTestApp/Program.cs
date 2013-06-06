using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Salient.ReliableHttpClient;
using Salient.ReliableHttpClient.Serialization.Newtonsoft;
using Salient.ReliableHttpClient.UnitTests;


namespace ConsoleTestApp
{
    class Program
    {
        static void Main(string[] args)
        {
          //NB!  DO NOT run this via Visual Studio.  You must compile the app and run it
          //from a command prompt

           SimulateHttpRequestTimeout();
           //SimulateExceptionFromBackgroundThread();
        }

        private static void SimulateExceptionFromBackgroundThread()
        {
            var abortAfterMs = 500;
            var client = new AbortingHttpClient();

            try
            {
                client.TriggerAbortFromBackgroundProcessingThread(abortAfterMs);

                Thread.Sleep(abortAfterMs * 10);
                Console.WriteLine("If exception was caught, should never get here");
            }
            catch (Exception backgroundException)
            {
                Console.WriteLine("Trapped exception from background thread: {0}", backgroundException);
            }

            Console.WriteLine("Successfully trapped background exception");
            Console.WriteLine("\r\nPress enter....");
            Console.ReadLine();
        }

        private static void SimulateHttpRequestTimeout()
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

                                            Console.WriteLine(
                                                "you will not see this line output, exception had already been triggered by .EndRequest()");
                                        }
                                        catch (Salient.ReliableHttpClient.TimeoutException ex)
                                        {
                                            Console.Write("our code properly caught a timeout exception:\r\n" + ex.ToString());
                                        }
                                        catch (Exception ex)
                                        {
                                            Console.Write("our code properly caught an exception:\r\n" + ex.ToString());
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
