using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using zone320.Common.DataAccess.Transactions;
using zone320.Common.Utility.DTO;
using zone320.Common.Utility.Validation;
using zone320.Recipe.DataAccess.Recipe;
using zone320.Recipe.DataAccess.Recipe.DTO;
using zone320.Recipe.Service.Interfaces;
using zone320.Recipe.Service.Resources;

namespace zone320.Recipe.Service.Implementations
{
    public class RecipeIngredientService : IRecipeIngredientService
    {
        private readonly RecipeIngredientDA recipeIngredientDA;

        public RecipeIngredientService(RecipeIngredientDA recipeIngredientDA)
        {
            this.recipeIngredientDA = recipeIngredientDA;
        }

        public RecipeIngredientDto GetIngredient(Guid recipeIngredientId)
        {
            var result = this.recipeIngredientDA.Load(recipeIngredientId);
            if (result?.DeleteDate.HasValue != false)
            {
                result = new RecipeIngredientDto() { HasData = false };
            }

            return result;
        }

        public List<RecipeIngredientDto> GetIngredients(Guid recipeId)
        {
            var results = this.recipeIngredientDA.LoadByRecipe(recipeId) ?? new List<RecipeIngredientDto>();
            return results.Where(x => !x.DeleteDate.HasValue).OrderBy(x => x.Sequence).ToList();
        }

        public List<RecipeIngredientDto> GetIngredients()
        {
            var results = this.recipeIngredientDA.Load() ?? new List<RecipeIngredientDto>();
            return results.Where(x => !x.DeleteDate.HasValue).ToList();
        }

        public ResultDto Save(RecipeIngredientDto saveItem, Guid userId)
        {
            var result = this.Validate(saveItem);
            if (result.IsSuccess)
            {
                using (ITransactionScope scope = new TransactionScopeWrapper())
                {
                    result.Result = this.recipeIngredientDA.Save(saveItem, userId);
                    scope.Complete();
                }
            }

            return result;
        }

        /// <summary>
        /// Validates an item
        /// </summary>
        /// <param name="saveItem"></param>
        public ResultDto Validate(RecipeIngredientDto saveItem)
        {
            var errorMessages = new List<string>();
            if (saveItem != null)
            {
                errorMessages.AddRange(Validator.GuidCheck(saveItem.RecipeId, RecipeResources.Recipe, true));
                errorMessages.AddRange(Validator.FieldLengthCheck(saveItem.Sequence, RecipeResources.Sequence, 0, null, true));
                errorMessages.AddRange(Validator.FieldLengthCheck(saveItem.Ingredient, RecipeResources.Ingredient, 2000, true));
            }
            else
            {
                errorMessages.Add(RecipeResources.RecipeIsRequired);
            }

            return new ResultDto(errorMessages);
        }

        public ResultDto Delete(Guid recipeIngredientId, Guid userId)
        {
            var result = new ResultDto();

            using (ITransactionScope scope = new TransactionScopeWrapper())
            {
                this.recipeIngredientDA.Delete(recipeIngredientId, userId);
                scope.Complete();
            }

            return result;
        }

        public ResultDto DeleteAll(Guid recipeId, Guid userId)
        {
            var result = new ResultDto();

            using (ITransactionScope scope = new TransactionScopeWrapper())
            {
                this.recipeIngredientDA.DeleteAll(recipeId, userId);
                scope.Complete();
            }

            return result;
        }
    }
}
