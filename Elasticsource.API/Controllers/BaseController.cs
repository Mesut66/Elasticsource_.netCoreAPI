using Elasticsource.API.DTO;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Elasticsource.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BaseController : Controller
    {
        [NonAction]
        public IActionResult CreateActionResult<T>(ResponseDto<T> response )
        {
            if (response.Status == HttpStatusCode.NoContent)
                return new ObjectResult(null){StatusCode = response.Status.GetHashCode()};

            return new ObjectResult(response){StatusCode = response.Status.GetHashCode() };

        }
    }
}
