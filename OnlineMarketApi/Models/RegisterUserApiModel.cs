namespace OnlineMarket.Api.Models
{
    public record RegisterUserApiModel(
        string Email,
        string Username,
        string Password,
        string Role);
}
