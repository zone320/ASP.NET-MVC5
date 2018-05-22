using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Transactions;
using System.Collections.Generic;
using FluentAssertions;

namespace zone320.Common.Tests
{
    [TestClass]
    public class BaseTest : BaseTestAssert
    {
        private TransactionScope transactionScope;
        private Random random;
        public Guid UserId = Guid.Empty;

        /// <summary>
        /// Initialize new transaction scope for unit tests
        /// </summary>
        [TestInitialize]
        public void TestInitialize()
        {
            var transactionOptions = new TransactionOptions();
            transactionOptions.IsolationLevel = IsolationLevel.ReadCommitted;
            transactionOptions.Timeout = new TimeSpan(0, 5, 0);

            transactionScope = new TransactionScope(TransactionScopeOption.RequiresNew, transactionOptions, TransactionScopeAsyncFlowOption.Enabled);

            random = new Random();
        }

        /// <summary>
        /// Dispose of transaction scope so nothing is committed to the database
        /// </summary>
        [TestCleanup]
        public void TestCleanup()
        {
            transactionScope.Dispose();
        }

        private string RandomStringCharacters = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz ";
        /// <summary>
        /// Gets a random string of an optional length
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public string GetRandomString(int? length = 5)
        {
            string result = string.Empty;
            for (int i = 0; i < length.Value; i++)
            {
                result += this.RandomStringCharacters[random.Next(0, this.RandomStringCharacters.Length)];
            }

            return result.Trim();
        }

        /// <summary>
        /// Gets a random number of optional length
        /// </summary>
        /// <param name="maxValue"></param>
        /// <returns></returns>
        public int GetRandomNumber(int? maxValue = 100000)
        {
            return random.Next(maxValue.Value);
        }
    }
}
