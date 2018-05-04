using Nop.Core.Domain.Media;

namespace Nop.Data.Mapping.Media
{
    public partial class VideoMap : NopEntityTypeConfiguration<Video>
    {
        public VideoMap()
        {
            this.ToTable("Video");
            this.HasKey(p => p.Id);
            this.Property(p => p.VideoUrl).IsMaxLength();
            this.Property(p => p.MimeType).IsRequired().HasMaxLength(40);
            this.Property(p => p.SeoFilename).HasMaxLength(300);
        }
    }
}