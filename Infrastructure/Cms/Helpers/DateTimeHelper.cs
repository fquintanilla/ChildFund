namespace ChildFund.Infrastructure.Cms.Helpers;

public static class DateTimeHelper
{
    public static TimeZoneInfo GetTimeZone(HttpRequest request)
    {
        var tz = TimeZoneInfo.Utc;
        var cookie = request.Cookies["time_zone"];
        if (cookie == null)
        {
            return tz;
        }

        int.TryParse(cookie, out var offset);
        var allTimeZones = TimeZoneInfo.GetSystemTimeZones();
        var newTimeZone = allTimeZones.FirstOrDefault(x => x.BaseUtcOffset == new TimeSpan(offset, 0, 0));
        tz = newTimeZone;

        return tz;
    }
}