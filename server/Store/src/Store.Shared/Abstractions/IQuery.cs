using Ardalis.Result;
using MediatR;

namespace Store.Shared.Abstractions;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>
{
}