using System.Text.RegularExpressions;
using Application.Clients.Command.CreateClient;
using FluentValidation;

namespace Application.Contacts.Commands.CreateClient
{
    public class CreateClientCommandValidator : AbstractValidator<CreateClientCommand>
    {
        public CreateClientCommandValidator()
        {
            RuleFor(v => v.FirstName).NotNull()
                                     .MaximumLength(50);

            RuleFor(v => v.LastName).NotNull()
                                    .MaximumLength(50);
                                 
            RuleFor(v => v.Email).NotNull()
                                 .EmailAddress()
                                 .WithMessage("A valid email is required");

            //Format 0123456789, 012-345-6789, and (012) -345-6789
            RuleFor(v => v.PhoneNumber).Matches(new Regex(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$"))
                                       .WithMessage("PhoneNumber not valid");
        }
    }
}