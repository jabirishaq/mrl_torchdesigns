using FluentValidation;
using Nop.Admin.Models.Shipping;
using Nop.Services.Localization;

namespace Nop.Admin.Validators.Shipping
{
    public class DeliveryDateValidator : AbstractValidator<DeliveryDateModel>
    {
        public DeliveryDateValidator(ILocalizationService localizationService)
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage(localizationService.GetResource("Admin.Configuration.Shipping.DeliveryDates.Fields.Name.Required"));
        }
    }
}