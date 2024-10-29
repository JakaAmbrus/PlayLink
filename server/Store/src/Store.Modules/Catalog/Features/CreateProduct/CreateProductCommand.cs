using Store.Shared.Abstractions;

namespace Catalog.Features.CreateProduct;

public class CreateProductCommand : ICommand<CreateProductResponse>
{
    public string Name { get; set; }
    
    public string Bla { get; set; }
}