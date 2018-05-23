using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using zone320.Common.DataAccess;

namespace zone320.Recipe.DataAccess
{
    public class RecipeBaseDA : BaseDA
    {
        public RecipeBaseDA(string connectionString = null)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                connectionString = this.GetConnectionStringFromConfiguration("RecipeDatabase");
            }

            this.SetConnectionString(connectionString);
        }
    }
}
