using Ardalis.Result;
using MediatR;

namespace SharedKernel.Abstractions;

public interface ICommand<TResponse> : IRequest<Result<TResponse>>
{
}