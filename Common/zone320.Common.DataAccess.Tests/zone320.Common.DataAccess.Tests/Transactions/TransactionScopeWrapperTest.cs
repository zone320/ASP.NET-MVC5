using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using zone320.Common.DataAccess.Transactions;
using zone320.Common.Tests;

namespace zone320.Common.DataAccess.Tests.Transactions
{
    [TestClass]
    public class TransactionScopeWrapperTest : BaseTest
    {
        [TestMethod]
        public void TransactionScopeWrapper()
        {
            var scope = new TransactionScopeWrapper();
            scope.Complete();
            scope.Dispose();
        }
    }
}
