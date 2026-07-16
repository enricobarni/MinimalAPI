using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetoAPI.Dominio.DTOs
{
    public class LoginDTO
    {
        public string Username { get; set; } = default!;
        public string Password { get; set; } = default!;
    }
}
