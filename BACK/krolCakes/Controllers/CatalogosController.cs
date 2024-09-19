using krolCakes.Models;
using krolCakes.Provider;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Win32;
using MySql.Data.MySqlClient;
using Mysqlx.Crud;
using System.Data;
using MySql.Data.MySqlClient;  // Para MySQL


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

        [HttpGet("tipos-entrega")]
        public IActionResult GetTiposEntrega()
        {
            try
            {
                // Consulta SQL para obtener todos los tipos de entrega
                var query = "SELECT id, nombre FROM tipo_entrega ORDER BY id";
                var resultado = db.ExecuteQuery(query);

                // Construir la lista de objetos tipoentregaModel con los datos obtenidos de la base de datos
                var tiposEntrega = resultado.AsEnumerable().Select(row => new tipoentregaModel
                {
                    id = row.Field<int?>("id"),
                    nombre = row.Field<string>("nombre")
                }).ToList();

                return Ok(tiposEntrega);
            }
            catch (Exception ex)
            {
                // Manejo de excepciones: retorna un BadRequest en caso de error
                return BadRequest($"Error al obtener los tipos de entrega: {ex.Message}");
            }
        }

        [HttpPost("nuevo-tipo-entrega")]
        public IActionResult NuevoTipoEntrega([FromBody] tipoentregaModel tipoEntrega)
        {
            try
            {
                var queryValidador = $"SELECT id FROM tipo_entrega WHERE nombre = '{tipoEntrega.nombre}'";
                var resultadoValidador = db.ExecuteQuery(queryValidador);

                if (resultadoValidador.Rows.Count == 0) // Si no existe un tipo de entrega con el mismo nombre
                {
                    var queryInsertar = $"INSERT INTO tipo_entrega (nombre) VALUES ('{tipoEntrega.nombre}')";
                    db.ExecuteQuery(queryInsertar);
                    return Ok("Tipo de entrega registrado correctamente");
                }
                else
                {
                    // El tipo de entrega ya existe, devolver un BadRequest
                    return BadRequest("El tipo de entrega ya está registrado");
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al registrar el tipo de entrega: {ex.Message}");
            }
        }

        [HttpPost("actualizar-tipo-entrega")]
        public IActionResult ActualizarTipoEntrega([FromBody] tipoentregaModel tipoEntrega)
        {
            try
            {
                // Validar que el tipo de entrega existe
                var queryValidador = $"SELECT id FROM tipo_entrega WHERE id = {tipoEntrega.id}";
                var resultadoValidador = db.ExecuteQuery(queryValidador);

                if (resultadoValidador.Rows.Count > 0) // Si el tipo de entrega existe
                {
                    // Actualizar el campo nombre del tipo de entrega
                    var queryActualizar = $"UPDATE tipo_entrega SET " +
                      $"nombre = '{tipoEntrega.nombre}' " +
                      $"WHERE id = {tipoEntrega.id}";
                    db.ExecuteQuery(queryActualizar);
                    return Ok("Tipo de entrega actualizado correctamente");
                }
                else
                {
                    // El tipo de entrega no existe, devolver un BadRequest
                    return BadRequest("El tipo de entrega no existe");
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al actualizar el tipo de entrega: {ex.Message}");
            }
        }
        //------------------------------Fin tipo de entrega----------------------------------------

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

        [HttpGet("unidad-medida-precio-sugerido")]
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

        [HttpGet("cotizaciononline")]
        public IActionResult GetCotizacionesOnline()
        {
            try
            {
                var query = @"SELECT c.id, c.nombre, c.descripcion, c.direccion, c.telefono, c.precio_aproximado, c.envio, c.estado,
             DATE_FORMAT(c.fecha, '%Y-%m-%d') AS fecha,
             TIME_FORMAT(c.hora, '%H:%i:%s') AS hora,
             i.correlativo AS imagen_id, i.ruta AS imagen_ruta, i.observacion AS imagen_observacion,
             d.correlativo AS desglose_id, d.id_producto AS desglose_id_producto, 
             d.subtotal AS desglose_subtotal, d.cantidad AS desglose_cantidad,
             o.correlativo AS observacion_id, o.Observacion AS observacion_text
      FROM cotizacion_online c
      LEFT JOIN imagen_referencia_online i ON c.id = i.id_cotizacion_online
      LEFT JOIN desglose_online d ON c.id = d.id_cotizacion_online
      LEFT JOIN observacion_cotizacion_online o ON c.id = o.id_cotizacion_online
      ORDER BY c.id";

                var resultado = db.ExecuteQuery(query);
                var cotizaciones = resultado.AsEnumerable().GroupBy(row => new
                {
                    id = Convert.ToInt32(row["id"]),
                    nombre = row["nombre"]?.ToString(),
                    descripcion = row["descripcion"]?.ToString(),
                    direccion = row["direccion"]?.ToString(),
                    telefono = row["telefono"] != DBNull.Value ? Convert.ToInt32(row["telefono"]) : (int?)null,
                    fecha = row["fecha"]?.ToString(),
                    hora = row["hora"]?.ToString(),
                    envio = row["envio"] != DBNull.Value ? Convert.ToBoolean(row["envio"]) : (bool?)null,
                    estado = row["estado"] != DBNull.Value ? Convert.ToBoolean(row["estado"]) : (bool?)null,
                    precio_aproximado = row["precio_aproximado"] != DBNull.Value ? Convert.ToDouble(row["precio_aproximado"]) : (double?)null
                })
                .Select(grp => new cotizaciononlineModel
                {
                    id = grp.Key.id,
                    nombre = grp.Key.nombre,
                    descripcion = grp.Key.descripcion,
                    direccion = grp.Key.direccion,
                    telefono = grp.Key.telefono,
                    fecha = grp.Key.fecha,
                    hora = grp.Key.hora,
                    envio = grp.Key.envio,
                    estado = grp.Key.estado, // Aquí incluimos el campo estado
                    precio_aproximado = grp.Key.precio_aproximado,
                    imagenes = grp
                        .Where(row => row["imagen_id"] != DBNull.Value)
                        .Select(row => new imagenreferenciaonlineModel
                        {
                            correlativo = Convert.ToInt32(row["imagen_id"]),
                            ruta = row["imagen_ruta"]?.ToString(),
                            observacion = row["imagen_observacion"]?.ToString()
                        }).ToList(),
                    desgloses = grp
                        .Where(row => row["desglose_id"] != DBNull.Value)
                        .Select(row => new desgloseonlineModel
                        {
                            correlativo = Convert.ToInt32(row["desglose_id"]),
                            id_producto = Convert.ToInt32(row["desglose_id_producto"]),
                            subtotal = Convert.ToDouble(row["desglose_subtotal"]),
                            cantidad = Convert.ToInt32(row["desglose_cantidad"])
                        }).ToList(),
                    Observacion = grp
                        .Where(row => row["observacion_id"] != DBNull.Value)
                        .Select(row => new observacion_cotizacion_onlineModel
                        {
                            correlativo = Convert.ToInt32(row["observacion_id"]),
                            Observacion = row["observacion_text"]?.ToString()
                        }).ToList()
                }).ToList();
                return Ok(cotizaciones);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}\nStack Trace: {ex.StackTrace}");
            }
        }




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


        [HttpPost("nueva-cotizaciononline")]
        public IActionResult NuevaCotizacionOnline([FromBody] cotizaciononlineModel cotizacion)
        {
            try
            {
                // Convertir los valores booleanos a enteros (1 o 0)
                int envio = cotizacion.envio == true ? 1 : 0;

                // Insertar la cotización online
                var queryInsertCotizacion = $"INSERT INTO cotizacion_online (nombre, descripcion, direccion, telefono, fecha, hora, precio_aproximado, envio, mano_obra, presupuesto_insumos, total_presupuesto, estado) " +
                                            $"VALUES ('{cotizacion.nombre}', '{cotizacion.descripcion}', '{cotizacion.direccion}', {cotizacion.telefono}, " +
                                            $"'{cotizacion.fecha}', '{cotizacion.hora}', {cotizacion.precio_aproximado}, {envio}, {cotizacion.mano_obra}, {cotizacion.presupuesto_insumos}, {cotizacion.total_presupuesto}, 1)";
                db.ExecuteQuery(queryInsertCotizacion);

                // Obtener el ID de la cotización recién insertada
                var idCotizacion = db.ExecuteQuery("SELECT LAST_INSERT_ID()").Rows[0][0];

                // Insertar las imágenes de referencia
                if (cotizacion.imagenes != null)
                {
                    foreach (var imagen in cotizacion.imagenes)
                    {
                        var queryInsertImagen = $"INSERT INTO imagen_referencia_online (ruta, observacion, id_cotizacion_online) " +
                                                $"VALUES ('{imagen.ruta}', '{imagen.observacion}', {idCotizacion})";
                        db.ExecuteQuery(queryInsertImagen);
                    }
                }

                // Insertar los desgloses
                if (cotizacion.desgloses != null)
                {
                    foreach (var desglose in cotizacion.desgloses)
                    {
                        var queryInsertDesglose = $"INSERT INTO desglose_online (id_producto, subtotal, cantidad, id_cotizacion_online) " +
                                                  $"VALUES ({desglose.id_producto}, {desglose.subtotal}, {desglose.cantidad}, {idCotizacion})";
                        db.ExecuteQuery(queryInsertDesglose);
                    }
                }

                // Insertar las observaciones
                if (cotizacion.Observacion != null)
                {
                    foreach (var observacion in cotizacion.Observacion)
                    {
                        var queryInsertObservacion = $"INSERT INTO observacion_cotizacion_online (id_cotizacion_online, Observacion) " +
                                                     $"VALUES ({idCotizacion}, '{observacion.Observacion}')";
                        db.ExecuteQuery(queryInsertObservacion);
                    }
                }

                return Ok("Cotización online registrada correctamente");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al registrar la cotización online: {ex.Message}");
            }
        }











        //---------------------Fin cotizacion online-------------------------------------------------------------------------------

        [HttpGet("desgloseonline")]
        public IActionResult GetDesgloseOnline(int? id_cotizacion_online)
        {
            try
            {
                // Construir la consulta SQL con el filtro si se proporciona id_cotizacion_online
                var query = @"SELECT correlativo, id_cotizacion_online, id_producto, subtotal, cantidad
              FROM desglose_online";

                if (id_cotizacion_online.HasValue)
                {
                    query += $" WHERE id_cotizacion_online = {id_cotizacion_online.Value}";
                }

                //query += " ORDER BY correlativo";  // Opcional: Agregar ordenamiento

                // Ejecutar la consulta
                var resultado = db.ExecuteQuery(query);  // Ejecutar consulta sin parámetros

                // Mapeo del resultado al modelo desgloseonlineModel
                var desgloses = resultado.AsEnumerable().Select(row => new desgloseonlineModel
                {
                    correlativo = row.Field<int?>("correlativo"),
                    id_cotizacion_online = row.Field<int?>("id_cotizacion_online"),
                    id_producto = row.Field<int?>("id_producto"),
                    subtotal = row.Field<double?>("subtotal"),
                    cantidad = row.Field<int?>("cantidad")
                }).ToList();

                return Ok(desgloses);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al obtener los desgloses online: {ex.Message}");
            }
        }







        //-----------------Fin actualizar desglose online----------------------------------------------------------------------------------------------

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

        [HttpGet("imagenreferenciaonline")]
        public IActionResult GetImagenesOnline(int? id_cotizacion_online)
        {
            try
            {
                // Construir la consulta SQL con el filtro si se proporciona id_cotizacion_online
                var query = @"
            SELECT correlativo, id_cotizacion_online, ruta, observacion
            FROM imagen_referencia_online";

                if (id_cotizacion_online.HasValue)
                {
                    query += $" WHERE id_cotizacion_online = {id_cotizacion_online.Value}";
                }

                query += " ORDER BY correlativo";  // Opcional: Agregar ordenamiento

                // Ejecutar la consulta
                var resultado = db.ExecuteQuery(query);  // Ejecutar consulta sin parámetros

                // Mapeo del resultado al modelo imagenreferenciaonlineModel
                var imagenes = resultado.AsEnumerable().Select(row => new imagenreferenciaonlineModel
                {
                    correlativo = row.Field<int?>("correlativo"),
                    id_cotizacion_online = row.Field<int?>("id_cotizacion_online"),
                    ruta = row.Field<string>("ruta"),
                    observacion = row.Field<string>("observacion")
                }).ToList();

                return Ok(imagenes);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al obtener las imágenes de referencia online: {ex.Message}");
            }
        }
        //-------------------------Fin imagen de referencia online--------------------------------------------------


        [HttpGet("pedidos")]
        public IActionResult GetPedidos()
        {
            try
            {
                var query = @"SELECT id, fecha, hora, id_estado, id_cliente, observaciones, direccion, id_tipo_entrega, precio_total, mano_obra, presupuesto_insumos
                      FROM pedidos"; // Asegúrate de que el nombre de la tabla coincida con el de tu base de datos

                var resultado = db.ExecuteQuery(query);

                var pedidos = resultado.AsEnumerable().Select(row => new pedidoModel
                {
                    id = row["id"] != DBNull.Value ? Convert.ToInt32(row["id"]) : (int?)null,
                    fecha = row["fecha"] != DBNull.Value ? DateOnly.FromDateTime(Convert.ToDateTime(row["fecha"])) : (DateOnly?)null,
                    hora = row["hora"] != DBNull.Value ? TimeOnly.FromDateTime(Convert.ToDateTime(row["hora"])) : (TimeOnly?)null,
                    id_estado = row["id_estado"] != DBNull.Value ? Convert.ToInt32(row["id_estado"]) : (int?)null,
                    id_cliente = row["id_cliente"] != DBNull.Value ? Convert.ToInt32(row["id_cliente"]) : (int?)null,
                    observaciones = row["observaciones"]?.ToString(),
                    direccion = row["direccion"]?.ToString(),
                    id_tipo_entrega = row["id_tipo_entrega"] != DBNull.Value ? Convert.ToInt32(row["id_tipo_entrega"]) : (int?)null,
                    precio_total = row["precio_total"] != DBNull.Value ? Convert.ToDouble(row["precio_total"]) : (double?)null,
                    mano_obra = row["mano_obra"] != DBNull.Value ? Convert.ToDouble(row["mano_obra"]) : (double?)null,
                    presupuesto_insumos = row["presupuesto_insumos"] != DBNull.Value ? Convert.ToDouble(row["presupuesto_insumos"]) : (double?)null
                }).ToList();

                return Ok(pedidos);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}\nStack Trace: {ex.StackTrace}");
            }
        }

        //----------------------------------Fin pedidos---------------------------------------------------------------------------------------------------------------------------

        [HttpPost("nueva-observacion")]
        public IActionResult NuevaObservacion([FromBody] observacion_cotizacion_onlineModel observacion)
        {
            try
            {
                // Validación básica
                if (observacion.id_cotizacion_online == null || string.IsNullOrEmpty(observacion.Observacion))
                {
                    return BadRequest("El ID de cotización y la observación son obligatorios.");
                }

                // Consulta para insertar la nueva observación
                var queryInsertar = $"INSERT INTO observacion_cotizacion_online (id_cotizacion_online, Observacion) " +
                                    $"VALUES ({observacion.id_cotizacion_online}, '{observacion.Observacion}')";

                // Ejecutar la consulta
                db.ExecuteQuery(queryInsertar);

                // Respuesta de éxito
                return Ok("Observación registrada correctamente");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al registrar la observación: {ex.Message}");
            }
        }
        //---------------------------------Fin observacion-------------------------------------------------------------------------------------------







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
