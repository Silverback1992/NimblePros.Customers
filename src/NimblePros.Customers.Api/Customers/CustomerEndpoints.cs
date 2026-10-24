using Microsoft.AspNetCore.Http.HttpResults;
using NimblePros.Customers.Application.Interfaces;
using NimblePros.Customers.Domain;

namespace NimblePros.Customers.Api.Customers;

public static class CustomerEndpoints
{
    public static void MapCustomerEndpoints(this WebApplication app)
    {
        var _customerGroup = app.MapGroup("/customers").WithTags("Customers");

        _customerGroup.MapGet("/", async (CustomerData data) =>
        {
            var customers = await data.ListAsync();
            return TypedResults.Ok(customers);
        })
        .WithSummary("Retrieves a list of all customers.")
        .WithDescription("derp.")
        .WithName("ListCustomers");

        _customerGroup.MapGet("/{id:guid}", async Task<Results<Ok<Customer>, NotFound>> (Guid id, CustomerData data) =>
            await data.GetByIdAsync(id) is Customer customer
                ? TypedResults.Ok(customer)
                : TypedResults.NotFound()
        )
        .WithName("GetCustomerById");

        _customerGroup.MapPost("/", async (CustomerRequest request,
                                           CustomerData data,
                                           ICustomerEmailService customerEmailService) =>
        {
            var customer = new Customer(Guid.NewGuid(), request.CompanyName, request.EmailAddress, request.Projects);

            await customerEmailService.SendWelcomeEmail(customer);

            await data.AddAsync(customer);
            return Results.Created($"/customers/{customer.Id}", customer);
        })
        .Produces<Customer>(StatusCodes.Status201Created)
        .ProducesValidationProblem(StatusCodes.Status400BadRequest)
        .WithName("AddCustomer");

        _customerGroup.MapPut("/{id:guid}",
            async Task<Results<Ok<Customer>, NotFound, ValidationProblem>> (Guid id, CustomerRequest request, CustomerData data) =>
            {
                var existingCustomer = await data.GetByIdAsync(id);

                if (existingCustomer is null)
                {
                    return TypedResults.NotFound();
                }

                var customerToUpdate = existingCustomer with
                {
                    CompanyName = request.CompanyName,
                    Projects = request.Projects
                };

                await data.UpdateAsync(id, customerToUpdate);
                return TypedResults.Ok(customerToUpdate);
            })
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithName("UpdateCustomer");

        _customerGroup.MapDelete("/{id:guid}", async Task<Results<NoContent, NotFound>> (Guid id, CustomerData data) =>
        {
            if (await data.GetByIdAsync(id) == null)
            {
                return TypedResults.NotFound();
            }

            await data.DeleteAsync(id);
            return TypedResults.NoContent();
        })
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithName("DeleteCustomer");
    }
}
