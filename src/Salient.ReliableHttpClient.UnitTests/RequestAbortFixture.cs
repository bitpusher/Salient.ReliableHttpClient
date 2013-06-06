using System.Threading;
using NUnit.Framework;

namespace Salient.ReliableHttpClient.UnitTests
{
    [TestFixture]
    public class RequestAbortFixture
    {
        [Test, Ignore("Should only be run manually")]
        public void WhatDoesRequestAbortDo()
        {
            var abortAfterMs = 500;
            var client = new AbortingHttpClient();
            client.TriggerAbort(abortAfterMs);

            Thread.Sleep(abortAfterMs * 10);
        }
    }
}



