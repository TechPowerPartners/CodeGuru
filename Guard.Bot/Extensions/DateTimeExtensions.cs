using System.Globalization;

namespace Guard.Bot.Extensions;

public static class DateTimeExtensions
{
	public static long ToUnixTimestamp(this DateTime dateTime) =>
	   new DateTimeOffset(dateTime).ToUnixTimeSeconds();

	public static bool TryParseWithRuCulture(this string? str, out DateOnly date) =>
	   DateOnly.TryParse(str, CultureInfo.GetCultureInfo("ru-RU"), out date);

	public static bool TryParseWithRuCulture(this string? str, out TimeOnly time) =>
	   TimeOnly.TryParse(str, CultureInfo.GetCultureInfo("ru-RU"), out time);
}