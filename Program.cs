using PowerBIService;
using PowerBIService.Services;
using Serilog;
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("log.txt")
    .CreateLogger();
var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddSingleton<IService, Service>();
builder.Services.AddHostedService<Worker>();
builder.Services.AddSerilog();
builder.Services.AddWindowsService(options =>
{
    options.ServiceName = "PowerBIService";
});

var host = builder.Build();
host.Run();
