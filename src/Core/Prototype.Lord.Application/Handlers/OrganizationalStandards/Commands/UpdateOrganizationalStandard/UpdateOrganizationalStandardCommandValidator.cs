using FluentValidation;

namespace Prototype.Lord.Application.Handlers.OrganizationalStandards.Commands;

public class UpdateOrganizationalStandardCommandValidator : AbstractValidator<UpdateOrganizationalStandardCommand>
{
    public UpdateOrganizationalStandardCommandValidator()
    {
        RuleFor(c => c.Id).NotEmpty();
        RuleFor(c => c.Name).NotEmpty().MaximumLength(200);
    }
}