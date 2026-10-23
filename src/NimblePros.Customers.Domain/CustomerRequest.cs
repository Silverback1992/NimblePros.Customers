using System.ComponentModel.DataAnnotations;

namespace NimblePros.Customers.Domain;

public record CustomerRequest : IValidatableObject
{
    [Required]
    [MinLength(10)]
    public string CompanyName { get; init; } = string.Empty;
    [Required]
    [MinLength(10)]
    public string EmailAddress { get; init; } = string.Empty;
    public List<Project> Projects { get; init; } = [];

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (CompanyName == "NimblePros")
        {
            yield return new ValidationResult("Company name cannot be 'NimblePros'.", [nameof(CompanyName)]);
        }
    }
}
