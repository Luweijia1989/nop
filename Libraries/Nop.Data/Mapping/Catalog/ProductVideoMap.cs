using Nop.Core.Domain.Catalog;

namespace Nop.Data.Mapping.Catalog
{
    public partial class ProductVideoMap : NopEntityTypeConfiguration<ProductVideo>
    {
        public ProductVideoMap()
        {
            this.ToTable("Product_Video_Mapping");
            this.HasKey(pp => pp.Id);
            
            this.HasRequired(pp => pp.Video)
                .WithMany()
                .HasForeignKey(pp => pp.VideoId);


            this.HasRequired(pp => pp.Product)
                .WithMany(p => p.ProductVideos)
                .HasForeignKey(pp => pp.ProductId);
        }
    }
}