using Ardalis.Result;
using FluentValidation;
using MediatR;

namespace Store.Shared.Behaviours
{
    public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
        where TResponse : Result
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var context = new ValidationContext<TRequest>(request);
            
            var failures = _validators
                .Select(v => v.Validate(context))
                .SelectMany(result => result.Errors)
                .Where(f => f != null)
                .ToList();
            
            if (failures.Count != 0)
            {
                var validationErrors = failures
                    .Select(f => new ValidationError
                    {
                        Identifier = f.PropertyName,
                        ErrorMessage = f.ErrorMessage
                    })
                    .ToList();
                
                return (TResponse)Result.Invalid(validationErrors);
            }
            
            return await next();
        }
    }
}