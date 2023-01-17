using System.Data.Entity.ModelConfiguration;
using Nop.Plugin.Widgets.TorchDesign_StoreLocator.Domain;

namespace Nop.Plugin.Widgets.TorchDesign_StoreLocator.Data
{
    public partial class StoreLocatorRecordMap : EntityTypeConfiguration<StoreLocator>
    {
        public StoreLocatorRecordMap()
        {
            this.ToTable("Td_StoreLocator");
            this.HasKey(x => x.Id);
            this.Property(x => x.Address).HasMaxLength(100);
            this.Property(x => x.City).HasMaxLength(50);
            this.Property(x => x.Region).HasMaxLength(50);
            this.Property(x => x.CountryCode).HasMaxLength(2);
            this.Property(x => x.PostalCode).HasMaxLength(10);
            this.Property(x => x.PhoneNumber).HasMaxLength(14);
            this.Property(x => x.StoreName).HasMaxLength(50);
            this.Property(x => x.StoreType).HasMaxLength(20);
            this.Property(x => x.Latitude).HasPrecision(10, 7);
            this.Property(x => x.Longitude).HasPrecision(10, 7);
         //   this.Property(x => x.Distance).HasPrecision(10, 7);
        }
    }
}