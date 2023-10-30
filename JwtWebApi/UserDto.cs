using System.ComponentModel.DataAnnotations;

namespace JwtWebApi
{
    public class UserDto
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

        [Required]
        [RegularExpression("^(ADMINISTRADOR|GENERAL)$")]
        public string TipoUsuario { get; set; } = string.Empty;
        public string Area { get; set; } = string.Empty;
        public string Nombres { get; set; } = string.Empty;
        public string Apellido_Paterno { get; set; } = string.Empty;
        public string Apellido_Materno { get; set; } = string.Empty;
        public int Edad { get; set; } = 0;

        public string Direccion { get; set; } = string.Empty;




    }
}
