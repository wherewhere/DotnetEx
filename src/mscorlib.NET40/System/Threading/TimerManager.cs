﻿using System.Collections.Generic;

namespace System.Threading
{
    internal static class TimerManager
    {
        private static readonly Dictionary<Timer, object?> s_rootedTimers = [];

        public static void Add(Timer timer)
        {
            lock (s_rootedTimers)
            {
                s_rootedTimers.Add(timer, null);
            }
        }

        public static void Remove(Timer timer)
        {
            lock (s_rootedTimers)
            {
                s_rootedTimers.Remove(timer);
            }
        }
    }
}
