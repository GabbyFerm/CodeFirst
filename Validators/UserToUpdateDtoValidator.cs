using CodeFirst.Dtos;
using FluentValidation;

namespace CodeFirst.Validators
{
    public class UserToUpdateDtoValidator : AbstractValidator<UserToUpdateDto>
    {
        public UserToUpdateDtoValidator()
        {
            RuleFor(user => user.Name).NotEmpty().WithMessage("Name is required.");

            RuleFor(user => user.Email)
            .EmailAddress().When(user => !string.IsNullOrWhiteSpace(user.Email)) 
            .WithMessage("Email must be a valid email address.");

            RuleFor(user => user.PhoneNumber)
                .Matches(@"^\d{10}$").When(user => !string.IsNullOrWhiteSpace(user.PhoneNumber)) 
                .WithMessage("Phone number must be 10 digits.");
        }
    }
}
