using EnumsNET;
using System.ComponentModel;

namespace Domain.Shared;

public enum CareerRole
{
    [Description("\ud83e\udd77 Опыт не известен")]
    Unknown = 0,

    [Description("\ud83e\uddd1\u200d\ud83c\udfeb Amateur")]
    Amateur = 10,

    [Description("\ud83e\uddd1\u200d\ud83c\udfeb Amateur +")]
    AmateurPlus = 11,

    [Description("\ud83e\uddd1\u200d\ud83c\udf93 Intern")]
    Intern = 20,

    [Description("\ud83e\uddd1\u200d\ud83c\udf93 Intern +")]
    InternPlus = 21,

    [Description("\ud83e\udd35 Junior")]
    Junior = 3,

    [Description("\ud83e\udd35\u200d\u2642\ufe0f Middle")]
    Middle = 4,

    [Description("\ud83e\uddb8\u200d\u2642\ufe0f Senior")]
    Senior = 5,
}

public static class CareerRoleExtensions
{
    /// <summary>
    /// Повысить роль по прохождению собеседования.
    /// </summary>
    /// <param name="role">Роль которую требуеются забустить.</param>
    /// <returns>Следующую роль, которая выдается после прохождения собеседования.</returns>
    /// <exception cref="ArgumentException">Роль нельзя забустить.</exception>
    public static CareerRole BoostByInterview(this CareerRole role)
    {
        if (role is CareerRole.Amateur)
            return CareerRole.AmateurPlus;

        if (role is CareerRole.AmateurPlus)
            return CareerRole.Intern;

        if (role is CareerRole.Intern)
            return CareerRole.InternPlus;

        throw new ArgumentException("Роль нельзя забустить");
    }

    /// <summary>
    /// Проверить что роль является следующей после текущей.
    /// </summary>
    public static bool HasNext(this CareerRole role, CareerRole nextRole) =>
        role is CareerRole.Amateur && nextRole is CareerRole.AmateurPlus ||
        role is CareerRole.AmateurPlus && nextRole is CareerRole.Intern ||
        role is CareerRole.Intern && nextRole is CareerRole.InternPlus;

    /// <summary>
    /// Получить реальное название роли.
    /// </summary>
    public static string GetRealName(this CareerRole role)
    {
        return role.AsString(EnumFormat.Description)!;
    }

    /// <summary>
    /// Получить реальное название роли.
    /// </summary>
    public static CareerRole GetFromRealName(string realRole)
    {
        foreach (var field in typeof(CareerRole).GetFields())
        {
            if (Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) is DescriptionAttribute attribute)
            {
                if (attribute.Description == realRole)
                    return (CareerRole)field.GetValue(null);
            }
            else
            {
                if (field.Name == realRole)
                    return (CareerRole)field.GetValue(null);
            }
        }

        throw new ArgumentException("Роль не найдена", nameof(realRole));
    }

    /// <summary>
    /// Проверить валидность роли.
    /// </summary>
    public static bool IsRealRoleValid(string realRole)
    {
        foreach (var field in typeof(CareerRole).GetFields())
        {
            if (Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) is DescriptionAttribute attribute)
            {
                if (attribute.Description == realRole)
                    return true;
            }
            else
            {
                if (field.Name == realRole)
                    return true;
            }
        }

        return false;
    }
}
