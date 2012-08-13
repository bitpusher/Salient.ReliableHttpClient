using System;
using System.Net;
using Salient.ReflectiveLoggingAdapter;

namespace Salient.ReliableHttpClient
{
    /// <summary>
    /// Default RequestFactory. Returns instances of <see cref="WebRequest"/>.
    /// </summary>
    public class RequestFactory : IRequestFactory
    {
        private const int DefaultRequestTimeoutInSec = 30;
#pragma warning disable 169
        private static readonly ILog Log = LogManager.GetLogger(typeof(RequestFactory));
#pragma warning restore 169

        /// <summary>
        /// The amount of time to wait for a response before throwing an exception.
        /// Defaults to 30 seconds
        /// </summary>
        public TimeSpan RequestTimeout { get; set; }

        ///<summary>
        ///</summary>
        public RequestFactory()
        {
            RequestTimeout = TimeSpan.FromSeconds(DefaultRequestTimeoutInSec);

#if !SILVERLIGHT
            DecompressionMethods = DecompressionMethods.GZip | DecompressionMethods.Deflate;
#endif
        }

#if !SILVERLIGHT
        ///<summary>
        ///</summary>
        public DecompressionMethods DecompressionMethods { get; set; }
#endif

        ///<summary>
        /// Returns instances of <see cref="WebRequest"/>.
        ///</summary>
        ///<param name="uri"></param>
        ///<returns></returns>
        public WebRequest Create(string uri)
        {
            var request = (HttpWebRequest)WebRequest.Create(uri);

            // TIMEOUT: all requests in ReliableHttpClient are Async - timeout does not work for async requests. \
            // we have to accomplish this out of band
#if !SILVERLIGHT
            //FIXME: Need a way to timeout requests 
#else
            //FIXME: Need a way to timeout requests in silverlight
#endif


#if !SILVERLIGHT
            request.AutomaticDecompression = DecompressionMethods;
#endif


            return request;
        }
    }
}