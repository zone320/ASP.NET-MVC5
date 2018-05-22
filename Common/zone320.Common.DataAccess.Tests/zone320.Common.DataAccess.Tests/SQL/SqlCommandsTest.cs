using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using zone320.Common.DataAccess.SQL;
using zone320.Common.Tests;
using FluentAssertions;

namespace zone320.Common.DataAccess.Tests.SQL
{
    [TestClass]
    public class SqlCommandsTest : BaseTest
    {
        [TestMethod]
        public void BlockedSqlCommands()
        {
            var blockedCommands = SqlCommands.BlockedSqlCommands;
            blockedCommands.Should().NotBeNullOrEmpty();

            blockedCommands.Should().Contain(SqlCommands.CreateTable.ToUpper());
            blockedCommands.Should().NotContain("invalid");
        }
    }
}
