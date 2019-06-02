using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using ViewModel;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NodeController : Controller
    {
        private readonly INodeService _service;
        public NodeController(INodeService service)
        {
            this._service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetNodes()
        {
            var nodes = await _service.GetNodesAsync();
            return Ok(nodes);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] VMAddNodes node)
        {
            try
            {
                var result = await _service.CreateAsync(node);
                return CreatedAtAction(nameof(GetNodes), new { IdNode = result.IdNode }, result);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> Edit(Guid id, [FromBody] VMEditNode node)
        {
            if (id.Equals(Guid.Empty))
            {
                return BadRequest();
            }

            try
            {
                var result = await _service.UpdateAsync(node, id);
                return CreatedAtAction(nameof(GetNodes), new { id = result.IdNode }, result);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (id.Equals(Guid.Empty))
            {
                return BadRequest();
            }

            try
            {
                var result = await _service.DeleteAsync(id);
                return Ok(result);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPost]
        [Route("Move/{id:guid}")]
        public async Task<IActionResult> Move(Guid id, [FromBody] VMMoveNodeData node)
        {
            if (id == node.DestinationId)
                return BadRequest();

            try
            {
                await _service.MoveNode(node, id);
                return Ok();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}