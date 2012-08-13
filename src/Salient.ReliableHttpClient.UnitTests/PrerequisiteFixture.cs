using System;
using System.Diagnostics;
using System.Threading;
using NUnit.Framework;

namespace Salient.ReliableHttpClient.UnitTests
{
    [TestFixture]
    public class PrerequisiteFixture
    {
        public class StateObj
        {
            public ManualResetEvent Handle = new ManualResetEvent(false);
            public bool Aborted = false;
        }
        [Test]
        public void TimeoutFunctionality()
        {
            var stateObj = new StateObj();

            ThreadPool.RegisterWaitForSingleObject(stateObj.Handle, (state, isTimedOut) =>
            {

                if (!isTimedOut) return;

                Trace.WriteLine("timeout");

                var rh = (StateObj)state;
                rh.Handle.Set();
                rh.Aborted = true;

            }, stateObj, 1000, true);

            bool aborted = stateObj.Handle.WaitOne();


        }
    }
}