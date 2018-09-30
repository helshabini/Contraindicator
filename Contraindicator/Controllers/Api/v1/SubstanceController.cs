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
    public class SubstanceController : Controller
    {
        private readonly IGraphClientRepository _repository;
        private readonly ILogger<SubstanceController> _logger;

        public SubstanceController(IGraphClientRepository repository, ILogger<SubstanceController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        // GET api/v1/subatsnce/9F2DA16F-DB7B-45D4-96EC-6A39E0D9CA0A
        [HttpGet("{substanceId}")]
        public async Task<IActionResult> Get(string substanceId)
        {
            try
            {
                var s = await _repository.GetSubstanceAsync(substanceId);
                return Ok(s);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error {0}", ex.Message);
            }

            return BadRequest("Something wrong...");
        }

        // POST api/v1/substance
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]Substance substance)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var s = await _repository.CreateSubstanceAsync(substance);
                    return Ok(s);
                }
                catch (Exception ex)
                {
                    _logger.LogError("Error {0}", ex.Message);
                }
            }

            return BadRequest("Something wrong...");
        }

        // PUT api/v1/substance/9F2DA16F-DB7B-45D4-96EC-6A39E0D9CA0A
        [HttpPut("{substanceId}")]
        public async Task<IActionResult> Put(string substanceId, [FromBody]Substance substance)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var s = await _repository.UpdateSubstanceAsync(substanceId, substance);
                    return Ok(s);
                }
                catch (Exception ex)
                {
                    _logger.LogError("Error {0}", ex.Message);
                }
            }

            return BadRequest("Something wrong...");
        }

        // DELETE api/v1/substance/9F2DA16F-DB7B-45D4-96EC-6A39E0D9CA0A
        [HttpDelete("{substanceId}")]
        public async Task<IActionResult> Delete(string substanceId)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (await _repository.DeleteSubstanceAsync(substanceId))
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
