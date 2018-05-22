using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using zone320.Common.Tests;
using FluentAssertions;
using zone320.Common.Utility.DTO;
using System.Collections.Generic;

namespace zone320.Common.Utility.Tests.DTO
{
    [TestClass]
    public class ResultDtoTest : BaseTest
    {
        [TestMethod]
        public void ResultDto()
        {
            var item = new ResultDto();
            item.IsSuccess.Should().BeTrue();
            item.ErrorMessages.Should().NotBeNull();

            item = new ResultDto(new List<string>());
            item.IsSuccess.Should().BeTrue();
            item.ErrorMessages.Should().NotBeNull();

            item = new ResultDto(new List<string>() { "error" });
            item.IsSuccess.Should().BeFalse();
            item.ErrorMessages.Should().NotBeNullOrEmpty();
            item.ErrorMessages.Count.Should().Be(1);
            item.ErrorMessages[0].Should().Be("error");
        }
    }
}
