using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using zone320.Common.Tests;
using FluentAssertions;
using zone320.Common.Utility.Validation;

namespace zone320.Common.Utility.Tests.Validation
{
    [TestClass]
    public class ValidatorTest : BaseTest
    {
        private string fieldName = "field name";

        [TestMethod]
        public void GuidCheck()
        {
            var results = Validator.GuidCheck(null, fieldName, true);
            this.AssertNotEmpty(results);
            results = Validator.GuidCheck(Guid.Empty, fieldName, true);
            this.AssertNotEmpty(results);
            results = Validator.GuidCheck(Guid.NewGuid(), fieldName, true);
            this.AssertEmpty(results);

            results = Validator.GuidCheck(null, fieldName, false);
            this.AssertEmpty(results);
            results = Validator.GuidCheck(Guid.Empty, fieldName, false);
            this.AssertNotEmpty(results);
            results = Validator.GuidCheck(Guid.NewGuid(), fieldName, false);
            this.AssertEmpty(results);
        }

        [TestMethod]
        public void FieldLengthCheck()
        {
            int maxlength = 5;

            var results = Validator.FieldLengthCheck(null, fieldName, maxlength, true);
            this.AssertNotEmpty(results);
            results = Validator.FieldLengthCheck(string.Empty, fieldName, maxlength, true);
            this.AssertNotEmpty(results);
            results = Validator.FieldLengthCheck(" ", fieldName, maxlength, true);
            this.AssertNotEmpty(results);
            results = Validator.FieldLengthCheck("12345", fieldName, maxlength, true);
            this.AssertEmpty(results);
            results = Validator.FieldLengthCheck("123456", fieldName, maxlength, true);
            this.AssertNotEmpty(results);

            results = Validator.FieldLengthCheck(null, fieldName, maxlength, false);
            this.AssertEmpty(results);
            results = Validator.FieldLengthCheck(string.Empty, fieldName, maxlength, false);
            this.AssertEmpty(results);
            results = Validator.FieldLengthCheck(" ", fieldName, maxlength, false);
            this.AssertEmpty(results);
            results = Validator.FieldLengthCheck("12345", fieldName, maxlength, false);
            this.AssertEmpty(results);
            results = Validator.FieldLengthCheck("123456", fieldName, maxlength, false);
            this.AssertNotEmpty(results);
        }

        [TestMethod]
        public void FieldLengthCheckInt()
        {
            int minValue = 1;
            int? maxValue = null;

            var results = Validator.FieldLengthCheck(null, fieldName, minValue, maxValue, true);
            this.AssertNotEmpty(results);
            results = Validator.FieldLengthCheck(0, fieldName, minValue, maxValue, true);
            this.AssertNotEmpty(results);
            results = Validator.FieldLengthCheck(1, fieldName, minValue, maxValue, true);
            this.AssertEmpty(results);
            results = Validator.FieldLengthCheck(2, fieldName, minValue, maxValue, true);
            this.AssertEmpty(results);
            results = Validator.FieldLengthCheck(3, fieldName, minValue, maxValue, true);
            this.AssertEmpty(results);

            results = Validator.FieldLengthCheck(null, fieldName, minValue, maxValue, false);
            this.AssertEmpty(results);
            results = Validator.FieldLengthCheck(0, fieldName, minValue, maxValue, false);
            this.AssertNotEmpty(results);
            results = Validator.FieldLengthCheck(1, fieldName, minValue, maxValue, false);
            this.AssertEmpty(results);
            results = Validator.FieldLengthCheck(2, fieldName, minValue, maxValue, false);
            this.AssertEmpty(results);
            results = Validator.FieldLengthCheck(3, fieldName, minValue, maxValue, false);
            this.AssertEmpty(results);


            maxValue = 2;

            results = Validator.FieldLengthCheck(null, fieldName, minValue, maxValue, true);
            this.AssertNotEmpty(results);
            results = Validator.FieldLengthCheck(0, fieldName, minValue, maxValue, true);
            this.AssertNotEmpty(results);
            results = Validator.FieldLengthCheck(1, fieldName, minValue, maxValue, true);
            this.AssertEmpty(results);
            results = Validator.FieldLengthCheck(2, fieldName, minValue, maxValue, true);
            this.AssertEmpty(results);
            results = Validator.FieldLengthCheck(3, fieldName, minValue, maxValue, true);
            this.AssertNotEmpty(results);

            results = Validator.FieldLengthCheck(null, fieldName, minValue, maxValue, false);
            this.AssertEmpty(results);
            results = Validator.FieldLengthCheck(0, fieldName, minValue, maxValue, false);
            this.AssertNotEmpty(results);
            results = Validator.FieldLengthCheck(1, fieldName, minValue, maxValue, false);
            this.AssertEmpty(results);
            results = Validator.FieldLengthCheck(2, fieldName, minValue, maxValue, false);
            this.AssertEmpty(results);
            results = Validator.FieldLengthCheck(3, fieldName, minValue, maxValue, false);
            this.AssertNotEmpty(results);
        }
    }
}
