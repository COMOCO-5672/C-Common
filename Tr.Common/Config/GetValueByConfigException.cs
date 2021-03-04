/******************************************************************
 *
** 文件名:GetValueByConfigException.cs
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
    /// 删除文件异常
    /// </summary>
    public class GetValueByConfigException : Exception
    {
        public GetValueByConfigException()
            : base()
        {

        }

        public GetValueByConfigException(string message)
            : base(message)
        {

        }

        public GetValueByConfigException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
            : base(info, context)
        {

        }
        public GetValueByConfigException(string message, Exception innerException)
            : base(message, innerException)
        {

        }
    }
}
