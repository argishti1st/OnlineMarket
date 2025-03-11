using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineMarket.Application.DtoS
{
    public record ProductDto(
        int Id,
        string Name,
        string Description,
        decimal Price,
        int Quantity);
}
