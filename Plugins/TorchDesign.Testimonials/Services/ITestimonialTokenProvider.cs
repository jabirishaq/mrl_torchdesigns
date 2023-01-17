using System.Collections.Generic;
using Nop.Core.Domain.Blogs;
using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.Customers;
using Nop.Core.Domain.Forums;
using Nop.Core.Domain.Messages;
using Nop.Core.Domain.News;
using Nop.Core.Domain.Orders;
using Nop.Core.Domain.Shipping;
using Nop.Core.Domain.Stores;
using Nop.Services.Messages;
using Nop.Plugin.TorchDesign.Testimonials.Models;
using Nop.Plugin.TorchDesign.Testimonials.Domain;

namespace Nop.Plugin.TorchDesign.Testimonials.Services
{
    public partial interface ITestimonialTokenProvider
    {
        void AddTestimonialTokens(IList<Token> tokens, Td_Testimonials testimonial, int[] pictureid, int languageId); 

        //string[] GetListOfCampaignAllowedTokens();

        string[] GetListOfAllowedTokens();
    }
}
