using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace PolicyBasedAuthourization.Controllers
{
    [Authorize(Roles = "Administrator, User")]
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryController : ControllerBase
    {
        // GET: api/Inventory
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // POST: api/Inventory
        [Authorize(Policy = "EmployeeWithMoreYearsRequirement")]
        [HttpPost]
        public void Post([FromBody] Inventory inventory)
        {
        }
    }
}
