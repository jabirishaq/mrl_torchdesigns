using FluentValidation;
using Nop.Core.Domain.Common;
using Nop.Services.Localization;
using Nop.Plugin.TorchDesign.Testimonials.Models;

namespace Nop.Plugin.TorchDesign.Testimonials.Models
{
    public class PublicInfoValidator : AbstractValidator<AddTestiMonialModel>
    {
        public PublicInfoValidator(ILocalizationService localizationService, AddressSettings addressSettings)
        {
            RuleFor(x => x.Message)
                .NotEmpty()
                .WithMessage(localizationService.GetResource("Plugin.Fields.Message.Required"));
           
            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage(localizationService.GetResource("Address.Fields.Email.Required"));
            RuleFor(x => x.Email)
                .EmailAddress()
                .WithMessage(localizationService.GetResource("Common.WrongEmail"));
         }
    }
}