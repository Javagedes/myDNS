using System.Collections.Generic;
using System.Linq;
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
        private Cache _Cache;
        public EditRecordController(DataContext context)
        {
            _Context = context;
            _Cache = new Cache(_Context);
        }

        //A Request to get all entries
        [HttpGet]
        public async Task<ActionResult<DNSEntry>> Get([FromBody] DNSEntryLookupItem item)
        {
            DNSEntry entry;
            entry = await _Cache.Get(_Context, item);
            
            if(entry == default)
            {
                System.Console.WriteLine("We are here!");
                return new EmptyResult();
            }
            
            return Ok(entry);
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

    public class Cache
    {
        private MemoryCache _Cache; 

        public Cache(DataContext context)
        {
            _Cache = new MemoryCache( new MemoryCacheOptions()
            {
                SizeLimit = 1024
            });

            foreach (DNSEntry entry in context.Entries.ToList())
            {
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSize(1)
                    .SetAbsoluteExpiration(System.TimeSpan.FromSeconds(entry.TTL));
                     
                     DNSEntryLookupItem key = new DNSEntryLookupItem(entry.HostName, entry.Type);
                    _Cache.Set(key, entry, cacheEntryOptions);
            }
        }


        /*
            This function queries the Entries database for a DNSEntry that has the correct Hostname and correct 
                entry value type (A, AAAA, etc);
            This function will return the matching DNSEntry, if found. Currently, if it is not found, or 
                multiple entries are found, it will return a null value. 
            When using this function, you must check for null!
        */
        public async Task<DNSEntry> Get(DataContext context, DNSEntryLookupItem item)
        {
            DNSEntry cacheEntry;

            if(!_Cache.TryGetValue(item, out cacheEntry))
            {   
                System.Console.WriteLine("Could not find the item in cache");
                try 
                {
                    cacheEntry = await context.Entries.SingleOrDefaultAsync(table => table.HostName == item.HostName && table.Type == item.Type);
                }
                catch
                {
                    //TODO: Will throw an exception if it finds more than one matching record.
                    // Need to handle that exception here.
                    cacheEntry = null;
                }
                
                if (cacheEntry != null) 
                {
                    var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSize(1)
                    .SetAbsoluteExpiration(System.TimeSpan.FromSeconds(cacheEntry.TTL));
                    _Cache.Set(cacheEntry.HostName, cacheEntry, cacheEntryOptions);
                }   
            }
            return cacheEntry;
        }

        public void Remove(string hostName)
        {

        }

        public void Add(DNSEntry item) 
        {

        }
    }

    public class DNSEntryLookupItem
    {   
        public string HostName  { get; set; }
        public string Type      { get; set; }

        public DNSEntryLookupItem(string HostName, string Type)
        {
            this.HostName = HostName;
            this.Type = Type;
        }
    }
}