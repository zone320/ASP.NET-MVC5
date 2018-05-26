using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace zone320.Common.DataAccess.SQL
{
    public static class SqlCommands
    {
        public static string CreateDatabase = "create database ";
        public static string AlterDatabase = "alter database ";
        public static string DropDatabase = "drop database ";

        public static string CreateTable = "create table ";
        public static string AlterTable = "alter table ";
        public static string DropTable = "drop table ";

        public static string CreateConstraint = "create constraint ";
        public static string AlterConstraint = "alter constraint ";
        public static string DropConstraint = "drop constraint ";

        public static string CreateIndex = "create index ";
        public static string AlterIndex = "alter index ";
        public static string DropIndex = "drop index ";

        public static string CreateFunction = "create function ";
        public static string AlterFunction = "alter function ";
        public static string DropFunction = "drop function ";

        public static string CreateProcedure = "create procedure ";
        public static string AlterProcedure = "alter procedure ";
        public static string DropProcedure = "drop procedure ";

        public static string CreateUser = "create user ";
        public static string AlterUser = "alter user ";
        public static string DropUser = "drop user ";

        /// <summary>
        /// SQL commands that should be blocked
        /// </summary>
        public static List<string> BlockedSqlCommands = new List<string>()
        {
            CreateDatabase.ToUpper(),
            AlterDatabase.ToUpper(),
            DropDatabase.ToUpper(),

            CreateTable.ToUpper(),
            AlterTable.ToUpper(),
            DropTable.ToUpper(),

            CreateConstraint.ToUpper(),
            AlterConstraint.ToUpper(),
            DropConstraint.ToUpper(),

            CreateIndex.ToUpper(),
            AlterIndex.ToUpper(),
            DropIndex.ToUpper(),

            CreateFunction.ToUpper(),
            AlterFunction.ToUpper(),
            DropFunction.ToUpper(),

            CreateProcedure.ToUpper(),
            AlterProcedure.ToUpper(),
            DropProcedure.ToUpper(),

            CreateUser.ToUpper(),
            AlterUser.ToUpper(),
            DropUser.ToUpper(),
        };
    }
}
