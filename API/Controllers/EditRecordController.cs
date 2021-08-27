using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Database;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Http;
using System.Net;
using Microsoft.Extensions.Caching.Memory;
using System.Web;
using Newtonsoft.Json;
using System.Net.Http.Json;

namespace API.Controllers {
    
    [Route("api/editrecord")]
    [ApiController]
    public class EditRecordController : ControllerBase
    {
        private readonly DataContext _Context;
        private IMemoryCache _Cache;

        public EditRecordController(DataContext context)
        {
            _Context = context;
            _Cache = new MemoryCache( new MemoryCacheOptions()
            {
                SizeLimit = 1024
            });

            foreach (DNSEntry entry in _Context.Entries.ToList())
            {
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSize(1)
                    .SetAbsoluteExpiration(System.TimeSpan.FromSeconds(entry.TTL));
                     
                    _Cache.Set(entry.HostName+entry.Type, entry, cacheEntryOptions);
            }
        }

        //A Request to get an entry
        [HttpGet]
        public async Task<ActionResult<DNSEntry>> Get([FromBody] LookupKey lookupKey)
        {
            DNSEntry record;

            if(!_Cache.TryGetValue(lookupKey.ToString(), out record))
            {
                System.Console.WriteLine("Key was not in cache");
                
                try 
                {
                    record = await _Context.Entries
                        .SingleOrDefaultAsync(table => table.HostName == lookupKey.HostName && table.Type == lookupKey.Type);
                }
                catch
                {
                    //TODO: Will throw an exception if it finds more than one matching record.
                    // Need to handle that exception here.
                    record = null;
                }     

                if (record != null) 
                {
                    var cacheEntryOptions = new MemoryCacheEntryOptions()
                        .SetSize(1)
                        .SetAbsoluteExpiration(System.TimeSpan.FromSeconds(record.TTL));

                    _Cache.Set(lookupKey, record, cacheEntryOptions);
                }
            }

            if(record != null)
            {
                return Ok(record);
            }
            else
            {
                return new EmptyResult();
            }  
        }

        //A Request to add an entry
        [HttpPost]
        public async Task<HttpResponseMessage> Post([FromBody] DNSEntry entry)
        {
            HttpResponseMessage returnMessage = new HttpResponseMessage();
            
            try {

                _Context.Entries.Add(entry);
                _Context.SaveChanges();
                string message = ($"Entry Created - {entry.HostName}");
                returnMessage = new HttpResponseMessage(HttpStatusCode.Created);
                returnMessage.RequestMessage = new HttpRequestMessage(HttpMethod.Post, message);
            }
            //TODO: Look into other possible errors
            catch
            {
                returnMessage = new HttpResponseMessage(HttpStatusCode.ExpectationFailed);
                returnMessage.RequestMessage = new HttpRequestMessage(HttpMethod.Post, "Entry Already Exists");
            }

            return await Task.FromResult(returnMessage);
        }

        [HttpDelete]
        public async Task<HttpResponseMessage> Delete([FromBody] LookupKey lookupKey)
        {
            HttpResponseMessage returnMessage=new HttpResponseMessage();
            
            _Cache.Remove(lookupKey.ToString());

            DNSEntry record = await _Context.Entries
                        .SingleOrDefaultAsync(table => table.HostName == lookupKey.HostName && table.Type == lookupKey.Type);

            if (record == null)
            {
                returnMessage = new HttpResponseMessage(HttpStatusCode.ExpectationFailed);
                returnMessage.RequestMessage = new HttpRequestMessage(HttpMethod.Post, "Entry Does not Exist");
                return await Task.FromResult(returnMessage);
            }
            
            _Context.Entries.Remove(record);
             _Context.SaveChanges();
            string message = ($"{record.HostName} Removed.");
            returnMessage = new HttpResponseMessage(HttpStatusCode.OK);
            return await Task.FromResult(returnMessage);
        }
    } 

    public class LookupKey
    {   
        public string HostName  { get; set; }
        public string Type      { get; set; }

        public LookupKey(string HostName, string Type)
        {
            this.HostName = HostName;
            this.Type = Type;
        }

        public override string ToString()
        {
            return HostName+Type;
        }
    }
}