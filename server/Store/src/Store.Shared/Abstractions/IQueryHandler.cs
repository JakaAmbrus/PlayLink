using Ardalis.Result;
using MediatR;

namespace Store.Shared.Abstractions;

public interface IQueryHandler<TQuery, TResponse> : IRequestHandler<TQuery, Result<TResponse>>
    where TQuery : IQuery<TResponse>
{
}