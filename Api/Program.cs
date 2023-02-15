using Core.Entities;
using DataAccess.Abstract;
using DataAccess.Concrete;
using DataAccess.Contexts;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Services.Abstract;
using Services.Concrete;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<DataAccess.Contexts.DepremAppContext>(opts =>
 opts.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));


var jwtSettings = builder.Configuration.GetSection("JwtSettings");
builder.Services.Configure<JwtSettings>(jwtSettings);

builder.Services.AddScoped<IJobRepository,JobRepository>();
builder.Services.AddScoped<IJobService, JobService>();

builder.Services.AddScoped<IJwtService, JwtService>();

builder.Services.AddIdentity<User, Role>(options =>
{

})
	.AddEntityFrameworkStores<DataAccess.Contexts.DepremAppContext>()
	.AddDefaultTokenProviders();


// Add JWT Authentication services


var jwtSettingsValue = jwtSettings.Get<JwtSettings>();

builder.Services.AddAuthentication(options =>
{
	options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
	options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
	.AddJwtBearer(options =>
	{
		options.RequireHttpsMetadata = false; // sadece development sýrasýnda false
		options.SaveToken = true;
		options.TokenValidationParameters = new TokenValidationParameters
		{
			ValidateIssuerSigningKey = true,
			IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSettingsValue.Secret)), // gizli anahtar
			ValidateIssuer = false,
			ValidateAudience = false,
			ValidateLifetime = true
		};
	});

var app = builder.Build();


using (var scope = app.Services.CreateScope())
{
	var dbContext = scope.ServiceProvider.GetRequiredService<DepremAppContext>();
	dbContext.Database.Migrate();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseAuthentication();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
