namespace NimblePros.Customers.Domain;

public record Project(Guid Id, string ProjectName, Guid CustomerId)
{
    private Project() : this(Guid.Empty, string.Empty, Guid.Empty)
    {
    }
}