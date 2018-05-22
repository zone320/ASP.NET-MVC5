using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using zone320.Common.DataAccess.DTO;
using zone320.Common.Tests;
using FluentAssertions;

namespace zone320.Common.DataAccess.Tests.DTO
{
    [TestClass]
    public class BaseDtoTest : BaseTest
    {
        [TestMethod]
        public void BaseDto()
        {
            new BaseDto().HasData.Should().BeTrue();
        }
    }
}
