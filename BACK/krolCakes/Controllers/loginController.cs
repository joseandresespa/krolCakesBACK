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


        private static readonly byte[] Salt = new byte[] { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0A, 0x0B, 0x0C, 0x0D, 0x0E, 0x0F, 0x10 }; // Salt fijo

        public string EncriptarContraseña(string contraseña)
        {
            // Concatenar la contraseña con el salt fijo
            byte[] contraseniaConSalt = Encoding.UTF8.GetBytes(contraseña).Concat(Salt).ToArray();

            // Calcular el hash utilizando SHA-256
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


        [HttpGet("sesion")]
        public IActionResult sesion()
        {
            try
            {
                var query = @"SELECT * FROM rol";
                var resultado = db.ExecuteQuery(query);
                var roles = resultado.AsEnumerable().Select(row => new rolModel
                {
                    id = Convert.ToInt32(row["id"]),
                    nombre = row["nombre"]?.ToString(),
                }).ToList();
                return Ok(roles);
            }
            catch (Exception ex )
            {

                return BadRequest();
            }
        x`}

    }
}
