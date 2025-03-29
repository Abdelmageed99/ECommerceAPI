using Braintree;
using ECommerceAPI.Modules.Orders;
using ECommerceAPI.Modules.Payments;
using ECommerceAPI.Modules.Payments.Configurations;
using ECommerceAPI.Modules.Products;
using ECommerceAPI.Modules.ShoppingCarts;
using ECommerceAPI.Modules.Users;
using ECommerceAPI.Modules.Users.Models;
using ECommerceAPI.Shared.Database;
using ECommerceAPI.Shared.Middleware;
using ECommerceAPI.Shared.RoleInitializer;
using ECommerceAPI.Shared.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Stripe;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Add Modules
builder.Services.AddUsersModule();
builder.Services.AddProductsModule();
builder.Services.AddOrdersModule();
builder.Services.AddCartsModule();
builder.Services.AddPaymentModule();


//Add ConnectionString
Console.WriteLine($"Environment: {builder.Environment.EnvironmentName}");
var conString = builder.Configuration.GetConnectionString("DefaultConnection");
Console.WriteLine($"Load Connection String{conString}");

builder.Services.AddDbContext<ApplicationDbContext>(option => option.UseSqlServer(conString));
    

//Add JwtSettings Instance 
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));

//Add Braintree Geteway
builder.Services.AddSingleton<IBraintreeGateway>(sp =>
{
    var config = builder.Configuration.GetSection("BraintreeGateway").Get<BraintreeGetewaySettings>();

    return new BraintreeGateway
    (
        config.Environment,  // "sandbox" ?? "production"
        config.MerchantId,
        config.PublicKey,
        config.PrivateKey
    );
});


//Add Identity 
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

//Add Authentication & Authorization
var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>();
builder.Services
     .AddAuthentication(options =>
     {
         options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
         options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
     })
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings.Issuer,
            ValidAudience = jwtSettings.Audience,
            IssuerSigningKey = new SymmetricSecurityKey
                           (Encoding.UTF8.GetBytes(jwtSettings.SecretKey)),
            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services.AddAuthorization();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "E-Commerce API", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter token. Example: 'abc123'"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
    });
});


var app = builder.Build();

app.UseMiddleware<GlobalExceptionHandlerMiddleware>();

using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
    await RoleInitializerHelper.SeedRoles(roleManager, userManager);
}

var allowHosts = builder.Configuration.GetSection("AllowedHosts").Get<string[]>() ?? new string[0];

app.UseCors(builder =>
{
    if (app.Environment.IsDevelopment())
    {
        builder.AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader();
        
    }
    else
    {
        builder.WithOrigins(allowHosts)
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials();
    }



});



// Configure the HTTP request pipeline.
if (app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

