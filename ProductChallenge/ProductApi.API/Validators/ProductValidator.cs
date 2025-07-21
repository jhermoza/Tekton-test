using FluentValidation;
using ProductApi.Domain.Entities;

namespace ProductApi.API.Validators
{
    public class ProductValidator : AbstractValidator<Product>
    {
        public ProductValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("El nombre es obligatorio")
                .MaximumLength(100);
            RuleFor(x => x.Status)
                .InclusiveBetween(0, 1).WithMessage("El status debe ser 0 o 1");
            RuleFor(x => x.Stock)
                .GreaterThanOrEqualTo(0);
            RuleFor(x => x.Description)
                .MaximumLength(500);
            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("El precio debe ser mayor a 0");
        }
    }
}
