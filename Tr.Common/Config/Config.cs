/******************************************************************
 *
** 文件名:Config.cs
** Copyright (c) 2007.01-2007 
** 创建人:ShiL
** 日 期:2017/01/06
** 修改人:
** 日 期:
** 描 述:
** 版 本:1.0.0
 *
******************************************************************/

namespace Tr.Common
{
    using System;
    
    /// <summary>
    /// 配置信息
    /// </summary>
    public static class Config
    {
        /// <summary>
        /// 获取key对应的Value
        /// </summary>
        /// <param name="key">key</param>
        /// <returns>return value</returns>
        /// <exception cref="GetValueByConfigException">获取Value失败</exception>
        public static string GetValue(string key)
        {
            try
            {
                string value = string.Empty;
                if (!string.IsNullOrEmpty(key))
                {
                    value = System.Configuration.ConfigurationManager.AppSettings[key];
                }
                return value;
            }
            catch (Exception ex)
            {
                throw new GetValueByConfigException(string.Format("获取key:{0} 失败", key), ex);
            }

        }
    }

    //public static class StringEx
    //{
    //    /// <summary>
    //    /// 转换int
    //    /// </summary>
    //    /// <param name="value">原始值</param>
    //    /// <returns>转换后值</returns>
    //    /// <exception cref="ConvertToIntException"></exception>
    //    public static int ToInt(this string value)
    //    {
    //        try
    //        {
    //            return Convert.ToInt32(value);
    //        }
    //        catch (Exception ex)
    //        {
    //            return 0;
    //        }
    //    }

    //    public static long ToLong(this string value)
    //    {
    //        try
    //        {
    //            return Convert.ToInt64(value);
    //        }
    //        catch (Exception ex)
    //        {
    //            return 0;
    //        }
    //    }
    //}
}
