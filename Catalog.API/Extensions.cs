using Catalog.API.DTOs;
using Catalog.API.Entities;

namespace Catalog.API
{
    public static class Extensions
    {
        public static ItemDTO AsDTO(this Item item)
        {
            return new ItemDTO(item.Id, item.Name, item.Description, item.Price, item.CreatedDate);
        }
    }
}