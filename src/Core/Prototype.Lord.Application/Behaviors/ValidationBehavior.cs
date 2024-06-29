using FluentValidation;
using MediatR;
using Prototype.Lord.Domain.Constants;
using Prototype.Lord.Domain.Enums;

namespace Prototype.Lord.Application.Behaviors;

public sealed class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators) => _validators = validators;

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (_validators.Any())
        {
            var context = new ValidationContext<TRequest>(request);
            var validationResults = await Task.WhenAll(_validators.Select(v => v.ValidateAsync(context, cancellationToken)));
            var failures = validationResults.SelectMany(r => r.Errors).Where(f => f != null).ToList();
            if (failures.Count > 0)
                return (TResponse)Activator.CreateInstance(typeof(TResponse), Status.Failure, Message.InvalidParameters, failures.Select(m => m.ErrorMessage).ToList());

            return await next();
        }

        return await next();
    }
}