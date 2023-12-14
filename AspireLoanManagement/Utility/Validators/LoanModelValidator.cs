using AspireLoanManagement.Business.Models;
using FluentValidation;

namespace AspireLoanManagement.Utility.Validators
{
    public class CreateLoanPayloadValidator: AbstractValidator<LoanModelVM>
    {
        public CreateLoanPayloadValidator()
        {
            RuleFor(x => x.Amount).GreaterThan(0);
            RuleFor(x => x.UserId).GreaterThan(0);
            RuleFor(x => x.Term).GreaterThan(0);
        }
    }

    public class GetLoanPayloadValidator : AbstractValidator<int>
    {
        public GetLoanPayloadValidator()
        {
            RuleFor(x => x > 0);
        }
    }
}
