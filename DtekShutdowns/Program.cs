using DtekShutdowns.Services;
using DtekShutdowns.Settings;
using Microsoft.Extensions.DependencyInjection.Extensions;

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
builder.Services.Configure<DtekScheduleProviderConfig>(configuration.GetSection(DtekScheduleProviderConfig.Name));
builder.Services.AddMemoryCache();
builder.Services.AddHttpClient();
var useMockScheduleProvider = configuration.GetValue<bool>("UseMockScheduleProvider");
if (useMockScheduleProvider)
{
    builder.Services.AddSingleton<IScheduleProvider, MockScheduleProvider>();
}
builder.Services.TryAddSingleton<IScheduleProvider, DtekScheduleProvider>();
builder.Services.AddSingleton<IShutdownScheduleService, ShutdownScheduleService>();
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
