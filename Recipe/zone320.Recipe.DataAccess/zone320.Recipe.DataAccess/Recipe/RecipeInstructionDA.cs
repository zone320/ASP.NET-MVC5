using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using zone320.Recipe.DataAccess.Recipe.DTO;

namespace zone320.Recipe.DataAccess.Recipe
{
    public class RecipeInstructionDA : RecipeBaseDA
    {
        public RecipeInstructionDto Load(Guid recipeInstructionId)
        {
            return this.Query<RecipeInstructionDto>("spRecipeInstructionGetRecipeInstruction", new
            {
                recipeInstructionId
            }).FirstOrDefault();
        }

        public List<RecipeInstructionDto> LoadByRecipe(Guid recipeId)
        {
            return this.Query<RecipeInstructionDto>("spRecipeInstructionGetRecipeInstruction", new { recipeId }).ToList();
        }

        public List<RecipeInstructionDto> Load()
        {
            return this.Query<RecipeInstructionDto>("spRecipeInstructionGetRecipeInstruction", new { }).ToList();
        }

        public Guid Save(RecipeInstructionDto item, Guid userId)
        {
            item.RecipeInstructionId = this.SetPrimaryKeyId(item.RecipeInstructionId);

            this.Execute("spRecipeInstructionSave", new
            {
                item.RecipeInstructionId,
                item.RecipeId,
                item.Sequence,
                item.Instruction,
                userId
            });

            return item.RecipeInstructionId;
        }

        public void Delete(Guid recipeInstructionId, Guid userId)
        {
            this.Execute("spRecipeInstructionDelete", new { recipeInstructionId, userId });
        }

        public void DeleteAll(Guid recipeId, Guid userId)
        {
            this.Execute("spRecipeInstructionDelete", new { recipeId, userId });
        }
    }
}
