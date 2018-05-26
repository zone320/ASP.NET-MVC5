using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace zone320.Common.DataAccess.Transactions
{
    public class TransactionScopeWrapper : ITransactionScope
    {
        private readonly TransactionScope transactionScope;
        private readonly TransactionOptions transactionOptions;

        public TransactionScopeWrapper()
        {
            transactionOptions = new TransactionOptions
            {
                IsolationLevel = IsolationLevel.ReadCommitted
            };
            transactionScope = new TransactionScope(TransactionScopeOption.Required, transactionOptions);
        }

        /// <summary>
        /// Commits transaction
        /// </summary>
        public void Complete()
        {
            transactionScope.Complete();
        }

        /// <summary>
        /// Rolls back transaction
        /// </summary>
        public void Dispose()
        {
            transactionScope.Dispose();
        }
    }
}
