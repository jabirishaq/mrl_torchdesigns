using Nop.Core.Domain.Blogs;
using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.Customers;
using Nop.Core.Domain.Discounts;
using Nop.Core.Domain.Forums;
using Nop.Core.Domain.Messages;
using Nop.Core.Domain.News;
using Nop.Core.Domain.Orders;
using Nop.Core.Domain.Shipping;
using Nop.Core.Domain.Vendors;
using Nop.Plugin.TorchDesign.Testimonials.Domain;
using Nop.Plugin.TorchDesign.Testimonials.Models;

namespace Nop.Plugin.TorchDesign.Testimonials.Services
{
    public partial interface ITestimonialMessageService 
    {
        #region Customer workflow

        int SendCustomerTestimonialMail(Td_Testimonials testimonial, int[] pictureid, int languageId); 

        #endregion 


    }
}
