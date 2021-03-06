using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Catalog.API.DTOs;
using Catalog.API.Entities;
using Catalog.API.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Catalog.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ItemsController : ControllerBase
    {
        private readonly IItemsRepository _repository;
        private readonly ILogger<ItemsController> _logger;

        public ItemsController(IItemsRepository repository, ILogger<ItemsController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        // GET /items
        [HttpGet]
        public async Task<IEnumerable<ItemDTO>> GetItemsAsync(string nameToMatch = null)
        {
            var items = (await _repository.GetItemsAsync())
                        .Select(item => item.AsDTO());
            
            if (!string.IsNullOrWhiteSpace(nameToMatch))
            {
                items = items.Where(item => item.Name.Contains(nameToMatch, StringComparison.OrdinalIgnoreCase));
            }

            _logger.LogInformation($"{DateTime.UtcNow.ToString("hh:mm:ss")}: Retrieved {items.Count()} items");

            return items;
        }

        // GET /items/{id}
        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<ItemDTO>> GetItemAsync(Guid id)
        {
            var item = await _repository.GetItemAsync(id);

            if (item is null)
            {
                return NotFound();
            }

            return item.AsDTO();
        }

        // POST /items
        [HttpPost]
        public async Task<ActionResult<ItemDTO>> CreateItemAsync(CreateItemDTO createItemDto)
        {
            Item item = new()
            {
                Id = Guid.NewGuid(),
                Name = createItemDto.Name,
                Description = createItemDto.Description,
                Price = createItemDto.Price,
                CreatedDate = DateTimeOffset.UtcNow
            };

            await _repository.CreateItemAsync(item);

            return CreatedAtAction(nameof(GetItemAsync), new { id = item.Id }, item.AsDTO());
        }

        // PUT /items/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult<ItemDTO>> UpdateItemAsync(Guid id, UpdateItemDTO updateItemDto)
        {
            var existingItem = await _repository.GetItemAsync(id);

            if (existingItem is null)
            {
                return NotFound();
            }

            existingItem.Name = updateItemDto.Name;
            existingItem.Description = updateItemDto.Description;
            existingItem.Price = updateItemDto.Price;

            await _repository.UpdateItemAsync(existingItem);

            return AcceptedAtAction(nameof(GetItemAsync), new { id = existingItem.Id }, existingItem.AsDTO());
        }

        // DELETE /items/{id}
        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult> DeleteItemAsync(Guid id)
        {
            var existingItem = await _repository.GetItemAsync(id);

            if (existingItem is null)
            {
                return NotFound();
            }

            await _repository.DeleteItemAsync(id);

            return Ok();
        }
    }
    
}