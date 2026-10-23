using NimblePros.Customers.Application.Interfaces;

namespace NimblePros.Customers.Application;

public class EmailMessageFactory : IEmailMessageFactory
{
    public string GenerateWelcomeEmailBody(string companyName)
    {
        string template = "Welcome to NimblePros, {{CompanyName}}!";
        string body = template.Replace("{{CompanyName}}", companyName);
        return body;
    }
}
