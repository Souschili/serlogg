using Serilog;
using Serilog.Events;

var builder = WebApplication.CreateBuilder(args);

// add serilog
builder.Host.UseSerilog();

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    //will cause all events from Microsoft, Microsoft.AspNetCore,
    //Microsoft.AspNetCore.Hosting, etc., to be logged at the Warning level
    // the same with system
    .MinimumLevel.Override("Microsoft",LogEventLevel.Error)  
    .MinimumLevel.Override("Microsoft.AspNetCore",LogEventLevel.Warning)  
    .MinimumLevel.Override("System",LogEventLevel.Error)
    .WriteTo.Console()
    .WriteTo.File("Logs/app-log-.txt",rollingInterval:RollingInterval.Day,
    rollOnFileSizeLimit:true,fileSizeLimitBytes: 524288000)
    .CreateLogger();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSerilogRequestLogging();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
