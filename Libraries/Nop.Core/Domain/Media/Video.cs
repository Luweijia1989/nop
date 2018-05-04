namespace Nop.Core.Domain.Media
{
    /// <summary>
    /// Represents a video
    /// </summary>
    public partial class Video : BaseEntity
    {
        /// <summary>
        /// Gets or sets the video url
        /// </summary>
        public string VideoUrl { get; set; }

        /// <summary>
        /// Gets or sets the video mime type
        /// </summary>
        public string MimeType { get; set; }

        /// <summary>
        /// Gets or sets the SEO friednly filename of the video
        /// </summary>
        public string SeoFilename { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the video is new
        /// </summary>
        public bool IsNew { get; set; }
    }
}
