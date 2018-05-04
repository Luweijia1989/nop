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

        /// <summary>
        /// Deletes a video
        /// </summary>
        /// <param name="video">Video</param>
        void DeleteVideo(Video video);

        /// <summary>
        /// Gets videos by product identifier
        /// </summary>
        /// <param name="productId">Product identifier</param>
        /// <param name="recordsToReturn">Number of records to return. 0 if you want to get all items</param>
        /// <returns>Pictures</returns>
        IList<Video> GetVideosByProductId(int productId, int recordsToReturn = 0);
    }
}
