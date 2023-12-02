using NewRelic.Headless.Logging;
using NewRelic.Headless.Logging.Client;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddNewRelicClient(options =>
{
    options.ApiKey = "YOUR_NEW_RELIC_INGEST_LICENSE_API_KEY";
    options.Region = NewRelicRegion.EU;
});

builder.Logging.ClearProviders();
builder.Logging.AddNewRelicLogger(config =>
{
    config.Enabled = true;
    config.EntityGuid = "YOUR_NEW_RELIC_ENTITY_GUID";
    config.EntityName = "YOUR_NEW_RELIC_ENTITY_NAME";
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
