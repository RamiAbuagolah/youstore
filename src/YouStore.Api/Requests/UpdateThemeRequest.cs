namespace YouStore.Api.Requests;

public sealed record UpdateThemeRequest(string PrimaryColor, string AccentColor, string FontFamily, string Background);
