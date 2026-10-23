using NimblePros.Customers.Application.Interfaces;
using NimblePros.Customers.Domain;

namespace NimblePros.Customers.Application;

public class CustomerEmailService : ICustomerEmailService
{
    private readonly IEmailMessageFactory _emailMessageFactory;
    private readonly IEmailSenderService _emailSenderService;

    public CustomerEmailService(IEmailMessageFactory emailMessageFactory, IEmailSenderService emailSenderService)
    {
        _emailMessageFactory = emailMessageFactory;
        _emailSenderService = emailSenderService;
    }

    public async Task SendWelcomeEmail(Customer customer)
    {
        var messageDetails = new MessageDetails
        {
            From = "donotreply@nimblepros.com",
            To = customer.EmailAddress,
            Subject = "Welcome to NimblePros!",
            Body = _emailMessageFactory.GenerateWelcomeEmailBody(customer.CompanyName)
        };

        Console.WriteLine($"Attempting to send email to {messageDetails.To} from {messageDetails.From} with subject {messageDetails.Subject}...");

        await _emailSenderService.SendEmailAsync(messageDetails);
    }
}
