using NimblePros.Customers.Domain;

namespace NimblePros.Customers.Application.Interfaces;

public interface ICustomerEmailService
{
    Task SendWelcomeEmail(Customer customer);
}
