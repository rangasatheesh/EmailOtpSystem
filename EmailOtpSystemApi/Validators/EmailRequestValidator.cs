using EmailOtpSystemApi.DTO;
using FluentValidation;

namespace EmailOtpSystemApi.Validators
{
    public class EmailRequestValidator : AbstractValidator<EmailRequest>
    {
        public EmailRequestValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.");
        }
    }
}
