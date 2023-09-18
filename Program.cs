using Microsoft.AspNetCore.Mvc;
using BokhandelensRESTApi.DATA;
using BokhandelensRESTApi.Repository;
using Serilog;
using BokhandelensRESTApi.Endpoints;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IBookRepository, BookHandler>();

builder.Host.UseSerilog((context, configuration) =>
{
    configuration.ReadFrom.Configuration(context.Configuration);

});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


var logger = (ILogger<Program>)app.Services.GetService(typeof(ILogger<Program>))!;

app.Use(async (context, next) =>
{
    try
    {
        await next(context);
    }
    catch (Exception ex)
    {

        logger.LogError(ex, "Something went wrong {@MAchine} {@TraceId}",
            Environment.MachineName,
            System.Diagnostics.Activity.Current?.Id);

        await Results.Problem(
            title: "Something went wrong",
            statusCode: StatusCodes.Status500InternalServerError,
            extensions: new Dictionary<string, Object?>
            {
                    { "traceID", System.Diagnostics.Activity.Current?.Id },

            }).ExecuteAsync(context);
    }

});

app.UseSerilogRequestLogging();
app.UseHttpsRedirection();
app.MapBookEndpoints();



app.Run();

