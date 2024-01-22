using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using NewSky.API.Data;
using NewSky.API.Middleware;
using NewSky.API.Models.Db;
using NewSky.API.Services;
using NewSky.API.Services.Interface;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient();
builder.Services.AddHttpContextAccessor();
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddControllersWithViews();
builder.Services.AddMemoryCache();
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin", builder =>
    {
        //builder.WithOrigins("https://newsky.fr").AllowAnyMethod().AllowAnyHeader();
#if DEBUG
        builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
#endif
    });
});


// Configure Internal Services
InstantiateServices(builder.Services);


builder.Services.ConfigureApplicationCookie(options =>
{
#if DEBUG
    options.Cookie.HttpOnly = false;
#endif
});

builder.Services.AddDbContext<NewSkyDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("NewSkyConnectionString")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseMiddleware<AuthMiddleware>();

app.UseCors("AllowSpecificOrigin");
app.UseHttpsRedirection();

app.UseRouting();
app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();


void InstantiateServices(IServiceCollection services)
{
    services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
    services.AddTransient<IVoteService, VoteService>();
    services.AddTransient<IUserService, UserService>();
    services.AddTransient<ISecurityService, SecurityService>();
    services.AddTransient<ITebexService, TebexService>();
    services.AddTransient<IAuthService, AuthService>();
    services.AddTransient<IRoleService, RoleService>();
}