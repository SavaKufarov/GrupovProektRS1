using BusinessLayer;
using DataLayer;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExpenseTracker.Controllers
{
    public class CategoryController : Controller
    {
        private readonly CategoryContext _categoryContext;

        public CategoryController(ExpenseTrackerDbContext dbContext)
        {
            _categoryContext = new CategoryContext(dbContext);
        }

        
        public IActionResult Category()
        {
            return View("Category");
        }

        
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                List<Category> categories = _categoryContext.ReadAll();
                return Ok(categories);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                Category category = _categoryContext.Read(id);
                if (category == null)
                {
                    return NotFound();
                }
                return Ok(category);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Category category)
        {
            try
            {
                if (category == null)
                {
                    return BadRequest("Category object is null");
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest("Invalid model object");
                }

                _categoryContext.Create(category);
                return CreatedAtAction(nameof(GetById), new { id = category.Id }, category);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        
        [HttpPut("{id}")]
        [Route("Category/Update/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Category category)
        {
            try
            {
                if (category == null)
                {
                    return BadRequest("Category object is null");
                }

                if (id != category.Id)
                {
                    return BadRequest("Category ID mismatch");
                }

                Category categoryToUpdate = _categoryContext.Read(id);
                if (categoryToUpdate == null)
                {
                    return NotFound();
                }

                _categoryContext.Update(category);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        
        [HttpDelete("{id}")]
        [Route("Category/Delete/{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                Category category = _categoryContext.Read(id);
                if (category == null)
                {
                    return NotFound();
                }

                _categoryContext.Delete(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}