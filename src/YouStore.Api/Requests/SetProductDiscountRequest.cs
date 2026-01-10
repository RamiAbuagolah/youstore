namespace YouStore.Api.Requests;

public sealed record SetProductDiscountRequest(decimal DiscountPrice, string Description);
