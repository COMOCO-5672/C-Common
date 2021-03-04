namespace Tr.Common
{
    using System;


    /// <summary>
    /// 系统时间转换扩展类
    /// </summary>
    public static class DateTimeEx
    {
        /// <summary>
        /// 日期转换成unix时间戳
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static long DateTimeToUnixTimestamp(this DateTime dateTime)
        {
            var start = new DateTime(1970, 1, 1, 0, 0, 0, dateTime.Kind);
            return Convert.ToInt64((dateTime.AddHours(-8) - start).TotalSeconds);
        }

        /// <summary>
        /// unix时间戳转换成日期
        /// </summary>
        /// <param name="unixTimeStamp">时间戳（秒）</param>
        /// <returns>unix时间戳换成日期时间</returns>
        public static DateTime UnixTimestampToDateTime(this long timestamp)
        {
            var start = new DateTime(1970, 1, 1, 0, 0, 0);
            return start.AddSeconds(timestamp).AddHours(8);
        }

        /// <summary>
        /// unix时间戳转换成日期
        /// </summary>
        /// <param name="unixTimeStamp">时间戳（秒）</param>
        /// <returns>unix时间戳换成日期时间</returns>
        public static DateTime ToDateTime(this string dateTimeString)
        {
            try
            {
                DateTime defaultDt = new DateTime(1970, 1, 1);
                if (string.IsNullOrEmpty(dateTimeString)) return defaultDt;

                var dt = Convert.ToDateTime(dateTimeString);
                return dt;
            }
            catch
            {
                return new DateTime(1970, 1, 1);
            }
        }


        /// <summary>
        /// 时间为null时转换
        /// </summary>
        /// <param name="unixTimeStamp">时间戳（秒）</param>
        /// <returns>unix时间戳换成日期时间</returns>
        public static DateTime ToDateTime(this DateTime dateTime, string format = "")
        {
            try
            {
                DateTime defaultDt = Convert.ToDateTime(new DateTime(1970, 1, 1).ToString(format));
                if (dateTime == null) return defaultDt;

                var dt = Convert.ToDateTime(dateTime);
                return dt;
            }
            catch
            {
                return new DateTime(1970, 1, 1);
            }
        }
        public static string TIME_4 = "HHmm";
        public static string TIME_8 = "yyyyMMdd";
        public static string TIME_14 = "yyyyMMddHHmmss";
        public static string TIME_17 = "yyyyMMddHHmmssfff";
        public static string TIME_20 = "yyyyMMddHHmmssffffff";
        public static DateTime strToTime(string datestr, string format)
        {
            try
            {
                DateTime dt = DateTime.ParseExact(datestr, format, System.Globalization.CultureInfo.InvariantCulture);
                return dt;
            }
            catch
            {
                var start = new DateTime(1970, 1, 1, 0, 0, 0);
                return start.AddSeconds(0).AddHours(8);

            }

        }
    }
}
