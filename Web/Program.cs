using Api.Configuration;
using Application.Configuration;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "ExceptionFilters.Api", Version = "v1" });
});


builder.Services.AddPayloadApplicationMediatR();


builder.Services.AddMvc(options =>
{
    options.AddErrorFilters();
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ExceptionFilters.Api v1"));
}


app.Use(async (ctx, next) =>
{
    ctx.Request.EnableBuffering();
    await next.Invoke();
});

app.MapControllers();

app.Run();
