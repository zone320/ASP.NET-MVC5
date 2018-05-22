using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace zone320.Common.Utility.DTO
{
    public class ResultDto
    {
        public ResultDto()
        {
            this.IsSuccess = true;
            this.ErrorMessages = new List<string>();
        }

        public ResultDto(List<string> errorMessages)
        {
            this.IsSuccess = true;
            this.ErrorMessages = errorMessages;

            if (this.ErrorMessages != null && this.ErrorMessages.Count > 0)
            {
                this.IsSuccess = false;
            }
        }

        public bool IsSuccess { get; set; }
        public List<string> ErrorMessages { get; set; }

        public object Result { get; set; }
    }
}
