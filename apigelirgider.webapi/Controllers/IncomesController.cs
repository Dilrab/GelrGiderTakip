using ApiGelirGider.WebApi.Context;
using ApiGelirGider.WebApi.DTOs.Income;
using AutoMapper;
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

        // 🔍 DTO ile listeleme
        [HttpGet]
        public IActionResult IncomeList()
        {
            var incomeEntities = _context.Incomes.ToList();
            var incomeDtos = _mapper.Map<List<IncomeDto>>(incomeEntities);
            return Ok(incomeDtos);
        }

        // 🆕 DTO ile ekleme
        [HttpPost]
        public IActionResult CreateIncome([FromBody] IncomeCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var newIncome = _mapper.Map<Income>(dto);
            _context.Incomes.Add(newIncome);
            _context.SaveChanges();
            return Ok("Gelir Ekleme Başarılı");
        }

        // 🔄 Güncelleme: İsteğe bağlı olarak DTO ile güncelleme yapılabilir
        [HttpPut]
        public IActionResult UpdateIncome([FromBody] Income income)
        {
            _context.Incomes.Update(income);
            _context.SaveChanges();
            return Ok("Gelir Güncelleme Başarılı");
        }

        // ❌ Silme
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteIncome(int id)
        {
            var income = await _context.Incomes.FindAsync(id);
            if (income == null)
                return NotFound();

            _context.Incomes.Remove(income);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // 🔍 Tek veri çekme + DTO dönüşümü
        [HttpGet("GetIncome")]
        public IActionResult GetIncome(int id)
        {
            var income = _context.Incomes.Find(id);
            if (income == null)
                return NotFound();

            var incomeDto = _mapper.Map<IncomeDto>(income);
            return Ok(incomeDto);
        }
    }
}
