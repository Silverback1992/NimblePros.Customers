using NimblePros.Customers.Domain;

/// <summary>
/// Historically here, nothing uses it, don't add. You could use validation like this as well if you wanted to with AddEndpointFilter at the endpoints.
/// </summary>
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
