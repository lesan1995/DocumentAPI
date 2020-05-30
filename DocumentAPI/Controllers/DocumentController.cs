using System;
using DocumentAPI.Abstract;
using DocumentAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace DocumentAPI.Controllers
{
    [Route("api/document")]
    [ApiController]
    public class DocumentController : ControllerBase
    {
        private readonly ICategoryRepo _categoryRepo;
        private readonly IDocumentRepo _documentRepo;
        public DocumentController(
            ICategoryRepo categoryRepo,
            IDocumentRepo documentRepo
            )
        {
            _categoryRepo = categoryRepo ?? throw new ArgumentNullException(nameof(categoryRepo));
            _documentRepo = documentRepo ?? throw new ArgumentNullException(nameof(documentRepo));
        }

        [HttpGet]
        [Route("get-documents")]
        public IActionResult GetDocuments([FromQuery] DocumentFilter documentFilter)
        {
            var response = _documentRepo.GetDocuments(documentFilter);
            return Ok(response);
        }

        [HttpPost]
        [Route("add-document")]
        public IActionResult AddDocument([FromBody]AddDocumentModel addDocumentModel)
        {
            //1. Validate Model
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //2. Validate category
            if (addDocumentModel.CategoryId.HasValue)
            {
                var currentCategory = _categoryRepo.GetCategory(addDocumentModel.CategoryId.Value);
                if (currentCategory == null) return NotFound("CategoryNotExist");
            }

            //3. If category id valid, start update document
            Document newDocument = new Document()
            {
                CategoryId = addDocumentModel.CategoryId,
                Title = addDocumentModel.Title,
                Description = addDocumentModel.Description,
                Cover = addDocumentModel.Cover,
                PublishYear = addDocumentModel.PublishYear
            };

            var response = _documentRepo.AddDocument(newDocument);

            return Ok(response);
        }

        [HttpPut]
        [Route("update-document")]
        public IActionResult UpdateDocument([FromBody]UpdateDocumentModel updateDocumentModel)
        {
            //1. Validate Model
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            //2. Check document exist
            var currentDocument = _documentRepo.GetDocument(updateDocumentModel.Id);
            if(currentDocument == null) return NotFound("DocumentNotExist");

            //3. Check category exist
            if (updateDocumentModel.CategoryId.HasValue)
            {
                var currentCategory = _categoryRepo.GetCategory(updateDocumentModel.CategoryId.Value);
                if (currentCategory == null) return NotFound("CategoryNotExist");
            }

            //4. Update document
            Document updateDocument = new Document()
            {
                Id = updateDocumentModel.Id,
                CategoryId = updateDocumentModel.CategoryId,
                Title = updateDocumentModel.Title,
                Description = updateDocumentModel.Description,
                Cover = updateDocumentModel.Cover,
                PublishYear = updateDocumentModel.PublishYear
            };

            var response = _documentRepo.UpdateDocument(updateDocument);

            return Ok(response);

        }

        [HttpDelete]
        [Route("remove-document/{documentId}")]
        public IActionResult RemoveDocument(int documentId)
        {

            Document currentDocument = _documentRepo.GetDocument(documentId);
            if(currentDocument == null) 
                return NotFound("DocumentNotExist");

            var response = _documentRepo.RemoveDocument(currentDocument);
            return Ok(response);
        }
    }
}