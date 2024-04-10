using Domain.Shared;

namespace Guard.Domain.ValueObjects;

/// <summary>
/// Повышение роли.
/// </summary>
public readonly record struct RoleEnhancement
{
    public CareerRole From { get; init; }
    public CareerRole To { get; init; }

    public static RoleEnhancement Create(CareerRole from, CareerRole to)
    {
        if (!from.HasNext(to))
            throw new ArgumentException("Роли невалидны");

        return new RoleEnhancement() { From = from, To = to };
    }
}