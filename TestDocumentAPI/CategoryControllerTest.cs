using DocumentAPI.Abstract;
using DocumentAPI.Concrete;
using DocumentAPI.Controllers;
using DocumentAPI.DAL;
using DocumentAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Configuration;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Microsoft.EntityFrameworkCore.SqlServer;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TestDocumentAPI
{
    public class CategoryControllerTest
    {
        #region snippet_GetCategories
        [Fact]
        public void GetCategories_ReturnOkResult()
        {
            // Arrange
            var mockRepo = new Mock<ICategoryRepo>();
            mockRepo.Setup(repo => repo.GetCategories())
                .Returns(GetCategories());
            var controller = new CategoryController(mockRepo.Object);

            // Act
            var result = controller.GetCategories();

            //Assert
            Assert.IsType<OkObjectResult>(result);

            var okObjectResult = result as OkObjectResult;
            Assert.IsType<List<Category>>(okObjectResult.Value);

            var model = okObjectResult.Value as List<Category>;
            Assert.Equal(2, model.Count);
        }

        #endregion

        #region snippet_AddCategory
        [Fact]
        public void AddCategory_ReturnBadRequestResult_WhenModelStateIsInvalid()
        {
            //Arrange
            var mockRepo = new Mock<ICategoryRepo>();
            var controller = new CategoryController(mockRepo.Object);
            controller.ModelState.AddModelError("error", "some error");

            //Act
            var result = controller.AddCategory(addCategoryModel: null);

            //Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void AddCategory_ReturnCreatedItem_WhenModelStateIsValid()
        {
            //Arrange
            var dbContext = new DocumentDbContext();
            dbContext.Categories = GetQueryableMockDbSetCategory();

            var categoryRepo = new CategoryRepo(string.Empty, dbContext);
            var controller = new CategoryController(categoryRepo);

            AddCategoryModel addCategoryModel = new AddCategoryModel()
            {
                Title = "Tình Cảm"
            };

            //Act
            var result = controller.AddCategory(addCategoryModel);

            //Assert
            Assert.IsType<OkObjectResult>(result);

            var okObjectResult = result as OkObjectResult;
            Assert.IsType<Category>(okObjectResult.Value);

            var categoryObject = okObjectResult.Value as Category;
            Assert.Equal(categoryObject.Title, addCategoryModel.Title);
        }

        #endregion

        #region snippet_UpdateCategory
        [Fact]
        public void UpdateCategory_ReturnBadRequestResult_WhenModelStateIsInvalid()
        {
            //Arrange
            var mockRepo = new Mock<ICategoryRepo>();
            var controller = new CategoryController(mockRepo.Object);
            controller.ModelState.AddModelError("error", "some error");

            //Act
            var result = controller.UpdateCategory(updateCategoryModel: null);

            //Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void UpdateCategory_ReturnNotFoundResult_WhenCategoryNotFound()
        {
            //Arrange
            var dbContext = new DocumentDbContext();
            dbContext.Categories = GetQueryableMockDbSetCategory();

            var categoryRepo = new CategoryRepo(string.Empty, dbContext);
            var controller = new CategoryController(categoryRepo);

            UpdateCategoryModel updateCategoryModel = new UpdateCategoryModel()
            {
                Id = 3,
                Title = "Khoa Học"
            };

            //Act
            var result = controller.UpdateCategory(updateCategoryModel);

            //Assert
            Assert.IsType<NotFoundObjectResult>(result);

        }

        [Fact]
        public void UpdateCategory_ReturnUpdateItem_WhenStateIsValidAndCategoryExist()
        {
            //Arrange
            var dbContext = new DocumentDbContext();
            dbContext.Categories = GetQueryableMockDbSetCategory();

            var categoryRepo = new CategoryRepo(string.Empty, dbContext);
            var controller = new CategoryController(categoryRepo);

            UpdateCategoryModel updateCategoryModel = new UpdateCategoryModel()
            {
                Id = 1,
                Title = "Khoa Học"
            };

            //Act
            var result = controller.UpdateCategory(updateCategoryModel);

            //Assert
            Assert.IsType<OkObjectResult>(result);

            var okObjectResult = result as OkObjectResult;
            Assert.IsType<Category>(okObjectResult.Value);

            var categoryObject = okObjectResult.Value as Category;
            Assert.Equal(categoryObject.Id, updateCategoryModel.Id);
            Assert.Equal(categoryObject.Title, updateCategoryModel.Title);
        }

        #endregion

        #region snippet_RemoveCategory

        [Fact]
        public void RemoveCategory_ReturnNotFoundResult_WhenCategoryNotFound()
        {
            //Arrange
            var dbContext = new DocumentDbContext();
            dbContext.Categories = GetQueryableMockDbSetCategory();

            var categoryRepo = new CategoryRepo(string.Empty, dbContext);
            var controller = new CategoryController(categoryRepo);

            int testCategoryId = 3;

            //Act
            var result = controller.RemoveCategory(testCategoryId);

            //Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public void RemoveCategory_ReturnRemovedItem_WhenCategoryExist()
        {
            //Arrange
            var dbContext = new DocumentDbContext();
            dbContext.Categories = GetQueryableMockDbSetCategory();

            var categoryRepo = new CategoryRepo(string.Empty, dbContext);
            var controller = new CategoryController(categoryRepo);

            int testCategoryId = 1;

            //Act
            var result = controller.RemoveCategory(testCategoryId);

            //Assert
            Assert.IsType<OkObjectResult>(result);

            var okObjectResult = result as OkObjectResult;
            Assert.IsType<Category>(okObjectResult.Value);

            var categoryObject = okObjectResult.Value as Category;
            Assert.Equal(categoryObject.Id, testCategoryId);

        }

        #endregion

        public List<Category> GetCategories()
        {
            var categories = new List<Category>();
            categories.Add(new Category() { Id = 1, Title = "Đời sống" });
            categories.Add(new Category() { Id = 2, Title = "Khoa học" });
            return categories;
        }

        private DbSet<Category> GetQueryableMockDbSetCategory()
        {
            List<Category> listCategory = GetCategories();
            IQueryable<Category> queryableList = listCategory.AsQueryable();

            var mockDbSet = new Mock<DbSet<Category>>();
            mockDbSet.As<IQueryable<Category>>().Setup(m => m.Provider).Returns(queryableList.Provider);
            mockDbSet.As<IQueryable<Category>>().Setup(m => m.Expression).Returns(queryableList.Expression);
            mockDbSet.As<IQueryable<Category>>().Setup(m => m.ElementType).Returns(queryableList.ElementType);
            mockDbSet.As<IQueryable<Category>>().Setup(m => m.GetEnumerator()).Returns(queryableList.GetEnumerator());

            return mockDbSet.Object;
        }
    }
}
