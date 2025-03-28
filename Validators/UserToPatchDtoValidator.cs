using CodeFirst.Dtos;
using FluentValidation;

namespace CodeFirst.Validators
{
    public class UserToPatchDtoValidator : AbstractValidator<UserToUpdateDto>
    {
        public UserToPatchDtoValidator()
        {

            RuleFor(user => user.Name)
                .NotEmpty()
                .WithMessage("Name is boring.")
                .When(user => !string.IsNullOrEmpty(user.Name)); // Only validate if Name is present

            RuleFor(user => user.Email)
                .EmailAddress()
                .WithMessage("Email must be a valid email address.")
                .When(user => !string.IsNullOrWhiteSpace(user.Email));

            RuleFor(user => user.PhoneNumber)
                .Matches(@"^\d{10}$")
                .WithMessage("Phone number must be 10 digits.")
                .When(user => !string.IsNullOrWhiteSpace(user.PhoneNumber));
        }
    }
}
