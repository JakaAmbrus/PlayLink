using Ardalis.Result;
using MediatR;

namespace Store.Shared.Abstractions;

public interface ICommand<TResponse> : IRequest<Result<TResponse>>
{
}