/******************************************************************
 *
** 文件名:ConvertToIntException.cs
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
    /// 转换Int异常
    /// </summary>
    public class ConvertToIntException : Exception
    {
        public ConvertToIntException()
            : base()
        {

        }

        public ConvertToIntException(string message)
            : base(message)
        {

        }

        public ConvertToIntException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
            : base(info, context)
        {

        }
        public ConvertToIntException(string message, Exception innerException)
            : base(message, innerException)
        {

        }
    }
}
