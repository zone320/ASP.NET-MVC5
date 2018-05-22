using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace zone320.Common.DataAccess.DTO
{
    public class DataAccessResultDto
    {
        public object Result { get; set; }
        public int NumberOfAttempts { get; set; }
    }
}
