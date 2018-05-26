using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using zone320.Recipe.DataAccess.Recipe.DTO;

namespace zone320.Recipe.Service.Interfaces
{
    public interface IRecipeTypeService
    {
        /// <summary>
        /// Gets recipe type
        /// </summary>
        /// <param name="recipeTypeId"></param>
        /// <returns></returns>
        RecipeTypeDto GetRecipeType(Guid recipeTypeId);

        /// <summary>
        /// Gets recipe type
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        RecipeTypeDto GetRecipeType(string name);

        /// <summary>
        /// Gets recipe types
        /// </summary>
        /// <returns></returns>
        List<RecipeTypeDto> GetRecipeTypes();
    }
}
