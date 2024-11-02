using krolCakes.Models;
using krolCakes.Provider;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Security.Cryptography;
using System.Text;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace krolCakes.Controllers
{

    public class Progra
    {
        private readonly DatabaseProvider db;

        public Progra(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("ConnectionString");
            db = new DatabaseProvider(connectionString);
        }


        private static readonly byte[] Salt = new byte[] { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0A, 0x0B, 0x0C, 0x0D, 0x0E, 0x0F, 0x10 }; 

        public string EncriptarContraseña(string contraseña)
        {
            byte[] contraseniaConSalt = Encoding.UTF8.GetBytes(contraseña).Concat(Salt).ToArray();

            using (var sha256 = SHA256.Create())
            {
                byte[] hashContraseña = sha256.ComputeHash(contraseniaConSalt);

                return Convert.ToBase64String(hashContraseña);
            }
        }

    }

    [Route("api/[controller]")]
    [ApiController]
    public class loginController : ControllerBase
    {
        private readonly DatabaseProvider db;
        private readonly Progra progra;

        public loginController(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("ConnectionString");
            db = new DatabaseProvider(connectionString);
            progra = new Progra(configuration);
        }


        [HttpPost("sesion")]
        public IActionResult sesion([FromBody] usuarioModel sesion)
        {
            try
            {

                var query = $"SELECT * FROM usuario WHERE correo = '{sesion.correo}' AND contrasenia = '{progra.EncriptarContraseña(sesion.contrasenia)}' AND visibilidad = FALSE";
                var resultado = db.ExecuteQuery(query);

                if (resultado.Rows.Count == 0)
                {
                    // Si no se encuentra ningún usuario con las credenciales proporcionadas, devolver un Unauthorized
                    return Unauthorized("Credenciales incorrectas o usuario inexistente");
                }

                // Construir el objeto usuario con los datos obtenidos de la base de datos
                var usuario = new usuarioModel
                {
                    id = Convert.ToInt32(resultado.Rows[0]["id"]),
                    nombre = resultado.Rows[0]["nombre"].ToString(),
                    correo = resultado.Rows[0]["correo"].ToString(),
                    id_rol = Convert.ToInt32(resultado.Rows[0]["id_rol"]),
                    visibilidad = Convert.ToBoolean(resultado.Rows[0]["visibilidad"])
                };

                // Devolver el usuario autenticado
                return Ok(usuario);
            }
            catch (Exception ex)
            {
                // En caso de error, devolver un BadRequest con el mensaje de error
                return BadRequest($"Error al autenticar al usuario: {ex.Message}");
            }
        }


    }
}
