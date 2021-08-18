using System.Collections.Generic;
using System.Threading.Tasks;
using Database;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers {
    [Route("api/DNSEntry")]
    [ApiController]
    public class EntryController : ControllerBase
    {
        private readonly DataContext _Context;
        public EntryController(DataContext context)
        {
            _Context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DNSEntry>>> Get()
        {
            var values = await _Context.Entries.ToListAsync();
            return Ok(values);
        }
    }
}