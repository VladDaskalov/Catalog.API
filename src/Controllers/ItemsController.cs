using System;
using System.Collections.Generic;
using System.Linq;
using Catalog.DTOs;
using Catalog.Entities;
using Catalog.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ItemsController : ControllerBase
    {
        private readonly IItemsRepository _repository;

        public ItemsController(IItemsRepository repository)
        {
            _repository = repository;
        }

        // GET /items
        [HttpGet]
        public IEnumerable<ItemDTO> GetItems()
        {
            var items = _repository.GetItems().Select(item => item.AsDTO());
            return items;
        }

        // GET /items/{id}
        [HttpGet]
        [Route("{id}")]
        public ActionResult<ItemDTO> GetItem(Guid id)
        {
            var item = _repository.GetItem(id);

            if (item is null)
            {
                return NotFound();
            }

            return item.AsDTO();
        }

        // POST /items
        [HttpPost]
        public ActionResult<ItemDTO> CreateItem(CreateItemDTO createItemDto)
        {
            Item item = new()
            {
                Id = Guid.NewGuid(),
                Name = createItemDto.Name,
                Price = createItemDto.Price,
                CreatedDate = DateTimeOffset.UtcNow
            };

            _repository.CreateItem(item);

            return CreatedAtAction(nameof(GetItem), new { id = item.Id }, item.AsDTO());
        }

        // PUT /items/{id}
        [HttpPut("{id}")]
        public ActionResult<ItemDTO> UpdateItem(Guid id, UpdateItemDTO updateItemDto)
        {
            var existingItem = _repository.GetItem(id);

            if (existingItem is null)
            {
                return NotFound();
            }

            Item updatedItem = new()
            {
                Id = id,
                Name = updateItemDto.Name,
                Price = updateItemDto.Price,
                CreatedDate = DateTimeOffset.UtcNow
            };

            _repository.UpdateItem(updatedItem);

            return AcceptedAtAction(nameof(GetItem), new { id = updatedItem.Id }, updatedItem.AsDTO());
        }

        // DELETE /items/{id}
        [HttpDelete]
        [Route("{id}")]
        public ActionResult DeleteItem(Guid id)
        {
            var existingItem = _repository.GetItem(id);

            if (existingItem is null)
            {
                return NotFound();
            }

            _repository.DeleteItem(id);

            return Ok();
        }
    }
    
}