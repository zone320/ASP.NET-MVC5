using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using zone320.Common.Utility.DTO;
using zone320.Recipe.DataAccess.Recipe.DTO;

namespace zone320.Recipe.Service.Interfaces
{
    public interface IRecipeInstructionService
    {
        /// <summary>
        /// Gets instruction
        /// </summary>
        /// <param name="recipeInstructionId"></param>
        /// <returns></returns>
        RecipeInstructionDto GetInstruction(Guid recipeInstructionId);

        /// <summary>
        /// Gets all instructions for a recipe
        /// </summary>
        /// <param name="recipeId"></param>
        /// <returns></returns>
        List<RecipeInstructionDto> GetInstructions(Guid recipeId);

        /// <summary>
        /// Gets instructions
        /// </summary>
        /// <returns></returns>
        List<RecipeInstructionDto> GetInstructions();

        /// <summary>
        /// Saves an instruction
        /// </summary>
        /// <param name="saveItem"></param>
        /// <param name="userId"></param>
        ResultDto Save(RecipeInstructionDto saveItem, Guid userId);

        /// <summary>
        /// Deletes an instruction
        /// </summary>
        /// <param name="recipeInstructionId"></param>
        /// <param name="userId"></param>
        ResultDto Delete(Guid recipeInstructionId, Guid userId);

        /// <summary>
        /// Deletes all instructions for a recipe
        /// </summary>
        /// <param name="recipeId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        ResultDto DeleteAll(Guid recipeId, Guid userId);
    }
}
