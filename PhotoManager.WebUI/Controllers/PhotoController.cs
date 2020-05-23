using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PhotoManager.Domain.Abstract;
using PhotoManager.Domain.Entities;
using PhotoManager.WebUI.Models;

namespace PhotoManager.WebUI.Controllers
{
    public class PhotoController : Controller
    {
        private IPhotoRepository repository;
        public int pageSize = 9;

        public PhotoController(IPhotoRepository repo)
        {
            repository = repo;
        }

        
        public ViewResult List(string category, int page = 1)
        {
            PhotosListViewModel model = new PhotosListViewModel
            {
                Photos = repository.Photos
                    .Where(p => category == null || p.Category == category)
                    .OrderBy(photo => photo.PhotoId)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = pageSize,
                    TotalItems = category == null ?
                    repository.Photos.Count() :
                    repository.Photos.Where(photo => photo.Category == category).Count()
                },
                CurrentCategory = category
            };
            return View(model);
        }

        // Вывод списка всех фото
        public ViewResult Index()
        {
            return View(repository.Photos);
        }

        // Редактирование фото
        public ViewResult Edit(int photoId)
        {
            Photo photo = repository.Photos
                .FirstOrDefault(g => g.PhotoId == photoId);
            return View(photo);
        }

        [HttpPost]
        public ActionResult Edit(Photo photo, HttpPostedFileBase image = null)
        {
            if (ModelState.IsValid)
            {
                if (image != null)
                {
                    photo.ImageMimeType = image.ContentType;
                    photo.ImageData = new byte[image.ContentLength];
                    image.InputStream.Read(photo.ImageData, 0, image.ContentLength);
                }
                repository.SavePhoto(photo);
                TempData["message"] = string.Format("\"{0}\" have been saved", photo.Name);
                return RedirectToAction("List");
            }
            else
            {
                // Что-то не так со значениями данных
                return View(photo);
            }
        }

        // Добавление фото
        public ViewResult Create()
        {
            return View("Create", new Photo());
        }

        public ActionResult Сreate(Photo photo, HttpPostedFileBase image = null)
        {
            if (ModelState.IsValid)
            {
                if (image != null)
                {
                    photo.ImageMimeType = image.ContentType;
                    photo.ImageData = new byte[image.ContentLength];
                    image.InputStream.Read(photo.ImageData, 0, image.ContentLength);
                }
                repository.SavePhoto(photo);
                TempData["message"] = string.Format("\"{0}\" have been added", photo.Name);
                return RedirectToAction("List");
            }
            else
            {
                // Что-то не так со значениями данных
                return View(photo);
            }           
        }

        // Удаление фото
        [HttpPost]
        public ActionResult Delete(int photoId)
        {
            Photo deletedPhoto = repository.DeletePhoto(photoId);
            if (deletedPhoto != null)
            {
                TempData["message"] = string.Format("\"{0}\" has been deleted",
                    deletedPhoto.Name);
            }
            return RedirectToAction("List");
        }

        public FileContentResult GetImage(int photoId)
        {
            Photo photo = repository.Photos
                .FirstOrDefault(g => g.PhotoId == photoId);

            if (photo != null)
            {
                return File(photo.ImageData, photo.ImageMimeType);
            }
            else
            {
                return null;
            }
        }

    }

}
