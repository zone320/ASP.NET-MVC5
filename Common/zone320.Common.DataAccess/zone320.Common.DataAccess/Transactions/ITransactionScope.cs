using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace zone320.Common.DataAccess.Transactions
{
    public interface ITransactionScope : IDisposable
    {
        void Complete();
    }
}
