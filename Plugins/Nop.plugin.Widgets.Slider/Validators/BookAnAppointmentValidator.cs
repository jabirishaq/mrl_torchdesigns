using FluentValidation;
using Nop.Core.Domain.Customers;
using Nop.Services.Localization;
using Nop.Plugin.Widgets.Slider.Models;


namespace Nop.Plugin.Widgets.Event.Validators
{
    //public class BookAnAppointmentValidator : AbstractValidator<SliderDetailModel.AppointmentFormModel>
    //{
    //    public BookAnAppointmentValidator(ILocalizationService localizationService)
    //    {
    //        RuleFor(x => x.Email).NotEmpty().WithMessage(localizationService.GetResource("Plugins.Widgets.Event.Email.Required"));
    //        RuleFor(x => x.Email).EmailAddress().WithMessage(localizationService.GetResource("Common.WrongEmail"));
    //        RuleFor(x => x.Name).NotEmpty().WithMessage(localizationService.GetResource("Plugins.Widgets.Event.Name.Required"));
    //        RuleFor(x => x.Date).NotEmpty().WithMessage(localizationService.GetResource("Plugins.Widgets.Event.Date.Required"));
    //        RuleFor(x => x.Comment).NotEmpty().WithMessage(localizationService.GetResource("Plugins.Widgets.Event.Comment.Required"));

    //    }
    //}
}