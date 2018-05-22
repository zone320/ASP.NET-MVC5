using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using zone320.Common.Utility.Resources;

namespace zone320.Common.Utility.Validation
{
    public class Validator
    {
        /// <summary>
        /// Checks if guid is empty or required
        /// </summary>
        /// <param name="fieldValue"></param>
        /// <param name="fieldName"></param>
        /// <param name="isRequired"></param>
        /// <returns></returns>
        public static List<string> GuidCheck(Guid? fieldValue, string fieldName, bool isRequired)
        {
            List<string> errorMessages = new List<string>();
            if (isRequired)
            {
                if (!fieldValue.HasValue || fieldValue.Value == Guid.Empty)
                {
                    errorMessages.Add(string.Format(ValidationResources.IsRequired, fieldName));
                }
            }
            else if (fieldValue.HasValue)
            {
                if (fieldValue.Value == Guid.Empty)
                {
                    errorMessages.Add(string.Format(ValidationResources.IsInvalid, fieldName));
                }
            }

            return errorMessages;
        }

        /// <summary>
        /// Validates a field length
        /// </summary>
        /// <param name="fieldValue"></param>
        /// <param name="fieldName"></param>
        /// <param name="maxLength"></param>
        /// <param name="isFieldRequired"></param>
        /// <returns></returns>
        public static List<string> FieldLengthCheck(string fieldValue, string fieldName, int maxLength, bool isFieldRequired)
        {
            List<string> errorMessages = new List<string>();
            if (!string.IsNullOrWhiteSpace(fieldValue))
            {
                if (fieldValue.Trim().Length > maxLength)
                {
                    if (maxLength > 1)
                    {
                        errorMessages.Add(string.Format(ValidationResources.MustBeCharactersOrLess, fieldName, maxLength));
                    }
                    else
                    {
                        errorMessages.Add(string.Format(ValidationResources.MustBeCharacter, fieldName, maxLength));
                    }
                }

            }
            else if (isFieldRequired)
            {
                errorMessages.Add(string.Format(ValidationResources.IsRequired, fieldName));
            }

            return errorMessages;
        }

        /// <summary>
        /// Validates a field length
        /// </summary>
        /// <param name="fieldValue"></param>
        /// <param name="fieldName"></param>
        /// <param name="minValue"></param>
        /// <param name="maxValue"></param>
        /// <param name="isFieldRequired"></param>
        /// <returns></returns>
        public static List<string> FieldLengthCheck(int? fieldValue, string fieldName, int minValue, int? maxValue, bool isFieldRequired)
        {
            List<string> errorMessages = new List<string>();

            if (fieldValue.HasValue)
            {
                if (fieldValue < minValue)
                {
                    errorMessages.Add(string.Format(ValidationResources.MustBeGreaterThan, fieldName, fieldValue, minValue));
                }
                else if (maxValue.HasValue && fieldValue > maxValue.Value)
                {
                    errorMessages.Add(string.Format(ValidationResources.MustBeLessThan, fieldName, fieldValue, maxValue.Value));
                }
            }
            else if (isFieldRequired && !fieldValue.HasValue)
            {
                errorMessages.Add(string.Format(ValidationResources.IsRequired, fieldName));
            }

            return errorMessages;
        }
    }
}
