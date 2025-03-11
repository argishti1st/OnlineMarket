using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineMarket.Application.DtoS
{
    public record OrderDto(
        int Id,
        DateTime CreatedAt,
        string CreatedBy,
        bool IsDeleted,
        IEnumerable<ProductDto> Products);
}
