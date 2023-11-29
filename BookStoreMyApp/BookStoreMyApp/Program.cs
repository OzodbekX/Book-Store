using BookStoreMyApp.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Microsoft.Extensions.DependencyInjection;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using BookStoreMyApp.Helpers;
using System.Text.Json;
using System.Text.Json.Serialization;
using BookStoreMyApp.Services;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

services.AddCors(options =>
{
    
    options.AddPolicy(name: "_myAllowSpecificOrigins",
      builder =>
      {
          builder.WithOrigins("https://localhost:8000", "http://localhost:5000").AllowAnyHeader().AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
      });

});

// Add services to the container.


services.AddDbContext<BookstoreDBContext>(options =>
{
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("BookStoresDB"),
        
        b=>b.MigrationsAssembly(typeof(BookstoreDBContext).Assembly.FullName));

});
services.AddHttpContextAccessor();
services.AddSingleton<IUriService>(o =>
{
    var accessor = o.GetRequiredService<IHttpContextAccessor>();
    var request = accessor.HttpContext.Request;
    var uri = string.Concat(request.Scheme, "://", request.Host.ToUriComponent());
    return new UriService(uri);
});

services.AddControllers().AddJsonOptions(ops =>
{
    ops.JsonSerializerOptions.IgnoreNullValues = true;
    ops.JsonSerializerOptions.WriteIndented = true;
    ops.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    ops.JsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
    ops.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
services.AddEndpointsApiExplorer();
services.AddSwaggerGen(gen =>
{
    gen.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Book Stories Api", Version = "v1" });
});
services.AddMvc(option => option.EnableEndpointRouting = false)
    .SetCompatibilityVersion(CompatibilityVersion.Version_3_0)
    .AddNewtonsoftJson(opt => opt.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);
services.AddAutoMapper(typeof(AutoMapperProfiles).Assembly);
services.AddControllersWithViews();

IConfiguration Configuration = builder.Configuration;
var jwtSection = Configuration.GetSection("JWTSettings");
//validate authentication
var appSettings = jwtSection.Get<JWTSettings>();

services.Configure<JWTSettings>(jwtSection);

var key = Encoding.ASCII.GetBytes(appSettings.SecretKey);

services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

})
    .AddJwtBearer(x =>
    {
        x.RequireHttpsMetadata = true;
        x.SaveToken = true;
        x.TokenValidationParameters = new TokenValidationParameters
        {
            //ValidateIssuer = true,
            //ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = Configuration["JWTSettings:ValidIssuer"],
            ValidAudience = Configuration["JWTSettings:ValidAudience"],  
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ClockSkew = TimeSpan.Zero
        };
    });
var app = builder.Build();
app.UseCors("_myAllowSpecificOrigins");
app.UseHttpsRedirection();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();


}

app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();
app.UseSwagger();

app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My app Enddpoind");
});

app.MapControllers();

app.Run();
