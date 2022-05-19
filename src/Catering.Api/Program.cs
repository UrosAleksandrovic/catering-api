using Catering.Api.Configuration;
using Catering.DependencyInjection;
using Catering.Infrastructure.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddCateringSwagger();

builder.Services.AddCateringAuthentication(builder.Configuration);

builder.Services.AddCateringDependecies(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseCateringSwagger();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
