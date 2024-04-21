

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using WebApi.Common.JwtFeatures;

namespace WebApi
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			AddAuthorization(builder);
			AddCors(builder);
			builder.Services.AddScoped<JwtHandler>();

			builder.Services.AddControllers();
			builder.Services.AddEndpointsApiExplorer();

			AddSwagger(builder);

			var app = builder.Build();
			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}

			app.UseHttpsRedirection();
			app.UseCors("AllowOrigin");

			app.UseAuthentication();
			app.UseAuthorization();
			app.MapControllers();
			app.Run();
		}

		private static void AddSwagger(WebApplicationBuilder builder)
		{
			builder.Services.AddSwaggerGen(setup =>
			{
				// Include 'SecurityScheme' to use JWT Authentication
				var jwtSecurityScheme = new OpenApiSecurityScheme
				{
					BearerFormat = "JWT",
					Name = "JWT Authentication",
					In = ParameterLocation.Header,
					Type = SecuritySchemeType.Http,
					Scheme = JwtBearerDefaults.AuthenticationScheme,
					Description = "Put **_ONLY_** your JWT Bearer token on textbox below!",

					Reference = new OpenApiReference
					{
						Id = JwtBearerDefaults.AuthenticationScheme,
						Type = ReferenceType.SecurityScheme
					}
				};

				setup.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);

				setup.AddSecurityRequirement(new OpenApiSecurityRequirement{{ jwtSecurityScheme, Array.Empty<string>() }});
			});
		}
		private static void AddCors(WebApplicationBuilder builder)
		{
			builder.Services.AddCors(c =>
			{
				c.AddPolicy("AllowOrigin", options =>
				{
					options.AllowAnyOrigin()
					.AllowAnyHeader()
					.AllowAnyMethod();
				});
			});
		}

		private static void AddAuthorization(WebApplicationBuilder builder)
		{
			var jwtSettings = builder.Configuration.GetSection("JwtSettings");
			builder.Services.AddAuthentication(opt =>
			{
				opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			}).AddJwtBearer(options =>
			{
				options.TokenValidationParameters = new TokenValidationParameters
				{
					ValidateIssuer = true,
					ValidateAudience = true,
					ValidateLifetime = true,
					ValidateIssuerSigningKey = true,
					ValidIssuer = jwtSettings["validIssuer"],
					ValidAudience = jwtSettings["validAudience"],
					IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8
								.GetBytes(jwtSettings.GetSection("securityKey").Value))
				};
			});
		}
	}
}
