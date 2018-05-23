using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using zone320.Recipe.DataAccess.Recipe.DTO;

namespace zone320.Recipe.DataAccess.Recipe
{
    public class RecipeIngredientDA : RecipeBaseDA
    {
        public RecipeIngredientDto Load(Guid recipeIngredientId)
        {
            return this.Query<RecipeIngredientDto>("spRecipeIngredientGetRecipeIngredient", new
            {
                recipeIngredientId
            }).FirstOrDefault();
        }

        public List<RecipeIngredientDto> LoadByRecipe(Guid recipeId)
        {
            return this.Query<RecipeIngredientDto>("spRecipeIngredientGetRecipeIngredient", new { recipeId }).ToList();
        }

        public List<RecipeIngredientDto> Load()
        {
            return this.Query<RecipeIngredientDto>("spRecipeIngredientGetRecipeIngredient", new { }).ToList();
        }

        public Guid Save(RecipeIngredientDto item, Guid userId)
        {
            item.RecipeIngredientId = this.SetPrimaryKeyId(item.RecipeIngredientId);

            this.Execute("spRecipeIngredientSave", new
            {
                item.RecipeIngredientId,
                item.RecipeId,
                item.Sequence,
                item.Ingredient,
                userId
            });

            return item.RecipeIngredientId;
        }

        public void Delete(Guid recipeIngredientId, Guid userId)
        {
            this.Execute("spRecipeIngredientDelete", new { recipeIngredientId, userId });
        }

        public void DeleteAll(Guid recipeId, Guid userId)
        {
            this.Execute("spRecipeIngredientDelete", new { recipeId, userId });
        }
    }
}
