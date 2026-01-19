using DtekShutdowns.Services;
using DtekShutdowns.Settings;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.Configure<GroupsConfig>(configuration.GetSection(GroupsConfig.Name));
builder.Services.Configure<DtekClientConfig>(configuration.GetSection(DtekClientConfig.Name));
builder.Services.AddHttpClient();
builder.Services.AddSingleton<IShutdownScheduleService, ShutdownScheduleService>();
//builder.Services.AddSingleton<IScheduleProvider, MockScheduleProvider>();
builder.Services.AddSingleton<IScheduleProvider, DtekScheduleProvider>();
builder.Services.AddSingleton<IScheduleConverter, ScheduleConverter>();
builder.Services.AddSingleton<IDtekPageParser, DtekPageParser>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
