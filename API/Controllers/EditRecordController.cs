using System.Collections.Generic;
using System.Threading.Tasks;
using Database;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Http;
using System.Net;
using Microsoft.Extensions.Caching.Memory;

namespace API.Controllers {
    
    [Route("api/editrecord")]
    [ApiController]
    public class EditRecordController : ControllerBase
    {
        private readonly DataContext _Context;
        public EditRecordController(DataContext context)
        {
            _Context = context;
        }

        //A Request to get all entries
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DNSEntry>>> Get()
        {
            var values = await _Context.Entries.ToListAsync();
            return Ok(values);
        }

        //A Request to add an entry
        [HttpPost]
        public async Task<HttpResponseMessage> Post([FromBody] DNSEntry entry)
        {
            //TODO: _Context.SaveChanges();
            System.Console.Write(entry.HostName);
            HttpResponseMessage returnMessage=new HttpResponseMessage();
            _Context.Entries.Add(entry);

            try {
                
                _Context.SaveChanges();
                string message = ($"Entry Created - {entry.HostName}");
                returnMessage = new HttpResponseMessage(HttpStatusCode.Created);
                returnMessage.RequestMessage = new HttpRequestMessage(HttpMethod.Post, message);
            }
            //TODO: Look into other possible errors
            catch (DbUpdateException)
            {
                returnMessage = new HttpResponseMessage(HttpStatusCode.ExpectationFailed);
                returnMessage.RequestMessage = new HttpRequestMessage(HttpMethod.Post, "Entry Already Exists");
            }

            return await Task.FromResult(returnMessage);
        }

        [HttpDelete]
        public async Task<HttpResponseMessage> Delete([FromBody] DNSEntry entry)
        {
            HttpResponseMessage returnMessage=new HttpResponseMessage();
            _Context.Entries.Remove(entry);

            try {
                 
                _Context.SaveChanges();
                string message = ($"Entry Removed - {entry.HostName}");
                returnMessage = new HttpResponseMessage(HttpStatusCode.OK);
            }
            catch (DbUpdateException)
            {
                returnMessage = new HttpResponseMessage(HttpStatusCode.ExpectationFailed);
                returnMessage.RequestMessage = new HttpRequestMessage(HttpMethod.Post, "Entry Already Exists");
            }

            return await Task.FromResult(returnMessage);
        }
    }

    public class Cache<TItem>
    {
        private MemoryCache _Cache; 
        private readonly DataContext _Context;

        public Cache(DataContext context)
        {
            _Cache = new MemoryCache( new MemoryCacheOptions()
            {
                SizeLimit = 1024
            });
            _Context = context;
        }


        //Attempts to get the item. If it does not exist in cache, it attempts
        //to get it from the database. If it doesn't get it there, it throws an
        //error
        public void Get(object key)
        {
            TItem CacheEntry;
            if(!_Cache.TryGetValue(key, out CacheEntry))
            {
                //Call Database to get the entry
                //Add the entry into the cache
                //Return the object
            }
        }

        public void Remove(object key)
        {

        }

        public void Add(TItem item) 
        {

        }
    }
}