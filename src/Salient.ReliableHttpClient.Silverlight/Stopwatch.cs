﻿using System;

namespace Salient.ReliableHttpClient
{
    public class Stopwatch
    {
        public static readonly bool IsHighResolution = false;
        public static readonly long Frequency = TimeSpan.TicksPerSecond;

        public long ElapsedTicks { get { return Elapsed.Ticks; } }
        public bool IsRunning { get; private set; }

        private DateTime? StartUtc { get; set; }
        private DateTime? EndUtc { get; set; }

        public TimeSpan Elapsed
        {
            get
            {
                if (!StartUtc.HasValue)
                    throw new InvalidOperationException();

                if (!EndUtc.HasValue)
                    return (DateTime.UtcNow - StartUtc.Value);

                return (EndUtc.Value - StartUtc.Value);
            }
        }

        public long ElapsedMilliseconds
        {
            get
            {
                return ElapsedTicks / TimeSpan.TicksPerMillisecond;
            }
        }

        public static long GetTimestamp()
        {
            return DateTime.UtcNow.Ticks;
        }

        public void Reset()
        {
            Stop();
            EndUtc = null;
            StartUtc = null;
        }

        public void Start()
        {
            if (IsRunning)
                return;

            StartUtc = DateTime.UtcNow;

            IsRunning = true;
            EndUtc = null;
        }

        public void Stop()
        {
            if (IsRunning)
            {
                IsRunning = false;
                EndUtc = DateTime.UtcNow;
            }
        }

        public void Restart()
        {
            Reset();
            Start();
        }

        public static Stopwatch StartNew()
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            return stopwatch;
        }
    }
}
