using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;

namespace TestWeb
{
    /// <summary>
    /// Summary description for DelayHandler
    /// </summary>
    public class DelayHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            Thread.Sleep(10000);
            context.Response.ContentType = "text/plain";
            context.Response.Write("Hello World");
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}