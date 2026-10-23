namespace NimblePros.Customers.Domain;

public record Customer(Guid Id, string CompanyName, string EmailAddress, List<Project> Projects);
