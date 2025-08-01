using ApiGelirGider.DTOs.Category;
using ApiGelirGider.WebApi.Context;
using AutoMapper;
using Azure;
using IncomeExpenseTracker.Entities;
using Microsoft.AspNetCore.Mvc;

namespace apigelirgider.webapi.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ApiContext _context; //Köklü değişikliklerde işleri kolaylaştırıcak olan yapı
        private readonly IMapper _mapper; //Köklü değişikliklerde işleri kolaylaştırıcak olan yapı
        public CategoriesController(ApiContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult CategoryList(int catagoriType)
        {
            var values = _context.Categories.Where(x => x.Type == catagoriType).ToList();
            return Ok(values);
        }

        [HttpPost]
        public async Task<IActionResult> Post(CategoryDtoEdit category)
        {
            if (!ModelState.IsValid)
            {
                throw new InvalidOperationException("Model geçersiz.");
            }

            //Response<object> value = new Response<object>();

            _context.Categories.Add(_mapper.Map<Category>(category));
            _context.SaveChanges();
            category.ResultMessage = "Kategori Ekleme Başarili";

            return Ok(category);
        }




        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = await _context.Categories.FindAsync(id);

            if (category == null)
                return NotFound(); // id'ye ait kategori bulunamadıysa 404 döner

            _context.Categories.Remove(category); // Artık null olmadığından eminiz
            await _context.SaveChangesAsync();
            return NoContent(); // Silme başarılıysa 204 döner
        }

        [HttpGet("GetCategory")]
        public IActionResult GetCategory(int id)
        {
            var value = _context.Categories.Find(id);
            return Ok(value);
        }
        [HttpPut]
        public IActionResult UpdateCategory(Category category)
        {
            _context.Categories.Update(category);
            _context.SaveChanges();
            return Ok("Kategori Güncelleme Başarili");
        }
    }
}
