using System;

namespace RMDev_JwtExample.DomainClasses
{
    public class User
    {
        public int UserId { get; set; }

        public string UserName { get; set; }

        public string DisplayName { get; set; }

        public bool IsActive { get; set; }

        public DateTime? LastLoggedIn { get; set; }

        public string Password { get; set; }

        public string[] Roles { get; set; }

        /// <summary>
        /// Cada vez que o usuário altera sua senha, 
        /// ou um administrador altera suas regras, 
        /// crie um novo GUID 'SerialNumber' e armazene-o no banco de dados.
        /// </summary>
        public string SerialNumber { get; set; }
    }
}
