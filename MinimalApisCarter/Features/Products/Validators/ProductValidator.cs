using FluentValidation;
using MinimalApisCarter.Features.Products.Models;

namespace MinimalApisCarter.Features.Products.Validators
{
    public class ProductValidator : AbstractValidator<Product>
    {
        public ProductValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Price).NotEmpty();
        }
    }
}