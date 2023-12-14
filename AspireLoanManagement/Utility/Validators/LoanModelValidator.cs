using AspireLoanManagement.Business.Models;
using FluentValidation;

namespace AspireLoanManagement.Utility.Validators
{
    public class CreateLoanPayloadValidator: AbstractValidator<LoanModelVM>
    {
        public CreateLoanPayloadValidator()
        {
            RuleFor(x => x.Amount > 0);
            RuleFor(x => x.UserId > 0);
            RuleFor(x => x.Term > 0);
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
