namespace AccountService.Utils.Time;

public static class TimeUtils
{
    public static DateTimeOffset GetTicksFromCurrentDate()
    {
        return DateTimeOffset.UtcNow;
    }
}