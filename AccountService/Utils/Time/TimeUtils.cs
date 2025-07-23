namespace AccountService.Utils.Time;

public static class TimeUtils
{ 
    public static long GetTicksFromCurrentDate()
    {
        return DateTime.UtcNow.Ticks;
    }
}