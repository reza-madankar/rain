using Microsoft.AspNetCore.Mvc;
using Rain.Helper.Attributes;
using Rain.Model;
using Rain.Context.Services;

namespace Rain.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/data")]
    public class RainController : Controller
    {
        private readonly IRain _rainServices;

        public RainController(IRain rainServices)
        {
            _rainServices = rainServices;
        }

        [HttpGet]
        [RequireUserId]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetRains(bool? isRain = null, int page = 1)
        {
            var userId = HttpContext.Items["UserId"]?.ToString();

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
                return BadRequest(new Message
                {
                    IsSuccess = false,
                    Description = "Please include a valid 'rain' value (true or false) in your request."
                });
            }

            var userId = HttpContext.Items["UserId"]?.ToString();
            var success = await _rainServices.AddRain(input.Rain.Value, userId);
            if (success)
            {
                return Created(string.Empty, new Message
                {
                    IsSuccess = true,
                    Description = "Rain data has been successfully recorded for you."
                });
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
