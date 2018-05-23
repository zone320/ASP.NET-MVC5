using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using zone320.Recipe.DataAccess.Recipe.DTO;

namespace zone320.Recipe.DataAccess.Recipe
{
    public class RecipeDA : RecipeBaseDA
    {
        public RecipeDto Load(Guid recipeId)
        {
            return this.Query<RecipeDto>("spRecipeGetRecipe", new
            {
                recipeId
            }).FirstOrDefault();
        }

        public List<RecipeDto> LoadByType(Guid recipeTypeId)
        {
            return this.Query<RecipeDto>("spRecipeGetRecipe", new { recipeTypeId }).ToList();
        }

        public List<RecipeDto> Load()
        {
            return this.Query<RecipeDto>("spRecipeGetRecipe", new { }).ToList();
        }

        public Guid Save(RecipeDto item, Guid userId)
        {
            item.RecipeId = this.SetPrimaryKeyId(item.RecipeId);

            this.Execute("spRecipeSave", new
            {
                item.RecipeId,
                item.Title,
                item.Description,
                item.RecipeTypeId,
                item.Author,
                item.AuthorWebsite,
                userId
            });

            return item.RecipeId;
        }

        public void Delete(Guid recipeId, Guid userId)
        {
            this.Execute("spRecipeDelete", new { recipeId, userId });
        }
    }
}
