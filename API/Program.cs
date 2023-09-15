using Microsoft.EntityFrameworkCore;
using Storage;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ForumDbContext>(options => options
        .UseNpgsql("User Id=andrey;Password=root;Host=localhost;Port=5432;Database=csharp_web_api;Pooling=true;MinPoolSize=0;MaxPoolSize=100;Connection Idle Lifetime=60;"),
        ServiceLifetime.Singleton);

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Services.GetRequiredService<ForumDbContext>().Database.Migrate();

app.Run();
