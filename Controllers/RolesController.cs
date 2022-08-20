using Macrosoft_Task.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Macrosoft_Task.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly AppDbContext context;

        public RolesController(AppDbContext context)
        {
            this.context = context;
        }

        [HttpPost, Route("Add")]
        // POST: /api/Roles/Add
        public async Task<IActionResult> CreateRole(string roleName)
        {
            var role = new Role { RoleName = roleName };
            context.Roles.Add(role);
            try
            {
                await context.SaveChangesAsync();
                return Ok(new { message = roleName +" Role has been created"});
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
