using System.Net;

namespace Salient.ReliableHttpClient
{
    public interface IRequestRetryDiscriminator
    {
        bool ShouldRetry(WebException webException);
    }
    /// <summary>
    /// #TODO: what happened to usage of this!?
    /// 
    /// 
    /// allow for retries on select exception types
    /// e.g. 50* server errors, timeouts and transport errors
    /// DO NOT RETRY THROTTLE, AUTHENTICATION OR ARGUMENT EXCEPTIONS ETC
    /// </summary>
    public class RequestRetryDiscriminator : IRequestRetryDiscriminator
    {
        public bool ShouldRetry(WebException webException)
        {
            if (webException.Message.ToLower().Contains("(500)")) return true;
            if (webException.Message.ToLower().Contains("(504)")) return true;
            if (webException.Message.ToLower().Contains("(408)")) return true;
            if (webException.Message.ToLower().Contains("(503)")) return true;
            if (webException.Message.ToLower().Contains("aborted"))
            {
                return true;
            }
            return false;
        }
    }
}