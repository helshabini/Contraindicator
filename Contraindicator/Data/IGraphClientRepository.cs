using Neo4jClient;
using System.Threading.Tasks;
using System.Collections.Generic;
using Contraindicator.Models.Entities;
using Contraindicator.Models.Nodes;
using Contraindicator.Models.Relationships;

namespace Contraindicator.Data
{
    public interface IGraphClientRepository
    {
        #region Initialization

        Task InitializeAsync();

        bool IsInitialized();

        #endregion

        #region Product

        Task<Node<Product>> CreateProductAsync(Product product);

        Task<Node<Product>> GetProductAsync(string productId);

        Task<Node<Product>> UpdateProductAsync(string productId, Product data);

        Task<bool> DeleteProductAsync(string productId);

        #endregion

        #region Substance

        Task<Node<Substance>> CreateSubstanceAsync(Substance substance);

        Task<Node<Substance>> GetSubstanceAsync(string substanceId);

        Task<Node<Substance>> UpdateSubstanceAsync(string substanceId, Substance data);

        Task<bool> DeleteSubstanceAsync(string substanceId);

        #endregion

        #region Contain

        Task<RelationshipInstance<Contain>> CreateContainAsync(string productId, string substanceId,
            Contain data = null);

        Task<RelationshipInstance<Contain>> GetContainAsync(string productId, string substanceId);

        Task<RelationshipInstance<Contain>> UpdateContainAsync(string productId, string substanceId,
            Contain data = null);

        Task<bool> DeleteContainAsync(string productId, string substanceId);

        Task<SubstanceList> GetIngredientsAsync(string productId);

        #endregion

        #region Contraindicate

        Task<RelationshipInstance<Contraindicate>> CreateContraindicateAsync(string sourceSubstanceId, string targetSubstanceId,
            Contraindicate data = null);

        Task<RelationshipInstance<Contraindicate>> GetContraindicateAsync(string sourceSubstanceId, string targetSubstanceId);

        Task<RelationshipInstance<Contraindicate>> UpdateContraindicateAsync(string sourceSubstanceId, string targetSubstanceId,
            Contain data = null);
        Task<bool> DeleteContraindicateAsync(string sourceSubstanceId, string targetSubstanceId);

        Task<SubstanceList> GetContraindicationsAsync(string substanceId);

        #endregion
    }
}
