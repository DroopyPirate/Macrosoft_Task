using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Macrosoft_Task.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Column(TypeName = "nvarchar(20)"), Required]
        public string FirstName { get; set; }

        [Column(TypeName = "nvarchar(20)"), Required]
        public string LastName { get; set; }

        [Column(TypeName = "nvarchar(50)"), Required]
        public string ImageName { get; set; }

        [Column(TypeName = "nvarchar(10)"), Required]
        public string Gender { get; set; }

        [Required]
        public int RoleId { get; set; }

        public bool IsDeleted { get; set; } = false;


        [ForeignKey("RoleId")]
        public Role Role { get; set; }
    }

    public class CreateUserViewModel
    {
        [Column(TypeName = "nvarchar(20)"), Required]
        public string FirstName { get; set; }

        [Column(TypeName = "nvarchar(20)"), Required]
        public string LastName { get; set; }

        [Column(TypeName = "nvarchar(10)"), Required]
        public string Gender { get; set; }

        [Required]
        public int RoleId { get; set; }

        public IFormFile ImageFile { get; set; }
    }

}
