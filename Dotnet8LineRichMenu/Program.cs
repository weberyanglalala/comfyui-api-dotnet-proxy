using System.Text;
using CloudinaryDotNet;
using Dotnet8LineRichMenu.Middleware;
using Dotnet8LineRichMenu.Models.Settings;
using Dotnet8LineRichMenu.Services;
using Dotnet8LineRichMenu.Services.Dify;
using Dotnet8LineRichMenu.Services.UserService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Serilog;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.Configure<MongoDbSettings>(
    builder.Configuration.GetSection("MongoDbSettings"));

builder.Services.AddScoped<UserService>(); 
builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });
builder.Host.UseSerilog((hostingContext, loggerConfiguration) => loggerConfiguration
    .ReadFrom.Configuration(hostingContext.Configuration));
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "default",
        policy => { policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod(); });
});
builder.Services.AddControllersWithViews();
builder.Services
    .Configure<LineMessagingApiSettings>(builder.Configuration.GetSection(nameof(LineMessagingApiSettings)))
    .AddSingleton(settings => settings.GetRequiredService<IOptions<LineMessagingApiSettings>>().Value);
builder.Services
    .Configure<SimpleTextPromptFlowSettings>(builder.Configuration.GetSection(nameof(SimpleTextPromptFlowSettings)))
    .AddSingleton(settings => settings.GetRequiredService<IOptions<SimpleTextPromptFlowSettings>>().Value);
builder.Services.AddHttpClient();
builder.Services.AddScoped<SimpleTextPromptService>();
builder.Services.AddScoped<StableDiffusionPromptEnhancerService>();
builder.Services
    .Configure<CloudinarySettings>(builder.Configuration.GetSection(nameof(CloudinarySettings)))
    .AddSingleton(settings => settings.GetRequiredService<IOptions<CloudinarySettings>>().Value);
builder.Services.AddSingleton(sp =>
{
    var cloudinarySettings = sp.GetRequiredService<CloudinarySettings>();
    return new Cloudinary(new Account(cloudinarySettings.CloudName, cloudinarySettings.ApiKey,
        cloudinarySettings.ApiSecret));
});
builder.Services.AddScoped<CloudinaryService>();
builder.Services.AddScoped<DifyService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseCors("default");
app.UseAuthentication();
app.UseAuthorization();
app.UseSerilogRequestLogging();
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.Run();