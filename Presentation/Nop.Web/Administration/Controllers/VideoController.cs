using System;
using System.IO;
using System.Web;
using System.Web.Mvc;
using Nop.Core;
using Nop.Services.Media;
using Nop.Web.Framework.Security;

namespace Nop.Admin.Controllers
{
    public partial class VideoController : BaseAdminController
    {
        private readonly IVideoService _videoService;

        public VideoController(IVideoService videoService)
        {
            this._videoService = videoService;
        }

        [HttpPost]
        //do not validate request token (XSRF)
        [AdminAntiForgery(true)] 
        public virtual ActionResult AsyncUpload()
        {
            //if (!_permissionService.Authorize(StandardPermissionProvider.UploadPictures))
            //    return Json(new { success = false, error = "You do not have required permissions" }, "text/plain");

            //we process it distinct ways based on a browser
            //find more info here http://stackoverflow.com/questions/4884920/mvc3-valums-ajax-file-upload
            Stream stream = null;
            var fileName = "";
            var contentType = "";
            var fileExtension = "";
            if (String.IsNullOrEmpty(Request["qqfile"]))
            {
                // IE
                HttpPostedFileBase httpPostedFile = Request.Files[0];
                if (httpPostedFile == null)
                    throw new ArgumentException("No file uploaded");
                stream = httpPostedFile.InputStream;
                fileName = Path.GetFileName(httpPostedFile.FileName);
                contentType = httpPostedFile.ContentType;
            }
            else
            {
                //Webkit, Mozilla
                stream = Request.InputStream;
                fileName = Request["qqfile"];
            }
            
            var fileBinary = new byte[stream.Length];
            stream.Read(fileBinary, 0, fileBinary.Length);

            fileExtension = Path.GetExtension(fileName);
            if (!String.IsNullOrEmpty(fileExtension))
                fileExtension = fileExtension.ToLowerInvariant();
            if (String.IsNullOrEmpty(contentType))
            {
                switch (fileExtension)
                {
                    case ".flv":
                        contentType = MimeTypes.VideoFlv;
                        break;
                    case ".mp4":
                        contentType = MimeTypes.VideoMp4;
                        break;
                    case ".m3u8":
                        contentType = MimeTypes.VideoM3u8;
                        break;
                    case ".avi":
                        contentType = MimeTypes.VideoAvi;
                        break;
                    case ".wmv":
                        contentType = MimeTypes.VideoWmv;
                        break;
                    case ".3gp":
                        contentType = MimeTypes.Video3GP;
                        break;
                    case ".mov":
                        contentType = MimeTypes.VideoMov;
                        break;
                    default:
                        break;
                }
            }

            var video = _videoService.InsertVideo(fileBinary, fileExtension, contentType, null);
            //File.WriteAllBytes(file, fileBinary);

            //var picture = _pictureService.InsertPicture(fileBinary, contentType, null);
            //when returning JSON the mime-type must be set to text/plain
            //otherwise some browsers will pop-up a "Save As" dialog.
            //return Json(new
            //{
            //    success = true,
            //    pictureId = picture.Id,
            //    imageUrl = _pictureService.GetPictureUrl(picture, 100)
            //},
            //    MimeTypes.TextPlain);

            return Json(new
            {
                success = true,
                videoId = video.Id,
                videoUrl = _videoService.GetVideoUrl(video)
            },
                MimeTypes.TextPlain);
        }
    }
}
