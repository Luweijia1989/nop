namespace Nop.Core.Domain.Media
{
    /// <summary>
    /// Represents a video
    /// </summary>
    public partial class Video : BaseEntity
    {
        /// <summary>
        /// Gets or sets the picture binary
        /// </summary>
        public string VideoUrl { get; set; }

        /// <summary>
        /// Gets or sets the picture mime type
        /// </summary>
        public string MimeType { get; set; }

        /// <summary>
        /// Gets or sets the SEO friednly filename of the picture
        /// </summary>
        public string SeoFilename { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the picture is new
        /// </summary>
        public bool IsNew { get; set; }
    }
}
