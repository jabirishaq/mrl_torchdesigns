using Nop.Plugin.Payments.AuthorizeNet.Domain;
using System.Data.Entity.ModelConfiguration;

namespace Nop.Plugin.Payments.AuthorizeNet.Data
{
    class Td_AuthorizeNet_DeclinedCard_LogMap : EntityTypeConfiguration<Td_AuthorizeNet_DeclinedCard_Log>
    {
        public Td_AuthorizeNet_DeclinedCard_LogMap()
        {
            this.ToTable("Td_AuthorizeNet_DeclinedCard_Log");
            this.HasKey(t => t.Id);
        }
    }
}
