using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using zone320.Common.DataAccess.DTO;

namespace zone320.Recipe.DataAccess.Recipe.DTO
{
    public class RecipeTypeDto : BaseChangeDto
    {
        public Guid RecipeTypeId { get; set; }
        public string Name { get; set; }
    }
}
