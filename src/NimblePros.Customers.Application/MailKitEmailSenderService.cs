using MailKit.Net.Smtp;
using MimeKit;
using NimblePros.Customers.Application.Interfaces;
using NimblePros.Customers.Domain;

namespace NimblePros.Customers.Application;

public class MailKitEmailSenderService : IEmailSenderService
{
    public async Task SendEmailAsync(MessageDetails messageDetails)
    {
        using var client = new SmtpClient();

        await client.ConnectAsync("localhost", 25, false);

        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(messageDetails.From, messageDetails.From));
        message.To.Add(new MailboxAddress(messageDetails.To, messageDetails.To));
        message.Subject = messageDetails.Subject;
        message.Body = new TextPart("plain") { Text = messageDetails.Body };

        await client.SendAsync(message);

        Console.WriteLine("Email sent successfully.");

        client.Disconnect(true);
    }
}
