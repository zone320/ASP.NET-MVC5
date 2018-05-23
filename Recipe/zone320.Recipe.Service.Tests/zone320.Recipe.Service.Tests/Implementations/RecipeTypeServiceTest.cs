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
    public class RecipeTypeServiceTest : RecipeBaseTest
    {
        [TestMethod]
        public void GetRecipeTypeById()
        {
            //not found
            var item = this.typeService.GetRecipeType(Guid.NewGuid());
            item.Should().NotBeNull();
            item.HasData.Should().BeFalse();
            item.RecipeTypeId.Should().BeEmpty();

            //found
            item = this.typeService.GetRecipeTypes().FirstOrDefault();
            Guid id = item.RecipeTypeId;

            var dbItem = this.typeService.GetRecipeType(id);
            this.AssertEquality(item, dbItem);
        }

        private void AssertEquality(RecipeTypeDto item1, RecipeTypeDto item2)
        {
            item1.Should().NotBeNull();
            item2.Should().NotBeNull();

            item1.ShouldBeEquivalentTo(item2);
        }

        [TestMethod]
        public void GetRecipeTypeByName()
        {
            //not found
            var item = this.typeService.GetRecipeType("invalid");
            item.Should().NotBeNull();
            item.HasData.Should().BeFalse();
            item.RecipeTypeId.Should().BeEmpty();

            //found
            item = this.typeService.GetRecipeTypes().FirstOrDefault();
            string name = item.Name;

            var dbItem = this.typeService.GetRecipeType(name);
            this.AssertEquality(item, dbItem);
        }

        [TestMethod]
        public void GetRecipeTypes()
        {
            var results = this.typeService.GetRecipeTypes();
            results.Should().NotBeNullOrEmpty();

            foreach (var item in results)
            {
                this.Assert(item);
            }
        }

        private void Assert(RecipeTypeDto item)
        {
            item.Should().NotBeNull();
            item.HasData.Should().BeTrue();

            item.RecipeTypeId.Should().NotBeEmpty();
            item.Name.Should().NotBeNullOrWhiteSpace();
        }
    }
}
