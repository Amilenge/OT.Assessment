using FluentValidation;
using OT.Assessment.Api.Contract.Request;

namespace OT.Assessment.Api.Contract.Validators
{
    public class CasinoWagerRequestValidator : AbstractValidator<CasinoWagerRequest>
    {
        public CasinoWagerRequestValidator()
        {
            RuleFor(x => x.WagerId).NotEmpty()
                .WithMessage("WagerId is required");

            RuleFor(x => x.TransactionId).NotEmpty()
                .WithMessage("TransactionId is required");

            RuleFor(x => x.BrandId).NotEmpty()
                .WithMessage("BrandId is required");

            RuleFor(x => x.Theme).NotEmpty().NotNull()
                .WithMessage("Theme is required");

            RuleFor(x => x.Provider).NotEmpty().NotNull()
                .WithMessage("Provider is required");

            RuleFor(x => x.GameName).NotEmpty().NotNull()
                .WithMessage("GameName is required");
            // ... more validations
        }
    }
}
