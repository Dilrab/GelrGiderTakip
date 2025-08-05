using ApiGelirGider.DTOs.Expense;
using ApiGelirGider.WebApi.Context;
using AutoMapper;
using IncomeExpenseTracker.Entities;
using Microsoft.AspNetCore.Mvc;
using Azure;

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

        //// 1. Giderleri listeleme (isteğe bağlı kategori filtresi)
        //[HttpGet]
        //public IActionResult ExpenseList(int? categoryId = null)
        //{
        //    var expenses = _context.Expenses
        //        .Where(x => categoryId == null || x.CategoryId == categoryId)
        //        .ToList();

        //    return Ok(expenses);
        //}

        // 2. Yeni gider ekleme
        [HttpPost]
        public async Task<IActionResult> Post(ExpenseDto expense)
        {
            if (!ModelState.IsValid)
            {
                throw new InvalidOperationException("Model geçersiz.");
            }

            //Response<object> value = new Response<object>();

            _context.Expenses.Add(_mapper.Map<Expense>(expense));
            _context.SaveChanges();
            expense.ResultMessage = "Gider Ekleme Başarili";

            return Ok(expense);
        }

        // 3. Gider silme
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteExpense(int id)
        {
            var expense = await _context.Expenses.FindAsync(id);
            if (expense == null)
                return NotFound("Gider bulunamadı.");

            _context.Expenses.Remove(expense);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // 4. Tek gider getirme
        [HttpGet("GetExpense")]
        public IActionResult GetExpense(int id)
        {
            var expense = _context.Expenses.Find(id);
            if (expense == null)
                return NotFound("Gider bulunamadı.");

            return Ok(expense);
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
