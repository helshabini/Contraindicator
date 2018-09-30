using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Neo4jClient;
using Contraindicator.Models.Entities;
using Contraindicator.Models.Nodes;
using Contraindicator.Models.Relationships;

namespace Contraindicator.Data
{
    public class GraphClientRepository : IGraphClientRepository
    {
        private readonly ILogger<GraphClientRepository> _logger;
        private readonly IGraphClient _client;

        public GraphClientRepository(IGraphClientFactory factory, ILogger<GraphClientRepository> logger)
        {
            _logger = logger;
            _client = factory.Create();
        }

        #region Initialization

        public async Task InitializeAsync()
        {
            try
            {
                await _client.Cypher.Create("CONSTRAINT ON (p:Product) ASSERT p.ProductId IS UNIQUE")
                    .ExecuteWithoutResultsAsync();
                await _client.Cypher.Create("CONSTRAINT ON (s:Substance) ASSERT s.SubstanceId IS UNIQUE")
                    .ExecuteWithoutResultsAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error executing graph query: {0}", ex.Message);
                throw new InvalidOperationException(ex.Message);
            }
        }

        public bool IsInitialized()
        {
            try
            {
                return _client.Cypher
                    .Match("(n)")
                    .Return<Node<object>>("n")
                    .Limit(1)
                    .Results.Any();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error executing graph query: {0}", ex.Message);
                throw new InvalidOperationException(ex.Message);
            }
        }

        #endregion

        #region Product

        public async Task<Node<Product>> CreateProductAsync(Product product)
        {
            try
            {
                var products = await _client.Cypher
                    .Create("(p:Product {product})")
                    .WithParams(new { product })
                    .Return<Node<Product>>("p")
                    .ResultsAsync;

                return products.Single();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error executing graph query: {0}", ex.Message);
                return null;
            }
        }

        public async Task<Node<Product>> GetProductAsync(string productId)
        {
            try
            {
                var products = await _client.Cypher
                    .Match("(p:Product)")
                    .Where((Product p) => p.ProductId == productId)
                    .Return<Node<Product>>("p")
                    .ResultsAsync;

                return products.SingleOrDefault();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error executing graph query: {0}", ex.Message);
                throw new InvalidOperationException(ex.Message);
            }
        }

        public async Task<Node<Product>> UpdateProductAsync(string productId, Product data)
        {
            try
            {
                var products = await _client.Cypher
                    .Match("(p:Product)")
                    .Where((Product p) => p.ProductId == productId)
                    .Set("p = {data}")
                    .WithParam("data", data)
                    .Return<Node<Product>>("p")
                    .ResultsAsync;

                return products.Single();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error executing graph query: {0}", ex.Message);
                throw new InvalidOperationException(ex.Message);
            }
        }

        public async Task<bool> DeleteProductAsync(string productId)
        {
            try
            {
                await _client.Cypher
                    .OptionalMatch("(p:Product)<-[r]-()")
                    .Where((Product p) => p.ProductId == productId)
                    .Delete("r, p")
                    .ExecuteWithoutResultsAsync();

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error executing graph query: {0}", ex.Message);
                throw new InvalidOperationException(ex.Message);
            }
        }

        #endregion

        #region Substance

        public async Task<Node<Substance>> CreateSubstanceAsync(Substance substance)
        {
            try
            {
                var substances = await _client.Cypher
                    .Create("(s:Substance {substance})")
                    .WithParams(new { substance })
                    .Return<Node<Substance>>("s")
                    .ResultsAsync;

                return substances.Single();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error executing graph query: {0}", ex.Message);
                return null;
            }
        }

        public async Task<Node<Substance>> GetSubstanceAsync(string substanceId)
        {
            try
            {
                var substances = await _client.Cypher
                    .Match("(s:Substance)")
                    .Where((Substance m) => m.SubstanceId == substanceId)
                    .Return<Node<Substance>>("s")
                    .ResultsAsync;

                return substances.SingleOrDefault();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error executing graph query: {0}", ex.Message);
                throw new InvalidOperationException(ex.Message);
            }
        }

        public async Task<Node<Substance>> UpdateSubstanceAsync(string substanceId, Substance data)
        {
            try
            {
                var substances = await _client.Cypher
                    .Match("(s:Substance)")
                    .Where((Substance m) => m.SubstanceId == substanceId)
                    .Set("s = {data}")
                    .WithParam("data", data)
                    .Return<Node<Substance>>("s")
                    .ResultsAsync;

                return substances.Single();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error executing graph query: {0}", ex.Message);
                throw new InvalidOperationException(ex.Message);
            }
        }

        public async Task<bool> DeleteSubstanceAsync(string substanceId)
        {
            try
            {
                await _client.Cypher
                    .Match("(s:Substance)")
                    .Where((Substance s) => s.SubstanceId == substanceId)
                    .Delete("s")
                    .ExecuteWithoutResultsAsync();

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error executing graph query: {0}", ex.Message);
                throw new InvalidOperationException(ex.Message);
            }
        }

        #endregion

        #region Contain

        public async Task<RelationshipInstance<Contain>> CreateContainAsync(string productId, string substanceId, Contain data = null)
        {
            try
            {
                var contains = await _client.Cypher
                    .Match("(s:Product)", "(t:Substance)")
                    .Where((Product s) => s.ProductId == productId)
                    .AndWhere((Substance t) => t.SubstanceId == substanceId)
                    .CreateUnique("(s)-[r:Contain {data}]->(t)")
                    .WithParam("data", data)
                    .Return<RelationshipInstance<Contain>>("r")
                    .ResultsAsync;

                return contains.Single();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error executing graph query: {0}", ex.Message);
                throw new InvalidOperationException(ex.Message);
            }
        }

        public async Task<RelationshipInstance<Contain>> GetContainAsync(string productId, string substanceId)
        {
            try
            {
                var contains = await _client.Cypher.Match("(s:Box)-[r:Contain]->(t:Message)")
                    .Where((Product s) => s.ProductId == productId)
                    .AndWhere((Substance t) => t.SubstanceId == substanceId)
                    .Return<RelationshipInstance<Contain>>("r")
                    .ResultsAsync;

                return contains.SingleOrDefault();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error executing graph query: {0}", ex.Message);
                throw new InvalidOperationException(ex.Message);
            }
        }

        public async Task<RelationshipInstance<Contain>> UpdateContainAsync(string productId, string substanceId, Contain data = null)
        {
            try
            {
                var contains = await _client.Cypher
                    .Match("(s:Product)-[r:Contain]->(t:Substance)")
                    .Where((Product s) => s.ProductId == productId)
                    .AndWhere((Substance t) => t.SubstanceId == substanceId)
                    .Set("r = {data}")
                    .WithParam("data", data)
                    .Return<RelationshipInstance<Contain>>("r")
                    .ResultsAsync;

                return contains.Single();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error executing graph query: {0}", ex.Message);
                throw new InvalidOperationException(ex.Message);
            }
        }

        public async Task<bool> DeleteContainAsync(string productId, string substanceId)
        {
            try
            {
                await _client.Cypher
                    .Match("(s:Product)-[r:Contain]->(t:Substance)")
                    .Where((Product s) => s.ProductId == productId)
                    .AndWhere((Substance t) => t.SubstanceId == substanceId)
                    .Delete("r")
                    .ExecuteWithoutResultsAsync();

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error executing graph query: {0}", ex.Message);
                throw new InvalidOperationException(ex.Message);
            }
        }

        public async Task<SubstanceList> GetIngredientsAsync(string productId)
        {
            try
            {
                var substances = await _client.Cypher.Match("(s:Product)-[r:Contain]->(t:Substance)")
                    .Where((Product s) => s.ProductId == productId)
                    .Return((r, t) => new SubstanceListItem()
                    {
                        Relationship = r.As<RelationshipInstance<Contraindicate>>(),
                        Node = t.As<Node<Substance>>()
                    })
                    .ResultsAsync;

                return new SubstanceList() { Substances = substances };
            }
            catch (Exception ex)
            {
                _logger.LogError("Error executing graph query: {0}", ex.Message);
                throw new InvalidOperationException(ex.Message);
            }
        }

        #endregion

        #region Contraindicate

        public async Task<RelationshipInstance<Contraindicate>> CreateContraindicateAsync(string sourceSubstanceId, string targetSubstanceId, Contraindicate data = null)
        {
            try
            {
                var contains = await _client.Cypher
                    .Match("(s:Substance)", "(t:Substance)")
                    .Where((Substance s) => s.SubstanceId == sourceSubstanceId)
                    .AndWhere((Substance t) => t.SubstanceId == targetSubstanceId)
                    .CreateUnique("(s)-[r:Contraindicate {data}]->(t)")
                    .WithParam("data", data)
                    .Return<RelationshipInstance<Contraindicate>>("r")
                    .ResultsAsync;

                return contains.Single();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error executing graph query: {0}", ex.Message);
                throw new InvalidOperationException(ex.Message);
            }
        }

        public async Task<RelationshipInstance<Contraindicate>> GetContraindicateAsync(string sourceSubstanceId, string targetSubstanceId)
        {
            try
            {
                var contains = await _client.Cypher.Match("(s:Substance)-[r:Contraindicate]->(t:Substance)")
                    .Where((Substance s) => s.SubstanceId == sourceSubstanceId)
                    .AndWhere((Substance t) => t.SubstanceId == targetSubstanceId)
                    .Return<RelationshipInstance<Contraindicate>>("r")
                    .ResultsAsync;

                return contains.SingleOrDefault();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error executing graph query: {0}", ex.Message);
                throw new InvalidOperationException(ex.Message);
            }
        }

        public async Task<RelationshipInstance<Contraindicate>> UpdateContraindicateAsync(string sourceSubstanceId, string targetSubstanceId, Contain data = null)
        {
            try
            {
                var contains = await _client.Cypher
                    .Match("(s:Substance)-[r:Contraindicate]->(t:Substance)")
                    .Where((Substance s) => s.SubstanceId == sourceSubstanceId)
                    .AndWhere((Substance t) => t.SubstanceId == targetSubstanceId)
                    .Set("r = {data}")
                    .WithParam("data", data)
                    .Return<RelationshipInstance<Contraindicate>>("r")
                    .ResultsAsync;

                return contains.Single();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error executing graph query: {0}", ex.Message);
                throw new InvalidOperationException(ex.Message);
            }
        }

        public async Task<bool> DeleteContraindicateAsync(string sourceSubstanceId, string targetSubstanceId)
        {
            try
            {
                await _client.Cypher
                    .Match("(s:Product)-[r:Contraindicate]->(t:Substance)")
                    .Where((Substance s) => s.SubstanceId == sourceSubstanceId)
                    .AndWhere((Substance t) => t.SubstanceId == targetSubstanceId)
                    .Delete("r")
                    .ExecuteWithoutResultsAsync();

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error executing graph query: {0}", ex.Message);
                throw new InvalidOperationException(ex.Message);
            }
        }

        public async Task<ProductList> GetProductContraindicationsAsync(string productId)
        {
            try
            {
                var products = await _client.Cypher.Match("(s:Product)-[:Contain]->(:Substance)-[:Contraindicate]->(:Substance)<-[r:Contain]-(t:Product)")
                    .Where((Product s) => s.ProductId == productId)
                    .Return((r, t) => new ProductListItem()
                    {
                        Relationship = r.As<RelationshipInstance<Contain>>(),
                        Node = t.As<Node<Product>>()
                    })
                    .ResultsAsync;

                return new ProductList() { Products = products };
            }
            catch (Exception ex)
            {
                _logger.LogError("Error executing graph query: {0}", ex.Message);
                throw new InvalidOperationException(ex.Message);
            }
        }

        public async Task<SubstanceList> GetSubstanceContraindicationsAsync(string substanceId)
        {
            try
            {
                var substances = await _client.Cypher.Match("(s:Substance)-[r:Contraindicate]->(t:Substance)")
                    .Where((Substance s) => s.SubstanceId == substanceId)
                    .Return((r, t) => new SubstanceListItem()
                    {
                        Relationship = r.As<RelationshipInstance<Contraindicate>>(),
                        Node = t.As<Node<Substance>>()
                    })
                    .ResultsAsync;

                return new SubstanceList() { Substances = substances };
            }
            catch (Exception ex)
            {
                _logger.LogError("Error executing graph query: {0}", ex.Message);
                throw new InvalidOperationException(ex.Message);
            }
        }

        #endregion
    }
}
