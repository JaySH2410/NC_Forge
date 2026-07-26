using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc.Filters;
using ValidationException = Forge.Shared.Exceptions.ValidationException;

namespace Forge.Shared.Filters;

public class ValidationFilter : IAsyncActionFilter
{
    private readonly IServiceProvider _serviceProvider;

    public ValidationFilter(
        IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task OnActionExecutionAsync(
        ActionExecutingContext context,
        ActionExecutionDelegate next)
    {
        foreach (var argument in context.ActionArguments.Values)
        {
            if (argument is null)
            {
                continue;
            }

            var validatorType = typeof(IValidator<>)
                .MakeGenericType(argument.GetType());

            var validator = _serviceProvider.GetService(
                validatorType);

            if (validator is null)
            {
                continue;
            }

            var validationContext = new ValidationContext<object>(
                argument);

            var validationResult =
                await ((IValidator)validator)
                    .ValidateAsync(validationContext);

            if (!validationResult.IsValid)
            {
                ThrowValidationException(
                    validationResult);
            }
        }

        await next();
    }

    private static void ThrowValidationException(
        ValidationResult validationResult)
    {
        var errors = validationResult.Errors
            .GroupBy(x => x.PropertyName)
            .ToDictionary(
                x => x.Key,
                x => x
                    .Select(e => e.ErrorMessage)
                    .ToArray());

        throw new ValidationException(errors);
    }
}
