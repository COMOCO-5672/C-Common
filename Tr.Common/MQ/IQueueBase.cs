using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tr.Common.MQ
{
   public interface IQueueBase
    {
        void CreateExchange(bool bCreated = true);
        void CreateQueue(bool bCreated = true);
    }
}
