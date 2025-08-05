using ApiGelirGider.DTOs.Income;
using ApiGelirGider.WebApi.Context;
using ApiGelirGider.WebApi.Services;
using AutoMapper;
using Azure;
using IncomeExpenseTracker.Entities;
using Microsoft.AspNetCore.Mvc;

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

        //// 1. Gelirleri listeleme (isteğe bağlı kategori filtresi)
        //[HttpGet]
        //public IActionResult ExpenseList(int? categoryId = null)
        //{
        //    var expenses = _context.Expenses
        //        .Where(x => categoryId == null || x.CategoryId == categoryId)
        //        .ToList();

        //    return Ok(expenses);
        //}

        // 2. Yeni gelir ekleme
        [HttpPost]
        public async Task<IActionResult> Post(IncomeDto ıncome)
        {
            if (!ModelState.IsValid)
            {
                throw new InvalidOperationException("Model geçersiz.");
            }

            //Response<object> value = new Response<object>();

            _context.Incomes.Add(_mapper.Map<Income>(ıncome));
            _context.SaveChanges();
            ıncome.ResultMessage = "Gelir Ekleme Başarili";

            return Ok(ıncome);
        }

        // 3. Gider silme
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteIncome(int id)
        {
            var ıncome = await _context.Incomes.FindAsync(id);
            if (ıncome == null)
                return NotFound("Gelir bulunamadı.");

            _context.Incomes.Remove(ıncome);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // 4. Tek gelir getirme
        [HttpGet("GetIncome")]
        public IActionResult GetIncome(int id)
        {
            var ıncome = _context.Incomes.Find(id);
            if (ıncome == null)
                return NotFound("Gelir bulunamadı.");

            return Ok(ıncome);
        }

        // 5. Gider güncelleme
        //[HttpPut]
        //public IActionResult UpdateExpense(Expense expense)
        //{
        //    var existing = _context.Expenses.Find(expense.Id);
        //    if (existing == null)
        //        return NotFound("Gider bulunamadı.");

        //    _context.Entry(existing).CurrentValues.SetValues(expense);
        //    _context.SaveChanges();
        //    return Ok("Gider güncelleme başarılı.");
        //}
    }

}
