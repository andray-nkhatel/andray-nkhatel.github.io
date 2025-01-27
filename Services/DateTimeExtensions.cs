namespace Rentbook.Services
{
    public static class DateTimeExtensions
    {
        public static string ToRelativeTime(this DateTime dateTime)
        {
            TimeSpan timeDiff = DateTime.Now - dateTime;

            if (timeDiff.TotalMinutes < 1)
                return "just now";
            if (timeDiff.TotalMinutes < 60)
                return $"{(int)timeDiff.TotalMinutes}m ago";
            if (timeDiff.TotalHours < 24)
                return $"{(int)timeDiff.TotalHours}h ago";
            if (timeDiff.TotalDays < 7)
                return $"{(int)timeDiff.TotalDays}d ago";
            if (timeDiff.TotalDays < 30)
                return $"{(int)(timeDiff.TotalDays / 7)}w ago";
            if (timeDiff.TotalDays < 365)
                return $"{(int)(timeDiff.TotalDays / 30)}mo ago";

            return $"{(int)(timeDiff.TotalDays / 365)}y ago";
        }
    }
}
