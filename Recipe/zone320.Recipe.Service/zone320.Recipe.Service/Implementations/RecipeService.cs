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
    public class RecipeService : IRecipeService
    {
        private readonly RecipeDA recipeDA;
        private readonly IRecipeIngredientService recipeIngredientService;
        private readonly IRecipeInstructionService recipeInstructionService;

        public RecipeService(RecipeDA recipeDA, IRecipeIngredientService recipeIngredientService, IRecipeInstructionService recipeInstructionService)
        {
            this.recipeDA = recipeDA;
            this.recipeIngredientService = recipeIngredientService;
            this.recipeInstructionService = recipeInstructionService;
        }

        public RecipeDto GetRecipe(Guid recipeId)
        {
            var result = this.recipeDA.Load(recipeId);
            if (result?.DeleteDate.HasValue != false)
            {
                result = new RecipeDto() { HasData = false };
            }

            return result;
        }

        public List<RecipeDto> GetRecipes(Guid recipeTypeId)
        {
            var results = this.recipeDA.LoadByType(recipeTypeId) ?? new List<RecipeDto>();
            return results.Where(x => !x.DeleteDate.HasValue).OrderBy(x => x.Title).ToList();
        }

        public List<RecipeDto> GetRecipes()
        {
            var results = this.recipeDA.Load() ?? new List<RecipeDto>();
            return results.Where(x => !x.DeleteDate.HasValue).OrderBy(x => x.Title).ToList();
        }

        public ResultDto Save(RecipeDto saveItem, Guid userId)
        {
            var result = this.Validate(saveItem);
            if (result.IsSuccess)
            {
                using (ITransactionScope scope = new TransactionScopeWrapper())
                {
                    result.Result = this.recipeDA.Save(saveItem, userId);

                    //TODO: save ingredients and instructions too

                    scope.Complete();
                }
            }

            return result;
        }

        /// <summary>
        /// Validates an item
        /// </summary>
        /// <param name="saveItem"></param>
        public ResultDto Validate(RecipeDto saveItem)
        {
            var errorMessages = new List<string>();
            if (saveItem != null)
            {
                errorMessages.AddRange(Validator.FieldLengthCheck(saveItem.Title, RecipeResources.Title, 250, true));
                errorMessages.AddRange(Validator.FieldLengthCheck(saveItem.Description, RecipeResources.Description, 500, false));
                errorMessages.AddRange(Validator.GuidCheck(saveItem.RecipeTypeId, RecipeResources.RecipeTypeId, true));
                errorMessages.AddRange(Validator.FieldLengthCheck(saveItem.Author, RecipeResources.Author, 250, false));
                errorMessages.AddRange(Validator.FieldLengthCheck(saveItem.AuthorWebsite, RecipeResources.AuthorWebsite, 250, false));
            }
            else
            {
                errorMessages.Add(RecipeResources.RecipeIsRequired);
            }

            return new ResultDto(errorMessages);
        }

        public ResultDto Delete(Guid recipeId, Guid userId)
        {
            var result = new ResultDto();

            using (ITransactionScope scope = new TransactionScopeWrapper())
            {
                this.recipeDA.Delete(recipeId, userId);

                this.recipeIngredientService.DeleteAll(recipeId, userId);
                this.recipeInstructionService.DeleteAll(recipeId, userId);

                scope.Complete();
            }

            return result;
        }
    }
}
