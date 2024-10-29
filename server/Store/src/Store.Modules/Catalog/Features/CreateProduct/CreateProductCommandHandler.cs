using Ardalis.Result;
using Store.Shared.Abstractions;

namespace Catalog.Features.CreateProduct;

public class CreateProductCommandHandler : ICommandHandler<CreateProductCommand, CreateProductResponse>
{

    public async Task<Result<CreateProductResponse>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {


        return Result.Success(new CreateProductResponse());
    }
}