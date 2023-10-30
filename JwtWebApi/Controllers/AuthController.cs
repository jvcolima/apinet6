using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace JwtWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IUserService _userService;

        public AuthController(IConfiguration configuration, IUserService userService) { 
            _configuration= configuration;
            _userService = userService;
        }

        //[HttpGet, Authorize]
        //public ActionResult<string> GetMe()
        //{

        //    // this is done by dependency injection
        //    //var userName = _userService.GetMyName();
        //    //return Ok(userName);
        //    return Ok();

        //    // this is normal
        //    /*
        //    var userName = User?.Identity?.Name;
        //    var userName2 = User.FindFirstValue(ClaimTypes.Name);
        //    var role = User?.FindFirstValue(ClaimTypes.Role);
        //    return Ok(new { userName , userName2, role});
        //    */
        //}


        [HttpGet]
        public ActionResult<IEnumerable<User>> GetUsers()
        {
            List<User> users = _userService.GetUsers();
            return users;
        }

        [HttpPost("register")]
        public async Task<ActionResult<User>> Regiter(UserDto request)
        {
            // Verificar si el usuario ya existe en la base de datos
            User existingUser = await _userService.GetUserByUsername(request.Username);
            if (existingUser != null)
            {
                return BadRequest("El nombre de usuario ya está en uso.");
            }

            // Crear un nuevo usuario con los datos proporcionados
            var newUser = new User
            {
                Username = request.Username,
                TipoUsuario = request.TipoUsuario,
                Area = request.Area,
                Nombres = request.Nombres,
                Apellido_Paterno = request.Apellido_Paterno,
                Apellido_Materno = request.Apellido_Materno,
                Edad = request.Edad,
                Direccion = request.Direccion
            };

            // Crear el hash de la contraseña
            CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);
            newUser.passwordHash = passwordHash;
            newUser.passwordSalt = passwordSalt;
            newUser.Activo = true;

            newUser.tokenCreated = DateTime.Now;
            newUser.tokenExpires = DateTime.Now.AddMinutes(60);

            // Agregar el nuevo usuario a la base de datos
            _userService.AddUser(newUser);

            return Ok("Usuario registrado exitosamente.");
        }








        //[HttpPost("login")]
        //public async Task<ActionResult<string>> Login(UserLoginDto requrest)
        //{
        //    if (user.Username != requrest.Username)
        //    {
        //        return BadRequest("user not found.");
        //    }
        //    if (!VerifyPasswordHash(requrest.Password, user.passwordHash, user.passwordSalt))
        //    {
        //        return BadRequest("Wrong password.");
        //    }
        //    string token = CreateToken(user);

        //    var refreshToken = GenerateRefreshToken();
        //    SetRefreshToken(refreshToken);
        //    return Ok(token);
        //}

        [HttpPost("login")]
        public async Task<ActionResult<string>> Login(UserLoginDto request)
        {
            // 1. Consultar la base de datos para encontrar al usuario por nombre de usuario
            var user = await _userService.GetUserByUsername(request.Username);

            if (user == null)
            {
                return BadRequest("Usuario no encontrado.");
            }

            //// 2. Validar la contraseña utilizando el hash y la sal almacenados en la base de datos
            //if (!VerifyPasswordHash(request.Password, user.passwordHash, user.passwordSalt))
            //{
            //    return BadRequest("Contraseña incorrecta.");
            //}

            //// 3. Usuario válido, generar un token
            //string token = CreateToken(user);

            //var refreshToken = GenerateRefreshToken();
            //SetRefreshToken(refreshToken);
            // por el momento solo regresaremos el usuario

            return Ok(user);
        }

        [HttpPost("refresh-token")]
        public async Task<ActionResult<string>> RefreshToken()
        {
            //var refreshToken = Request.Cookies["refreshToken"];

            //if (!user.refreshToken.Equals(refreshToken))
            //{
            //    return Unauthorized("Invalid Refresh Token.");
            //}
            //else if (user.tokenExpires < DateTime.Now)
            //{
            //    return Unauthorized("Token expired.");
            //}
            //string token = CreateToken(user);
            //var newRefreshToken = GenerateRefreshToken();
            //SetRefreshToken(newRefreshToken);
            //return Ok(token);
            return Ok();
        }



        private RefreshToken GenerateRefreshToken()
        {
            var refreshToken = new RefreshToken
            {
                Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                Expires = DateTime.Now.AddDays(7),
                Created = DateTime.Now
            };
            return refreshToken;
        }

        private void SetRefreshToken(RefreshToken newRefreshToken)
        {
            //var cookieOptions = new CookieOptions
            //{
            //    HttpOnly= true,
            //    Expires = newRefreshToken.Expires
            //};
            //Response.Cookies.Append("refreshToken", newRefreshToken.Token, cookieOptions);

            //user.refreshToken = newRefreshToken.Token;
            //user.tokenCreated = newRefreshToken.Created;
            //user.tokenExpires= newRefreshToken.Expires;
        }

        private string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, "Noob")
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
                _configuration.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using(var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using(var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }
    }
}
