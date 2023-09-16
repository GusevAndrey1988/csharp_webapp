using Microsoft.EntityFrameworkCore;
using Storage;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContextPool<ForumDbContext>(options => options
        .UseNpgsql("User Id=andrey;Password=root;Host=localhost;Port=5432;Database=csharp_web_api;Pooling=true;MinPoolSize=0;MaxPoolSize=100;Connection Idle Lifetime=60;"));

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthorization();

app.MapControllers();

app.Run();
