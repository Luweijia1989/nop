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
using Nop.Services.Configuration;
using System.Collections.Generic;
using System.Linq;

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
        private readonly ISettingService _settingService;
        private readonly IWebHelper _webHelper;
        private readonly IEventPublisher _eventPublisher;

        private string ak = "0a7txWBaXeWgxHgA-rvYl-ouKgS5J75ITLaWaUo-";
        private string sk = "Ep15IyuVbQeVIcj5zi0DGgUzwW3TS-3Vl3PqEAbZ";
        private string videoDomain = "http://files.taijibaijia.com/";
        string bucket = "test";
        private Mac mac;

        public VideoService(IRepository<Video> videoRepository,
            IRepository<ProductVideo> productVideoRepository,
            IDbContext dbContext,
            ISettingService settingService,
            IWebHelper webHelper,
            IEventPublisher eventPublisher
            )
        {
            this._videoRepository = videoRepository;
            this._productVideoRepository = productVideoRepository;
            this._dbContext = dbContext;
            this._settingService = settingService;
            this._webHelper = webHelper;
            this._eventPublisher = eventPublisher;

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
            HttpResult result = um.UploadData(videoBinary, saveKey, token);

            var video = new Video
            {
                VideoUrl = saveKey,
                MimeType = mimeType,
                SeoFilename = seoFilename,
                IsNew = isNew,
            };
            _videoRepository.Insert(video);

            _eventPublisher.EntityInserted(video);

            return video;
        }

        public string GetVideoUrl(Video video)
        {
            if (video == null)
                return "";
            return videoDomain + video.VideoUrl;
        }

        public string GetVideoThumbUrl(Video video)
        {
            if (video == null)
                return GetDefaultVideoThumbUrl();
            return GetVideoUrl(video) + "?vframe/jpg/offset/1";
        }

        public string GetDefaultVideoThumbUrl()
        { 
            var defaultImageFileName = _settingService.GetSettingByKey("Media.DefaultImageName", "default-image.png");
                    
            return _webHelper.GetStoreLocation() + "content/images/" + defaultImageFileName;
        }

        public void DeleteVideo(Video video)
        {
            if (video == null)
                throw new ArgumentNullException("video");

            //delete from database
            _videoRepository.Delete(video);

            //event notification
            _eventPublisher.EntityDeleted(video);
        }

        public IList<Video> GetVideosByProductId(int productId, int recordsToReturn = 0)
        {
            if (productId == 0)
                return new List<Video>();


            var query = from p in _videoRepository.Table
                        join pp in _productVideoRepository.Table on p.Id equals pp.VideoId
                        orderby pp.DisplayOrder, pp.Id
                        where pp.ProductId == productId
                        select p;

            if (recordsToReturn > 0)
                query = query.Take(recordsToReturn);

            var vids = query.ToList();
            return vids;
        }
    }
}
