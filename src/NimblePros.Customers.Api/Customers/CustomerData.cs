using NimblePros.Customers.Domain;

namespace NimblePros.Customers.Api.Customers;

public class CustomerData
{
    private readonly Guid _customer1Id = Guid.Parse("aab80f69-c6b9-4d68-bcfa-e71994680475");
    private readonly Guid _customer2Id = Guid.Parse("fdb83f86-4ea6-4c59-95b7-f32d3a3172f5");
    private readonly List<Customer> _customers;

    public CustomerData()
    {
        _customers = [
            new(_customer1Id, "Acme Corporation", "contact@acme.com",
            [
                new Project(Guid.NewGuid(), "Project A", Guid.NewGuid()),
                new Project(Guid.NewGuid(), "Project B", Guid.NewGuid())
            ]),
            new(_customer2Id, "Globex Corporation", "contact@globex.com",
            [
                new Project(Guid.NewGuid(), "Project C", Guid.NewGuid()),
                new Project(Guid.NewGuid(), "Project D", Guid.NewGuid())
            ])
        ];
    }

    public Task<List<Customer>> ListAsync()
    {
        return Task.FromResult(_customers);
    }

    public Task<Customer?> GetByIdAsync(Guid id)
    {
        return Task.FromResult(_customers.FirstOrDefault(c => c.Id == id));
    }

    public Task AddAsync(Customer customer)
    {
        _customers.Add(customer);
        return Task.CompletedTask;
    }

    public Task UpdateAsync(Guid id, Customer customer)
    {
        var index = _customers.FindIndex(c => c.Id == id);

        if (index >= 0)
        {
            _customers[index] = customer;
        }

        return Task.CompletedTask;
    }

    public Task DeleteAsync(Guid id)
    {
        var index = _customers.FindIndex(c => c.Id == id);

        if (index >= 0)
        {
            _customers.RemoveAt(index);
        }

        return Task.CompletedTask;
    }
}