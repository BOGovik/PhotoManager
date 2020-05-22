using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PhotoManager.Domain.Abstract;
using PhotoManager.Domain.Entities;
using PhotoManager.WebUI.Controllers;
using PhotoManager.WebUI.Models;
using PhotoManager.WebUI.HtmlHelpers;

namespace PhotoManager.UnitTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void Can_Paginate()
        {
            // Организация (arrange)
            Mock<IPhotoRepository> mock = new Mock<IPhotoRepository>();
            mock.Setup(m => m.Photos).Returns(new List<Photo>
                {
                    new Photo { PhotoId = 1, Name = "Фото1"},
                    new Photo { PhotoId = 2, Name = "Фото2"},
                    new Photo { PhotoId = 3, Name = "Фото3"},
                    new Photo { PhotoId = 4, Name = "Фото4"},
                    new Photo { PhotoId = 5, Name = "Фото5"},
                    new Photo { PhotoId = 6, Name = "Фото6"},
                    new Photo { PhotoId = 7, Name = "Фото7"},
                    new Photo { PhotoId = 8, Name = "Фото8"},
                    new Photo { PhotoId = 9, Name = "Фото9"},
                    new Photo { PhotoId = 10, Name = "Фото10"},
                    new Photo { PhotoId = 11, Name = "Фото11"},
                });
            PhotoController controller = new PhotoController(mock.Object);
            controller.pageSize = 9;

            // Действие (act)
            PhotosListViewModel result = (PhotosListViewModel)controller.List(null, 2).Model;

            // Утверждение
            List<Photo> photos = result.Photos.ToList();
            Assert.IsTrue(photos.Count == 2);
            Assert.AreEqual(photos[0].Name, "Фото12");
            Assert.AreEqual(photos[1].Name, "Фото13");
        }
        [TestMethod]
        public void Can_Generate_Page_Links()
        {

            // Организация - определение вспомогательного метода HTML - это необходимо
            // для применения расширяющего метода
            HtmlHelper myHelper = null;

            // Организация - создание объекта PagingInfo
            PagingInfo pagingInfo = new PagingInfo
            {
                CurrentPage = 2,
                TotalItems = 28,
                ItemsPerPage = 10
            };

            // Организация - настройка делегата с помощью лямбда-выражения
            Func<int, string> pageUrlDelegate = i => "Page" + i;

            // Действие
            MvcHtmlString result = myHelper.PageLinks(pagingInfo, pageUrlDelegate);

            // Утверждение
            Assert.AreEqual(@"<a class=""btn btn-default"" href=""Page1"">1</a>"
                + @"<a class=""btn btn-default btn-primary selected"" href=""Page2"">2</a>"
                + @"<a class=""btn btn-default"" href=""Page3"">3</a>",
                result.ToString());
        }

        public void Can_Send_Pagination_View_Model()
        {
            // Организация (arrange)
            Mock<IPhotoRepository> mock = new Mock<IPhotoRepository>();
            mock.Setup(m => m.Photos).Returns(new List<Photo>
    {
                    new Photo { PhotoId = 1, Name = "Фото1"},
                    new Photo { PhotoId = 2, Name = "Фото2"},
                    new Photo { PhotoId = 3, Name = "Фото3"},
                    new Photo { PhotoId = 4, Name = "Фото4"},
                    new Photo { PhotoId = 5, Name = "Фото5"},
                    new Photo { PhotoId = 6, Name = "Фото6"},
                    new Photo { PhotoId = 7, Name = "Фото7"},
                    new Photo { PhotoId = 8, Name = "Фото8"},
                    new Photo { PhotoId = 9, Name = "Фото9"},
                    new Photo { PhotoId = 10, Name = "Фото10"},
                    new Photo { PhotoId = 11, Name = "Фото11"},

    });
            PhotoController controller = new PhotoController(mock.Object);
            controller.pageSize = 9;

            // Act
            PhotosListViewModel result
                = (PhotosListViewModel)controller.List(null, 2).Model;

            // Assert
            PagingInfo pageInfo = result.PagingInfo;
            Assert.AreEqual(pageInfo.CurrentPage, 2);
            Assert.AreEqual(pageInfo.ItemsPerPage, 3);
            Assert.AreEqual(pageInfo.TotalItems, 5);
            Assert.AreEqual(pageInfo.TotalPages, 2);
        }

        [TestMethod]
        public void Can_Filter_Photos()
        {
            // Организация (arrange)
            Mock<IPhotoRepository> mock = new Mock<IPhotoRepository>();
            mock.Setup(m => m.Photos).Returns(new List<Photo>
    {
        new Photo { PhotoId = 1, Name = "Фото1", Category="Cat1"},
        new Photo { PhotoId = 2, Name = "Фото2", Category="Cat2"},
        new Photo { PhotoId = 3, Name = "Фото3", Category="Cat1"},
        new Photo { PhotoId = 4, Name = "Фото4", Category="Cat2"},
        new Photo { PhotoId = 5, Name = "Фото5", Category="Cat3"}
    });
            PhotoController controller = new PhotoController(mock.Object);
            controller.pageSize = 3;

            // Action
            List<Photo> result = ((PhotosListViewModel)controller.List("Cat2", 1).Model)
                .Photos.ToList();

            // Assert
            Assert.AreEqual(result.Count(), 2);
            Assert.IsTrue(result[0].Name == "Фото2" && result[0].Category == "Cat2");
            Assert.IsTrue(result[1].Name == "Фото4" && result[1].Category == "Cat2");
        }

        [TestMethod]
        public void Can_Create_Categories()
        {
            // Организация - создание имитированного хранилища
            Mock<IPhotoRepository> mock = new Mock<IPhotoRepository>();
            mock.Setup(m => m.Photos).Returns(new List<Photo> {
        new Photo { PhotoId = 1, Name = "Фото1", Category="Фон"},
        new Photo { PhotoId = 2, Name = "Фото2", Category="Портрет"},
        new Photo { PhotoId = 3, Name = "Фото3", Category="Фон"},
        new Photo { PhotoId = 4, Name = "Фото4", Category="Портрет"},
    });

            // Организация - создание контроллера
            NavController target = new NavController(mock.Object);

            // Действие - получение набора категорий
            List<string> results = ((IEnumerable<string>)target.Menu().Model).ToList();

            // Утверждение
            Assert.AreEqual(results.Count(), 3);
            Assert.AreEqual(results[0], "Фон");
            Assert.AreEqual(results[1], "Фон");
            Assert.AreEqual(results[2], "Портрет");
        }

        [TestMethod]
        public void Indicates_Selected_Category()
        {
            // Организация - создание имитированного хранилища
            Mock<IPhotoRepository> mock = new Mock<IPhotoRepository>();
            mock.Setup(m => m.Photos).Returns(new Photo[] {
        new Photo { PhotoId = 1, Name = "Фото1", Category="Портрет"},
        new Photo { PhotoId = 2, Name = "Фото2", Category="Фон"}
    });

            // Организация - создание контроллера
            NavController target = new NavController(mock.Object);

            // Организация - определение выбранной категории
            string categoryToSelect = "Фон";

            // Действие
            string result = target.Menu(categoryToSelect).ViewBag.SelectedCategory;

            // Утверждение
            Assert.AreEqual(categoryToSelect, result);
        }

        [TestMethod]
        public void Generate_Category_Specific_Photo_Count()
        {
            /// Организация (arrange)
            Mock<IPhotoRepository> mock = new Mock<IPhotoRepository>();
            mock.Setup(m => m.Photos).Returns(new List<Photo>
    {
        new Photo { PhotoId = 1, Name = "Фото1", Category="Cat1"},
        new Photo { PhotoId = 2, Name = "Фото2", Category="Cat2"},
        new Photo { PhotoId = 3, Name = "Фото3", Category="Cat1"},
        new Photo { PhotoId = 4, Name = "Фото4", Category="Cat2"},
        new Photo { PhotoId = 5, Name = "Фото5", Category="Cat3"}
    });
            PhotoController controller = new PhotoController(mock.Object);
            controller.pageSize = 3;

            // Действие - тестирование счетчиков товаров для различных категорий
            int res1 = ((PhotosListViewModel)controller.List("Cat1").Model).PagingInfo.TotalItems;
            int res2 = ((PhotosListViewModel)controller.List("Cat2").Model).PagingInfo.TotalItems;
            int res3 = ((PhotosListViewModel)controller.List("Cat3").Model).PagingInfo.TotalItems;
            int resAll = ((PhotosListViewModel)controller.List(null).Model).PagingInfo.TotalItems;

            // Утверждение
            Assert.AreEqual(res1, 2);
            Assert.AreEqual(res2, 2);
            Assert.AreEqual(res3, 1);
            Assert.AreEqual(resAll, 5);
        }

        [TestMethod]
        public void Index_Contains_All_Photos()
        {
            // Организация - создание имитированного хранилища данных
            Mock<IPhotoRepository> mock = new Mock<IPhotoRepository>();
            mock.Setup(m => m.Photos).Returns(new List<Photo>
            {
                    new Photo { PhotoId = 1, Name = "Фото1"},
                    new Photo { PhotoId = 2, Name = "Фото2"},
                    new Photo { PhotoId = 3, Name = "Фото3"},
                    new Photo { PhotoId = 4, Name = "Фото4"},
                    new Photo { PhotoId = 5, Name = "Фото5"}
            });

            // Организация - создание контроллера
            PhotoController controller = new PhotoController(mock.Object);

            // Действие
            List<Photo> result = ((IEnumerable<Photo>)controller.Index().
                ViewData.Model).ToList();

            // Утверждение
            Assert.AreEqual(result.Count(), 5);
            Assert.AreEqual("Фото1", result[0].Name);
            Assert.AreEqual("Фото2", result[1].Name);
            Assert.AreEqual("Фото3", result[2].Name);
        }

        [TestMethod]
        public void Can_Edit_Photo()
        {
            // Организация - создание имитированного хранилища данных
            Mock<IPhotoRepository> mock = new Mock<IPhotoRepository>();
            mock.Setup(m => m.Photos).Returns(new List<Photo>
            {
                    new Photo { PhotoId = 1, Name = "Фото1"},
                    new Photo { PhotoId = 2, Name = "Фото2"},
                    new Photo { PhotoId = 3, Name = "Фото3"},
                    new Photo { PhotoId = 4, Name = "Фото4"},
                    new Photo { PhotoId = 5, Name = "Фото5"}
            });

            // Организация - создание контроллера
            PhotoController controller = new PhotoController(mock.Object);

            // Действие
            Photo photo1 = controller.Edit(1).ViewData.Model as Photo;
            Photo photo2 = controller.Edit(2).ViewData.Model as Photo;
            Photo photo3 = controller.Edit(3).ViewData.Model as Photo;

            // Assert
            Assert.AreEqual(1, photo1.PhotoId);
            Assert.AreEqual(2, photo2.PhotoId);
            Assert.AreEqual(3, photo3.PhotoId);
        }

        [TestMethod]
        public void Cannot_Edit_Nonexistent_Photo()
        {
            // Организация - создание имитированного хранилища данных
            Mock<IPhotoRepository> mock = new Mock<IPhotoRepository>();
            mock.Setup(m => m.Photos).Returns(new List<Photo>
            {
                    new Photo { PhotoId = 1, Name = "Фото1"},
                    new Photo { PhotoId = 2, Name = "Фото2"},
                    new Photo { PhotoId = 3, Name = "Фото3"},
                    new Photo { PhotoId = 4, Name = "Фото4"},
                    new Photo { PhotoId = 5, Name = "Фото5"}
            });

            // Организация - создание контроллера
            PhotoController controller = new PhotoController(mock.Object);

            // Действие
            Photo result = controller.Edit(6).ViewData.Model as Photo;

            // Assert
        }

        [TestMethod]
        public void Can_Save_Valid_Changes()
        {
            // Организация - создание имитированного хранилища данных
            Mock<IPhotoRepository> mock = new Mock<IPhotoRepository>();

            // Организация - создание контроллера
            PhotoController controller = new PhotoController(mock.Object);

            // Организация - создание объекта Game
            Photo photo = new Photo { Name = "Test" };

            // Действие - попытка сохранения товара
            ActionResult result = controller.Edit(photo);

            // Утверждение - проверка того, что к хранилищу производится обращение
            mock.Verify(m => m.SavePhoto(photo));

            // Утверждение - проверка типа результата метода
            Assert.IsNotInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void Cannot_Save_Invalid_Changes()
        {
            // Организация - создание имитированного хранилища данных
            Mock<IPhotoRepository> mock = new Mock<IPhotoRepository>();

            // Организация - создание контроллера
            PhotoController controller = new PhotoController(mock.Object);

            // Организация - создание объекта Game
            Photo game = new Photo { Name = "Test" };

            // Организация - добавление ошибки в состояние модели
            controller.ModelState.AddModelError("error", "error");

            // Действие - попытка сохранения товара
            ActionResult result = controller.Edit(photo);

            // Утверждение - проверка того, что обращение к хранилищу НЕ производится 
            mock.Verify(m => m.SavePhoto(It.IsAny<Photo>()), Times.Never());

            // Утверждение - проверка типа результата метода
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void Can_Delete_Valid_Photos()
        {
            // Организация - создание объекта Фото
            Photo photo = new Photo { PhotoId = 2, Name = "Фото2" };

            // Организация - создание имитированного хранилища данных
            Mock<IPhotoRepository> mock = new Mock<IPhotoRepository>();
            mock.Setup(m => m.Photos).Returns(new List<Photo>
    {
                    new Photo { PhotoId = 1, Name = "Фото1"},
                    new Photo { PhotoId = 2, Name = "Фото2"},
                    new Photo { PhotoId = 3, Name = "Фото3"},
                    new Photo { PhotoId = 4, Name = "Фото4"},
                    new Photo { PhotoId = 5, Name = "Фото5"}
    });

            // Организация - создание контроллера
            PhotoController controller = new PhotoController(mock.Object);

            // Действие - удаление фото
            controller.Delete(photo.PhotoId);

            // Утверждение - проверка того, что метод удаления в хранилище
            // вызывается для корректного объекта Фото
            mock.Verify(m => m.DeletePhoto(photo.PhotoId));
        }
    }
}
