namespace OnlineMarket.Application.DtoS
{
    public record OrderDto(
        int Id,
        DateTime CreatedAt,
        string CreatedBy,
        bool IsDeleted,
        IEnumerable<ProductDto> Products);
}
