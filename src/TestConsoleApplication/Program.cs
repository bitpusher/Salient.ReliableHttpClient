using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace TestConsoleApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting...");
			new Thread(() =>
			{
				throw new Exception();
			}).Start();

            Console.WriteLine("Working for 1 sec...");
            Thread.Sleep(1000);
            Console.WriteLine("Finished successfully.");
        }
    }

    /* Sample output from command line run:
     * 
 C:\Dev\Salient.ReliableHttpClient\src\TestConsoleApplication\bin\Debug>TestConsoleApplication.exe
Starting...
Working for 1 sec...

Unhandled Exception: System.Exception: Exception of type 'System.Exception' was
thrown.
   at TestConsoleApplication.Program.<Main>b__0() in c:\Users\mrdavidlaing\Docum
ents\GitHub\Salient.ReliableHttpClient\src\TestConsoleApplication\Program.cs:lin
e 16
   at System.Threading.ExecutionContext.RunInternal(ExecutionContext executionCo
ntext, ContextCallback callback, Object state, Boolean preserveSyncCtx)
   at System.Threading.ExecutionContext.Run(ExecutionContext executionContext, C
ontextCallback callback, Object state, Boolean preserveSyncCtx)
   at System.Threading.ExecutionContext.Run(ExecutionContext executionContext, C
ontextCallback callback, Object state)
   at System.Threading.ThreadHelper.ThreadStart()
Finished successfully.
     * 
     */
}
