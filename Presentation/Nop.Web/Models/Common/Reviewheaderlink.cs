using Nop.Web.Framework.Mvc;

namespace Nop.Web.Models.Common
{
    public partial class Reviewheaderlink : BaseNopModel
    {
        public bool IsAuthenticated { get; set; }
        public string CustomerEmailUsername { get; set; }
        public int productid { get; set; }
      
    }
}