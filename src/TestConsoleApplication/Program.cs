using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Salient.ReliableHttpClient.UnitTests;

namespace TestConsoleApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            var abortAfterMs = 500;
            
            Console.WriteLine("Starting...");
			new Thread(() =>
			{
                var client = new AbortingHttpClient();
                client.TriggerAbort(abortAfterMs);
			}).Start();

            Console.WriteLine("Working...");
            Thread.Sleep(abortAfterMs * 10);

            Console.WriteLine("Finished successfully.");
        }
    }

    /* Sample output from command line run:
     * 
 C:\Dev\Salient.ReliableHttpClient\src\TestConsoleApplication\bin\Debug>TestConsoleApplication.exe
Starting...
Working...
Async requesting http://127.0.0.1/
Simulating a timeout occuring...
Aborting request after timeout

Unhandled Exception: System.Net.WebException: Simulated exception from request.A
bort
   at Salient.ReliableHttpClient.UnitTests.AbortingHttpClient.<>c__DisplayClass4
.<TriggerAbort>b__1() in c:\Users\mrdavidlaing\Documents\GitHub\Salient.Reliable
HttpClient\src\Salient.ReliableHttpClient.UnitTests\RequestAbortFixture.cs:line
81
   at System.Threading.ThreadHelper.ThreadStart_Context(Object state)
   at System.Threading.ExecutionContext.RunInternal(ExecutionContext executionCo
ntext, ContextCallback callback, Object state, Boolean preserveSyncCtx)
   at System.Threading.ExecutionContext.Run(ExecutionContext executionContext, C
ontextCallback callback, Object state, Boolean preserveSyncCtx)
   at System.Threading.ExecutionContext.Run(ExecutionContext executionContext, C
ontextCallback callback, Object state)
   at System.Threading.ThreadHelper.ThreadStart()
     * 
     */
}
