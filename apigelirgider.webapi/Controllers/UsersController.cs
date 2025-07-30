using ApiGelirGider.WebApi.Context;
using IncomeExpenseTracker.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace apigelirgider.webapi.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ApiContext _context; //Köklü değişikliklerde işleri kolaylaştırıcak olan yapı
        public UsersController(ApiContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult UserList()
        {
            var values = _context.Users.ToList();
            return Ok(values);
        }

        [HttpPost]
        public IActionResult CreateUser(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
            return Ok("Kullanıcı Ekleme Başarili");
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
                return NotFound(); // id'ye ait kategori bulunamadıysa 404 döner

            _context.Users.Remove(user); // Artık null olmadığından eminiz
            await _context.SaveChangesAsync();
            return NoContent(); // Silme başarılıysa 204 döner
        }

        [HttpGet("GetUser")]
        public IActionResult GetUser(int id)
        {
            var value = _context.Users.Find(id);
            return Ok(value);
        }
        [HttpPut]
        public IActionResult UpdateUser(User user)
        {
            _context.Users.Update(user);
            _context.SaveChanges();
            return Ok("Kullanıcı Güncelleme Başarili");
        }
    }
}