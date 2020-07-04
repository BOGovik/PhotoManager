using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PhotoManager.Domain.Abstract;

namespace PhotoManager.WebUI.Controllers
{
    public class NavController : Controller
    {
        private IPhotoRepository repository;

        public NavController(IPhotoRepository repo)
        {
            repository = repo;
        }

        public PartialViewResult Menu(string category = null)
        {
            ViewBag.SelectedCategory = category;

            IEnumerable<string> categories = repository.Photos
                .Select(photo => photo.Category)
                .Distinct()
                .OrderBy(x => x);
            return PartialView("Menu", categories);
        }
    }
}
