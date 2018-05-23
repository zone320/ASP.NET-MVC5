using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using zone320.Common.DataAccess.DTO;

namespace zone320.Recipe.DataAccess.Recipe.DTO
{
    public class RecipeDto : BaseCreateDto
    {
        public Guid RecipeId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Guid RecipeTypeId { get; set; }
        public string Author { get; set; }
        public string AuthorWebsite { get; set; }
    }
}
