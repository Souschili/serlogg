

using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, Configuration) =>
    Configuration.ReadFrom.Configuration(context.Configuration));

try
{
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
    app.UseSerilogRequestLogging(options =>
        {
            options.MessageTemplate =
                "{RemoteIpAddress} {RequestScheme} {RequestHost} {RequestMethod} {RequestPath} responded {StatusCode} in {Elapsed:0.0000} ms";
            options.EnrichDiagnosticContext = (
                diagnosticContext,
                httpContext) =>
            {
                diagnosticContext.Set("RequestHost", httpContext.Request.Host.Value);
                diagnosticContext.Set("RequestScheme", httpContext.Request.Scheme);
                diagnosticContext.Set("RemoteIpAddress", httpContext.Connection.RemoteIpAddress);
            };
        });

    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal($"Application stop working ");
}
finally
{
    Log.CloseAndFlush();
}
