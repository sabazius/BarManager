using AutoMapper;
using BarManager.BL.Interfaces;
using BarManager.Models.DTO;
using BarManager.Models.Requests;
using BarManager.Models.Responses;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BarManager.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ClientController : ControllerBase
    {
        private readonly IClientService _clientService;
        private readonly IMapper _mapper;

        public ClientController(IClientService clientService, IMapper mapper)
        {
            _clientService = clientService;
            _mapper = mapper;
        }

        [HttpGet("GetAll")]
        public async Task <IActionResult> GetAll()
        {
            var result = await _clientService.GetAll();

            var response = _mapper.Map<IEnumerable<ClientResponse>>(result);

            if(response != null) return Ok(response);

            return NoContent();
        }

        [HttpGet("GetById")]
        public async Task<IActionResult> GetById(int id)
        {
            if (id <= 0) return BadRequest();

            var result = await _clientService.GetById(id);

            if (result == null) return NotFound(id);

            var response = _mapper.Map<ClientResponse>(result);

            return Ok(response);
        }

        [HttpPost("Create")]
        public async Task<IActionResult> CreateClient([FromBody] ClientRequest clientRequest)
        {
            if (clientRequest == null) return BadRequest();

            var client = _mapper.Map<Client>(clientRequest);

            var result = await _clientService.Create(client);

            return Ok(client);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0) return BadRequest(id);

            await _clientService.Delete(id);
;

            return Ok();
        }

        [HttpPost("Update")]
        public async Task<IActionResult> Update([FromBody] ClientUpdateRequest clientRequest)
        {
            if (clientRequest == null) return BadRequest();

            var searchClient = await _clientService.GetById(clientRequest.Id);

            if (searchClient == null) return NotFound(clientRequest.Id);

            searchClient.Name = clientRequest.Name;

            var result = await _clientService.Update(searchClient);

            return Ok(result);
        }


    }
}
