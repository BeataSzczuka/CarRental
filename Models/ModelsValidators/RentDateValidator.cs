using CarRental.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CarRental.Models.ModelsValidators
{
    public class RentDateValidator : ValidationAttribute
    {
        public override string FormatErrorMessage(string name)
        {
            return "Date value should not be a future date";
        }

        protected override ValidationResult IsValid(object objValue,
                                                       ValidationContext validationContext)
        {
            var today = new DateTime();
            var dateFrom = ((Rent)validationContext.ObjectInstance).DateFrom;
            var dateTo = ((Rent)validationContext.ObjectInstance).DateTo;
            if (DateTime.Compare(dateFrom, today) < 0 || DateTime.Compare(dateFrom, today) < 0 || DateTime.Compare(dateTo, dateFrom) < 0)
            {
                return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
            }

            return ValidationResult.Success;
        }
    }
}
