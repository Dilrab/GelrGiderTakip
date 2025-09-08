using ApiGelirGider.DTOs.Income;
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
    [Authorize]
    public class IncomesController : ControllerBase
    {
        private readonly ApiContext _context;
        private readonly IMapper _mapper;

        public IncomesController(ApiContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // 1. Gelirleri listeleme (isteğe bağlı kategori filtresi)
        [HttpGet]
        public async Task<ActionResult<IEnumerable<IncomeDto>>> GetAll(int? categoryId = null)
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdStr, out var userId))
                return Unauthorized();

            var entities = await _context.Incomes
                .Where(x => x.UserId == userId && (categoryId == null || x.CategoryId == categoryId))
                .ToListAsync();

            var dtos = _mapper.Map<List<IncomeDto>>(entities);
            return Ok(dtos);
        }

        // 2. Tek bir geliri getirme
        [HttpGet("{id}")]
        public async Task<ActionResult<IncomeDto>> GetById(int id)
        {
            var entity = await _context.Incomes.FindAsync(id);
            if (entity == null)
                return NotFound("Gelir bulunamadı.");

            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdStr, out var userId) || entity.UserId != userId)
                return Unauthorized();

            var dto = _mapper.Map<IncomeDto>(entity);
            return Ok(dto);
        }

        // 3. Yeni gelir ekleme
        [HttpPost]
        public async Task<ActionResult<IncomeDto>> Create([FromBody] IncomeDto incomeDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdStr, out var userId))
                return Unauthorized();

            var entity = _mapper.Map<Income>(incomeDto);
            entity.UserId = userId;

            _context.Incomes.Add(entity);
            await _context.SaveChangesAsync();

            incomeDto.IncomeId = entity.IncomeId;
            incomeDto.UserId = userId;
            incomeDto.ResultMessage = "Gelir başarıyla kaydedildi.";

            return CreatedAtAction(nameof(GetById), new { id = entity.IncomeId }, incomeDto);
        }

        // 4. Gelir güncelleme
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] IncomeDto incomeDto)
        {
            if (id != incomeDto.IncomeId)
                return BadRequest("Yol parametresi ile DTO'daki ID uyuşmuyor.");

            var entity = await _context.Incomes.FindAsync(id);
            if (entity == null)
                return NotFound("Gelir bulunamadı.");

            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdStr, out var userId) || entity.UserId != userId)
                return Unauthorized();

            _mapper.Map(incomeDto, entity);
            entity.UserId = userId;

            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // 5. Gelir silme
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var entity = await _context.Incomes.FindAsync(id);
            if (entity == null)
                return NotFound("Gelir bulunamadı.");

            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdStr, out var userId) || entity.UserId != userId)
                return Unauthorized();

            _context.Incomes.Remove(entity);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // 6. Son 5 gelir
        [HttpGet("last5")]
        public async Task<IActionResult> GetLast5()
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdStr, out var userId))
                return Unauthorized();

            var data = await _context.Incomes
                .Where(x => x.UserId == userId)
                .OrderByDescending(x => x.IncomeDate)
                .Take(5)
                .ToListAsync();

            var dtos = _mapper.Map<List<IncomeDto>>(data);
            return Ok(dtos);
        }

        // 7. Toplam gelir
        [HttpGet("total")]
        public async Task<IActionResult> GetTotal()
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdStr, out var userId))
                return Unauthorized();

            var total = await _context.Incomes
                .Where(x => x.UserId == userId)
                .SumAsync(x => x.IncomeAmount);

            return Ok(total);
        }

        // 8. Aylık gelir toplamları (grafik için)
        [HttpGet("monthly")]
        public async Task<IActionResult> GetMonthlyIncome()
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdStr, out var userId))
                return Unauthorized();

            var data = await _context.Incomes
                .Where(x => x.UserId == userId)
                .GroupBy(x => new { x.IncomeDate.Year, x.IncomeDate.Month })
                .Select(g => new
                {
                    Ay = $"{g.Key.Month:00}/{g.Key.Year}",
                    Tutar = g.Sum(x => x.IncomeAmount)
                })
                .OrderBy(x => x.Ay)
                .ToListAsync();

            return Ok(data);
        }



    }
}
