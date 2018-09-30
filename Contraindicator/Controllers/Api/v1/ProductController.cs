using Contraindicator.Data;
using Contraindicator.Models.Nodes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Contraindicator.Controllers.Api.v1
{
    [Produces("application/json")]
    [Route("api/v1/[controller]")]
    public class ProductController : Controller
    {
        private readonly IGraphClientRepository _repository;
        private readonly ILogger<ProductController> _logger;

        public ProductController(IGraphClientRepository repository, ILogger<ProductController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        // GET api/v1/product/9F2DA16F-DB7B-45D4-96EC-6A39E0D9CA0A
        [HttpGet("{productId}")]
        public async Task<IActionResult> Get(string productId)
        {
            try
            {
                var p = await _repository.GetProductAsync(productId);
                return Ok(p);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error {0}", ex.Message);
            }

            return BadRequest("Something wrong...");
        }

        // POST api/v1/product
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]Product product)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var p = await _repository.CreateProductAsync(product);
                    return Ok(p);
                }
                catch (Exception ex)
                {
                    _logger.LogError("Error {0}", ex.Message);
                }
            }

            return BadRequest("Something wrong...");
        }

        // PUT api/v1/product/9F2DA16F-DB7B-45D4-96EC-6A39E0D9CA0A
        [HttpPut("{productId}")]
        public async Task<IActionResult> Put(string productId, [FromBody]Product product)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var p = await _repository.UpdateProductAsync(productId, product);
                    return Ok(p);
                }
                catch (Exception ex)
                {
                    _logger.LogError("Error {0}", ex.Message);
                }
            }

            return BadRequest("Something wrong...");
        }

        // DELETE api/v1/product/9F2DA16F-DB7B-45D4-96EC-6A39E0D9CA0A
        [HttpDelete("{productId}")]
        public async Task<IActionResult> Delete(string productId)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (await _repository.DeleteProductAsync(productId))
                        return Ok("Product deleted.");
                }
                catch (Exception ex)
                {
                    _logger.LogError("Error {0}", ex.Message);
                }
            }

            return BadRequest("Something wrong...");
        }
    }
}
