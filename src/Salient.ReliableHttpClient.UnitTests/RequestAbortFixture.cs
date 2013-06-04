using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
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

    public class AbortingHttpClient
    {
        /// <summary>
        /// <see cref="Salient.ReliableHttpClient.RequestController.ProcessQueue"/>
        /// </summary>
        public void TriggerAbort(int timeoutAfterMs)
        {
            var request = System.Net.WebRequest.Create("http://127.0.0.1");
            request.Timeout = timeoutAfterMs;
            try
            {
                var gate = new ManualResetEvent(false);
                IAsyncResult webRequestAsyncResult = request.BeginGetResponse(ar =>
                {
                    //Log.Info(string.Format("Received #{0} : {1} ", request.Index, request.Uri));
                    Console.WriteLine("Received {0}", request.RequestUri);

                    // let's try to complete the request
                    //_outstandingRequests--;

                    Console.WriteLine("Simulating a timeout occuring...");
                    Thread.Sleep(request.Timeout * 2);

                    gate.Set();

                    try
                    {
                        //request.CompleteRequest(ar);
                        request.EndGetResponse(ar);
                    }
                    catch (Exception ex)
                    {
                        // the only time an exception will come out of CompleteRequest is if the request wants to be retried
//                        request.AttemptedRetries++;
//                        request.Watch.Reset();
//                        request.Watch.Start();
//                        Log.Warn(string.Format("retrying request {3} {0}\r\nattempt #{1}\r\nerror:{2} \r\nrequest:\r\n{4}",
//                            request.Id, request.AttemptedRetries, ex.Message, request.Index, request.ToString()));
//                        // create a new httprequest for this guy
//                        request.BuildRequest(_requestFactory);
//                        //put it back in the queue. if it belongs in the cache it is already there.
//                        RequestQueue.Enqueue(request);
                    }
                }, null);

                new Thread(() =>
                    {
                        if (!gate.WaitOne(request.Timeout))
                        {
//                            // #TODO: disassociate the HttpWebRequest from the result, abort it, complete the result with a timeout exception
//                            Log.Error(string.Format("Aborting #{0} : {1} because it has exceeded timeout {2}",
//                                                    request.Index, request.Request.RequestUri, request.Timeout));
//                            request.Request.Abort();

                            Console.WriteLine("Aborting request after timeout");
                            request.Abort();
                            throw new WebException("Simulated exception from request.Abort");
                        }
                    }).Start();

                // #TODO: timeouts do not apply to async HttpWebRequests so we need to implement this ourselves

                //EnsureRequestWillAbortAfterTimeout(request, webRequestAsyncResult);

//                Log.Info(string.Format("Dispatched #{0} : {1} ", request.Index, request.Uri));
            }
            catch (Exception ex)
            {
//                string message = string.Format("Error dispatching #{0} : {1} \r\n{2} \r\n{3}", request.Index,
//                                               request.Uri, ex.Message, request.ToString());
//                Log.Error(message);
                Console.WriteLine(ex.ToString());

                string message = "Error dispatching";
                ReliableHttpException ex2 = ReliableHttpException.Create(message, ex);
                throw ex2;
            }
            finally
            {
//                RequestQueue.Dequeue();
//                _outstandingRequests++;
                // TODO: should this really be here if there was an error that prevented dispatch?
            }
        }

    }
}



