using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
 
using NUnit.Framework;
using Salient.ReliableHttpClient.ReferenceImplementation;
using Salient.ReliableHttpClient.Serialization.Newtonsoft;


namespace Salient.ReliableHttpClient.UnitTests
{
    //[TestFixture]
    //public class TestRequestFactoryFixture
    //{
  
    //    [Test]
    //    public void Test()
    //    {
    //        var server = new CassiniDevServer();

    //        server.StartServer(Environment.CurrentDirectory);
    //        server.Server.ProcessRequest += (i, e) =>
    //        {
             

    //            e.Continue = false;
    //            e.Response = "{\"Id\":1}";
    //        };


    //        var client = new SampleClient(server.RootUrl);

    //        var gate = new AutoResetEvent(false);
    //        TestClass response = null;
    //        Exception exception = null;



    //        client.BeginGetTestClass(ar =>
    //                                     {

    //                                         try
    //                                         {
    //                                             response = client.EndGetTestClass(ar);
    //                                         }
    //                                         catch (Exception ex)
    //                                         {

    //                                             exception = ex;
    //                                         }
    //                                         finally
    //                                         {
    //                                             server.Dispose();
    //                                             gate.Set();

    //                                         }

    //                                     }, null);

    //        if (!gate.WaitOne(40000))
    //        {
    //            throw new Exception("timed out");
    //        }

    //        if (exception != null)
    //        {
    //            throw exception;
    //        }
    //        Assert.AreEqual(1, response.Id);

    //    }

    //}
}
