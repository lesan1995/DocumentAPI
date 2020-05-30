using System;
using DocumentAPI.Abstract;
using DocumentAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace DocumentAPI.Controllers
{
    [Route("api/category")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryRepo _categoryRepo;
        public CategoryController(
            ICategoryRepo categoryRepo
            )
        {
            _categoryRepo = categoryRepo ?? throw new ArgumentNullException(nameof(categoryRepo));
        }

        [HttpGet]
        [Route("get-categories")]
        public IActionResult GetCategories()
        {
            var response = _categoryRepo.GetCategories();
            return Ok(response);
        }

        [HttpPost]
        [Route("add-category")]
        public IActionResult AddCategory([FromBody]AddCategoryModel addCategoryModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Category newCategory = new Category()
            {
                Title = addCategoryModel.Title
            };

            var response = _categoryRepo.AddCategory(newCategory);

            return Ok(response);
        }

        [HttpPut]
        [Route("update-category")]
        public IActionResult UpdateCategory([FromBody]UpdateCategoryModel updateCategoryModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Category currentCategory = _categoryRepo.GetCategory(updateCategoryModel.Id);
            if(currentCategory == null) return NotFound("CategoryNotFound");

            Category updateCategory = new Category()
            {
                Id = updateCategoryModel.Id,
                Title = updateCategoryModel.Title
            };

            var response = _categoryRepo.UpdateCategory(updateCategory);

            return Ok(response);

        }

        [HttpDelete]
        [Route("remove-category/{categoryId}")]
        public IActionResult RemoveCategory(int categoryId)
        {

            Category currentCategory = _categoryRepo.GetCategory(categoryId);
            if (currentCategory == null) return NotFound("CategoryNotFound");

            var response = _categoryRepo.RemoveCategory(currentCategory);
            return Ok(response);
        }
    }
}