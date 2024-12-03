using FluentValidation;

namespace Catalog.Features.Categories.GetCategories;

public class GetCategoriesCommandValidator : AbstractValidator<GetCategoriesCommand>
{
    public GetCategoriesCommandValidator()
    {
    }
}