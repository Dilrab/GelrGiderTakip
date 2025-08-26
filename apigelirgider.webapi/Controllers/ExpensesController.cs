using ApiGelirGider.DTOs.Expense;
using ApiGelirGider.WebApi.Context;
using AutoMapper;
using IncomeExpenseTracker.Entities;
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
    public class ExpensesController : ControllerBase
    {
        private readonly ApiContext _context;
        private readonly IMapper _mapper;

        public ExpensesController(ApiContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // ------------------------------------------------------------------------------------------------
        // 1. Giderleri listeleme (isteğe bağlı kategori filtresi)
        // GET: api/incomes?categoryId=5
        // ------------------------------------------------------------------------------------------------
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ExpenseDto>>> GetAll(int? categoryId = null)
        {
            var entities = await _context.Expenses
                .Where(x => categoryId == null || x.CategoryId == categoryId)
                .ToListAsync();

            var dtos = _mapper.Map<List<ExpenseDto>>(entities);
            return Ok(dtos);
        }

        // ------------------------------------------------------------------------------------------------
        // 2. Tek bir gideri getirme
        // GET: api/incomes/5
        // ------------------------------------------------------------------------------------------------
        [HttpGet("{id}")]
        public async Task<ActionResult<ExpenseDto>> GetById(int id)
        {
            var entity = await _context.Expenses.FindAsync(id);
            if (entity == null)
                return NotFound("Gelir bulunamadı.");

            var dto = _mapper.Map<ExpenseDto>(entity);
            return Ok(dto);
        }

        // ------------------------------------------------------------------------------------------------
        // 3. Yeni giderr ekleme
        // POST: api/incomes
        // ------------------------------------------------------------------------------------------------
        [HttpPost]
        public async Task<ActionResult<ExpenseDto>> Create([FromBody] ExpenseDto expenseDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var entity = _mapper.Map<Expense>(expenseDto);
            _context.Expenses.Add(entity);
            await _context.SaveChangesAsync();

            expenseDto.ExpenseId = entity.ExpenseId;
            expenseDto.ResultMessage = "Gelir ekleme başarılı";

            return CreatedAtAction(
                nameof(GetById),
                new { id = entity.ExpenseId },
                expenseDto);
        }

        // ------------------------------------------------------------------------------------------------
        // 4. Gelir güncelleme
        // PUT: api/incomes/5
        // ------------------------------------------------------------------------------------------------
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ExpenseDto expenseDto)
        {
            if (id != expenseDto.ExpenseId)
                return BadRequest("Yol parametresi ile DTO'daki ID uyuşmuyor.");

            var entity = await _context.Expenses.FindAsync(id);
            if (entity == null)
                return NotFound("Gelir bulunamadı.");

            _mapper.Map(expenseDto, entity);
            _context.Entry(entity).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _context.Expenses.AnyAsync(e => e.ExpenseId == id))
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
            var entity = await _context.Expenses.FindAsync(id);
            if (entity == null)
                return NotFound("Gelir bulunamadı.");

            _context.Expenses.Remove(entity);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpGet("last5")]
        public async Task<IActionResult> GetLast5()
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdStr, out var userId))
                return Unauthorized();

            var data = await _context.Expenses
                .Where(x => x.UserId == userId)
                .OrderByDescending(x => x.ExpenseDate)
                .Take(5)
                .ToListAsync();

            return Ok(data);
        }
        //Son beş giderr

        [HttpGet("total")]
        public async Task<IActionResult> GetTotal()
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdStr, out var userId))
                return Unauthorized();

            var total = await _context.Expenses
                .Where(x => x.UserId == userId)
                .SumAsync(x => x.ExpenseAmount);

            return Ok(total);
        }

    }
}
