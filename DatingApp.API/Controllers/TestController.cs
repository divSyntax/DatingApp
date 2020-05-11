using System.Threading.Tasks;
using DatingApp.API.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly DataContext context;

        public TestController(DataContext context)
        {
            this.context = context;
        }

        [HttpGet("getall")]
        public async Task<IActionResult> GetValues()
        {
            var values = await context.Values.ToListAsync();

            return Ok();
        }
    }
}