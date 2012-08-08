using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using CassiniDev;
using NUnit.Framework;
using Salient.ReliableHttpClient.ReferenceImplementation;
using Salient.ReflectiveLoggingAdapter;
using Salient.ReliableHttpClient.Serialization.Newtonsoft;

namespace Salient.ReliableHttpClient.Tests
{



    public class SampleClientFixture : CassiniDevServer
    {
        static readonly StringBuilder LogOutput = new StringBuilder();
        public static string GetLogOutput()
        {
            lock (LogOutput)
            {
                var output = LogOutput.ToString();
                LogOutput.Clear();
                return output;
            }
        }
        static SampleClientFixture()
        {
            //Hook up a logger for the CIAPI.CS libraries
            LogManager.CreateInnerLogger = (logName, logLevel, showLevel, showDateTime, showLogName, dateTimeFormat)
                                           =>
            {
                SimpleDebugAppender logger = new SimpleDebugAppender(logName, logLevel, showLevel, showDateTime, showLogName, dateTimeFormat);
                logger.LogEvent += (s, e) =>
                {
                    lock (LogOutput)
                    {
                        LogOutput.AppendLine(string.Format("{0} {1} {2}", e.Level, e.Message, e.Exception));
                    }
                };
                return logger;
            };
        }
        [TestFixtureSetUp]
        public void Setup()
        {
            string location = new ContentLocator(@"Salient.ReliableHttpClient.TestWeb").LocateContent();
            StartServer(location);
        }
        [Test]
        public void CanPurgeAndSHutdown()
        {
            //#FIXME - not a test - just an excercise - need to expose internals so we can peek the request queue

            string root = RootUrl.TrimEnd('/');
            var client = new ClientBase(new Serializer());


            for (int i = 0; i < 50; i++)
            {
                client.BeginRequest(RequestMethod.GET, root, "/SampleClientHandler.ashx?foo={foo}", null, new Dictionary<string, object>() { { "foo", "foo" + i } }, ContentType.TEXT, ContentType.JSON, TimeSpan.FromSeconds(1), 3000, 0, ar => { }, null);
            }

            var handle = client.ShutDown();
            if (!handle.WaitOne(20000))
            {
                throw new Exception("timed out");
            }


        }
        [Test]
        public void CanRetryFailedRequests()
        {

            var gate = new AutoResetEvent(false);

            var client = new SampleClient(RootUrl.TrimEnd('/'));
            Exception exception = null;
            client.BeginGetTestClassWithException(ar =>
            {

                try
                {
                    var result = client.EndGetTestClassWithException(ar);
                }
                catch (Exception ex)
                {

                    exception = ex;

                }
                gate.Set();
            }, null);

            if (!gate.WaitOne(10000))
            {
                throw new Exception("timed out");
            }
            if (exception == null)
            {
                Assert.Fail("was expecting an exception after retrying");
            }
            var logOutput = GetLogOutput();

            Assert.IsTrue(Regex.IsMatch(logOutput, "failed 3 times"));
            Console.WriteLine(exception.ToString());
        }

        [Test]
        public void TestServer()
        {
            var gate = new AutoResetEvent(false);
            Exception exception = null;
            var client = new SampleClient(RootUrl.TrimEnd('/'));
            client.BeginGetTestClass(ar =>
                                         {

                                             try
                                             {
                                                 var result = client.EndGetTestClass(ar);
                                             }
                                             catch (Exception ex)
                                             {
                                                 exception = ex;
                                             }
                                             gate.Set();
                                         }, null);

            if (!gate.WaitOne(10000))
            {
                throw new Exception("timed out");
            }
            if (exception != null)
            {
                throw exception;
            }
        }


    }
}
