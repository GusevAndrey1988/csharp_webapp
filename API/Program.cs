using Microsoft.EntityFrameworkCore;
using Domain.UseCases.GetForums;
using Storage;
using Domain.UseCases.CreateTopic;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString(name: "Postgresql");

builder.Services.AddScoped<IGetForumsUseCase, GetForumsUseCase>();
builder.Services.AddScoped<ICreateTopicUseCase, CreateTopicUseCase>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContextPool<ForumDbContext>(options => options.UseNpgsql(connectionString));

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthorization();

app.MapControllers();

app.Run();
