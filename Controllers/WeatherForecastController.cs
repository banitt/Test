using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace TestApi.Controllers
{
    [ApiController]
    [Route("account")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly BloggingContext _context;
        private readonly SignInManager<IdentityUser> _signInManager;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, BloggingContext context, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Test(RegisterRequest registerRequest)
        {
            var user = new IdentityUser { UserName = registerRequest.Login, Email = registerRequest.Email };
            var result = await _userManager.CreateAsync(user, registerRequest.Password);

            if (result.Succeeded)
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(RegisterRequest registerRequest)
        {
            var user = await _userManager.Users
                .SingleOrDefaultAsync(x => x.UserName == registerRequest.Login && x.Email == registerRequest.Email);

            if (user == null) 
                return Unauthorized("Invalid username");

            var identity = new IdentityUser();
            identity.UserName = registerRequest.Login;
            identity.Email = registerRequest.Email;

            var result = await _signInManager.CheckPasswordSignInAsync(user, registerRequest.Password, false);

            if (result.Succeeded)
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }
    }

    public class RegisterRequest
    {
        public string Login { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}