using Ardalis.Result;
using MediatR;

namespace SharedKernel.Abstractions;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>
{
}