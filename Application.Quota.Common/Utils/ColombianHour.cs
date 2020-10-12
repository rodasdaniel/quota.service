using System;

namespace Application.Quota.Common.Utils
{
    public class ColombianHour
    {
        public static DateTime GetDate()
        {
            return DateTime.UtcNow.AddHours(-5);
        }
    }
}
