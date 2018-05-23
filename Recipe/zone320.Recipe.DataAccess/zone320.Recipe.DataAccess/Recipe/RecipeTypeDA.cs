using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using zone320.Recipe.DataAccess.Recipe.DTO;

namespace zone320.Recipe.DataAccess.Recipe
{
    public class RecipeTypeDA : RecipeBaseDA
    {
        public RecipeTypeDto Load(Guid recipeTypeId)
        {
            return this.Query<RecipeTypeDto>("spRecipeTypeGetRecipeType", new
            {
                recipeTypeId
            }).FirstOrDefault();
        }

        public RecipeTypeDto Load(string name)
        {
            return this.Query<RecipeTypeDto>("spRecipeTypeGetRecipeType", new
            {
                name
            }).FirstOrDefault();
        }

        public List<RecipeTypeDto> Load()
        {
            return this.Query<RecipeTypeDto>("spRecipeTypeGetRecipeType", new { }).ToList();
        }
    }
}
