using CSharpFunctionalExtensions;
using PetHome.Domain.Shared.Error;

namespace PetHome.Domain.GeneralValueObjects;
public record Requisites
{
    public string Name { get; }
    public string Description { get; }
    public PaymentMethodEnum PaymentMethod { get; }

    private Requisites() { }
    private Requisites(string name, string description, PaymentMethodEnum paymentMethod)
    {
        Name = name;
        Description = description;
        PaymentMethod = paymentMethod;
    }

    public static Result<Requisites, Error> Create(string name, string description, PaymentMethodEnum paymentMethod)
    {
        bool isInvalidRequisite = string.IsNullOrEmpty(name) || string.IsNullOrEmpty(description) || paymentMethod == null;

        if (isInvalidRequisite)
            return Errors.Validation("Реквезиты");

        return new Requisites(name, description, paymentMethod);
    }
}
