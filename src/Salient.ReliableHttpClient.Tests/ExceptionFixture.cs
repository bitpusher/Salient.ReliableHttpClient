using System;
using System.Net;
using CassiniDev;
using NUnit.Framework;

namespace Salient.ReliableHttpClient.Tests
{
    [TestFixture]
    public class ExceptionFixture
    {
        [Test]
        public void EnsureReliableHttpExceptionContainsStackTrace()
        {
            ReliableHttpException ex = null;
            try
            {
                var ee = new ArgumentException("red is blue");
                throw ReliableHttpException.Create(ee);
            }
            catch (ReliableHttpException e)
            {

                ex = e;
            }

            var ts = ex.ToString();
            Assert.IsTrue(ts.Contains("at Salient.ReliableHttpClient.Tests.ExceptionFixture.EnsureReliableHttpExceptionContainsStackTrace"));
        }

    }
}
