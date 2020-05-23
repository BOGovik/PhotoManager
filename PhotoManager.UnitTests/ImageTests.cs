using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PhotoManager.Domain.Abstract;
using PhotoManager.Domain.Entities;
using PhotoManager.WebUI.Controllers;


namespace PhotoManager.UnitTests
{
    [TestClass]
    public class ImageTests
    {
        [TestMethod]
        public void Can_Retrieve_Image_Data()
        {
            // Организация - создание объекта Game с данными изображения
            Photo photo = new Photo
            {
                PhotoId = 2,
                Name = "Фото2",
                ImageData = new byte[] { },
                ImageMimeType = "image/png"
            };

            // Организация - создание имитированного хранилища
            Mock<IPhotoRepository> mock = new Mock<IPhotoRepository>();
            mock.Setup(m => m.Photos).Returns(new List<Photo> {
                new Photo {PhotoId = 1, Name = "Фото1"},
                photo,
                new Photo {PhotoId = 3, Name = "Фото3"}
            }.AsQueryable());

            // Организация - создание контроллера
            PhotoController controller = new PhotoController(mock.Object);

            // Действие - вызов метода действия GetImage()
            ActionResult result = controller.GetImage(2);

            // Утверждение
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(FileResult));
            Assert.AreEqual(photo.ImageMimeType, ((FileResult)result).ContentType);
        }

        [TestMethod]
        public void Cannot_Retrieve_Image_Data_For_Invalid_ID()
        {
            // Организация - создание имитированного хранилища
            Mock<IPhotoRepository> mock = new Mock<IPhotoRepository>();
            mock.Setup(m => m.Photos).Returns(new List<Photo> {
                new Photo {PhotoId = 1, Name = "Фото1"},
                new Photo {PhotoId = 2, Name = "Фото2"}
            }.AsQueryable());

            // Организация - создание контроллера
            PhotoController controller = new PhotoController(mock.Object);

            // Действие - вызов метода действия GetImage()
            ActionResult result = controller.GetImage(10);

            // Утверждение
            Assert.IsNull(result);
        }
    }
}