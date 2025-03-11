namespace OnlineMarket.Application.DtoS
{
    public record ProductDto(
        int Id,
        string Name,
        string Description,
        decimal Price,
        int Quantity,
        string Category);
}
