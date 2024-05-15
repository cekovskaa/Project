using AutoMapper;
using ComputerStoreApi.DTOs;
using ComputerStoreApi.Models;
using ComputerStoreApi.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ComputerStoreApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public CategoriesController(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryDTO>>> GetCategories()
        {
            var categories = await _categoryRepository.GetCategoriesAsync();
            return Ok(_mapper.Map<IEnumerable<CategoryDTO>>(categories));
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryDTO>> GetCategory(int id)
        {
            var category = await _categoryRepository.GetCategoryByIdAsync(id);

            if (category == null)
            {
                return NotFound();
            }

            return _mapper.Map<CategoryDTO>(category);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> PutCategory(int id, CategoryDTO categoryDto)
        {
            var category = _mapper.Map<Category>(categoryDto);
            category.Id = id; 
            
            try
            {
                await _categoryRepository.UpdateCategoryAsync(category);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Category with the given ID not found.")
                {
                    return NotFound(new { message = "Category with the given ID not found." });
                }
                else
                {
                    return StatusCode(500, "An error occurred while trying to update the category.");
                }
            }

            return NoContent();
        }


        [HttpPost]
        public async Task<ActionResult<CategoryDTO>> PostCategory(CategoryDTO categoryDto)
        {
            var category = _mapper.Map<Category>(categoryDto);
            await _categoryRepository.CreateCategoryAsync(category);

            return CreatedAtAction("GetCategory", new { id = category.Id }, _mapper.Map<CategoryDTO>(category));
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            try
            {
                await _categoryRepository.DeleteCategoryAsync(id);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Category with the given ID not found.")
                {
                    return NotFound(new { message = "Category with the given ID not found." });
                }
                else
                {
                    return StatusCode(500, "An error occurred while trying to delete the category.");
                }
            }

            return NoContent();
        }

    }
}