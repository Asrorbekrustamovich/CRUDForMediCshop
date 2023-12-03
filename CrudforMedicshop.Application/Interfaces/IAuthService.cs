﻿using CrudforMedicshop.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrudforMedicshop.Application.Interfaces
{
    public interface IAuthService
    {
        Task<(int, string)> Registration(RegisteredModel model, string role);
        Task<(int, string)> Login(LoginModel model);
    }
}