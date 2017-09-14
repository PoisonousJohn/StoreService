using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using StoreService.Models;
using StoreService.Utils;

namespace StoreService.Controllers
{
    public class StoreController : Controller
    {
        public StoreController(DocumentDBRepo<StoreCatalogEntry> repo)
        {
            _repo = repo;
        }

        [HttpGet("catalog")]
        public async Task<IEnumerable<StoreCatalogEntry>> GetCatalog()
        {
            return await _repo.GetItemsAsync();
        }

        [HttpGet("catalog/entry/{id}")]
        public async Task<StoreCatalogEntry> GetCatalogEntry([FromRoute]string id)
        {
            var item = await _repo.GetItemAsync(id);
            if (item == null)
            {
                return null;
            }

            return item;
        }

        [HttpDelete("entry/{id}")]
        public async Task<ActionResult> DeleteCatalogEntry([FromRoute]string id)
        {
            await _repo.DeleteItemAsync(id);
            return Ok();
        }

        [HttpPost("catalog/entry")]
        public async Task<StoreCatalogEntry> CreateCatalogEntry(StoreCatalogEntry entry)
        {
            if (!ModelState.IsValid)
            {
                throw new System.ArgumentException("Model is not valid");
            }
            var result = await _repo.CreateItemAsync(entry);
            return (StoreCatalogEntry)(dynamic)result;
        }
        private readonly DocumentDBRepo<StoreCatalogEntry> _repo;
    }
}