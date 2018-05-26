using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using zone320.Common.Tests;
using FluentAssertions;
using System.Configuration;
using System.Data;
using zone320.Common.DataAccess.SQL;

namespace zone320.Common.DataAccess.Tests
{
    [TestClass]
    public class BaseDATest : BaseTest
    {
        private readonly BaseDA dataAccess = new BaseDA();
        private readonly string ConnectionStringKey = "TestDatabase";

        [TestInitialize]
        public void RepositoryTestInitialize()
        {
            this.dataAccess.SetConnectionString(ConfigurationManager.ConnectionStrings[this.ConnectionStringKey].ConnectionString);
        }

        [TestMethod]
        public void SetConnectionString()
        {
            this.dataAccess.SetConnectionString(null);
            var connection = this.dataAccess.Connection;
            connection.ConnectionString.Should().Be(string.Empty);

            this.dataAccess.SetConnectionString(string.Empty);
            connection = this.dataAccess.Connection;
            connection.ConnectionString.Should().Be(string.Empty);

            var connectionString = ConfigurationManager.ConnectionStrings[this.ConnectionStringKey].ConnectionString;
            this.dataAccess.SetConnectionString(connectionString);

            connection = this.dataAccess.Connection;
            connection.ConnectionString.Should().Be(connectionString);
        }

        [TestMethod]
        public void GetConnectionStringFromConfiguration()
        {
            var connectionStringKey = this.ConnectionStringKey;
            var connectionString = ConfigurationManager.ConnectionStrings[connectionStringKey].ConnectionString;
            this.dataAccess.GetConnectionStringFromConfiguration(connectionStringKey).Should().Be(connectionString);

            this.dataAccess.GetConnectionStringFromConfiguration(null).Should().Be(null);
            this.dataAccess.GetConnectionStringFromConfiguration(string.Empty).Should().Be(null);
            this.dataAccess.GetConnectionStringFromConfiguration("not found").Should().Be(null);
        }

        [TestMethod]
        public void Query()
        {
            var results = this.dataAccess.Query<TestTableDto>(null);
            results.Should().NotBeNull();
            results.Should().BeEmpty();

            results = this.dataAccess.Query<TestTableDto>(string.Empty);
            results.Should().NotBeNull();
            results.Should().BeEmpty();

            this.dataAccess.Invoking(y => y.Query<TestTableDto>("create table test(id int)"))
                .Should().Throw<Exception>()
                .WithMessage("SQL contained blocked command (create table) and was not executed.");
        }

        [TestMethod]
        public void QuerySqlSingle()
        {
            var results = this.dataAccess.Query<TestTableDto>("select * from TestDatabase.dbo.TestTable where TestTableId = @TestTableId", new
            {
                TestTableId = Guid.NewGuid()
            }, commandType: CommandType.Text);

            results.Should().NotBeNull();
            results.Should().BeEmpty();

            var id = this.CreateTestTable();
            results = this.dataAccess.Query<TestTableDto>("select * from TestDatabase.dbo.TestTable where TestTableId = @TestTableId", new
            {
                TestTableId = id
            }, commandType: CommandType.Text);

            results.Should().NotBeNullOrEmpty();
            results.Count().Should().Be(1);

            var first = results.FirstOrDefault();
            this.Assert(first);
            first.TestTableId.Should().Be(id);
        }

        private Guid CreateTestTable()
        {
            var id = Guid.NewGuid();
            this.dataAccess.Execute("spTestTableSave", new
            {
                TestTableId = id,
                TestName = this.GetRandomString(50),
                TestValue = this.GetRandomString(50),
                this.UserId
            });

            var item = this.GetTestTable(id);
            item.TestTableId.Should().Be(id);

            return id;
        }

        private void Assert(TestTableDto item)
        {
            item.Should().NotBeNull();
            item.Should().NotBeNull();
            item.HasData.Should().BeTrue();

            item.TestTableId.Should().NotBeEmpty();
            item.TestName.Should().NotBeNullOrWhiteSpace();
            item.TestValue.Should().NotBeNullOrWhiteSpace();
        }

        [TestMethod]
        public void QuerySqlBlockedSql()
        {
            string name = SqlCommands.CreateTable + this.GetRandomString(20);
            var results = this.dataAccess.Query<TestTableDto>("select * from TestDatabase.dbo.TestTable where TestName = @TestName", new
            {
                TestName = name
            }, commandType: CommandType.Text);

            results.Should().NotBeNull();
            results.Should().BeEmpty();
        }

        [TestMethod]
        public void QuerySqlList()
        {
            this.CreateTestTable();
            this.CreateTestTable();
            this.CreateTestTable();

            var results = this.dataAccess.Query<TestTableDto>("select * from TestDatabase.dbo.TestTable where DeleteDate is null", commandType: CommandType.Text);
            results.Should().NotBeNullOrEmpty();
            results.Count().Should().BeGreaterOrEqualTo(3);

            foreach (var item in results)
            {
                this.Assert(item);
                item.DeleteDate.Should().NotHaveValue();
            }
        }

        [TestMethod]
        public void QueryStoredProcedureSingle()
        {
            var results = this.dataAccess.Query<TestTableDto>("spTestTableGetTestTable", new
            {
                TestTableId = Guid.NewGuid()
            });

            results.Should().NotBeNull();
            results.Should().BeEmpty();

            var id = this.CreateTestTable();
            results = this.dataAccess.Query<TestTableDto>("spTestTableGetTestTable", new
            {
                TestTableId = id
            });

            results.Should().NotBeNullOrEmpty();
            results.Count().Should().Be(1);

            var first = results.FirstOrDefault();
            this.Assert(first);
            first.TestTableId.Should().Be(id);
        }

        [TestMethod]
        public void QueryStoredProcedureList()
        {
            this.CreateTestTable();
            this.CreateTestTable();
            this.CreateTestTable();

            var results = this.dataAccess.Query<TestTableDto>("spTestTableGetTestTable", new { });
            results.Should().NotBeNullOrEmpty();
            results.Count().Should().BeGreaterOrEqualTo(3);

            foreach (var item in results)
            {
                this.Assert(item);
            }
        }

        [TestMethod]
        public void Execute()
        {
            this.dataAccess.Execute(null, commandType: CommandType.Text).Should().Be(0);
            this.dataAccess.Execute(string.Empty, commandType: CommandType.Text).Should().Be(0);

            this.dataAccess.Invoking(y => y.Execute("create table test(id int)", commandType: CommandType.Text))
                .Should().Throw<Exception>()
                .WithMessage("SQL contained blocked command (create table) and was not executed.");
        }

        [TestMethod]
        public void ExecuteInsert()
        {
            var id = Guid.NewGuid();
            var name = this.GetRandomString(50);
            var value = this.GetRandomString(50);

            var recordCount = this.dataAccess.Execute(@"INSERT INTO TestDatabase.dbo.TestTable
			                                               ([TestTableId]
			                                               ,[TestName]
			                                               ,[TestValue]
			                                               ,[CreateUserId]
			                                               ,[CreateDate]
			                                               ,[ChangeUserId]
			                                               ,[ChangeDate]
			                                               ,[DeleteDate])
		                                             VALUES
			                                               (@TestTableId
			                                               ,@TestName
			                                               ,@TestValue
			                                               ,@UserId
			                                               ,sysutcdatetime()
			                                               ,@UserId
			                                               ,sysutcdatetime()
			                                               ,null)", new
            {
                TestTableId = id,
                TestName = name,
                TestValue = value,
                this.UserId
            }, commandType: CommandType.Text);

            recordCount.Should().Be(1);

            var result = this.GetTestTable(id);
            result.TestTableId.Should().Be(id);
            result.TestName.Should().Be(name);
            result.TestValue.Should().Be(value);
        }

        private TestTableDto GetTestTable(Guid id)
        {
            var results = this.dataAccess.Query<TestTableDto>("spTestTableGetTestTable", new
            {
                TestTableId = id
            });

            results.Should().NotBeNullOrEmpty();

            var first = results.FirstOrDefault();
            this.Assert(first);

            return first;
        }

        [TestMethod]
        public void ExecuteUpdate()
        {
            var id = this.CreateTestTable();
            this.ExecuteUpdateAndAssert(id, this.GetRandomString(50));
        }

        private void ExecuteUpdateAndAssert(Guid id, string newName)
        {
            var recordCount = this.dataAccess.Execute("update TestDatabase.dbo.TestTable set TestName = @TestName where TestTableId = @TestTableId", new
            {
                TestTableId = id,
                TestName = newName
            }, commandType: CommandType.Text);

            recordCount.Should().Be(1);

            var dbItem = this.GetTestTable(id);
            dbItem.TestTableId.Should().Be(id);
            dbItem.TestName.Should().Be(newName);
        }

        [TestMethod]
        public void ExecuteUpdateBlockedSql()
        {
            var id = this.CreateTestTable();
            this.ExecuteUpdateAndAssert(id, SqlCommands.CreateTable + this.GetRandomString(20));
        }

        [TestMethod]
        public void ExecuteDelete()
        {
            var id = this.CreateTestTable();
            var recordCount = this.dataAccess.Execute("delete from TestDatabase.dbo.TestTable where TestTableId = @TestTableId", new
            {
                TestTableId = id
            }, commandType: CommandType.Text);

            recordCount.Should().Be(1);

            var results = this.dataAccess.Query<TestTableDto>("spTestTableGetTestTable", new
            {
                TestTableId = id
            });

            results.Should().NotBeNull();
            results.Should().BeEmpty();
        }

        [TestMethod]
        public void CheckForBlockedSqlCommands()
        {
            this.dataAccess.CheckForBlockedSqlCommands(null);
            this.dataAccess.CheckForBlockedSqlCommands(string.Empty);
            this.dataAccess.CheckForBlockedSqlCommands("select * from table");

            this.dataAccess.Invoking(y => y.Execute("create table test(id int)"))
                .Should().Throw<Exception>()
                .WithMessage("SQL contained blocked command (create table) and was not executed.");

            this.dataAccess.Invoking(y => y.Execute("create table test(id int)".ToUpper()))
                .Should().Throw<Exception>()
                .WithMessage("SQL contained blocked command (create table) and was not executed.");
        }

        [TestMethod]
        public void ShouldRetryError()
        {
            this.dataAccess.ShouldRetryError(null).Should().BeFalse();
        }

        [TestMethod]
        public void GetTimeout()
        {
            this.dataAccess.GetTimeout(null).Should().Be(this.dataAccess.DefaultTimeout);
            this.dataAccess.GetTimeout(60).Should().Be(60);
        }

        [TestMethod]
        public void SetPrimaryKeyId()
        {
            var id = Guid.Empty;
            this.dataAccess.SetPrimaryKeyId(id).Should().NotBe(id);

            id = Guid.NewGuid();
            this.dataAccess.SetPrimaryKeyId(id).Should().Be(id);
        }

        [TestMethod]
        public void IsNew()
        {
            this.dataAccess.IsNew(Guid.Empty).Should().BeTrue();
            this.dataAccess.IsNew(Guid.NewGuid()).Should().BeFalse();
        }

        [TestMethod]
        public void HasChanges()
        {
            TestTableDto item1 = null;
            TestTableDto item2 = null;

            this.dataAccess.HasChanges(item1, item2).Should().BeFalse();

            item1 = new TestTableDto();
            this.dataAccess.HasChanges(item1, item2).Should().BeFalse();

            item1 = null;
            item2 = new TestTableDto();
            this.dataAccess.HasChanges(item1, item2).Should().BeFalse();

            item1 = new TestTableDto();
            this.dataAccess.HasChanges(item1, item2).Should().BeFalse();

            item1.TestName = this.GetRandomString(5);
            item2.TestName = null;
            this.dataAccess.HasChanges(item1, item2).Should().BeTrue();

            item1.TestName = null;
            item2.TestName = this.GetRandomString(5);
            this.dataAccess.HasChanges(item1, item2).Should().BeTrue();

            //equal
            item1.TestTableId = Guid.NewGuid();
            item1.TestName = this.GetRandomString(5);
            item1.TestValue = this.GetRandomString(100);

            item2.TestTableId = item1.TestTableId;
            item2.TestName = item1.TestName;
            item2.TestValue = item1.TestValue;

            this.dataAccess.HasChanges(item1, item2).Should().BeFalse();

            //excluded properties are different
            item1.ChangeDate = DateTime.Today;
            item2.ChangeDate = DateTime.Today.AddDays(1);

            this.dataAccess.HasChanges(item1, item2).Should().BeFalse();

            //actually different
            item1.TestTableId = Guid.NewGuid();
            this.dataAccess.HasChanges(item1, item2).Should().BeTrue();

            //exclude properties that differ
            this.dataAccess.HasChanges(item1, item2, new List<string>() { "TestTableId" }).Should().BeFalse();
        }

        [TestMethod]
        public void GetExcludedProperties()
        {
            var results = this.dataAccess.GetExcludedProperties();
            this.Assert(results);

            var excludedProperties = new List<string>();
            results = this.dataAccess.GetExcludedProperties(excludedProperties);
            this.Assert(results);

            excludedProperties = new List<string> { "one", "two", "three", "HasData" };
            results = this.dataAccess.GetExcludedProperties(excludedProperties);
            this.Assert(results);

            foreach (var item in excludedProperties)
            {
                results.Should().Contain(item);
            }
        }

        private void Assert(List<string> results)
        {
            results.Should().NotBeNull();
            results.Should().Contain("HasData");
            results.Should().Contain("CreateDate");
            results.Should().Contain("DeleteDate");
        }
    }
}
