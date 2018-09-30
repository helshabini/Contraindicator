using Neo4jClient;
using Contraindicator.Models.Nodes;
using Contraindicator.Models.Relationships;

namespace Contraindicator.Models.Entities
{
    public class SubstanceListItem
    {
        public RelationshipInstance<Contraindicate> Relationship { get; set; }
        public Node<Substance> Node { get; set; }
    }
}
