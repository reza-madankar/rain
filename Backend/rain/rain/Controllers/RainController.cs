using Microsoft.AspNetCore.Mvc;
using rain.Context.Repository;
using rain.Context.Services;
using rain.Helper.Attributes;
using rain.Model;

namespace rain.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class RainController : Controller
    {
        private readonly IRain _rainServices;

        public RainController(IRain rainServices)
        {
            _rainServices = rainServices;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetRains(bool? isRain = null, string userId = "", int page = 1)
        {

            var dto = await _rainServices.GetRains(isRain, userId, page);
            dto.IsSuccess = true;
            dto.Description = "Data fetched successfully";

            return Ok(dto);
        }

        [HttpPost]
        [RequireUserId]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddRain([FromBody] RainInputDto input)
        {
            if (input == null || input.Rain == null)
            {
                var message = new Message
                {
                    IsSuccess = false,
                    Description = "Please include a valid 'rain' value (true or false) in your request."
                };

                return StatusCode(400, message);
            }

            var userId = HttpContext.Items["UserId"]?.ToString();
            var success = await _rainServices.AddRain(input.Rain.Value, userId);
            if (success)
            {
                var message = new Message
                {
                    IsSuccess = true,
                    Description = "Rain data has been successfully recorded for you."
                };

                return StatusCode(201, message);
            }

            var errorMessage = new Message
            {
                IsSuccess = false,
                Description = "Sorry, we couldn't save your rain data. Please try again later."
            };

            return StatusCode(500, errorMessage);
        }

    }
}
