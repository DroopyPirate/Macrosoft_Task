using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Macrosoft_Task.Models
{
    public class Role
    {
        [Key]
        public int Id { get; set; }

        [Column(TypeName = "nvarchar(20)"), Required]
        public string RoleName { get; set; }


        public List<User> Users { get; set; }
    }
}
