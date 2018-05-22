using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using zone320.Common.DataAccess.DTO;

namespace zone320.Common.DataAccess.Tests
{
    public class TestTableDto : BaseChangeDto
    {
        public Guid TestTableId { get; set; }
        public string TestName { get; set; }
        public string TestValue { get; set; }
    }
}
