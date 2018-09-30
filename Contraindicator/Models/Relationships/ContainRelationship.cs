using Neo4jClient;
using Contraindicator.Models.Nodes;

namespace Contraindicator.Models.Relationships
{
    public class ContainRelationship : Relationship<Contain>, IRelationshipAllowingSourceNode<Product>, IRelationshipAllowingTargetNode<Substance>
    {
        public static readonly string TypeKey = "Contain";

        public ContainRelationship() : base(-1, null)
        {
        }

        public ContainRelationship(NodeReference targetNode, Contain data) : base(targetNode, data)
        {
        }

        public override string RelationshipTypeKey => TypeKey;
    }
}
