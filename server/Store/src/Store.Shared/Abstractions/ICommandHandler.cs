using Ardalis.Result;
using MediatR;

namespace Store.Shared.Abstractions;

public interface ICommandHandler<TCommand, TResponse> : IRequestHandler<TCommand, Result<TResponse>>
    where TCommand : ICommand<TResponse>
{
}