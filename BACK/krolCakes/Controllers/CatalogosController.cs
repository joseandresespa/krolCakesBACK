using krolCakes.Models;
using krolCakes.Provider;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Win32;
using MySql.Data.MySqlClient;
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
