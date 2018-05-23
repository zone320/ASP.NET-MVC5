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
    public class RecipeIngredientServiceTest : RecipeBaseTest
    {
        [TestMethod]
        public void GetIngredient()
        {
            //not found
            var item = this.recipeIngredientService.GetIngredient(Guid.NewGuid());
            item.Should().NotBeNull();
            item.HasData.Should().BeFalse();

            //found
            item = this.CreateRecipeIngredient();
            Guid id = item.RecipeIngredientId;

            var dbItem = this.recipeIngredientService.GetIngredient(id);
            this.AssertEquality(item, dbItem);
        }

        private void AssertEquality(RecipeIngredientDto item1, RecipeIngredientDto item2)
        {
            item1.Should().NotBeNull();
            item2.Should().NotBeNull();

            item1.HasData.Should().BeTrue();
            item2.HasData.Should().BeTrue();

            item1.ShouldBeEquivalentTo(item2, options => options.Excluding(x => x.ChangeDate).Excluding(x => x.CreateDate));
        }

        [TestMethod]
        public void GetIngredientsByRecipe()
        {
            Guid recipeId = Guid.NewGuid();

            var items = this.recipeIngredientService.GetIngredients(recipeId);
            //TODO: AssertEmpty
            items.Should().NotBeNull();
            items.Should().BeEmpty();

            recipeId = this.CreateRecipe().RecipeId;

            this.CreateRecipeIngredient(recipeId);
            this.CreateRecipeIngredient(recipeId);
            this.CreateRecipeIngredient(recipeId);

            items = this.recipeIngredientService.GetIngredients(recipeId);
            items.Should().NotBeNullOrEmpty();
            items.Count.Should().Be(3);

            foreach (var item in items)
            {
                this.Assert(item);
                item.RecipeId.Should().Be(recipeId);
            }
        }

        [TestMethod]
        public void GetIngredients()
        {
            var recipeId = this.CreateRecipe().RecipeId;

            this.CreateRecipeIngredient(recipeId);
            this.CreateRecipeIngredient();
            this.CreateRecipeIngredient();

            var items = this.recipeIngredientService.GetIngredients();
            items.Should().NotBeNullOrEmpty();
            items.Count.Should().BeGreaterOrEqualTo(3);

            foreach (var item in items)
            {
                this.Assert(item);
            }
        }

        private void Assert(RecipeIngredientDto item)
        {
            item.Should().NotBeNull();
            item.HasData.Should().BeTrue();

            item.RecipeIngredientId.Should().NotBeEmpty();
            item.RecipeId.Should().NotBeEmpty();
            item.Sequence.Should().BeGreaterThan(0);
            item.Ingredient.Should().NotBeNullOrWhiteSpace();
        }

        [TestMethod]
        public void SaveNew()
        {
            var item = this.BuildRecipeIngredient();

            var result = this.recipeIngredientService.Save(item, this.UserId);
            this.AssertSuccess(result);

            item.RecipeIngredientId = this.GetId(result);

            var dbItem = this.recipeIngredientService.GetIngredient(item.RecipeIngredientId);
            this.AssertEquality(item, dbItem);
        }

        [TestMethod]
        public void SaveExisting()
        {
            var item = this.CreateRecipeIngredient();
            item.Sequence++;
            item.Ingredient += "updated";

            var result = this.recipeIngredientService.Save(item, this.UserId);
            this.AssertSuccess(result);

            var dbItem = this.recipeIngredientService.GetIngredient(item.RecipeIngredientId);
            this.AssertEquality(item, dbItem);
        }

        [TestMethod]
        public void SaveDuplicate()
        {
            var item = this.CreateRecipeIngredient();
            item.RecipeIngredientId = Guid.Empty;

            var result = this.recipeIngredientService.Save(item, this.UserId);
            this.AssertSuccess(result);
        }

        [TestMethod]
        public void Validate()
        {
            RecipeIngredientDto item = null;
            this.AssertFailure(this.recipeIngredientService.Validate(item));

            item = new RecipeIngredientDto();
            this.AssertFailure(this.recipeIngredientService.Validate(item));

            item = this.BuildRecipeIngredient();
            this.AssertSuccess(this.recipeIngredientService.Validate(item));


            item.RecipeId = Guid.Empty;
            this.AssertFailure(this.recipeIngredientService.Validate(item));

            item.RecipeId = Guid.NewGuid();
            this.AssertSuccess(this.recipeIngredientService.Validate(item));


            item.Sequence = -1;
            this.AssertFailure(this.recipeIngredientService.Validate(item));

            item.Sequence = 0;
            this.AssertSuccess(this.recipeIngredientService.Validate(item));

            item.Sequence = 1000;
            this.AssertSuccess(this.recipeIngredientService.Validate(item));


            item.Ingredient = null;
            this.AssertFailure(this.recipeIngredientService.Validate(item));

            item.Ingredient = string.Empty;
            this.AssertFailure(this.recipeIngredientService.Validate(item));

            item.Ingredient = " ";
            this.AssertFailure(this.recipeIngredientService.Validate(item));

            item.Ingredient = this.GetRandomString(2001);
            this.AssertFailure(this.recipeIngredientService.Validate(item));

            item.Ingredient = this.GetRandomString(2000);
            this.AssertSuccess(this.recipeIngredientService.Validate(item));
        }

        [TestMethod]
        public void Delete()
        {
            //not found
            var result = this.recipeIngredientService.Delete(Guid.NewGuid(), this.UserId);
            this.AssertSuccess(result);

            //found
            var item = this.CreateRecipeIngredient();
            Guid id = item.RecipeIngredientId;

            result = this.recipeIngredientService.Delete(id, this.UserId);
            this.AssertSuccess(result);

            var dbItem = this.recipeIngredientService.GetIngredient(id);
            dbItem.Should().NotBeNull();
            dbItem.HasData.Should().BeFalse();

            //save again
            item.RecipeIngredientId = Guid.Empty;

            result = this.recipeIngredientService.Save(item, this.UserId);
            this.AssertSuccess(result);

            item.RecipeIngredientId = this.GetId(result);

            dbItem = this.recipeIngredientService.GetIngredient(item.RecipeIngredientId);
            this.AssertEquality(item, dbItem);
        }

        [TestMethod]
        public void DeleteAll()
        {
            //not found
            var result = this.recipeIngredientService.DeleteAll(Guid.NewGuid(), this.UserId);
            this.AssertSuccess(result);

            //found
            var id = this.CreateRecipe().RecipeId;
            var item1 = this.CreateRecipeIngredient(id);
            var item2 = this.CreateRecipeIngredient(id);
            var item3 = this.CreateRecipeIngredient(id);
            var itemFromDifferentRecipe = this.CreateRecipeIngredient();

            result = this.recipeIngredientService.DeleteAll(id, this.UserId);
            this.AssertSuccess(result);

            var dbItems = this.recipeIngredientService.GetIngredients(id);
            dbItems.Should().NotBeNull();
            dbItems.Should().BeEmpty();

            var dbItemFromDifferentRecipe = this.recipeIngredientService.GetIngredient(itemFromDifferentRecipe.RecipeIngredientId);
            dbItemFromDifferentRecipe.Should().NotBeNull();
            dbItemFromDifferentRecipe.HasData.Should().BeTrue();
        }
    }
}
