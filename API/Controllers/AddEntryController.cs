using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net;
using Database;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Http;

namespace API.Controllers {
    
    [Route("api/adddnsentry")]
    [ApiController]
    public class AddEntryController : ControllerBase
    {
        private readonly DataContext _Context;

        public AddEntryController(DataContext context)
        {
            _Context = context;
        }

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
            catch (DbUpdateException e)
            {
                returnMessage = new HttpResponseMessage(HttpStatusCode.ExpectationFailed);
                returnMessage.RequestMessage = new HttpRequestMessage(HttpMethod.Post, "Entry Already Exists");
            }

            return await Task.FromResult(returnMessage);
        }
    }
}