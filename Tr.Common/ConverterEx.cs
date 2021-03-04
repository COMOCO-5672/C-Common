using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tr.Common
{
   
    public static class TrConverter
    {
        public static string ToString(object value, string defaultValue = "")
        {
            try
            {
                if (value == null) return defaultValue;
   
                return Convert.ToString(value);
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }
    }

    public static class ConverterEx
    {
        /// <summary>
        /// 将string类型转换为int32类型的扩展方法
        /// </summary>
        /// <param name="value">将要转换的值</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>转换失败为默认值</returns>
        public static Int32 ToInt32(this string value, Int32 defaultValue = 0)
        {
            try
            {
                return Convert.ToInt32(value);
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// 将string类型转换为int64类型的扩展方法
        /// </summary>
        /// <param name="value">将要转换的值</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>转换失败为默认值</returns>
        public static long ToLong(this string value, long defaultValue = 0)
        {
            try
            {
                if (string.IsNullOrEmpty(value)) return defaultValue;
                return Convert.ToInt64(value);
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }
    }

    public static class DataFormatConvert {
        public static void bytes2file(byte[] by, string filename)
        {
            FileStream fs = new FileStream(filename, FileMode.Create);
            BinaryWriter bw = new BinaryWriter(fs);
            bw.Write(by);
            bw.Close();
            fs.Close();
        }
    }
}
