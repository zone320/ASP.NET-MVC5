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
    public class RecipeInstructionService : IRecipeInstructionService
    {
        private readonly RecipeInstructionDA recipeInstructionDA;

        public RecipeInstructionService(RecipeInstructionDA recipeInstructionDA)
        {
            this.recipeInstructionDA = recipeInstructionDA;
        }

        public RecipeInstructionDto GetInstruction(Guid recipeInstructionId)
        {
            var result = this.recipeInstructionDA.Load(recipeInstructionId);
            if (result?.DeleteDate.HasValue != false)
            {
                result = new RecipeInstructionDto() { HasData = false };
            }

            return result;
        }

        public List<RecipeInstructionDto> GetInstructions(Guid recipeId)
        {
            var results = this.recipeInstructionDA.LoadByRecipe(recipeId) ?? new List<RecipeInstructionDto>();
            return results.Where(x => !x.DeleteDate.HasValue).OrderBy(x => x.Sequence).ToList();
        }

        public List<RecipeInstructionDto> GetInstructions()
        {
            var results = this.recipeInstructionDA.Load() ?? new List<RecipeInstructionDto>();
            return results.Where(x => !x.DeleteDate.HasValue).ToList();
        }

        public ResultDto Save(RecipeInstructionDto saveItem, Guid userId)
        {
            var result = this.Validate(saveItem);
            if (result.IsSuccess)
            {
                using (ITransactionScope scope = new TransactionScopeWrapper())
                {
                    result.Result = this.recipeInstructionDA.Save(saveItem, userId);
                    scope.Complete();
                }
            }

            return result;
        }

        /// <summary>
        /// Validates an item
        /// </summary>
        /// <param name="saveItem"></param>
        public ResultDto Validate(RecipeInstructionDto saveItem)
        {
            var errorMessages = new List<string>();
            if (saveItem != null)
            {
                errorMessages.AddRange(Validator.GuidCheck(saveItem.RecipeId, RecipeResources.Recipe, true));
                errorMessages.AddRange(Validator.FieldLengthCheck(saveItem.Sequence, RecipeResources.Sequence, 0, null, true));
                errorMessages.AddRange(Validator.FieldLengthCheck(saveItem.Instruction, RecipeResources.Instruction, 2000, true));
            }
            else
            {
                errorMessages.Add(RecipeResources.RecipeIsRequired);
            }

            return new ResultDto(errorMessages);
        }

        public ResultDto Delete(Guid recipeInstructionId, Guid userId)
        {
            var result = new ResultDto();

            using (ITransactionScope scope = new TransactionScopeWrapper())
            {
                this.recipeInstructionDA.Delete(recipeInstructionId, userId);
                scope.Complete();
            }

            return result;
        }

        public ResultDto DeleteAll(Guid recipeId, Guid userId)
        {
            var result = new ResultDto();

            using (ITransactionScope scope = new TransactionScopeWrapper())
            {
                this.recipeInstructionDA.DeleteAll(recipeId, userId);
                scope.Complete();
            }

            return result;
        }
    }
}
