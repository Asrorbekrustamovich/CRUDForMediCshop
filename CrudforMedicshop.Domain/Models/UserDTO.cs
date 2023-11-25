﻿using CrudforMedicshop.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrudforMedicshop.Domain.Models
{
    public class UserDTO
    {
        public string Name { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string phone { get; set; }
        public ICollection<int> RolesId { get; set; }
    }
}
