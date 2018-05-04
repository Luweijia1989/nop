using System.Collections.Generic;
using Nop.Core;
using Nop.Core.Domain.Media;

namespace Nop.Services.Media
{
    /// <summary>
    /// Picture service interface
    /// </summary>
    public partial interface IVideoService
    {
        Video GetVideoById(int videoId);

        Video InsertVideo(byte[] videoBinary, string extension, string mimeType, string seoFilename,
            bool isNew = true);

        string GetVideoUrl(Video video);

        string GetVideoThumbUrl(Video video);

        string GetDefaultVideoThumbUrl();
    }
}
