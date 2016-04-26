using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using VideoLab.Web.Services.VideoLab;

namespace VideoLab.Web.Controllers
{
    public class BrowseController : Controller
    {
        private readonly IVideoService _service;
        public BrowseController(IVideoService service)
        {
            _service = service;
        }

        [HttpPost]
        public ActionResult Results(string[] tagsInput)
        {
            return View(tagsInput.ToList());
        }

        public PartialViewResult BrowseResult(IList<string> tagsInput)
        {
            var videos = _service.FetchVideos(tagsInput);
            return PartialView(@"..\Partial\BrowseResults", videos);
        }
    }
}