using AspireLoanManagement.Business.Models;
using FluentValidation;

namespace AspireLoanManagement.Utility.Validators
{
    public class RepaymentModelValidator : AbstractValidator<RepaymentModelVM>
    {
        public RepaymentModelValidator()
        {
            RuleFor(x => x.RepaymentAmount).NotEmpty();
            RuleFor(x => x.RepaymentAmount > 0);
            RuleFor(x => x.LoanID).NotEmpty();
            RuleFor(x => x.LoanID > 0);
            RuleFor(x => x.UserId).NotEmpty();
            RuleFor(x => x.UserId > 0);
        }
    }
}
