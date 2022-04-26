using CoreJwtExample.IRepository;
using CoreJwtExample.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Security.Cryptography.Xml;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

var corsBuilder = new CorsPolicyBuilder();
corsBuilder.AllowAnyHeader();
corsBuilder.AllowAnyMethod();
corsBuilder.AllowAnyOrigin();
corsBuilder.WithOrigins("http://localhost:8080");
corsBuilder.AllowCredentials();
builder.Services.AddCors(options =>
{
    options.AddPolicy("SiteCorsPolicy", corsBuilder.Build());
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Issuer"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(
    c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "CoreJwtExample", Version = "v1" });
        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Description = @"JWT Authorization header using the Bearer scheme.
                            Enter 'Bearer' [space] and then your token in the input below.
                            Example: 'Bearer asdewq123'",
            Name = "Authorization",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Bearer"
        });
        c.AddSecurityRequirement(new OpenApiSecurityRequirement()
        {
            {
                new OpenApiSecurityScheme()
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    },
                    Scheme = "oauth2",
                    Name = "Bearer",
                    In = ParameterLocation.Header,
                },
                new List<string>()
            }
        });
    });
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c=>c.SwaggerEndpoint("/swagger/v1/swagger.json","CoreJwtExample v1"));
}

app.UseHttpsRedirection();

app.UseCors("SiteCorsPolicy");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
