using Elasticsearch.Net;
using Nest;
using Elasticsource.API.Extention;
using Elasticsource.API.Services;
using Elasticsource.API.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<ProductRepository>();

builder.Services.AddSingleton<IElasticClient>(sp =>
{
    var settings = new ConnectionSettings(
        new Uri(builder.Configuration["Elasticsearch:Url"]!));

    return new ElasticClient(settings);
});
// Add Swagger/Swashbuckle
builder.Services.AddSwaggerGen();

builder.Services.AddElasticsource(builder.Configuration);
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
