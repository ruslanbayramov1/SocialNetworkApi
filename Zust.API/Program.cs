using Microsoft.Extensions.Options;
using Zust.API;
using Zust.BL;
using Zust.DAL;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(builder.Configuration);

builder.Services.AddHttpContextAccessor();
builder.Services.AddOptionPatterns(builder.Configuration);
builder.Services.AddJwtAuthentication(builder.Configuration);

builder.Services.AddControllersWithViews();
builder.Services.AddDbContext(builder.Configuration);
builder.Services.AddRepositories();
builder.Services.AddServices();
builder.Services.AddAutoMapper();
builder.Services.AddFluentValidation();
builder.Services.AddCacheServices(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(opt =>
    {
        opt.EnablePersistAuthorization();
        opt.SwaggerEndpoint("/swagger/v1/swagger.json", "Zust API v1");
    });
}

app.UseHttpsRedirection();

app.UseZustExceptionHandler();

app.UseAuthorization();
app.UseSeedData(builder.Configuration);

app.MapControllers();

app.Run();
