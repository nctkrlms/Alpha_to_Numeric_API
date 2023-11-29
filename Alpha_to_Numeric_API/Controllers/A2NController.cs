using Alpha_to_Numeric_API.Services;
using Alpha_to_Numeric_API.Data;
using Microsoft.AspNetCore.Mvc;

namespace Alpha_to_Numeric_API.Controllers
{
    [Route("api/converter")]
    [ApiController]
    public class A2NController : Controller
    {
        [HttpPost]
        public ActionResult<Output> Index([FromBody] UserText text)
        {
            string output = NumberConverter.ConvertNumber(text.InputText);

            var response = new Output
            {
                OutputText = output
            };

            return Json(response);
        }
    }
}
