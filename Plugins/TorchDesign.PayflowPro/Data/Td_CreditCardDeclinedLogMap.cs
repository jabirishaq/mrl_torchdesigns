using Nop.Plugin.TorchDesign.PayflowPro.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Plugin.TorchDesign.PayflowPro.Data
{
    public partial class Td_CreditCardDeclinedLogMap : EntityTypeConfiguration<Td_CreditCardDeclinedLog>
    {
        public Td_CreditCardDeclinedLogMap()
        {
            this.ToTable("Td_CreditCardDeclinedLog");
            this.HasKey(t => t.Id);
        }
    }
}
