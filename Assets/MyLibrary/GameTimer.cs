using System;

public class GameTime
{
    public static DateTime GetDateTimeCurr()
    {
        DateTime now = DateTime.Now;
        return now;
    }
    public static long GetLongTimeCurr()
    {
        return ((DateTimeOffset)DateTime.Now).ToUnixTimeSeconds();
    }
    public static int GetDaysBetween(DateTime from, DateTime to)
    {
        int fromSeconds = from.Hour * 3600 + from.Minute * 60 + from.Second;
        int toSeconds = to.Hour * 3600 + to.Minute * 60 + to.Second;
        return toSeconds - fromSeconds;
    }
    public static int GetDaysBetween(long startTimestamp, long endTimestamp)
    {
        DateTime startDateTime = DateTimeOffset.FromUnixTimeSeconds(startTimestamp).DateTime.ToLocalTime();
        DateTime endDateTime = DateTimeOffset.FromUnixTimeSeconds(endTimestamp).DateTime.ToLocalTime();
        TimeSpan difference = endDateTime - startDateTime;
        return difference.Days;
    }
    public static long ConvertDateTimeToLong(DateTime dateTime)
    {
        DateTime now = dateTime;
        long unixTimeSeconds = ((DateTimeOffset)dateTime).ToUnixTimeSeconds();
        return unixTimeSeconds;
    }
    public static DateTime ConvertLongToDateTime(long unixTimestamp)
    {
        DateTime dateTimeUtc = DateTimeOffset.FromUnixTimeSeconds(unixTimestamp).DateTime;
        DateTime dateTimeLocal = dateTimeUtc.ToLocalTime();
        return dateTimeLocal;
    }
    public static (int hours, int minutes, int seconds) GetRemainingTimeToday()
    {
        DateTime now = DateTime.Now;

        int remainingHours = 23 - now.Hour;
        int remainingMinutes = 59 - now.Minute;
        int remainingSeconds = 59 - now.Second;

        return (remainingHours, remainingMinutes, remainingSeconds);
    }
}