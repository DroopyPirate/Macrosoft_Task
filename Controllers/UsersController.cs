using Macrosoft_Task.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Macrosoft_Task.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly AppDbContext context;
        private readonly IWebHostEnvironment webHostEnvironment;

        public UsersController(AppDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            this.context = context;
            this.webHostEnvironment = webHostEnvironment;
        }

        [HttpPost, Route("Add")]
        // POST: /api/Users/Add
        public async Task<IActionResult> CreateUser([FromForm] CreateUserViewModel model)
        {
            try
            {
                string imageName = await CopyImage(model.ImageFile);

                var user = new User
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Gender = model.Gender,
                    RoleId = model.RoleId,
                    ImageName = imageName
                };
                context.Users.Add(user);
                await context.SaveChangesAsync();

                return Ok("User has been created");

            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPut, Route("Update/{id}")]
        // POST: /api/Users/Update/3
        public async Task<IActionResult> UpdateUser([FromRoute] int id, [FromForm] CreateUserViewModel model)
        {
            var user = await context.Users.FindAsync(id);
            string imageName = user.ImageName;

            if (model.ImageFile != null)
            {
                DeleteImage(imageName);
                imageName = await CopyImage(model.ImageFile);
            }

            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.Gender = model.Gender;
            user.RoleId = model.RoleId;
            user.ImageName = imageName;
            context.Entry(user).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete, Route("Delete/{id}")]
        // POST: /api/Users/Delete/5
        public async Task<IActionResult> DeleteUser([FromRoute] int id)
        {
            var user = await context.Users.FindAsync(id);
            user.IsDeleted = true;
            context.Entry(user).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return Ok("User Deleted Successfully");
        }

        [HttpGet, Route("GetAll")]
        // POST: /api/Users/GetAll
        public IActionResult GetAllUsers()
        {
            var users = context.Users
                .Where(u => u.IsDeleted == false)
                .Include(u => u.Role)
                .Select(u => new { 
                    id = u.Id, 
                    firstName = u.FirstName,
                    lastName = u.LastName, 
                    gender = u.Gender, 
                    role = u.Role.RoleName,
                    imageSrc = String.Format("{0}://{1}{2}/Images/{3}", Request.Scheme, Request.Host, Request.PathBase, u.ImageName)
                }).OrderBy(u => u.role);
            return Ok(users);
        }

        private async Task<string> CopyImage(IFormFile imageFile)
        {
            string name = Path.GetFileNameWithoutExtension(imageFile.FileName);
            string extension = Path.GetExtension(imageFile.FileName);
            string imageName = name + DateTime.Now.ToString("yymmssfff") + extension;
            string path = webHostEnvironment.WebRootPath + "\\Images\\";
            using (FileStream fileStream = System.IO.File.Create(path + imageName))
            {
                await imageFile.CopyToAsync(fileStream);
                fileStream.Flush();
            }

            return imageName;
        }

        private void DeleteImage(string imageName)
        {
            string path = webHostEnvironment.WebRootPath + "\\Images\\" + imageName;
            if (System.IO.File.Exists(path))
                System.IO.File.Delete(path);
        }
    }
}
