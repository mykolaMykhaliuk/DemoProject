
namespace WebApi
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);
			builder.Services.AddCors(c =>
			{
				c.AddPolicy("AllowOrigin", options =>
				{
					options.AllowAnyOrigin()
					.AllowAnyHeader()
					.AllowAnyMethod();
				});
			});
			// Add services to the container.

			builder.Services.AddControllers();
			// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen();

			var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}

			app.UseHttpsRedirection();
			app.UseCors("AllowOrigin");
			app.UseAuthorization();


			app.MapControllers();

			app.Run();
		}
	}
}