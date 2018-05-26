using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FluentAssertions;
using zone320.Recipe.Service.Implementations;
using zone320.Recipe.DataAccess.Recipe;
using zone320.Recipe.DataAccess.Recipe.DTO;

namespace zone320.Recipe.Service.Tests.Implementations
{
    [TestClass]
    public class RecipeServiceTest : RecipeBaseTest
    {
        [TestMethod]
        public void GetRecipe()
        {
            //not found
            var item = this.recipeService.GetRecipe(Guid.NewGuid());
            item.Should().NotBeNull();
            item.HasData.Should().BeFalse();

            //found
            item = this.CreateRecipe();
            Guid id = item.RecipeId;

            var dbItem = this.recipeService.GetRecipe(id);
            this.AssertEquality(item, dbItem);
        }

        private void AssertEquality(RecipeDto item1, RecipeDto item2)
        {
            item1.Should().NotBeNull();
            item2.Should().NotBeNull();

            item1.HasData.Should().BeTrue();
            item2.HasData.Should().BeTrue();

            item1.ShouldBeEquivalentTo(item2, options => options.Excluding(x => x.ChangeDate).Excluding(x => x.CreateDate));
        }

        [TestMethod]
        public void GetRecipesByType()
        {
            Guid recipeTypeId = Guid.NewGuid();

            var items = this.recipeService.GetRecipes(recipeTypeId);
            //TODO: AssertEmpty
            items.Should().NotBeNull();
            items.Should().BeEmpty();

            recipeTypeId = this.typeService.GetRecipeTypes().FirstOrDefault()?.RecipeTypeId ?? default(Guid);

            this.CreateRecipe(recipeTypeId);
            this.CreateRecipe(recipeTypeId);
            this.CreateRecipe(recipeTypeId);

            items = this.recipeService.GetRecipes(recipeTypeId);
            items.Should().NotBeNullOrEmpty();
            items.Count.Should().BeGreaterOrEqualTo(3);

            foreach (var item in items)
            {
                this.Assert(item);
                item.RecipeTypeId.Should().Be(recipeTypeId);
            }
        }

        [TestMethod]
        public void GetRecipes()
        {
            this.CreateRecipe();
            this.CreateRecipe();
            this.CreateRecipe();

            var items = this.recipeService.GetRecipes();
            items.Should().NotBeNullOrEmpty();
            items.Count.Should().BeGreaterOrEqualTo(3);

            foreach (var item in items)
            {
                this.Assert(item);
            }
        }

        private void Assert(RecipeDto item)
        {
            item.Should().NotBeNull();
            item.HasData.Should().BeTrue();

            item.RecipeId.Should().NotBeEmpty();
            item.Title.Should().NotBeNullOrWhiteSpace();
            item.RecipeTypeId.Should().NotBeEmpty();
        }

        [TestMethod]
        public void SaveNew()
        {
            var item = this.BuildRecipe();

            var result = this.recipeService.Save(item, this.UserId);
            this.AssertSuccess(result);

            item.RecipeId = this.GetId(result);

            var dbItem = this.recipeService.GetRecipe(item.RecipeId);
            this.AssertEquality(item, dbItem);
        }

        [TestMethod]
        public void SaveExisting()
        {
            var item = this.CreateRecipe();
            item.Title += "updated";
            item.Description += "updated";
            item.RecipeTypeId = this.typeService.GetRecipeTypes().Find(x => x.RecipeTypeId != item.RecipeTypeId).RecipeTypeId;
            item.Author += "updated";
            item.AuthorWebsite += "updated";

            var result = this.recipeService.Save(item, this.UserId);
            this.AssertSuccess(result);

            var dbItem = this.recipeService.GetRecipe(item.RecipeId);
            this.AssertEquality(item, dbItem);
        }

        [TestMethod]
        public void SaveDuplicate()
        {
            var item = this.CreateRecipe();
            item.RecipeId = Guid.Empty;

            var result = this.recipeService.Save(item, this.UserId);
            this.AssertSuccess(result);
        }

        [TestMethod]
        public void Validate()
        {
            RecipeDto item = null;
            this.AssertFailure(this.recipeService.Validate(item));

            item = new RecipeDto();
            this.AssertFailure(this.recipeService.Validate(item));

            item = this.BuildRecipe();
            this.AssertSuccess(this.recipeService.Validate(item));


            item.Title = null;
            this.AssertFailure(this.recipeService.Validate(item));

            item.Title = string.Empty;
            this.AssertFailure(this.recipeService.Validate(item));

            item.Title = " ";
            this.AssertFailure(this.recipeService.Validate(item));

            item.Title = this.GetRandomString(251);
            this.AssertFailure(this.recipeService.Validate(item));

            item.Title = this.GetRandomString(250);
            this.AssertSuccess(this.recipeService.Validate(item));


            item.Description = null;
            this.AssertSuccess(this.recipeService.Validate(item));

            item.Description = string.Empty;
            this.AssertSuccess(this.recipeService.Validate(item));

            item.Description = " ";
            this.AssertSuccess(this.recipeService.Validate(item));

            item.Description = this.GetRandomString(501);
            this.AssertFailure(this.recipeService.Validate(item));

            item.Description = this.GetRandomString(500);
            this.AssertSuccess(this.recipeService.Validate(item));


            item.RecipeTypeId = Guid.Empty;
            this.AssertFailure(this.recipeService.Validate(item));

            item.RecipeTypeId = Guid.NewGuid();
            this.AssertSuccess(this.recipeService.Validate(item));


            item.Author = null;
            this.AssertSuccess(this.recipeService.Validate(item));

            item.Author = string.Empty;
            this.AssertSuccess(this.recipeService.Validate(item));

            item.Author = " ";
            this.AssertSuccess(this.recipeService.Validate(item));

            item.Author = this.GetRandomString(251);
            this.AssertFailure(this.recipeService.Validate(item));

            item.Author = this.GetRandomString(250);
            this.AssertSuccess(this.recipeService.Validate(item));


            item.AuthorWebsite = null;
            this.AssertSuccess(this.recipeService.Validate(item));

            item.AuthorWebsite = string.Empty;
            this.AssertSuccess(this.recipeService.Validate(item));

            item.AuthorWebsite = " ";
            this.AssertSuccess(this.recipeService.Validate(item));

            item.AuthorWebsite = this.GetRandomString(251);
            this.AssertFailure(this.recipeService.Validate(item));

            item.AuthorWebsite = this.GetRandomString(250);
            this.AssertSuccess(this.recipeService.Validate(item));
        }

        [TestMethod]
        public void Delete()
        {
            //not found
            var result = this.recipeService.Delete(Guid.NewGuid(), this.UserId);
            this.AssertSuccess(result);

            //found
            var item = this.CreateRecipe();
            Guid id = item.RecipeId;

            this.CreateRecipeIngredient(id);
            this.CreateRecipeIngredient(id);

            this.CreateRecipeInstruction(id);
            this.CreateRecipeInstruction(id);

            var ingredients = this.recipeIngredientService.GetIngredients(id);
            ingredients.Should().NotBeNullOrEmpty();
            ingredients.Count.Should().Be(2);

            var instructions = this.recipeInstructionService.GetInstructions(id);
            instructions.Should().NotBeNullOrEmpty();
            instructions.Count.Should().Be(2);

            result = this.recipeService.Delete(id, this.UserId);
            this.AssertSuccess(result);

            var dbItem = this.recipeService.GetRecipe(id);
            dbItem.Should().NotBeNull();
            dbItem.HasData.Should().BeFalse();

            ingredients = this.recipeIngredientService.GetIngredients(id);
            ingredients.Should().NotBeNull();
            ingredients.Should().BeEmpty();

            instructions = this.recipeInstructionService.GetInstructions(id);
            instructions.Should().NotBeNull();
            instructions.Should().BeEmpty();

            //save again
            item.RecipeId = Guid.Empty;

            result = this.recipeService.Save(item, this.UserId);
            this.AssertSuccess(result);

            item.RecipeId = this.GetId(result);

            dbItem = this.recipeService.GetRecipe(item.RecipeId);
            this.AssertEquality(item, dbItem);

            ingredients = this.recipeIngredientService.GetIngredients(id);
            ingredients.Should().NotBeNull();
            ingredients.Should().BeEmpty();

            instructions = this.recipeInstructionService.GetInstructions(id);
            instructions.Should().NotBeNull();
            instructions.Should().BeEmpty();
        }
    }
}
