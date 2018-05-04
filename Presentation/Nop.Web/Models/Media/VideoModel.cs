using Nop.Web.Framework.Mvc;

namespace Nop.Web.Models.Media
{
    public partial class VideoModel : BaseNopModel
    {
        public string VideoUrl { get; set; }
        public string VideoThumbUrl { get; set; }
    }
}