using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using vsdotnet.data;
using vsdotnet.Entities;

namespace vsdotnet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    
    public class CustomerController : ControllerBase
    {
        private readonly IMongoCollection<Customer>? _customer;
        public CustomerController(MongoDbService mongoDbService)
        {
            _customer = mongoDbService.Database?.GetCollection<Customer>("customer");
        }
        [HttpGet]
        [Authorize]

        
        public async Task<IEnumerable<Customer>> Get()
        {
            return await _customer.Find(FilterDefinition<Customer>.Empty).ToListAsync();
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Customer?>> GetById(string id)
        {
            var filter = Builders<Customer>.Filter.Eq(x => x.Id, id);
            var customer = _customer.Find(filter).FirstOrDefault();
            return customer is not null ? Ok(customer) : NotFound();

        }



        [HttpPost]
        public async Task<ActionResult> Post(Customer customer)
        {
            await _customer.InsertOneAsync(customer);
            return CreatedAtAction(nameof(GetById), new { id = customer.Id }, customer);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Customer?>> Delete(string id)
        {
            var filter = Builders<Customer>.Filter.Eq(x => x.Id, id);
            var customer = _customer.DeleteOne(filter);
            return customer is not null ? Ok() : NotFound();

        }


    }
}
