using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace zone320.Common.DataAccess.SQL
{
    public static class SqlExceptions
    {
        public static int Deadlock = 1205;
        public static int Timeout = -2;
        public static int NoLockDataMovement = 601;
        public static int GeneralNetworkError = 11;
        public static int ConnectionForciblyClosed = 10054;

        public static int CannotOpenDatabase = 4060;
        public static int ErrorProcessingRequest = 40197;
        public static int ServiceCurrentlyBusy = 40501;
        public static int DatabaseNotAvailable = 40613;
        public static int NotEnoughResources = 49918;
        public static int TooManyCreateOrUpdateOperationsInProgress = 49919;
        public static int TooManyOperationsInProgress = 49920;
        public static int LoginFailedDueToLongWait = 4221;

        /// <summary>
        /// Transient SQL exceptions that should be retried
        /// </summary>
        public static List<int> TransientSqlExceptions = new List<int>()
        {
            Deadlock,
            Timeout,
            NoLockDataMovement,
            GeneralNetworkError,
            ConnectionForciblyClosed,

            //Microsoft recommended exceptions
            //https://docs.microsoft.com/en-us/azure/sql-database/sql-database-develop-error-messages#transient-fault-error-codes
            CannotOpenDatabase,
            ErrorProcessingRequest,
            ServiceCurrentlyBusy,
            DatabaseNotAvailable,
            NotEnoughResources,
            TooManyCreateOrUpdateOperationsInProgress,
            TooManyOperationsInProgress,
            LoginFailedDueToLongWait
        };
    }
}
