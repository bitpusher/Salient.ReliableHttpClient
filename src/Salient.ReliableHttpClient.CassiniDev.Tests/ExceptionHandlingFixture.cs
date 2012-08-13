using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using CassiniDev;
using NUnit.Framework;
using Salient.ReliableHttpClient.Serialization.Newtonsoft;

namespace Salient.ReliableHttpClient.Tests
{
    /// <summary>
    /// #FIXME: strangely, this test, if run before CanRetryFailedRequests causes CanRetryFailedRequests to fail.  need to figure out exactly why
    /// 
    /// </summary>
    [TestFixture]
    public class ExceptionHandlingFixture
    {
        [Test]
        public void RequestCanTimeout()
        {
            var server = new CassiniDevServer();
            server.StartServer(Environment.CurrentDirectory);

            server.Server.ProcessRequest += (i, e) =>
                                                {
                                                    e.Continue = false;
                                                    Thread.Sleep(TimeSpan.FromSeconds(10));
                                                };

            var client = new ClientBase(new Serializer());
            Exception ex = null;
            try
            {
                client.Request(RequestMethod.GET, server.NormalizeUrl("/"), "", null, null, ContentType.JSON,
                               ContentType.JSON, TimeSpan.Zero, 1000, 0);

            }
            catch (Exception ex2)
            {
                ex = ex2;
            }
            finally
            {
                server.Dispose();
            }

            Assert.IsNotNull(ex, "Expected Exception");
            Assert.IsInstanceOf(typeof(TimeoutException), ex, "expected a TimeoutException");
        }
    }
}
