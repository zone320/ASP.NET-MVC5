using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace zone320.Common.DataAccess.DTO
{
    public class BaseChangeDto : BaseDto
    {
        public Guid ChangeUserId { get; set; }
        public DateTime ChangeDate { get; set; }
        public DateTime? DeleteDate { get; set; }
    }
}
