using System;

namespace Salient.ReflectiveLoggingAdapter
{
    internal interface ILog
    {
        void Error(string s, Exception e);
        void Debug(string s);
        void Info(string s);
        void Error(string s);
        void Warn(string s);
        void Warn(string s, Exception e);
        void Error(Exception e);
    }

    public class DebugLogger : ILog
    {
        public void Error(string s, Exception e)
        {
            System.Diagnostics.Debug.WriteLine(string.Format("ERROR: {0}\r\n{1}", s, e.ToString()));
        }

        public void Debug(string s)
        {
            System.Diagnostics.Debug.WriteLine(string.Format("DEBUG: {0}", s));
        }

        public void Info(string s)
        {
            System.Diagnostics.Debug.WriteLine(string.Format("INFO: {0}", s));
        }

        public void Error(string s)
        {
            System.Diagnostics.Debug.WriteLine(string.Format("ERROR: {0}", s));
        }

        public void Warn(string s)
        {
            System.Diagnostics.Debug.WriteLine(string.Format("WARN: {0}", s));
        }

        public void Warn(string s, Exception e)
        {
            System.Diagnostics.Debug.WriteLine(string.Format("WARN: {0}\r\n{1}", s, e.ToString()));
        }

        public void Error(Exception e)
        {
            System.Diagnostics.Debug.WriteLine(string.Format("ERROR: {0}", e.ToString()));
        }
    }
}