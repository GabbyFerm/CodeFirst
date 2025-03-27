using CodeFirst.Dtos;
using FluentValidation;

namespace CodeFirst.Validators
{
    public class UserToPatchDtoValidator : AbstractValidator<UserToUpdateDto>
    {
        public UserToPatchDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().When(x => !string.IsNullOrWhiteSpace(x.Name)) // Only validate if Name is provided
                .WithMessage("Name is required.");

            RuleFor(x => x.Email)
                .EmailAddress().When(x => !string.IsNullOrWhiteSpace(x.Email)) // Only validate if Email is provided
                .WithMessage("Email must be a valid email address.");

            RuleFor(x => x.PhoneNumber)
                .Matches(@"^\d{10}$").When(x => !string.IsNullOrWhiteSpace(x.PhoneNumber)) // Only validate if PhoneNumber is provided
                .WithMessage("Phone number must be 10 digits.");
        }
    }
}
