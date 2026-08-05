using NimblePros.Customers.Api.Customers;

public class ValidateCustomer : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var customer = context.Arguments.OfType<Customer>().FirstOrDefault();

        if (customer is null || string.IsNullOrWhiteSpace(customer.CompanyName))
        {
            return TypedResults.ValidationProblem(new Dictionary<string, string[]>
            {
                [nameof(Customer.CompanyName)] = ["Company name is required."]
            });
        }

        return await next(context);
    }
}
