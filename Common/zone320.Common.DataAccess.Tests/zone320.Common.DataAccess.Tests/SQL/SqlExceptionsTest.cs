using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using zone320.Common.DataAccess.SQL;
using zone320.Common.Tests;
using FluentAssertions;

namespace zone320.Common.DataAccess.Tests.SQL
{
    [TestClass]
    public class SqlExceptionsTest : BaseTest
    {
        [TestMethod]
        public void TransientSqlExceptions()
        {
            var transientExceptions = SqlExceptions.TransientSqlExceptions;
            transientExceptions.Should().NotBeNullOrEmpty();

            transientExceptions.Should().Contain(SqlExceptions.TooManyOperationsInProgress);
            transientExceptions.Should().NotContain(-1);
        }
    }
}
