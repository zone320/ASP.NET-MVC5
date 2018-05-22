using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace zone320.Common.DataAccess.DTO
{
    public class BaseDto
    {
        public BaseDto()
        {
            this.HasData = true;
        }

        public bool HasData { get; set; }
    }
}
