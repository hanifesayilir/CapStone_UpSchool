using Application;
using Application.Common.Interfaces;
using Domain.Settings;
using Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using WebApi.Filters;
using WebApi.Hubs;
using WebApi.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

builder.Services.AddScoped<ICurrentUserService, CurrentUserManager>();

builder.Services.AddControllers(opt=>
{
    opt.Filters.Add<GlobalExceptionFilter>();
});


builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));
builder.Services.Configure<GoogleSettings>(builder.Configuration.GetSection("GoogleSettings"));

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(setupAction =>
{
    setupAction.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        Description = $"Input your Bearer token in this format - Bearer token to access this API",
    });
    setupAction.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer",
                },
            }, new List<string>()
        },
    });
});


builder.Services.AddSignalR();
builder.Services.AddScoped<IOrderEventHubService, OrderEventHubManager>();
builder.Services.AddScoped<INotificationApplicationHubService, NotificationApplicationHubManager>();

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructure(builder.Configuration, builder.Environment.WebRootPath);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
       .AddJwtBearer(o =>
       {
           o.RequireHttpsMetadata = false;
           o.SaveToken = false;
           o.TokenValidationParameters = new TokenValidationParameters
           {
               ValidateIssuerSigningKey = true,
               ValidateIssuer = true,
               ValidateAudience = true,
               ValidateLifetime = true,
               ClockSkew = TimeSpan.Zero,
               ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
               ValidAudience = builder.Configuration["JwtSettings:Audience"],
               IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:SecretKey"]))
           };

           o.Events = new JwtBearerEvents
           {
               OnMessageReceived = context =>
               {
                   var accessToken = context.Request.Query["access_token"];

                 
                   var path = context.HttpContext.Request.Path;
                   if (!string.IsNullOrEmpty(accessToken) &&
                       (
                       path.StartsWithSegments("/Hubs/CrawlerLogHub") ||                      
                        path.StartsWithSegments("/Hubs/OrderEventHub") || 
                      // path.StartsWithSegments("/Hubs/CrawlerSendingParameters")  || 
                       path.StartsWithSegments("/Hubs/NotificationApplicationHub"
                       )
                       ))
                   {
                     
                       context.Token = accessToken;
                   }
                   return Task.CompletedTask;
               }
           };
       });

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder => builder
            .AllowAnyMethod()
            .AllowCredentials()
            .SetIsOriginAllowed((host) => true)
            .AllowAnyHeader());
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseStaticFiles();


app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();

app.MapHub<CrawlerLogHub>("/Hubs/CrawlerLogHub");
app.MapHub<OrderEventHub>("/Hubs/OrderEventHub");
app.MapHub<CrawlerSendingParameters>("/Hubs/CrawlerSendingParameters");
app.MapHub<NotificationApplicationHub>("/Hubs/NotificationApplicationHub");


app.Run();
