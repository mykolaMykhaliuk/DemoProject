using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using WebApi.Common.DataTransferObjects;

namespace WebApi.Controllers
{
	[Authorize]
	[ApiController]
	[Route("[controller]")]
	public class DemoController : Controller
	{
		[HttpGet(Name = "GetBeeceptorClients")]
		public async Task<IEnumerable<ClientDto>> Get()
		{
			List<ClientDto> result = new List<ClientDto>();
			using (var httpClient = new HttpClient())
			{
				using (var response = await httpClient.GetAsync("https://demo-project.free.beeceptor.com/api/clients"))
				{
					string apiResponse = await response.Content.ReadAsStringAsync();
					var options = new JsonSerializerOptions
					{
						PropertyNameCaseInsensitive = true
					};
					result = JsonSerializer.Deserialize<List<ClientDto>>(apiResponse, options);
				}
			}
			return result;
		}
	}
}
