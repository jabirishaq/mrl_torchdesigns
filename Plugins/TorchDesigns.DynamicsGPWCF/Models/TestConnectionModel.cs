using Nop.Plugin.TorchDesigns.DynamicsGPWCF.DynamicsGP;
using System.Collections.Generic;

namespace Nop.Plugin.TorchDesigns.DynamicsGPWCF.Models
{
    public partial class TestConnectionModel
    {
        public TestConnectionModel()
        {
             CompanyName = new List<string>();
            CustomerName= new List<string>();
            Companies = new List<Company>();
            Errors = new List<string>();

        }
        public List<string> CompanyName { get; set; }

        public List<string> CustomerName { get; set; }
        public List<string> Errors { get; set; }

        public List<Company> Companies { get; set; }

    }
}
