using AutoMapper;
using Core.DTOs.UserSubscriptionDTO;
using Core.Interfaces.Services;
using Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserSubscriptionController : Controller
    {
        private readonly IMapper _mapper;

        private readonly IUserSubcriptionService _userSubscriptionService;

        public UserSubscriptionController(IMapper mapper, IUserSubcriptionService userSubscriptionService)
        {
            _mapper = mapper;
            _userSubscriptionService = userSubscriptionService;
        }

        [HttpGet("getAll")]
        public async Task<IActionResult> Get()
        {
            var result = await _userSubscriptionService.GetAllUserSubscriptionPlansAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var subscription = await _userSubscriptionService.GetByIdAsync(id);

            if (subscription == null)
            {
                return NotFound();
            }


            return Ok(subscription);
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] CreateUserSubscriptionDto dto)
        {


            var sub = _mapper.Map<Core.Models.UserSubscription>(dto);

            var createdSub = await _userSubscriptionService.CreateAsync(sub);

            var createdSubresponse = _mapper.Map<UserSubscriptionResponseDto>(createdSub);

            //var services = HttpContext.RequestServices;
            //var logger = services.GetRequiredService<ILogger<DailyTrackingBackgroundService>>();

            //var testService = new DailyTrackingBackgroundService(
            //    services,
            //    logger,
            //    isTesting: true); // ← Testing mode ON
            //await testService.ForceCreateRecordsForTestAsync(DateTime.Now); // Creates for "tomorrow"


            return Ok(createdSubresponse);

        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateChapter(int id, [FromBody] UserSubscription dto)
        {
            var existingChapter = await _userSubscriptionService.GetByIdAsync(id);
            if (existingChapter == null)
            {
                return NotFound();
            }

            // Map the DTO to the existing TopicProgress entity
            //_mapper.Map(dto, existingChapter);


            var result = await _userSubscriptionService.UpdateAsync(existingChapter);
            if (!result)
            {
                return BadRequest("Failed to update chapter.");
            }

            return Ok(existingChapter);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var existingChapter = await _userSubscriptionService.GetByIdAsync(id);
            if (existingChapter == null)
            {
                return NotFound();
            }

            var result = await _userSubscriptionService.DeleteAsync(id);
            if (!result)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error deleting chapter.");
            }

            return NoContent();
        }
    }
}
