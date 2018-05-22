using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using zone320.Common.Utility.DTO;

namespace zone320.Common.Tests
{
    public class BaseTestAssert
    {
        #region Collections
        /// <summary>
        /// Asserts a collection is not null but is empty
        /// </summary>
        /// <param name="results"></param>
        public void AssertEmpty(List<string> results)
        {
            results.Should().NotBeNull();
            results.Should().BeEmpty();
        }

        /// <summary>
        /// Asserts a collection is not null but is empty
        /// </summary>
        /// <param name="results"></param>
        public void AssertEmpty(List<object> results)
        {
            results.Should().NotBeNull();
            results.Should().BeEmpty();
        }

        /// <summary>
        /// Asserts a collection is not null or empty and has a specified length
        /// </summary>
        /// <param name="results"></param>
        /// <param name="length"></param>
        public void AssertNotEmpty(List<string> results, int? length = null)
        {
            results.Should().NotBeNullOrEmpty();
            if (length.HasValue)
            {
                results.Count.Should().Be(length.Value);
            }
        }

        /// <summary>
        /// Asserts a collection is not null or empty and has a specified length
        /// </summary>
        /// <param name="results"></param>
        /// <param name="length"></param>
        public void AssertNotEmpty(List<object> results, int length)
        {
            results.Should().NotBeNullOrEmpty();
            results.Count.Should().Be(length);
        }
        #endregion

        #region ResultDto
        public void AssertSuccess(ResultDto result)
        {
            result.Should().NotBeNull();
            result.IsSuccess.Should().BeTrue();

            result.ErrorMessages.Should().BeEmpty();
        }

        public Guid GetId(ResultDto result)
        {
            this.AssertSuccess(result);

            result.Result.Should().NotBeNull();
            result.Result.Should().BeOfType(typeof(Guid));

            return Guid.Parse(result.Result.ToString());
        }

        public void AssertFailure(ResultDto result)
        {
            result.Should().NotBeNull();
            result.IsSuccess.Should().BeFalse();

            result.ErrorMessages.Should().NotBeNullOrEmpty();
        }

        #endregion
    }
}
