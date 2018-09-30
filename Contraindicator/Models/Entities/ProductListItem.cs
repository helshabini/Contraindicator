using Neo4jClient;
using Contraindicator.Models.Nodes;
using Contraindicator.Models.Relationships;

namespace Contraindicator.Models.Entities
{
    public class ProductListItem
    {
        public RelationshipInstance<Contain> Relationship { get; set; }
        public Node<Product> Node { get; set; }
    }
}
