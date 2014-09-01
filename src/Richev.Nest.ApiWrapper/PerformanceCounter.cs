using System;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace Richev.Nest.ApiWrapper
{
    /// <summary>
    /// Wraps the Win32 high performance counter.
    /// </summary>
    internal class PerformanceCounter
    {
        [DllImport("KERNEL32")]
        private static extern bool QueryPerformanceCounter(out long lpPerformanceCount);

        [DllImport("Kernel32.dll")]
        private static extern bool QueryPerformanceFrequency(out long lpFrequency);

        private long _start;

        private long _stop;

        private readonly long _frequency;

        public PerformanceCounter()
        {
            if (QueryPerformanceFrequency(out _frequency) == false)
            {
                throw new Win32Exception("Frequency not supported.");
            }
        }

        /// <summary>
        /// Starts the counter.
        /// </summary>
        internal void Start()
        {
            QueryPerformanceCounter(out _start);
        }

        /// <summary>
        /// Stops the counter.
        /// </summary>
        internal void Stop()
        {
            QueryPerformanceCounter(out _stop);
        }

        /// <summary>
        /// Returns the time duration between the Start and Stop methods being called.
        /// </summary>
        internal TimeSpan Duration
        {
            get
            {
                return TimeSpan.FromSeconds((double)(_stop - _start) / _frequency);
            }
        }
    }
}
