using System;

namespace Salient.ReflectiveLoggingAdapter
{
    internal class LogManager
    {
        public static ILog GetLogger(Type type)
        {
            return new DebugLogger();
        }
    }
}