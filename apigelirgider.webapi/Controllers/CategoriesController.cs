using ApiGelirGider.DTOs.Category;
using ApiGelirGider.WebApi.Context;
using AutoMapper;
using IncomeExpenseTracker.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ApiGelirGider.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // 👈 Tüm endpoint’ler için token zorunlu
    public class CategoriesController : ControllerBase
    {
        private readonly ApiContext _context;
        private readonly IMapper _mapper;

        public CategoriesController(ApiContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // 1. Kullanıcıya özel kategori listesi
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryDtoEdit>>> GetAll()
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "id");
            if (userIdClaim == null)
                return Unauthorized("Kullanıcı kimliği alınamadı.");

            int userId = int.Parse(userIdClaim.Value);

            var entities = await _context.Categories
                .Where(x => x.UserId == userId)
                .ToListAsync();

            var dtos = _mapper.Map<List<CategoryDtoEdit>>(entities);
            return Ok(dtos);
        }

        // 2. Tek bir kategori getirme
        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryDtoEdit>> GetById(int id)
        {
            var entity = await _context.Categories.FindAsync(id);
            if (entity == null)
                return NotFound("Kategori bulunamadı.");

            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "id");
            if (userIdClaim == null || entity.UserId != int.Parse(userIdClaim.Value))
                return Forbid("Bu kategoriye erişim yetkiniz yok.");

            var dto = _mapper.Map<CategoryDtoEdit>(entity);
            return Ok(dto);
        }

        // 3. Yeni kategori ekleme
        [HttpPost]
        public async Task<ActionResult<CategoryDtoEdit>> Create([FromBody] CategoryDtoEdit dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "id");
            if (userIdClaim == null)
                return Unauthorized("Kullanıcı kimliği alınamadı.");

            dto.UserId = int.Parse(userIdClaim.Value); // 👈 Kategoriye kullanıcıyı bağla

            var entity = _mapper.Map<Category>(dto);
            _context.Categories.Add(entity);
            await _context.SaveChangesAsync();

            dto.Id = entity.Id;
            dto.ResultMessage = "Kategori başarıyla eklendi.";
            return CreatedAtAction(nameof(GetById), new { id = entity.Id }, dto);
        }

        // 4. Kategori güncelleme
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CategoryDtoEdit dto)
        {
            if (id != dto.Id)
                return BadRequest("Yol parametresi ile DTO'daki ID uyuşmuyor.");

            var entity = await _context.Categories.FindAsync(id);
            if (entity == null)
                return NotFound("Kategori bulunamadı.");

            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "id");
            if (userIdClaim == null || entity.UserId != int.Parse(userIdClaim.Value))
                return Forbid("Bu kategoriye erişim yetkiniz yok.");

            _mapper.Map(dto, entity);
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // 5. Kategori silme
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var entity = await _context.Categories.FindAsync(id);
            if (entity == null)
                return NotFound("Kategori bulunamadı.");

            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "id");
            if (userIdClaim == null || entity.UserId != int.Parse(userIdClaim.Value))
                return Forbid("Bu kategoriye erişim yetkiniz yok.");

            _context.Categories.Remove(entity);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
