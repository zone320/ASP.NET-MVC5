using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using zone320.Common.DataAccess.DTO;

namespace zone320.Recipe.DataAccess.Recipe.DTO
{
    public class RecipeInstructionDto : BaseCreateDto
    {
        public Guid RecipeInstructionId { get; set; }
        public Guid RecipeId { get; set; }
        public int Sequence { get; set; }
        public string Instruction { get; set; }
    }
}
