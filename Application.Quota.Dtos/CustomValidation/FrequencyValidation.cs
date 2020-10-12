using System;
using System.ComponentModel.DataAnnotations;

namespace Application.Quota.Dtos
{
    public class FrequencyValidation : ValidationAttribute
    {
        public FrequencyValidation()
        {
        }
        public override bool IsValid(object value)
        {
            if (value == null) return false;
            int intValue = Convert.ToInt32(value);
            return (intValue == 15 || intValue == 30);
        }
    }
}
