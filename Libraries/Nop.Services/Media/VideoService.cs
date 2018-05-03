using Nop.Core;
using Nop.Core.Data;
using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.Media;
using Nop.Data;
using Nop.Services.Events;

using Qiniu.Util;
using Qiniu.Common;
using Qiniu.IO;
using Qiniu.IO.Model;
using Qiniu.Http;

using System;

namespace Nop.Services.Media
{
    /// <summary>
    /// Picture service
    /// </summary>
    public partial class VideoService : IVideoService
    {
        private readonly IRepository<Video> _videoRepository;
        private readonly IRepository<ProductVideo> _productVideoRepository;
        private readonly IDbContext _dbContext;

        private string ak = "0a7txWBaXeWgxHgA-rvYl-ouKgS5J75ITLaWaUo-";
        private string sk = "Ep15IyuVbQeVIcj5zi0DGgUzwW3TS-3Vl3PqEAbZ";
        private string videoDomain = "http://files.taijibaijia.com/";
        string bucket = "test";
        private Mac mac;

        public VideoService(IRepository<Video> videoRepository,
            IRepository<ProductVideo> productVideoRepository,
            IDbContext dbContext
            )
        {
            this._videoRepository = videoRepository;
            this._productVideoRepository = productVideoRepository;
            this._dbContext = dbContext;

            mac = new Mac(ak, sk);
        }

        public Video GetVideoById(int videoId)
        {
            if (videoId == 0)
                return null;

            return _videoRepository.GetById(videoId);
        }

        public Video InsertVideo(byte[] videoBinary, string extension, string mimeType, string seoFilename, bool isNew = true)
        {
            string saveKey = Guid.NewGuid().ToString() + extension;
            PutPolicy putPolicy = new PutPolicy();
            putPolicy.Scope = bucket + ":" + saveKey;
            putPolicy.SetExpires(3600);
            putPolicy.InsertOnly = 1;
            string jstr = putPolicy.ToJsonString();
            string token = Auth.CreateUploadToken(mac, jstr);
            UploadManager um = new UploadManager();
            //HttpResult result = um.UploadData(videoBinary, saveKey, token);

            var video = new Video
            {
                VideoUrl = saveKey,
                MimeType = mimeType,
                SeoFilename = seoFilename,
                IsNew = isNew,
            };
            _videoRepository.Insert(video);

            return video;
        }

        public string GetVideoUrl(Video video)
        {
            return videoDomain + video.VideoUrl;
        }
    }
}
