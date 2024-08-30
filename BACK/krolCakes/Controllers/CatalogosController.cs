using krolCakes.Models;
using krolCakes.Provider;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Win32;
using Mysqlx.Crud;
using System.Data;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace krolCakes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CatalogosController : ControllerBase
    {

        private readonly DatabaseProvider db;
        private readonly Progra progra;

        public CatalogosController(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("ConnectionString");
            db = new DatabaseProvider(connectionString);
            progra = new Progra(configuration);
        }

        [HttpGet("usuarios")]
        public IActionResult getUsuarios()
        {
            try
            {

                var query = @"SELECT a.id, a.nombre, a.correo, a.contrasenia, a.visibilidad, a.id_rol, b.nombre AS rol
                            FROM usuario a
                            INNER JOIN rol b ON a.id_rol = b.id
                            ORDER BY a.id";
               var resultado = db.ExecuteQuery(query);
                // Construir el objeto usuario con los datos obtenidos de la base de datos
                
                var usuarios = resultado.AsEnumerable().Select(row => new usuarioModelCompleto
                {
                    usuario = new usuarioModel // Instancia del objeto usuarioModel
                    {
                        id = Convert.ToInt32(resultado.Rows[0]["id"]),
                        nombre = resultado.Rows[0]["nombre"].ToString(),
                        correo = resultado.Rows[0]["correo"].ToString(),
                        contrasenia = resultado.Rows[0]["contrasenia"].ToString(),
                        id_rol = Convert.ToInt32(resultado.Rows[0]["id_rol"]),
                        visibilidad = Convert.ToBoolean(resultado.Rows[0]["visibilidad"])
                    },
                    rol = new rolModel // Instancia del objeto rolModel (si es necesario)
                    {
                        nombre = resultado.Rows[0]["rol"].ToString()
                    }
                }).ToList();
                return Ok(usuarios);

            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        [HttpPost("nuevo-usuario")]
        public IActionResult nuevoUsuario([FromBody] usuarioModel usuario)
        {
            try
            {
                var queryValidador = $"SELECT id FROM usuario WHERE correo = '{usuario.correo}'";
                var resultadoValidador = db.ExecuteQuery(queryValidador);
                if (resultadoValidador.Rows.Count == 0) // si no coincide con nada, el usuario no existe y por eso en la ejecucion del query devuelve 0 filas
                {
                    var queryInsertar = $"INSERT INTO usuario (nombre,correo,contrasenia, visibilidad, id_rol) " +
                                    $"VALUES ( '{usuario.nombre}', '{usuario.correo}', '{usuario.contrasenia}', '{usuario.visibilidad}', '{usuario.id}')";
                    db.ExecuteQuery(queryInsertar);
                    return Ok("Usuario Registrado Correctamente");
                }
                else
                {
                    // El usuario ya existe, devolver un BadRequest
                    return BadRequest("El usuario ya está registrado");
                }


            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }








        // GET: api/<CatalogosController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<CatalogosController>/5
        [HttpGet("cambio estastado/{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<CatalogosController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<CatalogosController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<CatalogosController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
