using System.Text.RegularExpressions;
using Application.Clients.Command.UpdateClient;
using FluentValidation;

namespace Application.Contacts.Commands.UpdateContact
{
    public class CreateClientCommandValidator : AbstractValidator<UpdateClientCommand>
    {
        public CreateClientCommandValidator()
        {
            RuleFor(v => v.FirstName).MaximumLength(50);

            RuleFor(v => v.LastName).MaximumLength(50);
                                 
            RuleFor(v => v.Email).NotNull()
                                 .EmailAddress()
                                 .WithMessage("A valid email is required");

            //Format 0123456789, 012-345-6789, and (012) -345-6789
            RuleFor(v => v.PhoneNumber).Matches(new Regex(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$"))
                                       .WithMessage("PhoneNumber not valid");
        }
    }
}