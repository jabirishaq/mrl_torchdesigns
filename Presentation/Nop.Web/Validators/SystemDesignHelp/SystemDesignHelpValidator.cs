using FluentValidation;
using Nop.Services.Localization;
using Nop.Web.Models.SystemDesignHelp; 

namespace Nop.Web.Validators.Common
{
    public class SystemDesignHelpValidator : AbstractValidator<SystemDesignHelpModel>
    {
        public SystemDesignHelpValidator(ILocalizationService localizationService)
        {
            //RuleFor(x => x.StateId)
            //      .()
            //      .WithMessage(localizationService.GetResource("Address.Fields.Country.Required"));
           //RuleFor(x => x.FirstName).NotEmpty().WithMessage(localizationService.GetResource("Address.Fields.FirstName.Required"));
            //RuleFor(x => x.LastName).NotEmpty().WithMessage(localizationService.GetResource("Address.Fields.LastName.Required"));

            RuleFor(x => x.Email).NotEmpty().WithMessage(localizationService.GetResource("Address.Fields.Email.Required"));
            RuleFor(x => x.Email).EmailAddress().WithMessage(localizationService.GetResource("Common.WrongEmail"));
            RuleFor(x => x.StateId).NotEqual("0").WithMessage("State Is Required");
        
        }
    }
}