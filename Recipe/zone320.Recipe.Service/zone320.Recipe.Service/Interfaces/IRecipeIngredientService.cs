using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using zone320.Common.Utility.DTO;
using zone320.Recipe.DataAccess.Recipe.DTO;

namespace zone320.Recipe.Service.Interfaces
{
    public interface IRecipeIngredientService
    {
        /// <summary>
        /// Gets ingredient
        /// </summary>
        /// <param name="recipeIngredientId"></param>
        /// <returns></returns>
        RecipeIngredientDto GetIngredient(Guid recipeIngredientId);

        /// <summary>
        /// Gets all ingredients for a recipe
        /// </summary>
        /// <param name="recipeId"></param>
        /// <returns></returns>
        List<RecipeIngredientDto> GetIngredients(Guid recipeId);

        /// <summary>
        /// Gets ingredients
        /// </summary>
        /// <returns></returns>
        List<RecipeIngredientDto> GetIngredients();

        /// <summary>
        /// Saves an ingredient
        /// </summary>
        /// <param name="saveItem"></param>
        /// <param name="userId"></param>
        ResultDto Save(RecipeIngredientDto saveItem, Guid userId);

        /// <summary>
        /// Deletes an ingredient
        /// </summary>
        /// <param name="recipeIngredientId"></param>
        /// <param name="userId"></param>
        ResultDto Delete(Guid recipeIngredientId, Guid userId);

        /// <summary>
        /// Deletes all ingredients for a recipe
        /// </summary>
        /// <param name="recipeId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        ResultDto DeleteAll(Guid recipeId, Guid userId);
    }
}
