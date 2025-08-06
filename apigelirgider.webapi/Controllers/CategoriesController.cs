using ApiGelirGider.DTOs.Category;
using ApiGelirGider.WebApi.Context;
using AutoMapper;
using IncomeExpenseTracker.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace ApiGelirGider.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ApiContext _context;
        private readonly IMapper _mapper;

        public CategoriesController(ApiContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // ------------------------------------------------------------------------------------------------
        // 1. Gelirleri listeleme (isteğe bağlı kategori filtresi)
        // GET: api/incomes?categoryId=5
        // ------------------------------------------------------------------------------------------------
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryDtoEdit>>> GetAll(int? categoryId = null)
        {
            var entities = await _context.Categories
                .Where(x => categoryId == null || x.Id == categoryId)
                .ToListAsync();

            var dtos = _mapper.Map<List<CategoryDtoEdit>>(entities);
            return Ok(dtos);
        }

        // ------------------------------------------------------------------------------------------------
        // 2. Tek bir geliri getirme
        // GET: api/incomes/5
        // ------------------------------------------------------------------------------------------------
        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryDtoEdit>> GetById(int id)
        {
            var entity = await _context.Categories.FindAsync(id);
            if (entity == null)
                return NotFound("Gelir bulunamadı.");

            var dto = _mapper.Map<CategoryDtoEdit>(entity);
            return Ok(dto);
        }

        // ------------------------------------------------------------------------------------------------
        // 3. Yeni gelir ekleme
        // POST: api/incomes
        // ------------------------------------------------------------------------------------------------
        [HttpPost]
        public async Task<ActionResult<CategoryDtoEdit>> Create([FromBody] CategoryDtoEdit CategoryDtoEdit)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var entity = _mapper.Map<Category>(CategoryDtoEdit);
            _context.Categories.Add(entity);
            await _context.SaveChangesAsync();

            CategoryDtoEdit.Id = entity.Id;
            CategoryDtoEdit.ResultMessage = "Gelir ekleme başarılı";

            return CreatedAtAction(
                nameof(GetById),
                new { id = entity.Id },
                CategoryDtoEdit);
        }

        // ------------------------------------------------------------------------------------------------
        // 4. Gelir güncelleme
        // PUT: api/incomes/5
        // ------------------------------------------------------------------------------------------------
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CategoryDtoEdit CategoryDtoEdit)
        {
            if (id != CategoryDtoEdit.Id)
                return BadRequest("Yol parametresi ile DTO'daki ID uyuşmuyor.");

            var entity = await _context.Categories.FindAsync(id);
            if (entity == null)
                return NotFound("Gelir bulunamadı.");

            _mapper.Map(CategoryDtoEdit, entity);
            _context.Entry(entity).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _context.Categories.AnyAsync(e => e.Id == id))
                    return NotFound("Gelir bulunamadı.");
                throw;
            }

            return NoContent();
        }

        // ------------------------------------------------------------------------------------------------
        // 5. Gelir silme
        // DELETE: api/incomes/5
        // ------------------------------------------------------------------------------------------------
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var entity = await _context.Categories.FindAsync(id);
            if (entity == null)
                return NotFound("Gelir bulunamadı.");

            _context.Categories.Remove(entity);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
