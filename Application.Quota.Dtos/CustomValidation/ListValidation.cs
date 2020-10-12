using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace Application.Quota.Dtos
{
    public class ListValidation : ValidationAttribute
    {
        public ListValidation()
        {
        }

        public override bool IsValid(object value)
        {
            var listValue = value as IList;
            if (listValue != null)
            {
                return listValue.Count > 0;
            }
            return false;
        }
    }
}
