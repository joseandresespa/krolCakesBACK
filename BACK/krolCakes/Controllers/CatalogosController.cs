using krolCakes.Models;
using krolCakes.Provider;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Win32;
using MySql.Data.MySqlClient;
using Mysqlx.Crud;
using MySqlX.XDevAPI.Relational;
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
                      WHERE a.visibilidad != 1
                      ORDER BY a.id, a.visibilidad";

                var resultado = db.ExecuteQuery(query);

                // Construir la lista de objetos usuarioModelCompleto con los datos obtenidos de la base de datos
                var usuarios = resultado.AsEnumerable().Select(row => new usuarioModelCompleto
                {
                    id = Convert.ToInt32(row["id"]),
                    nombre = row["nombre"].ToString(),
                    correo = row["correo"].ToString(),
                    contrasenia = row["contrasenia"].ToString(),
                    id_rol = Convert.ToInt32(row["id_rol"]),
                    visibilidad = Convert.ToBoolean(row["visibilidad"]),
                    rol = row["rol"].ToString()
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
                                    $"VALUES ( '{usuario.nombre}', '{usuario.correo}', '{progra.EncriptarContraseña(usuario.contrasenia)}', '0', '{usuario.id_rol}')";
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
                return BadRequest("Error al registrar al Usuario");
            }
        }

        [HttpPost("actualizar-usuario")]
        public IActionResult ActualizarUsuario([FromBody] usuarioModel usuario)
        {
            try
            {
                // Validar si cambio de contrasenia
                    var queryValidador = $"SELECT contrasenia FROM usuario WHERE id = '{usuario.id}'";
                var resultado = db.ExecuteQuery(queryValidador);

                // Asegúrate de que el DataTable no esté vacío
                if (resultado.Rows.Count > 0)
                {
                    // Extrae el valor de la primera fila y la columna 'contrasenia'
                    var resultadoValidador = resultado.Rows[0]["contrasenia"].ToString();

                    if (resultadoValidador == usuario.contrasenia)
                    {
                        // Actualizar sin contrasenia
                        var queryActualizar = $"UPDATE usuario SET " +
                          $"nombre = '{usuario.nombre}', " +
                          $"id_rol = {usuario.id_rol}, " +
                          $"visibilidad = {(usuario.visibilidad.HasValue && usuario.visibilidad.Value ? 1 : 0)} " +
                          $"WHERE correo = '{usuario.correo}'";

                        db.ExecuteQuery(queryActualizar);
                        return Ok("Usuario actualizado correctamente");
                    }
                    else
                    {
                        // Actualizar con nueva contrasenia
                        var queryActualizar = $"UPDATE usuario SET " +
                          $"nombre = '{usuario.nombre}', " +
                          $"contrasenia = '{progra.EncriptarContraseña(usuario.contrasenia)}', " +
                          $"id_rol = {usuario.id_rol}, " +
                          $"visibilidad = {(usuario.visibilidad.HasValue && usuario.visibilidad.Value ? 1 : 0)} " +
                          $"WHERE correo = '{usuario.correo}'";

                        db.ExecuteQuery(queryActualizar);
                        return Ok("Usuario actualizado correctamente");
                    }
                }
                else
                {
                    return BadRequest("Usuario no encontrado");
                }


            }
            catch (Exception ex)
            {
                // Capturar la excepción y devolver un BadRequest con el mensaje de error
                return BadRequest("Error al actualizar el usuario");
            }
        }


        //-----------------------------Fin Usuarios--------------------------------------------------------

        [HttpGet("modulos")]
        public IActionResult GetModulos([FromQuery] int? idRol)
        {
            try
            {
                // Construir la consulta SQL con un filtro opcional por idRol
                var query = idRol.HasValue
                    ? $"SELECT id, nombre, url, icono FROM modulo WHERE id_rol = {idRol.Value} ORDER BY id"
                    : "SELECT id, nombre, url, icono FROM modulo ORDER BY id";

                var resultado = db.ExecuteQuery(query);

                // Construir la lista de objetos moduloModel con los datos obtenidos de la base de datos
                var modulos = resultado.AsEnumerable().Select(row => new moduloModel
                {
                    id = row.Field<int?>("id"),
                    nombre = row.Field<string>("nombre"),
                    url = row.Field<string>("url"),
                    icono = row.Field<string>("icono")
                }).ToList();

                return Ok(modulos);
            }
            catch (Exception ex)
            {
                // Manejo de excepciones: retornar un BadRequest en caso de error
                return BadRequest($"Error al obtener los módulos: {ex.Message}");
            }
        }




        [HttpPost("nuevo-modulo")]
        public IActionResult nuevoModulo([FromBody] moduloModel modulo)
        {
            try
            {
                var queryValidador = $"SELECT id FROM modulo WHERE nombre = '{modulo.nombre}'";
                var resultadoValidador = db.ExecuteQuery(queryValidador);
                if (resultadoValidador.Rows.Count == 0) // si no coincide con nada, el usuario no existe y por eso en la ejecucion del query devuelve 0 filas
                {
                    var queryInsertar = $"INSERT INTO modulo (nombre,url,icono) " +
                                    $"VALUES ( '{modulo.nombre}', '{modulo.url}', '{modulo.icono}')";
                    db.ExecuteQuery(queryInsertar);
                    return Ok("Modulo Registrado Correctamente");
                }
                else
                {
                    // El usuario ya existe, devolver un BadRequest
                    return BadRequest("El Modulo ya está registrado");
                }


            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        [HttpPost("actualizar-modulo")]
        public IActionResult ActualizarModulo([FromBody] moduloModel modulo)
        {
            try
            {
                // Validar que el módulo existe
                var queryValidador = $"SELECT id FROM modulo WHERE id = {modulo.id}";
                var resultadoValidador = db.ExecuteQuery(queryValidador);

                if (resultadoValidador.Rows.Count > 0) // El módulo existe
                {
                    // Actualizar los campos necesarios
                    var queryActualizar = $"UPDATE modulo SET " +
                                          $"nombre = '{modulo.nombre}', " +
                                          $"url = '{modulo.url}', " +
                                          $"icono = '{modulo.icono}' " +  
                                          $"WHERE id = {modulo.id}";      

                    db.ExecuteQuery(queryActualizar);
                    return Ok("Módulo actualizado correctamente");
                }
                else
                {
                    // El módulo no existe, devolver un BadRequest
                    return BadRequest("El módulo no existe");
                }
            }
            catch (Exception ex)
            {
                // Capturar la excepción y devolver un BadRequest con el mensaje de error
                return BadRequest($"Error al actualizar el módulo: {ex.Message}");
            }
        }
        //-----------------------Fin Modulo-------------------------------------------------------------------------------------

        [HttpGet("roles")]
        public IActionResult GetRoles()
        {
            try
            {
                // Consulta SQL para obtener todos los roles
                var query = "SELECT id, nombre FROM rol ORDER BY id";
                var resultado = db.ExecuteQuery(query);

                // Construir la lista de objetos rolModel con los datos obtenidos de la base de datos
                var roles = resultado.AsEnumerable().Select(row => new rolModel
                {
                    id = row.Field<int?>("id"),
                    nombre = row.Field<string>("nombre")
                }).ToList();

                return Ok(roles);
            }
            catch (Exception ex)
            {
                // Manejo de excepciones: retorna un BadRequest en caso de error
                return BadRequest($"Error al obtener los roles: {ex.Message}");
            }
        }

        [HttpPost("nuevo-rol")]
        public IActionResult NuevoRol([FromBody] rolModel rol)
        {
            try
            {
                var queryValidador = $"SELECT id FROM rol WHERE nombre = '{rol.nombre}'";
                var resultadoValidador = db.ExecuteQuery(queryValidador);

                if (resultadoValidador.Rows.Count == 0) // Si no existe un rol con el mismo nombre
                {
                    var queryInsertar = $"INSERT INTO rol (nombre) VALUES ('{rol.nombre}')";
                    db.ExecuteQuery(queryInsertar);
                    return Ok("Rol registrado correctamente");
                }
                else
                {
                    // El rol ya existe, devolver un BadRequest
                    return BadRequest("El rol ya está registrado");
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al registrar el rol: {ex.Message}");
            }
        }

        [HttpPost("actualizar-rol")]
        public IActionResult ActualizarRol([FromBody] rolModel rol)
        {
            try
            {
                // Validar que el rol existe
                var queryValidador = $"SELECT id FROM rol WHERE id = {rol.id}";
                var resultadoValidador = db.ExecuteQuery(queryValidador);

                if (resultadoValidador.Rows.Count > 0) // Si el rol existe
                {
                    // Actualizar el nombre del rol
                    var queryActualizar = $"UPDATE rol SET nombre = '{rol.nombre}' WHERE id = {rol.id}";
                    db.ExecuteQuery(queryActualizar);
                    return Ok("Rol actualizado correctamente");
                }
                else
                {
                    // El rol no existe, devolver un BadRequest
                    return BadRequest("El rol no existe");
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al actualizar el rol: {ex.Message}");
            }
        }
        //--------------------Fin Rol-------------------------------------------------------------------------------

        [HttpGet("masas")]
        public IActionResult GetMasas()
        {
            try
            {
                // Consulta SQL para obtener todos los tipos de masas
                var query = "SELECT id, sabor_masa FROM masas ORDER BY id";
                var resultado = db.ExecuteQuery(query);

                // Construir la lista de objetos masaModel con los datos obtenidos de la base de datos
                var masas = resultado.AsEnumerable().Select(row => new masaModel
                {
                    id = row.Field<int?>("id"),
                    sabor_masa = row.Field<string>("sabor_masa")
                }).ToList();

                return Ok(masas);
            }
            catch (Exception ex)
            {
                // Manejo de excepciones: retorna un BadRequest en caso de error
                return BadRequest($"Error al obtener las masas: {ex.Message}");
            }
        }

        [HttpPost("nueva-masa")]
        public IActionResult NuevaMasa([FromBody] masaModel masa)
        {
            try
            {
                var queryValidador = $"SELECT id FROM masas WHERE sabor_masa = '{masa.sabor_masa}'";
                var resultadoValidador = db.ExecuteQuery(queryValidador);

                if (resultadoValidador.Rows.Count == 0) // Si no existe una masa con el mismo sabor
                {
                    var queryInsertar = $"INSERT INTO masas (sabor_masa) VALUES ('{masa.sabor_masa}')";
                    db.ExecuteQuery(queryInsertar);
                    return Ok("Masa registrada correctamente");
                }
                else
                {
                    // La masa ya existe, devolver un BadRequest
                    return BadRequest("El sabor de masa ya está registrado");
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al registrar la masa: {ex.Message}");
            }
        }

        [HttpPost("actualizar-masa")]
        public IActionResult ActualizarMasa([FromBody] masaModel masa)
        {
            try
            {
                // Validar que la masa existe
                var queryValidador = $"SELECT id FROM masas WHERE id = {masa.id}";
                var resultadoValidador = db.ExecuteQuery(queryValidador);

                if (resultadoValidador.Rows.Count > 0) // Si la masa existe
                {
                    // Actualizar el sabor de la masa
                    var queryActualizar = $"UPDATE masas SET sabor_masa = '{masa.sabor_masa}' WHERE id = {masa.id}";
                    db.ExecuteQuery(queryActualizar);
                    return Ok("Masa actualizada correctamente");
                }
                else
                {
                    // La masa no existe, devolver un BadRequest
                    return BadRequest("La masa no existe");
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al actualizar la masa: {ex.Message}");
            }
        }
        //-------------------------------Fin Masa---------------------------------------------------------------------------

        [HttpGet("rellenos")]
        public IActionResult GetRellenos()
        {
            try
            {
                // Consulta SQL para obtener todos los tipos de rellenos
                var query = "SELECT id, sabor_relleno FROM relleno ORDER BY id";
                var resultado = db.ExecuteQuery(query);

                // Construir la lista de objetos rellenoModel con los datos obtenidos de la base de datos
                var rellenos = resultado.AsEnumerable().Select(row => new rellenoModel
                {
                    id = row.Field<int?>("id"),
                    sabor_relleno = row.Field<string>("sabor_relleno")
                }).ToList();

                return Ok(rellenos);
            }
            catch (Exception ex)
            {
                // Manejo de excepciones: retorna un BadRequest en caso de error
                return BadRequest($"Error al obtener los rellenos: {ex.Message}");
            }
        }

        [HttpPost("nuevo-relleno")]
        public IActionResult NuevoRelleno([FromBody] rellenoModel relleno)
        {
            try
            {
                var queryValidador = $"SELECT id FROM relleno WHERE sabor_relleno = '{relleno.sabor_relleno}'";
                var resultadoValidador = db.ExecuteQuery(queryValidador);

                if (resultadoValidador.Rows.Count == 0) // Si no existe un relleno con el mismo sabor
                {
                    var queryInsertar = $"INSERT INTO relleno (sabor_relleno) VALUES ('{relleno.sabor_relleno}')";
                    db.ExecuteQuery(queryInsertar);
                    return Ok("Relleno registrado correctamente");
                }
                else
                {
                    // El relleno ya existe, devolver un BadRequest
                    return BadRequest("El sabor de relleno ya está registrado");
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al registrar el relleno: {ex.Message}");
            }
        }

        [HttpPost("actualizar-relleno")]
        public IActionResult ActualizarRelleno([FromBody] rellenoModel relleno)
        {
            try
            {
                // Validar que el relleno existe
                var queryValidador = $"SELECT id FROM relleno WHERE id = {relleno.id}";
                var resultadoValidador = db.ExecuteQuery(queryValidador);

                if (resultadoValidador.Rows.Count > 0) // Si el relleno existe
                {
                    // Actualizar el sabor del relleno
                    var queryActualizar = $"UPDATE relleno SET sabor_relleno = '{relleno.sabor_relleno}' WHERE id = {relleno.id}";
                    db.ExecuteQuery(queryActualizar);
                    return Ok("Relleno actualizado correctamente");
                }
                else
                {
                    // El relleno no existe, devolver un BadRequest
                    return BadRequest("El relleno no existe");
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al actualizar el relleno: {ex.Message}");
            }
        }
        //-----------------------------------Fin Relleno-----------------------------------------------------------------------------------

        [HttpGet("recetas")]
        public IActionResult GetRecetas()
        {
            try
            {
                // Consulta SQL para obtener todas las recetas
                var query = "SELECT id, nombre, descripcion FROM receta ORDER BY id";
                var resultado = db.ExecuteQuery(query);

                // Construir la lista de objetos recetaModel con los datos obtenidos de la base de datos
                var recetas = resultado.AsEnumerable().Select(row => new recetaModel
                {
                    id = row.Field<int?>("id"),
                    nombre = row.Field<string>("nombre"),
                    descripcion = row.Field<string>("descripcion")
                }).ToList();

                return Ok(recetas);
            }
            catch (Exception ex)
            {
                // Manejo de excepciones: retorna un BadRequest en caso de error
                return BadRequest($"Error al obtener las recetas: {ex.Message}");
            }
        }

        [HttpPost("nueva-receta")]
        public IActionResult NuevaReceta([FromBody] recetaModel receta)
        {
            try
            {
                var queryValidador = $"SELECT id FROM receta WHERE nombre = '{receta.nombre}'";
                var resultadoValidador = db.ExecuteQuery(queryValidador);

                if (resultadoValidador.Rows.Count == 0) // Si no existe una receta con el mismo nombre
                {
                    var queryInsertar = $"INSERT INTO receta (nombre, descripcion) VALUES ('{receta.nombre}', '{receta.descripcion}')";
                    db.ExecuteQuery(queryInsertar);
                    return Ok("Receta registrada correctamente");
                }
                else
                {
                    // La receta ya existe, devolver un BadRequest
                    return BadRequest("El nombre de la receta ya está registrado");
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al registrar la receta: {ex.Message}");
            }
        }

        [HttpPost("actualizar-receta")]
        public IActionResult ActualizarReceta([FromBody] recetaModel receta)
        {
            try
            {
                // Validar que la receta existe
                var queryValidador = $"SELECT id FROM receta WHERE id = {receta.id}";
                var resultadoValidador = db.ExecuteQuery(queryValidador);

                if (resultadoValidador.Rows.Count > 0) // Si la receta existe
                {
                    // Actualizar el nombre y la descripción de la receta
                    var queryActualizar = $"UPDATE receta SET nombre = '{receta.nombre}', descripcion = '{receta.descripcion}' WHERE id = {receta.id}";
                    db.ExecuteQuery(queryActualizar);
                    return Ok("Receta actualizada correctamente");
                }
                else
                {
                    // La receta no existe, devolver un BadRequest
                    return BadRequest("La receta no existe");
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al actualizar la receta: {ex.Message}");
            }
        }
        //--------------------------------Fin Receta-----------------------------------------------------------------------------------------------------------

        [HttpGet("productos")]
        public IActionResult GetProductos()
            {
            try
            {
                // Consulta SQL para obtener todos los productos
                var query = "SELECT id, nombre, descripcion, precio_online FROM producto ORDER BY id";
                var resultado = db.ExecuteQuery(query);

                // Construir la lista de objetos productoModel con los datos obtenidos de la base de datos
                var productos = resultado.AsEnumerable().Select(row => new productoModel
                {
                    id = row.Field<int?>("id"),
                    nombre = row.Field<string>("nombre"),
                    descripcion = row.Field<string>("descripcion"),
                    precio_online = row.Field<double?>("precio_online")
                }).ToList();

                return Ok(productos);
            }
            catch (Exception ex)
            {
                // Manejo de excepciones: retorna un BadRequest en caso de error
                return BadRequest($"Error al obtener los productos: {ex.Message}");
            }
        }

        [HttpPost("nuevo-producto")]
        public IActionResult NuevoProducto([FromBody] productoModel producto)
        {
            try
            {
                var queryValidador = $"SELECT id FROM producto WHERE nombre = '{producto.nombre}'";
                var resultadoValidador = db.ExecuteQuery(queryValidador);

                if (resultadoValidador.Rows.Count == 0) // Si no existe un producto con el mismo nombre
                {
                    var queryInsertar = $"INSERT INTO producto (nombre, descripcion, precio_online) VALUES ('{producto.nombre}', '{producto.descripcion}', {producto.precio_online})";
                    db.ExecuteQuery(queryInsertar);
                    return Ok("Producto registrado correctamente");
                }
                else
                {
                    // El producto ya existe, devolver un BadRequest
                    return BadRequest("El nombre del producto ya está registrado");
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al registrar el producto: {ex.Message}");
            }
        }

        [HttpPost("actualizar-producto")]
        public IActionResult ActualizarProducto([FromBody] productoModel producto)
        {
            try
            {
                // Validar que el producto existe
                var queryValidador = $"SELECT id FROM producto WHERE id = {producto.id}";
                var resultadoValidador = db.ExecuteQuery(queryValidador);

                if (resultadoValidador.Rows.Count > 0) // Si el producto existe
                {
                    // Actualizar los campos nombre, descripcion y precio_online del producto
                    var queryActualizar = $"UPDATE producto SET " +
                      $"nombre = '{producto.nombre}', " +
                      $"descripcion = '{producto.descripcion}', " +
                      $"precio_online = {producto.precio_online} " +
                      $"WHERE id = {producto.id}";
                    db.ExecuteQuery(queryActualizar);
                    return Ok("Producto actualizado correctamente");
                }
                else
                {
                    // El producto no existe, devolver un BadRequest
                    return BadRequest("El producto no existe");
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al actualizar el producto: {ex.Message}");
            }
        }
        //----------------------------Fin Producto---------------------------------------------

        [HttpGet("clientes")]
        public IActionResult GetClientes()
        {
            try
            {
                // Consulta SQL para obtener todos los clientes
                var query = "SELECT id, nombre, telefono, nit FROM cliente ORDER BY id";
                var resultado = db.ExecuteQuery(query);

                // Construir la lista de objetos clienteModel con los datos obtenidos de la base de datos
                var clientes = resultado.AsEnumerable().Select(row => new clienteModel
                {
                    id = row.Field<int?>("id"),
                    nombre = row.Field<string>("nombre"),
                    telefono = row.Field<int?>("telefono"),
                    nit = row.Field<string>("nit")
                }).ToList();

                return Ok(clientes);
            }
            catch (Exception ex)
            {
                // Manejo de excepciones: retorna un BadRequest en caso de error
                return BadRequest($"Error al obtener los clientes: {ex.Message}");
            }
        }

        [HttpPost("nuevo-cliente")]
        public IActionResult NuevoCliente([FromBody] clienteModel cliente)
        {
            try
            {
                var queryValidador = $"SELECT id FROM cliente WHERE nit = '{cliente.nit}'";
                var resultadoValidador = db.ExecuteQuery(queryValidador);

                if (resultadoValidador.Rows.Count == 0) // Si no existe un cliente con el mismo NIT
                {
                    var queryInsertar = $"INSERT INTO cliente (nombre, telefono, nit) VALUES ('{cliente.nombre}', {cliente.telefono}, '{cliente.nit}')";
                    db.ExecuteQuery(queryInsertar);
                    return Ok("Cliente registrado correctamente");
                }
                else
                {
                    // El cliente ya existe, devolver un BadRequest
                    return BadRequest("El NIT del cliente ya está registrado");
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al registrar el cliente: {ex.Message}");
            }
        }

        [HttpPost("actualizar-cliente")]
        public IActionResult ActualizarCliente([FromBody] clienteModel cliente)
        {
            try
            {
                // Validar que el cliente existe
                var queryValidador = $"SELECT id FROM cliente WHERE id = {cliente.id}";
                var resultadoValidador = db.ExecuteQuery(queryValidador);

                if (resultadoValidador.Rows.Count > 0) // Si el cliente existe
                {
                    // Actualizar los campos nombre, telefono y nit del cliente
                    var queryActualizar = $"UPDATE cliente SET " +
                      $"nombre = '{cliente.nombre}', " +
                      $"telefono = {cliente.telefono}, " +
                      $"nit = '{cliente.nit}' " +
                      $"WHERE id = {cliente.id}";
                    db.ExecuteQuery(queryActualizar);
                    return Ok("Cliente actualizado correctamente");
                }
                else
                {
                    // El cliente no existe, devolver un BadRequest
                    return BadRequest("El cliente no existe");
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al actualizar el cliente: {ex.Message}");
            }
        }
        //------------------------------Fin cliente----------------------------------------------

        [HttpGet("tipos-evento")]
        public IActionResult GetTiposEvento()
        {
            try
            {
                // Consulta SQL para obtener todos los tipos de evento
                var query = "SELECT id, nombre FROM tipo_evento ORDER BY id";
                var resultado = db.ExecuteQuery(query);

                // Construir la lista de objetos tipoeventoModel con los datos obtenidos de la base de datos
                var tiposEvento = resultado.AsEnumerable().Select(row => new tipoeventoModel
                {
                    id = row.Field<int?>("id"),
                    nombre = row.Field<string>("nombre")
                }).ToList();

                return Ok(tiposEvento);
            }
            catch (Exception ex)
            {
                // Manejo de excepciones: retorna un BadRequest en caso de error
                return BadRequest($"Error al obtener los tipos de evento: {ex.Message}");
            }
        }

        [HttpPost("nuevo-tipo-evento")]
        public IActionResult NuevoTipoEvento([FromBody] tipoeventoModel tipoEvento)
        {
            try
            {
                var queryValidador = $"SELECT id FROM tipo_evento WHERE nombre = '{tipoEvento.nombre}'";
                var resultadoValidador = db.ExecuteQuery(queryValidador);

                if (resultadoValidador.Rows.Count == 0) // Si no existe un tipo de evento con el mismo nombre
                {
                    var queryInsertar = $"INSERT INTO tipo_evento (nombre) VALUES ('{tipoEvento.nombre}')";
                    db.ExecuteQuery(queryInsertar);
                    return Ok("Tipo de evento registrado correctamente");
                }
                else
                {
                    // El tipo de evento ya existe, devolver un BadRequest
                    return BadRequest("El tipo de evento ya está registrado");
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al registrar el tipo de evento: {ex.Message}");
            }
        }

        [HttpPost("actualizar-tipo-evento")]
        public IActionResult ActualizarTipoEvento([FromBody] tipoeventoModel tipoEvento)
        {
            try
            {
                // Validar que el tipo de evento existe
                var queryValidador = $"SELECT id FROM tipo_evento WHERE id = {tipoEvento.id}";
                var resultadoValidador = db.ExecuteQuery(queryValidador);

                if (resultadoValidador.Rows.Count > 0) // Si el tipo de evento existe
                {
                    // Actualizar el campo nombre del tipo de evento
                    var queryActualizar = $"UPDATE tipo_evento SET " +
                      $"nombre = '{tipoEvento.nombre}' " +
                      $"WHERE id = {tipoEvento.id}";
                    db.ExecuteQuery(queryActualizar);
                    return Ok("Tipo de evento actualizado correctamente");
                }
                else
                {
                    // El tipo de evento no existe, devolver un BadRequest
                    return BadRequest("El tipo de evento no existe");
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al actualizar el tipo de evento: {ex.Message}");
            }
        }
        //---------------------------------Fin tipo evento---------------------------------------

        [HttpGet("unidad-medida-precio-sugerido")] // este es para el costo 
        public IActionResult GetUnidadMedidaPrecioSugerido()
        {
            try
            {
                var query = @"SELECT id, nombre FROM unidad_medida_precio_sugerido ORDER BY id";
                var resultado = db.ExecuteQuery(query);

                var unidades = resultado.AsEnumerable().Select(row => new unidadmedidapreciosugeridoModel
                {
                    id = row.Field<int?>("id"),
                    nombre = row.Field<string>("nombre")
                }).ToList();

                return Ok(unidades);
            }
            catch (Exception ex)
            {
                return BadRequest("Error al obtener las unidades de medida de precio sugerido.");
            }
        }

        [HttpPost("nueva-unidad-medida-precio-sugerido")]
        public IActionResult NuevaUnidadMedidaPrecioSugerido([FromBody] unidadmedidapreciosugeridoModel unidad)
        {
            try
            {
                var queryValidador = $"SELECT id FROM unidad_medida_precio_sugerido WHERE nombre = '{unidad.nombre}'";
                var resultadoValidador = db.ExecuteQuery(queryValidador);

                if (resultadoValidador.Rows.Count == 0)
                {
                    var queryInsertar = $"INSERT INTO unidad_medida_precio_sugerido (nombre) VALUES ('{unidad.nombre}')";
                    db.ExecuteQuery(queryInsertar);
                    return Ok("Unidad de medida de precio sugerido registrada correctamente");
                }
                else
                {
                    return BadRequest("La unidad de medida de precio sugerido ya está registrada");
                }
            }
            catch (Exception ex)
            {
                return BadRequest("Error al registrar la unidad de medida de precio sugerido.");
            }
        }

        [HttpPost("actualizar-unidad-medida-precio-sugerido")]
        public IActionResult ActualizarUnidadMedidaPrecioSugerido([FromBody] unidadmedidapreciosugeridoModel unidad)
        {
            try
            {
                var queryValidador = $"SELECT id FROM unidad_medida_precio_sugerido WHERE id = {unidad.id}";
                var resultadoValidador = db.ExecuteQuery(queryValidador);

                if (resultadoValidador.Rows.Count > 0)
                {
                    var queryActualizar = $"UPDATE unidad_medida_precio_sugerido SET " +
                                          $"nombre = '{unidad.nombre}' " +
                                          $"WHERE id = {unidad.id}";
                    db.ExecuteQuery(queryActualizar);
                    return Ok("Unidad de medida de precio sugerido actualizada correctamente");
                }
                else
                {
                    return BadRequest("La unidad de medida de precio sugerido no existe");
                }
            }
            catch (Exception ex)
            {
                return BadRequest("Error al actualizar la unidad de medida de precio sugerido.");
            }
        }

        //---------------------------Fin unidad de mededida precio sugerido------------------------------------------------------


        [HttpGet("unidad-medida-inventario")]// para el inventario
        public IActionResult GetUnidadMedidaCosto()
        {
            try
            {
                var query = @"SELECT id, nombre FROM unidad_medida ORDER BY id";
                var resultado = db.ExecuteQuery(query);

                var unidades = resultado.AsEnumerable().Select(row => new unidadmedidaModel
                {
                    id = row.Field<int?>("id"),
                    nombre = row.Field<string>("nombre")
                }).ToList();

                return Ok(unidades);
            }
            catch (Exception ex)
            {
                return BadRequest("Error al obtener las unidades de medida de precio sugerido.");
            }
        }

        [HttpPost("nueva-unidad-medida-inventario")]
        public IActionResult NuevaUnidadMedidaCosto([FromBody] unidadmedidaModel unidad)
        {
            try
            {
                var queryValidador = $"SELECT id FROM unidad_medida WHERE nombre = '{unidad.nombre}'";
                var resultadoValidador = db.ExecuteQuery(queryValidador);

                if (resultadoValidador.Rows.Count == 0)
                {
                    var queryInsertar = $"INSERT INTO unidad_medida (nombre) VALUES ('{unidad.nombre}')";
                    db.ExecuteQuery(queryInsertar);
                    return Ok("Unidad de medida de precio sugerido registrada correctamente");
                }
                else
                {
                    return BadRequest("La unidad de medida de precio sugerido ya está registrada");
                }
            }
            catch (Exception ex)
            {
                return BadRequest("Error al registrar la unidad de medida de precio sugerido.");
            }
        }

        [HttpPost("actualizar-unidad-medida-inventario")]
        public IActionResult ActualizarUnidadCosto([FromBody] unidadmedidaModel unidad)
        {
            try
            {
                var queryValidador = $"SELECT id FROM unidad_medida WHERE id = {unidad.id}";
                var resultadoValidador = db.ExecuteQuery(queryValidador);

                if (resultadoValidador.Rows.Count > 0)
                {
                    var queryActualizar = $"UPDATE unidad_medida SET " +
                                          $"nombre = '{unidad.nombre}' " +
                                          $"WHERE id = {unidad.id}";
                    db.ExecuteQuery(queryActualizar);
                    return Ok("Unidad de medida de precio sugerido actualizada correctamente");
                }
                else
                {
                    return BadRequest("La unidad de medida de precio sugerido no existe");
                }
            }
            catch (Exception ex)
            {
                return BadRequest("Error al actualizar la unidad de medida de precio sugerido.");
            }
        }

        //---------------------------Fin unidad de mededida precio sugerido------------------------------------------------------

        [HttpGet("estados")]
        public IActionResult GetEstados()
        {
            try
            {
                var query = @"SELECT id, estado FROM estado ORDER BY id";
                var resultado = db.ExecuteQuery(query);

                var estados = resultado.AsEnumerable().Select(row => new estadoModel
                {
                    id = row.Field<int?>("id"),
                    estado = row.Field<string>("estado")
                }).ToList();

                return Ok(estados);
            }
            catch (Exception ex)
            {
                return BadRequest("Error al obtener los estados");
            }
        }

        [HttpPost("nuevo-estado")]
        public IActionResult NuevoEstado([FromBody] estadoModel estado)
        {
            try
            {
                var queryInsertar = $"INSERT INTO estado (estado) VALUES ('{estado.estado}')";
                db.ExecuteQuery(queryInsertar);
                return Ok("Estado registrado correctamente");
            }
            catch (Exception ex)
            {
                return BadRequest("Error al registrar el estado");
            }
        }

        [HttpPost("actualizar-estado")]
        public IActionResult ActualizarEstado([FromBody] estadoModel estado)
        {
            try
            {
                var queryValidador = $"SELECT id FROM estado WHERE id = {estado.id}";
                var resultadoValidador = db.ExecuteQuery(queryValidador);

                if (resultadoValidador.Rows.Count > 0)
                {
                    var queryActualizar = $"UPDATE estado SET estado = '{estado.estado}' WHERE id = {estado.id}";
                    db.ExecuteQuery(queryActualizar);
                    return Ok("Estado actualizado correctamente");
                }
                else
                {
                    return BadRequest("El estado no existe");
                }
            }
            catch (Exception ex)
            {
                return BadRequest("Error al actualizar el estado");
            }
        }
        //---------------------------Fin estado-----------------------------------------------------------------------------

        private static DateOnly? ParseDateOnly(string dateString)
        {
            if (string.IsNullOrEmpty(dateString) || dateString == "0000-00-00")
            {
                return null;
            }
            return DateOnly.ParseExact(dateString, "yyyy-MM-dd", null);
        }

        private static TimeOnly? ParseTimeOnly(string timeString)
        {
            if (string.IsNullOrEmpty(timeString) || timeString == "00:00:00")
            {
                return null;
            }
            return TimeOnly.ParseExact(timeString, "HH:mm:ss", null);
        }




        [HttpGet("tipos-insumo-utensilio")]
        public IActionResult GetTiposInsumoUtensilio()
        {
            try
            {
                var query = @"SELECT id, tipo FROM tipo_insumo_utensilio ORDER BY id";
                var resultado = db.ExecuteQuery(query);

                var tiposInsumoUtensilio = resultado.AsEnumerable().Select(row => new tipoinsumoutensilioModel
                {
                    id = row.Field<int?>("id"),
                    tipo = row.Field<string>("tipo")
                }).ToList();

                return Ok(tiposInsumoUtensilio);
            }
            catch (Exception ex)
            {
                return BadRequest("Error al obtener los tipos de insumos y utensilios");
            }
        }

        [HttpPost("nuevo-tipo-insumo-utensilio")]
        public IActionResult NuevoTipoInsumoUtensilio([FromBody] tipoinsumoutensilioModel tipoInsumoUtensilio)
        {
            try
            {
                var queryInsertar = $"INSERT INTO tipo_insumo_utensilio (tipo) VALUES ('{tipoInsumoUtensilio.tipo}')";
                db.ExecuteQuery(queryInsertar);
                return Ok("Tipo de insumo/utensilio registrado correctamente");
            }
            catch (Exception ex)
            {
                return BadRequest("Error al registrar el tipo de insumo/utensilio");
            }
        }

        [HttpPost("actualizar-tipo-insumo-utensilio")]
        public IActionResult ActualizarTipoInsumoUtensilio([FromBody] tipoinsumoutensilioModel tipoInsumoUtensilio)
        {
            try
            {
                var queryValidador = $"SELECT id FROM tipo_insumo_utensilio WHERE id = {tipoInsumoUtensilio.id}";
                var resultadoValidador = db.ExecuteQuery(queryValidador);

                if (resultadoValidador.Rows.Count > 0)
                {
                    var queryActualizar = $"UPDATE tipo_insumo_utensilio SET tipo = '{tipoInsumoUtensilio.tipo}' WHERE id = {tipoInsumoUtensilio.id}";
                    db.ExecuteQuery(queryActualizar);
                    return Ok("Tipo de insumo/utensilio actualizado correctamente");
                }
                else
                {
                    return BadRequest("El tipo de insumo/utensilio no existe");
                }
            }
            catch (Exception ex)
            {
                return BadRequest("Error al actualizar el tipo de insumo/utensilio");
            }
        }
        //-----------------Fin tipo insumo utensilio-------------------------------------------------------------------------------------

        [HttpGet("motivos-salida")]
        public IActionResult GetMotivosSalida()
        {
            try
            {
                var query = @"SELECT id, nombre FROM motivo_salida ORDER BY id";
                var resultado = db.ExecuteQuery(query);

                var motivosSalida = resultado.AsEnumerable().Select(row => new motivosalidaModel
                {
                    id = row.Field<int?>("id"),
                    nombre = row.Field<string>("nombre")
                }).ToList();

                return Ok(motivosSalida);
            }
            catch (Exception ex)
            {
                return BadRequest("Error al obtener los motivos de salida");
            }
        }

        [HttpPost("nuevo-motivo-salida")]
        public IActionResult NuevoMotivoSalida([FromBody] motivosalidaModel motivoSalida)
        {
            try
            {
                var queryInsertar = $"INSERT INTO motivo_salida (nombre) VALUES ('{motivoSalida.nombre}')";
                db.ExecuteQuery(queryInsertar);
                return Ok("Motivo de salida registrado correctamente");
            }
            catch (Exception ex)
            {
                return BadRequest("Error al registrar el motivo de salida");
            }
        }

        [HttpPost("actualizar-motivo-salida")]
        public IActionResult ActualizarMotivoSalida([FromBody] motivosalidaModel motivoSalida)
        {
            try
            {
                var queryValidador = $"SELECT id FROM motivo_salida WHERE id = {motivoSalida.id}";
                var resultadoValidador = db.ExecuteQuery(queryValidador);

                if (resultadoValidador.Rows.Count > 0)
                {
                    var queryActualizar = $"UPDATE motivo_salida SET nombre = '{motivoSalida.nombre}' WHERE id = {motivoSalida.id}";
                    db.ExecuteQuery(queryActualizar);
                    return Ok("Motivo de salida actualizado correctamente");
                }
                else
                {
                    return BadRequest("El motivo de salida no existe");
                }
            }
            catch (Exception ex)
            {
                return BadRequest("Error al actualizar el motivo de salida");
            }
        }
        //-------------------Fin actualizar motivo salida----------------------------------------------------------

        [HttpGet("proveedores")]
        public IActionResult GetProveedores()
        {
            try
            {
                var query = @"SELECT id, nombre, telefono, descripcion FROM proveedor ORDER BY id";
                var resultado = db.ExecuteQuery(query);

                var proveedores = resultado.AsEnumerable().Select(row => new proveedorModel
                {
                    id = row.Field<int?>("id"),
                    nombre = row.Field<string>("nombre"),
                    telefono = row.Field<int?>("telefono"),
                    descripcion = row.Field<string>("descripcion")
                }).ToList();

                return Ok(proveedores);
            }
            catch (Exception ex)
            {
                return BadRequest("Error al obtener los proveedores");
            }
        }

        [HttpPost("nuevo-proveedor")]
        public IActionResult NuevoProveedor([FromBody] proveedorModel proveedor)
        {
            try
            {
                var queryInsertar = $"INSERT INTO proveedor (nombre, telefono, descripcion) " +
                                    $"VALUES ('{proveedor.nombre}', {proveedor.telefono}, '{proveedor.descripcion}')";
                db.ExecuteQuery(queryInsertar);
                return Ok("Proveedor registrado correctamente");
            }
            catch (Exception ex)
            {
                return BadRequest("Error al registrar el proveedor");
            }
        }

        [HttpPost("actualizar-proveedor")]
        public IActionResult ActualizarProveedor([FromBody] proveedorModel proveedor)
        {
            try
            {
                var queryValidador = $"SELECT id FROM proveedor WHERE id = {proveedor.id}";
                var resultadoValidador = db.ExecuteQuery(queryValidador);

                if (resultadoValidador.Rows.Count > 0)
                {
                    var queryActualizar = $"UPDATE proveedor SET " +
                                          $"nombre = '{proveedor.nombre}', " +
                                          $"telefono = {proveedor.telefono}, " +
                                          $"descripcion = '{proveedor.descripcion}' " +
                                          $"WHERE id = {proveedor.id}";
                    db.ExecuteQuery(queryActualizar);
                    return Ok("Proveedor actualizado correctamente");
                }
                else
                {
                    return BadRequest("El proveedor no existe");
                }
            }
            catch (Exception ex)
            {
                return BadRequest("Error al actualizar el proveedor");
            }
        }
        //-----------------------------Fin proveedor-------------------------------------------------------
        [HttpGet("pastel-realizado")]
        public IActionResult pastelRealizado()
        {
            try
            {
                var query = @"SELECT a.id, a.id_tipo_evento, a.id_pedido, a.imagen, b.nombre as tipo
                            FROM pastel_realizado a
                            JOIN tipo_evento b ON a.id = b.id
                            ORDER BY id";
                var resultado = db.ExecuteQuery(query);

                var pasteles = resultado.AsEnumerable().Select(row => new pastelrealizadoModel
                {
                    id = row.Field<int?>("id"),
                    id_tipo_evento = row.Field<int?>("id_tipo_evento"),
                    id_pedido = row.Field<int?>("id_pedido"),
                    tipo = row.Field<string>("tipo"),
                    imagen = row.Field<string>("imagen")
                }).ToList();

                return Ok(pasteles);
            }
            catch (Exception ex)
            {
                return BadRequest("Error al obtener las pasteles.");
            }
        }
        //----------------------------------------------------------------------------


        [HttpGet("insumos")]
        public IActionResult GetIsumos()
        {
            try
            {
                var query = @"SELECT a.*, 
                DATE_FORMAT(a.fecha_ingreso, '%Y-%m-%d') AS fechaing, 
                DATE_FORMAT(a.fecha_vencimiento, '%Y-%m-%d') AS fechaven, 
                b.nombre AS nombre_unidad_medida, 
                c.tipo
                FROM insumo_utensilio a
                JOIN unidad_medida b ON a.id_unidad_medida = b.id
                JOIN tipo_insumo_utensilio c ON a.id_tipo_insumo = c.id
                ORDER BY a.id;
                ";
                var resultado = db.ExecuteQuery(query);

                var insummos = resultado.AsEnumerable().Select(row => new insumoutensilioModelCompleto
                {
                    id = row.Field<int?>("id"),
                    id_tipo_insumo = row.Field<int?>("id_tipo_insumo"),
                    nombre = row.Field<string>("nombre"),
                    id_unidad_medida = row.Field<int?>("id_unidad_medida"),
                    precio_unitario = row.Field<double?>("precio_unitario"),
                    cantidad = row.Field<int?>("cantidad"),
                    inventarioRenovable = Convert.ToBoolean(row["inventarioRenovable"]),
                    fecha_ingreso = row.Field<string>("fechaing"),
                    fecha_vencimiento = row.Field<string>("fechaven"),
                    tipo = row.Field<string>("tipo"),
                    nombre_unidad_medida = row.Field<string>("nombre_unidad_medida")
                }).ToList();


                return Ok(insummos);
            }
            catch (Exception ex)
            {
                return BadRequest("Error al obtener los proveedores");
            }
        }

        [HttpPost("actualizar-insumo")]
        public IActionResult ActualizarInsumo([FromBody] insumoutensilioModel insumo)
        {
            try
            {
                // Verificar si el insumo existe
                var queryValidador = $"SELECT id FROM insumo_utensilio WHERE id = {insumo.id}";
                var resultadoValidador = db.ExecuteQuery(queryValidador);

                if (resultadoValidador.Rows.Count > 0)
                {
                    // Convertir inventarioRenovable a 1 o 0
                    int inventarioRenovableValue = insumo.inventarioRenovable.HasValue && insumo.inventarioRenovable.Value ? 1 : 0;

                    // Actualizar el insumo
                    var queryActualizar = $"UPDATE insumo_utensilio SET " +
                                          $"id_tipo_insumo = '{insumo.id_tipo_insumo}', " +
                                          $"nombre = '{insumo.nombre}', " +
                                          $"id_unidad_medida = '{insumo.id_unidad_medida}', " +
                                          $"precio_unitario = '{insumo.precio_unitario}', " +
                                          $"cantidad = '{insumo.cantidad}', " +
                                          $"inventarioRenovable = {inventarioRenovableValue}, " + 
                                          $"fecha_ingreso = '{insumo.fecha_ingreso:yyyy-MM-dd}', " +
                                          $"fecha_vencimiento = '{insumo.fecha_vencimiento:yyyy-MM-dd}' " +
                                          $"WHERE id = {insumo.id}";

                    db.ExecuteQuery(queryActualizar);
                    return Ok("Insumo actualizado correctamente");
                }
                else
                {
                    return BadRequest("El insumo no existe");
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al actualizar el insumo: {ex.Message}");
            }
        }



        [HttpPost("nuevo-insumo")]
        public IActionResult NuevoInsumo([FromBody] insumoutensilioModel insumo)
        {
            try
            {
                int inventarioRenovableValue = insumo.inventarioRenovable.HasValue && insumo.inventarioRenovable.Value ? 1 : 0;
                var queryInsertar = $"INSERT INTO insumo_utensilio (id_tipo_insumo, nombre,id_unidad_medida, precio_unitario, cantidad, inventarioRenovable, " +
                                    $"fecha_ingreso, fecha_vencimiento) " +
                                    $"VALUES ('{insumo.id_tipo_insumo}', '{insumo.nombre}', '{insumo.id_unidad_medida}', '{insumo.precio_unitario}', " +
                                    $"'{insumo.cantidad}', '{inventarioRenovableValue}', '{insumo.fecha_ingreso:yyyy-MM-dd}', '{insumo.fecha_vencimiento:yyyy-MM-dd}')";
                db.ExecuteQuery(queryInsertar);
                return Ok("insumo_utensilio registrado correctamente");
            }
            catch (Exception ex)
            {
                return BadRequest("Error al registrar el insumo_utensilio");
            }
        }
        //-----------------------------Fin insumos-------------------------------------------------------

    }
}
