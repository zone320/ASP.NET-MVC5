using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace zone320.Common.DataAccess.DTO
{
    public class BaseCreateDto : BaseChangeDto
    {
        public Guid CreateUserId { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
