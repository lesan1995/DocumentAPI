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
    public class DocumentControllerTest
    {
        #region snippet_GetDocuments
        [Fact]
        public void GetDocuments_ReturnOkResult()
        {
            // Arrange
            var mockDocumentRepo = new Mock<IDocumentRepo>();
            var mockCategoryRepo = new Mock<ICategoryRepo>();
            mockDocumentRepo.Setup(x => x.GetDocuments(null))
                .Returns(GetDocuments());
            var controller = new DocumentController(mockCategoryRepo.Object, mockDocumentRepo.Object);

            // Act
            var result = controller.GetDocuments(null);

            //Assert
            Assert.IsType<OkObjectResult>(result);

            var okObjectResult = result as OkObjectResult;
            Assert.IsType<List<Document>>(okObjectResult.Value);

            var model = okObjectResult.Value as List<Document>;
            Assert.Equal(2, model.Count);
        }

        #endregion

        #region snippet_AddDocument
        [Fact]
        public void AddDocument_ReturnBadRequestResult_WhenModelStateIsInvalid()
        {
            //Arrange
            var mockCategoryRepo = new Mock<ICategoryRepo>();
            var mockDocumentRepo = new Mock<IDocumentRepo>();
            var controller = new DocumentController(mockCategoryRepo.Object, mockDocumentRepo.Object);
            controller.ModelState.AddModelError("error", "some error");

            //Act
            var result = controller.AddDocument(addDocumentModel: null);

            //Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void AddDocument_ReturnNotFoundResult_WhenModelCategoryNotFound()
        {
            //Arrange
            var dbContext = new DocumentDbContext();
            dbContext.Categories = GetQueryableMockDbSetCategory();
            dbContext.Documents = GetQueryableMockDbSetDocument();

            var categoryRepo = new CategoryRepo(string.Empty, dbContext);
            var documentRepo = new DocumentRepo(string.Empty, dbContext);
            var controller = new DocumentController(categoryRepo, documentRepo);

            AddDocumentModel addDocumentModel = new AddDocumentModel { Title = "Đời sống con người", Description = "Nói về đời sống con người", CategoryId = 3, Cover = "", PublishYear = 2013 };
            //Act
            var result = controller.AddDocument(addDocumentModel);

            //Assert
            Assert.IsType<NotFoundObjectResult>(result);


        }

        [Fact]
        public void AddDocument_ReturnCreateItem_WhenModelModelValid()
        {
            //Arrange
            var dbContext = new DocumentDbContext();
            dbContext.Categories = GetQueryableMockDbSetCategory();
            dbContext.Documents = GetQueryableMockDbSetDocument();

            var categoryRepo = new CategoryRepo(string.Empty, dbContext);
            var documentRepo = new DocumentRepo(string.Empty, dbContext);
            var controller = new DocumentController(categoryRepo, documentRepo);

            AddDocumentModel addDocumentModel = new AddDocumentModel { Title = "Đời sống con người", Description = "Nói về đời sống con người", CategoryId = 1, Cover = "", PublishYear = 2013 };
            //Act
            var result = controller.AddDocument(addDocumentModel);

            //Assert
            Assert.IsType<OkObjectResult>(result);

            var okObjectResult = result as OkObjectResult;
            Assert.IsType<Document>(okObjectResult.Value);

            var model = okObjectResult.Value as Document;
            Assert.Equal(model.Title, addDocumentModel.Title);
            Assert.Equal(model.Description, addDocumentModel.Description);
            Assert.Equal(model.CategoryId, addDocumentModel.CategoryId);
            Assert.Equal(model.Cover, addDocumentModel.Cover);
            Assert.Equal(model.PublishYear, addDocumentModel.PublishYear);
        }

        #endregion

        #region snippet_UpdateDocument
        [Fact]
        public void UpdateDocument_ReturnBadRequestResult_WhenModelStateIsInvalid()
        {
            //Arrange
            var mockCategoryRepo = new Mock<ICategoryRepo>();
            var mockDocumentRepo = new Mock<IDocumentRepo>();
            var controller = new DocumentController(mockCategoryRepo.Object, mockDocumentRepo.Object);
            controller.ModelState.AddModelError("error", "some error");

            //Act
            var result = controller.UpdateDocument(updateDocumentModel: null);

            //Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void UpdateDocument_ReturnNotFoundResult_WhenDocumentNotFound()
        {
            //Arrange
            var dbContext = new DocumentDbContext();
            dbContext.Categories = GetQueryableMockDbSetCategory();
            dbContext.Documents = GetQueryableMockDbSetDocument();

            var categoryRepo = new CategoryRepo(string.Empty, dbContext);
            var documentRepo = new DocumentRepo(string.Empty, dbContext);
            var controller = new DocumentController(categoryRepo, documentRepo);

            UpdateDocumentModel updateDocumentModel = new UpdateDocumentModel { Id = 3, Title = "Đời sống con người", Description = "Nói về đời sống con người", CategoryId = 1, Cover = "", PublishYear = 2013 };

            //Act
            var result = controller.UpdateDocument(updateDocumentModel);

            //Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public void UpdateDocument_ReturnNotFoundResult_WhenCategoryNotFound()
        {
            //Arrange
            var dbContext = new DocumentDbContext();
            dbContext.Categories = GetQueryableMockDbSetCategory();
            dbContext.Documents = GetQueryableMockDbSetDocument();

            var categoryRepo = new CategoryRepo(string.Empty, dbContext);
            var documentRepo = new DocumentRepo(string.Empty, dbContext);
            var controller = new DocumentController(categoryRepo, documentRepo);

            UpdateDocumentModel updateDocumentModel = new UpdateDocumentModel { Id = 1, Title = "Đời sống con người", Description = "Nói về đời sống con người", CategoryId = 3, Cover = "", PublishYear = 2013 };

            //Act
            var result = controller.UpdateDocument(updateDocumentModel);

            //Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public void UpdateDocument_ReturnUpdateItem_WhenStateIsValid()
        {
            //Arrange
            var dbContext = new DocumentDbContext();
            dbContext.Categories = GetQueryableMockDbSetCategory();
            dbContext.Documents = GetQueryableMockDbSetDocument();

            var categoryRepo = new CategoryRepo(string.Empty, dbContext);
            var documentRepo = new DocumentRepo(string.Empty, dbContext);
            var controller = new DocumentController(categoryRepo, documentRepo);

            UpdateDocumentModel updateDocumentModel = new UpdateDocumentModel { Id = 1, Title = "Đời sống con người", Description = "Nói về đời sống con người", CategoryId = 1, Cover = "", PublishYear = 2013 };

            //Act
            var result = controller.UpdateDocument(updateDocumentModel);

            //Assert
            Assert.IsType<OkObjectResult>(result);

            var okObjectResult = result as OkObjectResult;
            Assert.IsType<Document>(okObjectResult.Value);

            var documentObject = okObjectResult.Value as Document;
            Assert.Equal(updateDocumentModel.Id, documentObject.Id);
            Assert.Equal(updateDocumentModel.Title, documentObject.Title);
            Assert.Equal(updateDocumentModel.Description, documentObject.Description);
            Assert.Equal(updateDocumentModel.CategoryId, documentObject.CategoryId);
            Assert.Equal(updateDocumentModel.Cover, documentObject.Cover);
            Assert.Equal(updateDocumentModel.PublishYear, documentObject.PublishYear);
        }

        #endregion

        #region snippet_RemoveDocument

        [Fact]
        public void RemoveDocument_ReturnNotFoundResult_WhenDocumentNotFound()
        {
            //Arrange
            var dbContext = new DocumentDbContext();
            dbContext.Categories = GetQueryableMockDbSetCategory();
            dbContext.Documents = GetQueryableMockDbSetDocument();

            var categoryRepo = new CategoryRepo(string.Empty, dbContext);
            var documentRepo = new DocumentRepo(string.Empty, dbContext);
            var controller = new DocumentController(categoryRepo, documentRepo);

            int testDocumentId = 3;

            //Act
            var result = controller.RemoveDocument(testDocumentId);

            //Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public void RemoveDocument_ReturnRemovedItem_WhenDocumentExist()
        {
            //Arrange
            var dbContext = new DocumentDbContext();
            dbContext.Categories = GetQueryableMockDbSetCategory();
            dbContext.Documents = GetQueryableMockDbSetDocument();

            var categoryRepo = new CategoryRepo(string.Empty, dbContext);
            var documentRepo = new DocumentRepo(string.Empty, dbContext);
            var controller = new DocumentController(categoryRepo, documentRepo);

            int testDocumentId = 1;

            //Act
            var result = controller.RemoveDocument(testDocumentId);

            //Assert
            Assert.IsType<OkObjectResult>(result);

            var okObjectResult = result as OkObjectResult;
            Assert.IsType<Document>(okObjectResult.Value);

            var documentObject = okObjectResult.Value as Document;
            Assert.Equal(documentObject.Id, testDocumentId);

        }

        #endregion

        public List<Document> GetDocuments()
        {
            var documents = new List<Document>();
            documents.Add(new Document() { Id = 1, Title = "Đời sống động vật", Description = "Nói về đời sống động vật", CategoryId = 1, Cover = "", PublishYear = 2013 });
            documents.Add(new Document() { Id = 2, Title = "Nghiên cứu trái đất", Description = "Nói về trái đất", CategoryId = 2, Cover = "", PublishYear = 2013 });
            return documents;
        }

        private DbSet<Document> GetQueryableMockDbSetDocument()
        {
            List<Document> listDocument = GetDocuments();
            IQueryable<Document> queryableList = listDocument.AsQueryable();

            var mockDbSet = new Mock<DbSet<Document>>();
            mockDbSet.As<IQueryable<Document>>().Setup(m => m.Provider).Returns(queryableList.Provider);
            mockDbSet.As<IQueryable<Document>>().Setup(m => m.Expression).Returns(queryableList.Expression);
            mockDbSet.As<IQueryable<Document>>().Setup(m => m.ElementType).Returns(queryableList.ElementType);
            mockDbSet.As<IQueryable<Document>>().Setup(m => m.GetEnumerator()).Returns(queryableList.GetEnumerator());

            return mockDbSet.Object;
        }

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
