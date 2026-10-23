using NimblePros.Customers.Domain;

namespace NimblePros.Customers.Application.Interfaces;

public interface IEmailSenderService
{
    Task SendEmailAsync(MessageDetails messageDetails);
}
