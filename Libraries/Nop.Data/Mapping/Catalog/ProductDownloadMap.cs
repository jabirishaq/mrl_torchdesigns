using System.Data.Entity.ModelConfiguration;
using Nop.Core.Domain.Catalog;

namespace Nop.Data.Mapping.Catalog
{
    public partial class ProductDownloadMap : EntityTypeConfiguration<ProductDownload>
    {
        public ProductDownloadMap()
        {
            this.ToTable("Td_Product_Download_Mapping");
            this.HasKey(pp => pp.Id);

            this.HasRequired(pp => pp.Download)
                .WithMany(p => p.ProductDownloads)
                .HasForeignKey(pp => pp.DownloadId);


            this.HasRequired(pp => pp.Product)
                .WithMany(p => p.ProductDownloadsP)
                .HasForeignKey(pp => pp.ProductId);


            //this.HasRequired(pp => pp.Picture)
            //   .WithMany(p => p.ProductPictures)
            //   .HasForeignKey(pp => pp.PictureId);


            //this.HasRequired(pp => pp.Product)
            //    .WithMany(p => p.ProductPictures)
            //    .HasForeignKey(pp => pp.ProductId);
        }
    }
}