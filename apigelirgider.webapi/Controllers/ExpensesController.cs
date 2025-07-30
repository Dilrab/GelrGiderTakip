using ApiGelirGider.WebApi.Context;
using IncomeExpenseTracker.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace apigelirgider.webapi.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class ExpensesController : ControllerBase
    {
        private readonly ApiContext _context; //Köklü değişikliklerde işleri kolaylaştırıcak olan yapı
        public ExpensesController(ApiContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult ExpenseList()
        {
            var values = _context.Expenses.ToList();
            return Ok(values);
        }

        [HttpPost]
        public IActionResult CreateExpense(Expense expense)
        {
            _context.Expenses.Add(expense);
            _context.SaveChanges();
            return Ok("Gider Ekleme Başarili");
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteExpense(int id)
        {
            var expens = await _context.Expenses.FindAsync(id);

            if (expens == null)
                return NotFound(); // id'ye ait kategori bulunamadıysa 404 döner

            _context.Expenses.Remove(expens); // Artık null olmadığından eminiz
            await _context.SaveChangesAsync();
            return NoContent(); // Silme başarılıysa 204 döner
        }

        [HttpGet("GetExpens")]
        public IActionResult GetExpense(int id)
        {
            var value = _context.Expenses.Find(id);
            return Ok(value);
        }
        [HttpPut]
        public IActionResult UpdateExpens(Expense expens )
        {
            _context.Expenses.Update(expens);
            _context.SaveChanges();
            return Ok("Gider Güncelleme Başarili");
        }
    }
}
