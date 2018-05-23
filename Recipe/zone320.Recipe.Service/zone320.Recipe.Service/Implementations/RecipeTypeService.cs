using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using zone320.Recipe.DataAccess.Recipe;
using zone320.Recipe.DataAccess.Recipe.DTO;
using zone320.Recipe.Service.Interfaces;

namespace zone320.Recipe.Service.Implementations
{
    public class RecipeTypeService : IRecipeTypeService
    {
        private RecipeTypeDA recipeTypeDA;

        public RecipeTypeService(RecipeTypeDA recipeTypeDA)
        {
            this.recipeTypeDA = recipeTypeDA;
        }

        public RecipeTypeDto GetRecipeType(Guid recipeTypeId)
        {
            var result = this.recipeTypeDA.Load(recipeTypeId);
            if (result == null || result.DeleteDate.HasValue)
            {
                result = new RecipeTypeDto() { HasData = false };
            }

            return result;
        }

        public RecipeTypeDto GetRecipeType(string name)
        {
            var result = this.recipeTypeDA.Load(name);
            if (result == null || result.DeleteDate.HasValue)
            {
                result = new RecipeTypeDto() { HasData = false };
            }

            return result;
        }

        public List<RecipeTypeDto> GetRecipeTypes()
        {
            var results = this.recipeTypeDA.Load() ?? new List<RecipeTypeDto>();
            return results.Where(x => !x.DeleteDate.HasValue).OrderBy(x => x.Name).ToList();
        }
    }
}
