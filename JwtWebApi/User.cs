using System.ComponentModel.DataAnnotations;

namespace JwtWebApi
{
    public class User
    {

        [Key]
        public int id { get; set; }
        public string Username { get; set; } = string.Empty;

        public string Rol { get; set; } = "GENERAL";
        public string? Password { get; set; }

        public string? Usuario { get; set; }


        [Required]
        [RegularExpression("^(ADMINISTRADOR|GENERAL)$")]
        public string TipoUsuario { get; set; }
        public string Area { get; set; }
        public bool Activo { get; set; }
        public string Nombres { get; set; }
        public string Apellido_Paterno { get; set; }
        public string Apellido_Materno { get; set; }
        public int Edad { get; set; }
        public string Direccion { get; set; }

        public byte[] passwordHash { get; set; }
        public byte[] passwordSalt { get; set;}

        public string refreshToken { get; set; } = string.Empty;

        public DateTime tokenCreated { get; set; }
        public DateTime tokenExpires { get; set; }
    }
}
