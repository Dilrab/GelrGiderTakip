using ApiGelirGider.DTOs.Income;
using ApiGelirGider.WebApi.Context;
using AutoMapper;
using IncomeExpenseTracker.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ApiGelirGider.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IncomesController : ControllerBase
    {
        private readonly ApiContext _context;
        private readonly IMapper _mapper;

        public IncomesController(ApiContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // ------------------------------------------------------------------------------------------------
        // 1. Gelirleri listeleme (isteğe bağlı kategori filtresi)
        // GET: api/incomes?categoryId=5
        // ------------------------------------------------------------------------------------------------
        [HttpGet]
        public async Task<ActionResult<IEnumerable<IncomeDto>>> GetAll(int? categoryId = null)
        {
            var entities = await _context.Incomes
                .Where(x => categoryId == null || x.CategoryId == categoryId)
                .ToListAsync();

            var dtos = _mapper.Map<List<IncomeDto>>(entities);
            return Ok(dtos);
        }

        // ------------------------------------------------------------------------------------------------
        // 2. Tek bir geliri getirme
        // GET: api/incomes/5
        // ------------------------------------------------------------------------------------------------
        [HttpGet("{id}")]
        public async Task<ActionResult<IncomeDto>> GetById(int id)
        {
            var entity = await _context.Incomes.FindAsync(id);
            if (entity == null)
                return NotFound("Gelir bulunamadı.");

            var dto = _mapper.Map<IncomeDto>(entity);
            return Ok(dto);
        }

        // ------------------------------------------------------------------------------------------------
        // 3. Yeni gelir ekleme
        // POST: api/incomes
        // ------------------------------------------------------------------------------------------------
        [HttpPost]
        public async Task<ActionResult<IncomeDto>> Create([FromBody] IncomeDto incomeDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var entity = _mapper.Map<Income>(incomeDto);
            _context.Incomes.Add(entity);
            await _context.SaveChangesAsync();

            incomeDto.IncomeId = entity.IncomeId;
            incomeDto.ResultMessage = "Gelir ekleme başarılı";

            return CreatedAtAction(
                nameof(GetById),
                new { id = entity.IncomeId },
                incomeDto);
        }

        // ------------------------------------------------------------------------------------------------
        // 4. Gelir güncelleme
        // PUT: api/incomes/5
        // ------------------------------------------------------------------------------------------------
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] IncomeDto incomeDto)
        {
            if (id != incomeDto.IncomeId)
                return BadRequest("Yol parametresi ile DTO'daki ID uyuşmuyor.");

            var entity = await _context.Incomes.FindAsync(id);
            if (entity == null)
                return NotFound("Gelir bulunamadı.");

            _mapper.Map(incomeDto, entity);
            _context.Entry(entity).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _context.Incomes.AnyAsync(e => e.IncomeId == id))
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
            var entity = await _context.Incomes.FindAsync(id);
            if (entity == null)
                return NotFound("Gelir bulunamadı.");

            _context.Incomes.Remove(entity);
            await _context.SaveChangesAsync();
            return NoContent();
        }
        // Son 5 Gelir
        [Authorize]
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

            return Ok(data);
        }
        //toplam gelir
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
    }
}
