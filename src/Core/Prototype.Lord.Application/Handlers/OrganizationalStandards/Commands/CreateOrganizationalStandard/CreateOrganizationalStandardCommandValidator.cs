using FluentValidation;

namespace Prototype.Lord.Application.Handlers.OrganizationalStandards.Commands;

public class CreateOrganizationalStandardCommandValidator : AbstractValidator<CreateOrganizationalStandardCommand>
{
    public CreateOrganizationalStandardCommandValidator()
    {
        RuleFor(c => c.Name).NotEmpty().MaximumLength(200);
    }
}