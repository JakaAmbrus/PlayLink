using Ardalis.Result;
using Store.Shared.Abstractions;

namespace Catalog.Features.Categories.GetCategories;

public class CommandHandler : ICommandHandler<GetCategoriesCommand, GetCategoriesResponse>
{

    public async Task<Result<GetCategoriesResponse>> Handle(GetCategoriesCommand request, CancellationToken cancellationToken)
    {


        return Result.Success(new GetCategoriesResponse());
    }
}