using Microsoft.AspNetCore.Http.HttpResults;
using System.ComponentModel.DataAnnotations;

namespace NimblePros.Customers.Api.Customers;

public static class CustomerEndpoints
{
    public static void MapCustomerEndpoints(this WebApplication app)
    {
        var _customerGroup = app.MapGroup("/customers");

        var _customerGroupWithValidation = _customerGroup.MapGroup("/")
            .WithParameterValidation();

        _customerGroup.MapGet("/", async (CustomerData data) =>
        {
            var customers = await data.ListAsync();
            return TypedResults.Ok(customers);
        })
        .WithName("ListCustomers");

        _customerGroup.MapGet("/{id:guid}", async Task<Results<Ok<Customer>, NotFound>> (Guid id, CustomerData data) =>
            await data.GetByIdAsync(id) is Customer customer
                ? TypedResults.Ok(customer)
                : TypedResults.NotFound()
        )
        .WithName("GetCustomerById");

        _customerGroupWithValidation.MapPost("/", async (CustomerRequest request, CustomerData data) =>
        {
            var customer = new Customer(Guid.NewGuid(), request.CompanyName, []);
            await data.AddAsync(customer);
            return Results.Created($"/customers/{customer.Id}", customer);
        })
        .WithName("AddCustomer");

        _customerGroupWithValidation.MapPut("/{id:guid}",
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
        .WithName("DeleteCustomer");
    }
}

public record Customer(Guid Id, string CompanyName, List<Project> Projects);
public record Project(Guid Id, string ProjectName, Guid CustomerId);

public record CustomerRequest : IValidatableObject
{
    [Required]
    [MinLength(10)]
    public required string CompanyName { get; init; }
    public List<Project> Projects { get; init; } = [];

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (CompanyName == "NimblePros")
        {
            yield return new ValidationResult("Company name cannot be 'NimblePros'.", [nameof(CompanyName)]);
        }
    }
}
