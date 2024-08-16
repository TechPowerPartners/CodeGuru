using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddResourcesController : ControllerBase
    {
        //TODO Для будущих ресурсов
        [HttpPost]
        public IActionResult AddController(CreateResource createResource)
        {
            return Ok();
        }

        public record CreateResource(string title, string link);
    }
}
