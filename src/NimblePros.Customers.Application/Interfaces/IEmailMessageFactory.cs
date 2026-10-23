namespace NimblePros.Customers.Application.Interfaces;

public interface IEmailMessageFactory
{
    string GenerateWelcomeEmailBody(string companyName);
}
