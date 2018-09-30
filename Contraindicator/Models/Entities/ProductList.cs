using System.Collections.Generic;

namespace Contraindicator.Models.Entities
{
    public class ProductList
    {
        public IEnumerable<ProductListItem> Products { get; set; }
    }
}
