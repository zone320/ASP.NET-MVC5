using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using zone320.Common.Utility.DTO;
using zone320.Recipe.DataAccess.Recipe.DTO;

namespace zone320.Recipe.Service.Interfaces
{
    //TODO: add services for recipe favorites and comments
    public interface IRecipeService
    {
        /// <summary>
        /// Gets recipe
        /// </summary>
        /// <param name="recipeId"></param>
        /// <returns></returns>
        RecipeDto GetRecipe(Guid recipeId);

        /// <summary>
        /// Gets recipes by type
        /// </summary>
        /// <param name="recipeTypeId"></param>
        /// <returns></returns>
        List<RecipeDto> GetRecipes(Guid recipeTypeId);

        /// <summary>
        /// Gets recipes
        /// </summary>
        /// <returns></returns>
        List<RecipeDto> GetRecipes();

        /// <summary>
        /// Saves a recipe
        /// </summary>
        /// <param name="saveItem"></param>
        /// <param name="userId"></param>
        ResultDto Save(RecipeDto saveItem, Guid userId);

        /// <summary>
        /// Deletes a recipe
        /// </summary>
        /// <param name="recipeId"></param>
        /// <param name="userId"></param>
        ResultDto Delete(Guid recipeId, Guid userId);
    }
}
