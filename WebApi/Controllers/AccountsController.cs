using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using WebApi.Common.DataTransferObjects;
using WebApi.Common.JwtFeatures;

namespace WebApi.Controllers
{
	public class AccountsController : Controller
	{
		private readonly ILogger<WeatherForecastController> _logger;
		private readonly JwtHandler _jwtHandler;

		public AccountsController(ILogger<WeatherForecastController> logger, JwtHandler jwtHandler)
        {
			_jwtHandler = jwtHandler;
			_logger = logger;
        }

		[HttpPost("Login")]
		public async Task<IActionResult> Login([FromBody] UserForAuthenticationDto userForAuthentication)
		{
			var userEmail = userForAuthentication.Email;
			var userPassword = userForAuthentication.Password;
			if (userEmail == null || userPassword == null || userEmail != "testuser" || userPassword != "testpassword")
				return Unauthorized(new AuthResponseDto { ErrorMessage = "Invalid Authentication" });

			var signingCredentials = _jwtHandler.GetSigningCredentials();
			var identityUser = new IdentityUser("testuser");
			identityUser.Email = userEmail;
			var claims = _jwtHandler.GetClaims(identityUser);
			var tokenOptions = _jwtHandler.GenerateTokenOptions(signingCredentials, claims);
			var token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
			return Ok(new AuthResponseDto { IsAuthSuccessful = true, Token = token });
		}
	}
}
