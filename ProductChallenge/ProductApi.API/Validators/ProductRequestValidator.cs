using FluentValidation;
using ProductApi.Application.DTOs;

namespace ProductApi.API.Validators
{
    public class ProductRequestValidator : AbstractValidator<ProductRequest>
    {
        public ProductRequestValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(100).WithMessage("Name must be at most 100 characters.");
            RuleFor(x => x.Status)
                .InclusiveBetween(0, 1).WithMessage("Status must be 0 (Inactive) or 1 (Active).");
            RuleFor(x => x.Stock)
                .GreaterThanOrEqualTo(0).WithMessage("Stock must be zero or positive.");
            RuleFor(x => x.Description)
                .MaximumLength(500).WithMessage("Description must be at most 500 characters.");
            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("Price must be greater than zero.");
        }
    }
}
