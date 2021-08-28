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

//TODO: Add logging
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
        [HttpGet("hostname={hostName}&type={type}")]
        public async Task<ActionResult<DNSEntry>> Get(string hostName, string type)
        {
            DNSEntry record;
            LookupKey lookupKey = new LookupKey(hostName, type);

            if(!_Cache.TryGetValue(lookupKey.ToString(), out record))
            {                
                try 
                {
                    //Record can get set to null here too
                    record = await _Context.Entries
                        .SingleOrDefaultAsync(table => table.HostName == lookupKey.HostName && table.Type == lookupKey.Type);
                }
                catch
                {
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
                return NotFound();
            }  
        }

        //A Request to add an entry
        //Disabling this warning as Ok() & StatusCode() require Async,
        //But the Compiler is still throwing a warning that
        //I don't technically need Async
        [HttpPost]
        #pragma warning disable 1998
        public async Task<ActionResult> Post([FromBody] DNSEntry entry)
        {      
            try 
            {
                _Context.Entries.Add(entry);
                _Context.SaveChanges();
                return Ok();
            }
            catch
            {
                //Conflict status code
                return StatusCode(409);
            }
        }
        #pragma warning restore 1998
       
        [HttpDelete]
        public async Task<ActionResult> Delete([FromBody] LookupKey lookupKey)
        { 
            _Cache.Remove(lookupKey.ToString());

            DNSEntry record = await _Context.Entries
                        .FirstOrDefaultAsync(table => table.HostName == lookupKey.HostName && table.Type == lookupKey.Type);

            if (record == null)
            {
                return NotFound();
            }
            try 
            {
                _Context.Entries.Remove(record);
                _Context.SaveChanges();
                return Ok();
            }
            catch
            {
                return NotFound();
            }
            
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