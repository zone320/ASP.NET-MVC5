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
    public class RecipeInstructionServiceTest : RecipeBaseTest
    {
        [TestMethod]
        public void GetInstruction()
        {
            //not found
            var item = this.recipeInstructionService.GetInstruction(Guid.NewGuid());
            item.Should().NotBeNull();
            item.HasData.Should().BeFalse();

            //found
            item = this.CreateRecipeInstruction();
            Guid id = item.RecipeInstructionId;

            var dbItem = this.recipeInstructionService.GetInstruction(id);
            this.AssertEquality(item, dbItem);
        }

        private void AssertEquality(RecipeInstructionDto item1, RecipeInstructionDto item2)
        {
            item1.Should().NotBeNull();
            item2.Should().NotBeNull();

            item1.HasData.Should().BeTrue();
            item2.HasData.Should().BeTrue();

            item1.ShouldBeEquivalentTo(item2, options => options.Excluding(x => x.ChangeDate).Excluding(x => x.CreateDate));
        }

        [TestMethod]
        public void GetInstructionsByRecipe()
        {
            Guid recipeId = Guid.NewGuid();

            var items = this.recipeInstructionService.GetInstructions(recipeId);
            //TODO: AssertEmpty
            items.Should().NotBeNull();
            items.Should().BeEmpty();

            recipeId = this.CreateRecipe().RecipeId;

            this.CreateRecipeInstruction(recipeId);
            this.CreateRecipeInstruction(recipeId);
            this.CreateRecipeInstruction(recipeId);

            items = this.recipeInstructionService.GetInstructions(recipeId);
            items.Should().NotBeNullOrEmpty();
            items.Count.Should().Be(3);

            foreach (var item in items)
            {
                this.Assert(item);
                item.RecipeId.Should().Be(recipeId);
            }
        }

        [TestMethod]
        public void GetInstructions()
        {
            var recipeId = this.CreateRecipe().RecipeId;

            this.CreateRecipeInstruction(recipeId);
            this.CreateRecipeInstruction();
            this.CreateRecipeInstruction();

            var items = this.recipeInstructionService.GetInstructions();
            items.Should().NotBeNullOrEmpty();
            items.Count.Should().BeGreaterOrEqualTo(3);

            foreach (var item in items)
            {
                this.Assert(item);
            }
        }

        private void Assert(RecipeInstructionDto item)
        {
            item.Should().NotBeNull();
            item.HasData.Should().BeTrue();

            item.RecipeInstructionId.Should().NotBeEmpty();
            item.RecipeId.Should().NotBeEmpty();
            item.Sequence.Should().BeGreaterThan(0);
            item.Instruction.Should().NotBeNullOrWhiteSpace();
        }

        [TestMethod]
        public void SaveNew()
        {
            var item = this.BuildRecipeInstruction();

            var result = this.recipeInstructionService.Save(item, this.UserId);
            this.AssertSuccess(result);

            item.RecipeInstructionId = this.GetId(result);

            var dbItem = this.recipeInstructionService.GetInstruction(item.RecipeInstructionId);
            this.AssertEquality(item, dbItem);
        }

        [TestMethod]
        public void SaveExisting()
        {
            var item = this.CreateRecipeInstruction();
            item.Sequence++;
            item.Instruction += "updated";

            var result = this.recipeInstructionService.Save(item, this.UserId);
            this.AssertSuccess(result);

            var dbItem = this.recipeInstructionService.GetInstruction(item.RecipeInstructionId);
            this.AssertEquality(item, dbItem);
        }

        [TestMethod]
        public void SaveDuplicate()
        {
            var item = this.CreateRecipeInstruction();
            item.RecipeInstructionId = Guid.Empty;

            var result = this.recipeInstructionService.Save(item, this.UserId);
            this.AssertSuccess(result);
        }

        [TestMethod]
        public void Validate()
        {
            RecipeInstructionDto item = null;
            this.AssertFailure(this.recipeInstructionService.Validate(item));

            item = new RecipeInstructionDto();
            this.AssertFailure(this.recipeInstructionService.Validate(item));

            item = this.BuildRecipeInstruction();
            this.AssertSuccess(this.recipeInstructionService.Validate(item));


            item.RecipeId = Guid.Empty;
            this.AssertFailure(this.recipeInstructionService.Validate(item));

            item.RecipeId = Guid.NewGuid();
            this.AssertSuccess(this.recipeInstructionService.Validate(item));


            item.Sequence = -1;
            this.AssertFailure(this.recipeInstructionService.Validate(item));

            item.Sequence = 0;
            this.AssertSuccess(this.recipeInstructionService.Validate(item));

            item.Sequence = 1000;
            this.AssertSuccess(this.recipeInstructionService.Validate(item));


            item.Instruction = null;
            this.AssertFailure(this.recipeInstructionService.Validate(item));

            item.Instruction = string.Empty;
            this.AssertFailure(this.recipeInstructionService.Validate(item));

            item.Instruction = " ";
            this.AssertFailure(this.recipeInstructionService.Validate(item));

            item.Instruction = this.GetRandomString(2001);
            this.AssertFailure(this.recipeInstructionService.Validate(item));

            item.Instruction = this.GetRandomString(2000);
            this.AssertSuccess(this.recipeInstructionService.Validate(item));
        }

        [TestMethod]
        public void Delete()
        {
            //not found
            var result = this.recipeInstructionService.Delete(Guid.NewGuid(), this.UserId);
            this.AssertSuccess(result);

            //found
            var item = this.CreateRecipeInstruction();
            Guid id = item.RecipeInstructionId;

            result = this.recipeInstructionService.Delete(id, this.UserId);
            this.AssertSuccess(result);

            var dbItem = this.recipeInstructionService.GetInstruction(id);
            dbItem.Should().NotBeNull();
            dbItem.HasData.Should().BeFalse();

            //save again
            item.RecipeInstructionId = Guid.Empty;

            result = this.recipeInstructionService.Save(item, this.UserId);
            this.AssertSuccess(result);

            item.RecipeInstructionId = this.GetId(result);

            dbItem = this.recipeInstructionService.GetInstruction(item.RecipeInstructionId);
            this.AssertEquality(item, dbItem);
        }

        [TestMethod]
        public void DeleteAll()
        {
            //not found
            var result = this.recipeInstructionService.DeleteAll(Guid.NewGuid(), this.UserId);
            this.AssertSuccess(result);

            //found
            var id = this.CreateRecipe().RecipeId;
            var item1 = this.CreateRecipeInstruction(id);
            var item2 = this.CreateRecipeInstruction(id);
            var item3 = this.CreateRecipeInstruction(id);
            var itemFromDifferentRecipe = this.CreateRecipeInstruction();

            result = this.recipeInstructionService.DeleteAll(id, this.UserId);
            this.AssertSuccess(result);

            var dbItems = this.recipeInstructionService.GetInstructions(id);
            dbItems.Should().NotBeNull();
            dbItems.Should().BeEmpty();

            var dbItemFromDifferentRecipe = this.recipeInstructionService.GetInstruction(itemFromDifferentRecipe.RecipeInstructionId);
            dbItemFromDifferentRecipe.Should().NotBeNull();
            dbItemFromDifferentRecipe.HasData.Should().BeTrue();
        }
    }
}
