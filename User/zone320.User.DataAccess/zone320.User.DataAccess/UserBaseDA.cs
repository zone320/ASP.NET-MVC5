using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using zone320.Common.DataAccess;

namespace zone320.User.DataAccess
{
    public class UserBaseDA : BaseDA
    {
        public UserBaseDA(string connectionString = null)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                //TODO: add to configs
                connectionString = this.GetConnectionStringFromConfiguration("UserDatabase");
            }

            this.SetConnectionString(connectionString);
        }
    }
}
