using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FluentAssertions;
using zone320.Common.Tests;
using zone320.Recipe.DataAccess.Recipe.DTO;
using zone320.Recipe.Service.Implementations;
using zone320.Recipe.DataAccess.Recipe;

namespace zone320.Recipe.Service.Tests
{
    [TestClass]
    public class RecipeBaseTest : BaseTest
    {
        //TODO: switch to dependency injection
        public RecipeIngredientService recipeIngredientService = new RecipeIngredientService(new RecipeIngredientDA());
        public RecipeInstructionService recipeInstructionService = new RecipeInstructionService(new RecipeInstructionDA());
        public RecipeService recipeService = new RecipeService(new RecipeDA(), new RecipeIngredientService(new RecipeIngredientDA()), new RecipeInstructionService(new RecipeInstructionDA()));
        public RecipeTypeService typeService = new RecipeTypeService(new RecipeTypeDA());

        #region Recipe
        public RecipeDto BuildRecipe(Guid? recipeTypeId = null)
        {
            if (!recipeTypeId.HasValue)
            {
                recipeTypeId = this.typeService.GetRecipeTypes().FirstOrDefault()?.RecipeTypeId;
            }

            var item = new RecipeDto
            {
                HasData = true,
                Title = this.GetRandomString(50),
                Description = this.GetRandomString(50),
                RecipeTypeId = recipeTypeId.Value,
                Author = this.GetRandomString(50),
                AuthorWebsite = this.GetRandomString(50)
            };

            return item;
        }

        public RecipeDto CreateRecipe(Guid? recipeTypeId = null)
        {
            var item = this.BuildRecipe(recipeTypeId);
            var result = this.recipeService.Save(item, this.UserId);
            this.AssertSuccess(result);

            item.RecipeId = this.GetId(result);

            return item;
        }
        #endregion

        #region Ingredient
        public RecipeIngredientDto BuildRecipeIngredient(Guid? recipeId = null)
        {
            if (!recipeId.HasValue)
            {
                recipeId = this.CreateRecipe().RecipeId;
            }

            var item = new RecipeIngredientDto
            {
                HasData = true,
                RecipeId = recipeId.Value,
                Sequence = this.GetRandomNumber(),
                Ingredient = this.GetRandomString(250)
            };

            return item;
        }

        public RecipeIngredientDto CreateRecipeIngredient(Guid? recipeId = null)
        {
            var item = this.BuildRecipeIngredient(recipeId);
            var result = this.recipeIngredientService.Save(item, this.UserId);
            this.AssertSuccess(result);

            item.RecipeIngredientId = this.GetId(result);

            return item;
        }
        #endregion

        #region Instruction
        public RecipeInstructionDto BuildRecipeInstruction(Guid? recipeId = null)
        {
            if (!recipeId.HasValue)
            {
                recipeId = this.CreateRecipe().RecipeId;
            }

            var item = new RecipeInstructionDto
            {
                HasData = true,
                RecipeId = recipeId.Value,
                Sequence = this.GetRandomNumber(),
                Instruction = this.GetRandomString(250)
            };

            return item;
        }

        public RecipeInstructionDto CreateRecipeInstruction(Guid? recipeId = null)
        {
            var item = this.BuildRecipeInstruction(recipeId);
            var result = this.recipeInstructionService.Save(item, this.UserId);
            this.AssertSuccess(result);

            item.RecipeInstructionId = this.GetId(result);

            return item;
        }
        #endregion
    }
}
