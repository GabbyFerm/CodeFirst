using CodeFirst.Dtos;
using FluentValidation;

namespace CodeFirst.Validators
{
    public class UserToUpdateDtoValidator : AbstractValidator<UserToUpdateDto>
    {
        public UserToUpdateDtoValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required.");

            RuleFor(x => x.Email)
            .EmailAddress().When(x => !string.IsNullOrWhiteSpace(x.Email)) 
            .WithMessage("Email must be a valid email address.");

            RuleFor(x => x.PhoneNumber)
                .Matches(@"^\d{10}$").When(x => !string.IsNullOrWhiteSpace(x.PhoneNumber)) 
                .WithMessage("Phone number must be 10 digits.");
        }
    }
}
