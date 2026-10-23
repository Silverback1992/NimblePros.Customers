using NimblePros.Customers.Application.Interfaces;
using NimblePros.Customers.Domain;

namespace NimblePros.Customers.Application;

public class ConsoleOnlyEmailSenderService : IEmailSenderService
{
    public Task SendEmailAsync(MessageDetails messageDetails)
    {
        Console.WriteLine($"Sending email to {messageDetails.To} from {messageDetails.From} with subject {messageDetails.Subject}...");
        return Task.CompletedTask;
    }
}
