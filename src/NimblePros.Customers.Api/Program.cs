using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;
using NimblePros.Customers.Api.Customers;
using NimblePros.Customers.Application;
using NimblePros.Customers.Application.Interfaces;
using NimblePros.Customers.Domain;

var builder = WebApplication.CreateBuilder(args);

// Add EF Core servoces to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<CustomerDbContext>(options =>
    options.UseSqlite(connectionString));

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer((document, context, cancellationToken) =>
    {
        document.Info = new OpenApiInfo
        {
            Title = "NimblePros Customers API",
            Description = "API for managing customers",
            Version = "1.0"
        };
        return Task.CompletedTask;
    });
});
builder.Services.AddSingleton<CustomerData>();
builder.Services.AddValidation();
builder.Services.AddTransient<IEmailMessageFactory, EmailMessageFactory>();
if (builder.Environment.IsDevelopment())
{
    builder.Services.AddTransient<IEmailSenderService, ConsoleOnlyEmailSenderService>();
}
else
{
    builder.Services.AddTransient<IEmailSenderService, MailKitEmailSenderService>();
}
builder.Services.AddTransient<ICustomerEmailService, CustomerEmailService>();

var mailSettings = builder.Configuration.GetSection(nameof(MailSettings)).Get<MailSettings>();
builder.Services.AddSingleton(mailSettings);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options => options.SwaggerEndpoint("/openapi/v1.json", "v1"));
}

app.UseHttpsRedirection();

app.MapCustomerEndpoints();

app.MapBrewCoffee();

app.Run();
