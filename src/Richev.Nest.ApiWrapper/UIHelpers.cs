using System;

namespace Richev.Nest.ApiWrapper
{
    public static class UIHelpers
    {
        public static string DateTimeToRelative(DateTime dateTime)
        {
            var ts = DateTime.Now - dateTime;
            if (ts < new TimeSpan(0, 1, 0))
            {
                return "moments ago";
            }
            if (ts < new TimeSpan(1, 0, 0))
            {
                return string.Format("{0} minute{1} ago", ts.Minutes, ts.Minutes == 1 ? string.Empty : "s" );
            }
            if (ts < new TimeSpan(24, 0, 0))
            {
                return string.Format("{0} hour{1} ago", ts.Hours, ts.Hours == 1 ? string.Empty : "s");
            }
            if (DateTime.Now.Date.AddDays(-1) == dateTime.Date)
            {
                return "yesterday";
            }

            var days = (DateTime.Now.Date - dateTime.Date).Days;

            return string.Format("{0} day{1} ago", days, days == 1 ? string.Empty : "s");
        }
    }
}