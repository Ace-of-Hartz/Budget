using System;
using System.Collections.Generic;
using System.Text;

namespace Budget.model
{
    public static class ModelHelper
    {
        private static DateTime EPOCH_START = new DateTime(1970, 1, 1);

        public static long ToEpoch(this DateTime dateTime)
        {
            return (long)(dateTime - EPOCH_START).TotalMilliseconds;
        }

        public static DateTime FromEpoch(this long millisecondsFromEpoch)
        {
            return EPOCH_START.AddMilliseconds(millisecondsFromEpoch);
        }
    }
}
