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

        // 🔍 Tüm gelirleri listele
        [HttpGet]
        public IActionResult GetAll()
        {
            var incomeEntities = _context.Incomes.ToList();
            var incomeDtos = _mapper.Map<List<IncomeDto>>(incomeEntities);
            return Ok(incomeDtos);
        }

        // 🔍 Tek gelir bilgisi
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var income = _context.Incomes.Find(id);
            if (income == null)
                return NotFound($"ID {id} için gelir bulunamadı.");

            var incomeDto = _mapper.Map<IncomeDto>(income);
            return Ok(incomeDto);
        }

        // 🆕 Yeni gelir ekle
        [HttpPost]
        public IActionResult Create([FromBody] IncomeCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var income = _mapper.Map<Income>(dto);
            _context.Incomes.Add(income);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetById), new { id = income.Id }, _mapper.Map<IncomeDto>(income));
        }

        // 🔄 Güncelle
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] IncomeUpdateDto dto)
        {
            var existingIncome = _context.Incomes.Find(id);
            if (existingIncome == null)
                return NotFound();

            _mapper.Map(dto, existingIncome);
            _context.SaveChanges();
            return NoContent();
        }

        // ❌ Sil
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var income = _context.Incomes.Find(id);
            if (income == null)
                return NotFound();

            _context.Incomes.Remove(income);
            _context.SaveChanges();
            return NoContent();
        }
    }
}
