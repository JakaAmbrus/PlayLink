using Ardalis.Result;
using Catalog.Data;
using Catalog.Data.Entities;
using Store.Shared.Abstractions;

namespace Catalog.Features.Categories.AddCategory;

public class AddCategoryCommandHandler : ICommandHandler<AddCategoryCommand, AddCategoryResponse>
{
    private readonly CatalogDbContext _dbContext;

    public AddCategoryCommandHandler(CatalogDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<AddCategoryResponse>> Handle(AddCategoryCommand request, CancellationToken cancellationToken)
    {
        if (request.ParentCategoryId.HasValue)
        {
            var parentCategory = await _dbContext.Categories.FindAsync(request.ParentCategoryId.Value);
            if (parentCategory == null)
            {
                return Result.NotFound("Parent category not found");
            }
        }
        
        var category = new Category
        {
            Name = request.Name,
            Description = request.Description,
            ParentCategoryId = request.ParentCategoryId
        };
        
        _dbContext.Categories.Add(category);
        await _dbContext.SaveChangesAsync(cancellationToken);
        
        return Result.Success(new AddCategoryResponse
        {
            Id = category.Id,
            Name = category.Name,
            Description = category.Description,
            ParentCategoryId = category.ParentCategoryId
        });
    }
}