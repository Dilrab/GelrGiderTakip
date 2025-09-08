using ApiGelirGider.DTOs.Expense;
using ApiGelirGider.WebApi.Context;
using AutoMapper;
using IncomeExpenseTracker.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ApiGelirGider.WebApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ExpensesController : ControllerBase
    {
        private readonly ApiContext _context;
        private readonly IMapper _mapper;

        public ExpensesController(ApiContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // 🔐 Yardımcı metod: Token'dan UserId al
        private int? GetUserIdFromToken()
        {
            var userIdStr =
                User.FindFirstValue(ClaimTypes.NameIdentifier) ??
                User.FindFirstValue("sub") ??
                User.FindFirstValue("id");

            return int.TryParse(userIdStr, out var userId) ? userId : null;
        }

        // 1. Giderleri listeleme (isteğe bağlı kategori filtresi)
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ExpenseDto>>> GetAll(int? categoryId = null)
        {
            var userId = GetUserIdFromToken();
            if (userId == null)
                return Unauthorized("Token içinde geçerli kullanıcı ID bulunamadı.");

            var entities = await _context.Expenses
                .Where(x => x.UserId == userId && (categoryId == null || x.CategoryId == categoryId))
                .ToListAsync();

            var dtos = _mapper.Map<List<ExpenseDto>>(entities);
            return Ok(dtos);
        }

        // 2. Tek bir gideri getirme
        [HttpGet("{id}")]
        public async Task<ActionResult<ExpenseDto>> GetById(int id)
        {
            var userId = GetUserIdFromToken();
            if (userId == null)
                return Unauthorized();

            var entity = await _context.Expenses.FindAsync(id);
            if (entity == null || entity.UserId != userId)
                return NotFound("Gider bulunamadı veya erişim yetkiniz yok.");

            var dto = _mapper.Map<ExpenseDto>(entity);
            return Ok(dto);
        }

        // 3. Yeni gider ekleme
        [HttpPost]
        public async Task<ActionResult<ExpenseDto>> Create([FromBody] ExpenseDto expenseDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = GetUserIdFromToken();
            if (userId == null)
                return Unauthorized();

            var entity = _mapper.Map<Expense>(expenseDto);
            entity.UserId = userId.Value;

            _context.Expenses.Add(entity);
            await _context.SaveChangesAsync();

            expenseDto.ExpenseId = entity.ExpenseId;
            expenseDto.UserId = userId.Value;
            expenseDto.ResultMessage = "Gider başarıyla kaydedildi.";

            return CreatedAtAction(nameof(GetById), new { id = entity.ExpenseId }, expenseDto);
        }

        // 4. Gider güncelleme
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ExpenseDto expenseDto)
        {
            if (id != expenseDto.ExpenseId)
                return BadRequest("Yol parametresi ile DTO'daki ID uyuşmuyor.");

            var userId = GetUserIdFromToken();
            if (userId == null)
                return Unauthorized();

            var entity = await _context.Expenses.FindAsync(id);
            if (entity == null || entity.UserId != userId)
                return NotFound("Gider bulunamadı veya erişim yetkiniz yok.");

            _mapper.Map(expenseDto, entity);
            entity.UserId = userId.Value;

            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // 5. Gider silme
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = GetUserIdFromToken();
            if (userId == null)
                return Unauthorized();

            var entity = await _context.Expenses.FindAsync(id);
            if (entity == null || entity.UserId != userId)
                return NotFound("Gider bulunamadı veya erişim yetkiniz yok.");

            _context.Expenses.Remove(entity);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // 6. Son 5 gider
        [HttpGet("last5")]
        public async Task<IActionResult> GetLast5()
        {
            var userId = GetUserIdFromToken();
            if (userId == null)
                return Unauthorized();

            var data = await _context.Expenses
                .Where(x => x.UserId == userId)
                .OrderByDescending(x => x.ExpenseDate)
                .Take(5)
                .ToListAsync();

            var dtos = _mapper.Map<List<ExpenseDto>>(data);
            return Ok(dtos);
        }

        // 7. Toplam gider
        [HttpGet("total")]
        public async Task<IActionResult> GetTotal()
        {
            var userId = GetUserIdFromToken();
            if (userId == null)
                return Unauthorized();

            var total = await _context.Expenses
                .Where(x => x.UserId == userId)
                .SumAsync(x => x.ExpenseAmount);

            return Ok(total);
        }
    }
}
