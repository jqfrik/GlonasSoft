using GlonasSoft.Bll;
using GlonasSoft.Bll.Services;
using GlonasSoft.Dal;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMemoryCache();
var connectionString = builder.Configuration.GetConnectionString("GlonasDb");
builder.Services.AddBll(connectionString);

var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<GlonasContext>();
    await DataSeeder.Seed(dbContext);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Services.GetRequiredService<ShutdownHandler>();

app.Run();