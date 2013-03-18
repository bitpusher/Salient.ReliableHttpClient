using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Salient.ReliableHttpClient.Serialization.Newtonsoft;

namespace Salient.ReliableHttpClient.UnitTests
{
    [TestFixture]
    public class LiveTests
    {
        [Test]
        public void Noid()
        {
            var c = new ClientBase(new Serializer());
            var r = c.Request(RequestMethod.GET, "http://gooogle.com", "", null, null, ContentType.TEXT, ContentType.TEXT, TimeSpan.FromMinutes(1), 10000, 3);
            Assert.IsTrue(!string.IsNullOrEmpty(r));


        }
    }
}
