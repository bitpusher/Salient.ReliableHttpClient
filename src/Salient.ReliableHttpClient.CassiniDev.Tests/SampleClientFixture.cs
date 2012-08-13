﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using CassiniDev;
using NUnit.Framework;
using Salient.ReliableHttpClient.ReferenceImplementation;
using Salient.ReflectiveLoggingAdapter;

namespace Salient.ReliableHttpClient.Tests
{
    [TestFixture]
    public class SampleClientFixture 
    {
         
        //static SampleClientFixture ()
        //{
        //    //Hook up a logger for the CIAPI.CS libraries
        //    LogManager.CreateInnerLogger = (logName, logLevel, showLevel, showDateTime, showLogName, dateTimeFormat)
        //                                   =>
        //    {
        //        SimpleDebugAppender logger = new SimpleDebugAppender(logName, logLevel, showLevel, showDateTime, showLogName, dateTimeFormat);
   
        //        return logger;
        //    };
        //}
        private string ContentLocation
        {
            get
            {
                return new ContentLocator(@"Salient.ReliableHttpClient.TestWeb").LocateContent();
            }
        }
        


        /// <summary>
        /// strange test behavior, when run alone, passes, when in batches fails.
        /// </summary>
        [Test]
        public void CanRetryFailedRequests()
        {
            Thread.Sleep(10000);
            var server = new CassiniDevServer();
            server.StartServer(ContentLocation);

            var gate = new AutoResetEvent(false);

            var client = new SampleClient(server.RootUrl);
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
                finally
                {
                    server.Dispose();
                }
                gate.Set();
            }, null);

            if (!gate.WaitOne(10000))
            {
                throw new Exception("timed out");
            }
            if(exception==null)
            {
                Assert.Fail("was expecting an exception after retrying");
            }
            Console.WriteLine(exception.ToString());
            Assert.IsTrue(Regex.IsMatch(exception.Message, "failed 3 times"));
 
        }

        [Test]
        public void aTestServer()
        {
            var server = new CassiniDevServer();
            server.StartServer(ContentLocation);

            var gate = new AutoResetEvent(false);
            Exception exception = null;
            var client = new SampleClient(server.RootUrl);
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
                                             finally
                                             {
                                                 server.Dispose();
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
