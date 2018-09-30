using Neo4jClient;
using Contraindicator.Models.Nodes;

namespace Contraindicator.Models.Relationships
{
    public class ContraindicateRelationship : Relationship<Contraindicate>, IRelationshipAllowingSourceNode<Substance>, IRelationshipAllowingTargetNode<Substance>
    {
        public static readonly string TypeKey = "Contraindicate";

        public ContraindicateRelationship() : base(-1, null)
        {
        }

        public ContraindicateRelationship(NodeReference targetNode, Contraindicate data) : base(targetNode, data)
        {
        }

        public override string RelationshipTypeKey => TypeKey;
    }
}
