using krolCakes.Models;
using krolCakes.Provider;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Win32;
using MySql.Data.MySqlClient;
using Mysqlx.Crud;
using System.Data;
using Firebase.Auth;
using Firebase.Storage;
using static System.Net.Mime.MediaTypeNames;

namespace krolCakes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CotizacionPedidoController : ControllerBase
    {

        private readonly DatabaseProvider db;
        private readonly Progra progra;

        /// ingresasddo por el deyvid
        private const string FirebaseApiKey = "AIzaSyAKNUby3Oy_YdtFOYSBSwA2w41gG1P7FL8";
        private const string FirebaseEmail = "armandito@gmail.com";
        private const string FirebasePassword = "armando123";
        private const string FirebaseBucket = "subida-ab43b.appspot.com";


        [HttpPost("upload-image")]
        public async Task<IActionResult> UploadImage([FromForm] IFormFile image)
        {
            if (image == null || image.Length == 0)
                return BadRequest("No se proporcionó ninguna imagen válida.");

            try
            {
                // Subir la imagen a Firebase
                string imageUrl = await UploadToFirebase(image);

                // Devolver la URL de la imagen
                return Ok(new { ImageUrl = imageUrl });
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, $"Error al subir la imagen: {ex.Message}");
            }
        }

        private async Task<string> UploadToFirebase(IFormFile image)
        {
            using (var stream = image.OpenReadStream())
            {
                var auth = new FirebaseAuthProvider(new FirebaseConfig(FirebaseApiKey));
                var authResult = await auth.SignInWithEmailAndPasswordAsync(FirebaseEmail, FirebasePassword);

                var uploadTask = new FirebaseStorage(
                    FirebaseBucket,
                    new FirebaseStorageOptions
                    {
                        AuthTokenAsyncFactory = () => Task.FromResult(authResult.FirebaseToken),
                        ThrowOnCancel = true
                    })
                    .Child("images") // Puedes organizar tus imágenes en carpetas si lo prefieres
                    .Child(image.FileName)
                    .PutAsync(stream);

                return await uploadTask;
            }
        }
        //terminao el deyvi
        //------------------------------Fin logica imagenes-------------------------------------------------------------------
        public CotizacionPedidoController(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("ConnectionString");
            db = new DatabaseProvider(connectionString);
            progra = new Progra(configuration);
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

            /*[HttpGet("pedidos")]
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
            */
        // insert de pedidos falta bb

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

        //-----------------Fin actualizar desglose online----------------------------------------------------------------------------------------------

        [HttpGet("cotizaciononline")]
        public IActionResult GetCotizacionesOnline()
        {
            try
            {
                var query = @"SELECT c.id, c.descripcion, c.direccion, c.precio_aproximado, c.envio, c.estado, c.mano_obra, 
                     c.presupuesto_insumos, (c.mano_obra + c.presupuesto_insumos) AS total_presupuesto,
                     c.cliente_id, DATE_FORMAT(c.fecha, '%Y-%m-%d') AS fecha, TIME_FORMAT(c.hora, '%H:%i:%s') AS hora, 
                     cl.nombre AS cliente_nombre, cl.telefono AS cliente_telefono, cl.nit AS cliente_nit,
                     i.correlativo AS imagen_id, i.ruta AS imagen_ruta, i.observacion AS imagen_observacion,
                     d.correlativo AS desglose_id, d.id_producto AS desglose_id_producto, d.subtotal AS desglose_subtotal, 
                     d.cantidad AS desglose_cantidad, d.precio_pastelera AS desglose_precio_pastelera,
                     p.nombre AS producto_nombre, p.descripcion AS producto_descripcion, p.precio_online AS producto_precio_online,
                     o.correlativo AS observacion_id, o.Observacion AS observacion_text
              FROM cotizacion_online c
              LEFT JOIN cliente cl ON c.cliente_id = cl.id
              LEFT JOIN imagen_referencia_online i ON c.id = i.id_cotizacion_online
              LEFT JOIN desglose_online d ON c.id = d.id_cotizacion_online
              LEFT JOIN producto p ON d.id_producto = p.id
              LEFT JOIN observacion_cotizacion_online o ON c.id = o.id_cotizacion_online
              ORDER BY c.id";

                var resultado = db.ExecuteQuery(query);

                var cotizaciones = resultado.AsEnumerable().GroupBy(row => new
                {
                    id = row["id"] != DBNull.Value ? Convert.ToInt32(row["id"]) : (int?)null,
                    descripcion = row["descripcion"] != DBNull.Value ? row["descripcion"].ToString() : null,
                    direccion = row["direccion"] != DBNull.Value ? row["direccion"].ToString() : null,
                    fecha = row["fecha"] != DBNull.Value ? row["fecha"].ToString() : null,
                    hora = row["hora"] != DBNull.Value ? row["hora"].ToString() : null,
                    envio = row["envio"] != DBNull.Value ? (bool?)Convert.ToBoolean(row["envio"]) : null,
                    estado = row["estado"] != DBNull.Value ? Convert.ToSByte(row["estado"]) : (short?) null,
                    precio_aproximado = row["precio_aproximado"] != DBNull.Value ? (double?)Convert.ToDouble(row["precio_aproximado"]) : null,
                    mano_obra = row["mano_obra"] != DBNull.Value ? (double?)Convert.ToDouble(row["mano_obra"]) : null,
                    presupuesto_insumos = row["presupuesto_insumos"] != DBNull.Value ? (double?)Convert.ToDouble(row["presupuesto_insumos"]) : null,
                    total_presupuesto = row["total_presupuesto"] != DBNull.Value ? (double?)Convert.ToDouble(row["total_presupuesto"]) : null,
                    cliente_id = row["cliente_id"] != DBNull.Value ? (int?)Convert.ToInt32(row["cliente_id"]) : null,
                    cliente_nombre = row["cliente_nombre"] != DBNull.Value ? row["cliente_nombre"].ToString() : null,
                    cliente_telefono = row["cliente_telefono"] != DBNull.Value ? (int?)Convert.ToInt32(row["cliente_telefono"]) : null,
                    cliente_nit = row["cliente_nit"] != DBNull.Value ? row["cliente_nit"].ToString() : null
                })
                .Select(grp => new cotizaciononlineModelCompleto
                {
                id = grp.Key.id,
                descripcion = grp.Key.descripcion,
                direccion = grp.Key.direccion,
                fecha = grp.Key.fecha,
                hora = grp.Key.hora,
                envio = grp.Key.envio,
                estado = Convert.ToSByte(grp.Key.estado),
                precio_aproximado = grp.Key.precio_aproximado,
                mano_obra = grp.Key.mano_obra,
                presupuesto_insumos = grp.Key.presupuesto_insumos,
                total_presupuesto = grp.Key.total_presupuesto,
                cliente_id = grp.Key.cliente_id,
                nombre = grp.Key.cliente_nombre,
                telefono = grp.Key.cliente_telefono,
                nit = grp.Key.cliente_nit,
                imagenes = grp
                    .Where(row => row["imagen_id"] != DBNull.Value)
                    .Select(row => new imagenreferenciaonlineModel
                    {
                        correlativo = Convert.ToInt32(row["imagen_id"]),
                        ruta = row["imagen_ruta"] != DBNull.Value ? row["imagen_ruta"].ToString() : null,
                        observacion = row["imagen_observacion"] != DBNull.Value ? row["imagen_observacion"].ToString() : null
                    }).ToList(),
                desgloses = grp
                    .Where(row => row["desglose_id"] != DBNull.Value)
                    .Select(row => new desgloseonlineModelCompleto
                    {
                        correlativo = Convert.ToInt32(row["desglose_id"]),
                        id_producto = Convert.ToInt32(row["desglose_id_producto"]),
                        subtotal = row["desglose_subtotal"] != DBNull.Value ? Convert.ToDouble(row["desglose_subtotal"]) : (double?)null,
                        cantidad = row["desglose_cantidad"] != DBNull.Value ? Convert.ToInt32(row["desglose_cantidad"]) : (int?)null,
                        precio_pastelera = row["desglose_precio_pastelera"] != DBNull.Value ? (double?)Convert.ToDouble(row["desglose_precio_pastelera"]) : null,
                        nombrep = row["producto_nombre"] != DBNull.Value ? row["producto_nombre"].ToString() : null,
                        descripcionproducto = row["producto_descripcion"] != DBNull.Value ? row["producto_descripcion"].ToString() : null,
                        precio_online = row["producto_precio_online"] != DBNull.Value ? (double?)Convert.ToDouble(row["producto_precio_online"]) : null
                    }).ToList(),
                    Observacion = grp
                    .Where(row => row["observacion_id"] != DBNull.Value)
                    .Select(row => new observacion_cotizacion_onlineModel
                    {
                        correlativo = Convert.ToInt32(row["observacion_id"]),
                        Observacion = row["observacion_text"] != DBNull.Value ? row["observacion_text"].ToString() : null
                    })
                    .Distinct() // Eliminar duplicados
                    .ToList()
                }).ToList();



                return Ok(cotizaciones);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}\nStack Trace: {ex.StackTrace}");
            }
        }






        [HttpPost("nueva-cotizaciononline")]
        public IActionResult NuevaCotizacionOnline([FromBody] cotizaciononlineModelCompleto cotizacion)
        {
            try
            {
                // Verificar si el cliente ya existe por su número de teléfono
                var queryCheckCliente = $"SELECT id FROM cliente WHERE telefono = {cotizacion.telefono}";
                var result = db.ExecuteQuery(queryCheckCliente);

                int clienteId;

                if (result.Rows.Count == 0)
                {
                    // Insertar el nuevo cliente
                    var queryInsertCliente = $"INSERT INTO cliente (nombre, telefono, nit) VALUES ('{cotizacion.nombre}', {cotizacion.telefono}, '{cotizacion.nit}')";
                    db.ExecuteQuery(queryInsertCliente);
                    clienteId = Convert.ToInt32(db.ExecuteQuery("SELECT LAST_INSERT_ID()").Rows[0][0]);
                }
                else
                {
                    // Obtener el ID del cliente existente
                    clienteId = Convert.ToInt32(result.Rows[0]["id"]);
                }

                // Convertir los valores booleanos a enteros (1 o 0)
                int envio = cotizacion.envio == true ? 1 : 0;

                // Insertar la cotización online
                var queryInsertCotizacion = $"INSERT INTO cotizacion_online (descripcion, precio_aproximado, envio, hora, fecha, direccion, estado, cliente_id) " +
                                            $"VALUES ('{cotizacion.descripcion}', {cotizacion.precio_aproximado}, {cotizacion.envio}, '{cotizacion.hora}', " +
                                            $"'{cotizacion.fecha}', '{cotizacion.direccion}', 7, {clienteId})";
                db.ExecuteQuery(queryInsertCotizacion);

                // Obtener el ID de la cotización recién insertada
                var idCotizacion = db.ExecuteQuery("SELECT LAST_INSERT_ID()").Rows[0][0];

                // Insertar las imágenes de referencia si existen
                if (cotizacion.imagenes != null)
                {
                    foreach (var imagen in cotizacion.imagenes)
                    {
                        var queryInsertImagen = $"INSERT INTO imagen_referencia_online (ruta, id_cotizacion_online, observacion) " +
                                                $"VALUES ('{imagen.ruta}', {idCotizacion}, '{imagen.observacion}')";
                        db.ExecuteQuery(queryInsertImagen);
                    }
                }

                // Insertar los desgloses si existen
                if (cotizacion.desgloses != null)
                {
                    foreach (var desglose in cotizacion.desgloses)
                    {
                        var queryInsertDesglose = $"INSERT INTO desglose_online (id_producto, subtotal, cantidad, id_cotizacion_online) " +
                                                  $"VALUES ({desglose.id_producto}, {desglose.subtotal}, {desglose.cantidad}, {idCotizacion})";
                        db.ExecuteQuery(queryInsertDesglose);
                    }
                }

                return Ok("Cotización online registrada correctamente");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al registrar la cotización online: {ex.Message}");
            }
        }

        [HttpPost("confirmar-cotizacion")]
        public IActionResult ConfirmarCotizacion([FromBody] ConfirmarPedidoModel nuevoPedido)
        {
            try
            {
                // Validador: Verificar si ya existe un pedido con la misma cotización
                var queryValidador = $"SELECT id FROM pedido WHERE cotizacion_online_id = '{nuevoPedido.id_cotizacion_online}'";
                var resultadoValidador = db.ExecuteQuery(queryValidador);

                if (resultadoValidador.Rows.Count == 0) // Si no existe un pedido con la misma cotización
                {
                    //actualizar tabla con presupuesto

                    var queryPresupuesto = $@"UPDATE cotizacion_online SET presupuesto_insumos = {nuevoPedido.presupuesto_insumos},
                    total_presupuesto =  {nuevoPedido.total_presupuesto} , mano_obra = {nuevoPedido.mano_obra} 
                    estado = 1 WHERE id = {nuevoPedido.id_cotizacion_online};";
                    var resultadoUpdate = db.ExecuteQuery(queryPresupuesto);


                    // Insertar en la tabla de pedidos
                    var queryInsertarPedido = $@"
                    INSERT INTO pedido (id_estado, observaciones, cotizacion_online_id) 
                    VALUES (1, '{nuevoPedido.observaciones}', '{nuevoPedido.id_cotizacion_online}'); 
                    SELECT LAST_INSERT_ID();";

                    var resultadoInsertarPedido = db.ExecuteQuery(queryInsertarPedido);

                    // Obtener el ID del pedido recién insertado
                    var pedidoId = Convert.ToInt32(resultadoInsertarPedido.Rows[0][0]);

                    // Insertar desgloses (detallepedido)
                    if (nuevoPedido.detalles != null && nuevoPedido.detalles.Count > 0)
                    {
                        foreach (var desglose in nuevoPedido.detalles)
                        {
                            // Calcular el total
                            var total = desglose.precio_unitario * desglose.cantidad_porciones;

                            // Insertar el desglose con el total
                            var queryInsertarDesglose = $@"
                        INSERT INTO detalle_pedido (id_pedido, producto_id, id_masas, id_relleno, cantidad_porciones, precio_unitario, total)
                        VALUES ({pedidoId}, {desglose.producto_id}, {desglose.id_masas}, {desglose.id_relleno}, {desglose.cantidad_porciones}, {desglose.precio_unitario}, {total})";

                            db.ExecuteQuery(queryInsertarDesglose);
                        }
                    }         

                    return Ok("Pedido registrado correctamente");
                }
                else
                {
                    // El pedido ya existe, devolver un BadRequest
                    return BadRequest("Ya existe un pedido con esta cotización online.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al registrar el pedido: {ex.Message}");
            }
        }

        //---------------------Fin cotizacion online-------------------------------------------------------------------------------

        [HttpGet("pedidos")]
        public IActionResult ObtenerTodosPedidos()
        {
            try
            {
                // Consulta para obtener todos los pedidos
                var queryPedidos = @"
            SELECT 
                p.id, p.id_estado, p.observaciones, p.cotizacion_online_id,
                c.descripcion, c.precio_aproximado, c.envio, c.hora, c.fecha, c.direccion, c.mano_obra, c.presupuesto_insumos, c.total_presupuesto, c.cliente_id, 
                e.estado, cl.nombre, cl.telefono, cl.nit
            FROM pedido p
            JOIN cotizacion_online c ON p.cotizacion_online_id = c.id
            JOIN estado e ON p.id_estado = e.id
            JOIN cliente cl ON c.cliente_id = cl.id";

                var resultadoPedidos = db.ExecuteQuery(queryPedidos);

                if (resultadoPedidos.Rows.Count == 0)
                {
                    return NotFound("No se encontraron pedidos.");
                }

                var listaPedidos = new List<pedidoModelCompleto>();

                // Recorremos todos los pedidos obtenidos
                foreach (DataRow pedidoRow in resultadoPedidos.Rows)
                {
                    var pedido = new pedidoModelCompleto
                    {
                        id = Convert.ToInt32(pedidoRow["id"]),
                        id_estado = Convert.ToInt32(pedidoRow["id_estado"]),
                        observaciones = pedidoRow["observaciones"].ToString(),
                        id_cotizacion_online = pedidoRow["cotizacion_online_id"].ToString(),
                        descripcion = pedidoRow["descripcion"].ToString(),
                        precio_aproximado = Convert.ToDouble(pedidoRow["precio_aproximado"]),
                        envio = Convert.ToBoolean(pedidoRow["envio"]),
                        hora = pedidoRow["hora"].ToString(),
                        fecha = pedidoRow["fecha"].ToString(),
                        direccion = pedidoRow["direccion"].ToString(),
                        mano_obra = Convert.ToDouble(pedidoRow["mano_obra"]),
                        presupuesto_insumos = Convert.ToDouble(pedidoRow["presupuesto_insumos"]),
                        total_presupuesto = Convert.ToDouble(pedidoRow["total_presupuesto"]),
                        cliente_id = Convert.ToInt32(pedidoRow["cliente_id"]),
                        estado = pedidoRow["estado"].ToString(),
                        nombre = pedidoRow["nombre"].ToString(),
                        telefono = Convert.ToInt32(pedidoRow["telefono"]),
                        nit = pedidoRow["nit"].ToString(),
                        detalles = new List<detallepedidoModelCompleto>(),
                        imagenes = new List<imagenreferenciaonlineModel>(),
                        Observacion = new List<observacion_cotizacion_onlineModel>()
                    };

                    // Obtener desgloses (detallepedido) para cada pedido
                    var queryDesgloses = $@"
                SELECT dp.correlativo, dp.producto_id, dp.id_masas, dp.id_relleno, dp.cantidad_porciones, dp.precio_unitario, 
                       p.nombre, p.descripcion, p.precio_online, m.sabor_masa, r.sabor_relleno
                FROM detalle_pedido dp
                JOIN producto p ON dp.producto_id = p.id
                JOIN masas m ON dp.id_masas = m.id
                JOIN relleno r ON dp.id_relleno = r.id
                WHERE dp.id_pedido = {pedido.id}";

                    var resultadoDesgloses = db.ExecuteQuery(queryDesgloses);

                    foreach (DataRow row in resultadoDesgloses.Rows)
                    {
                        var desglose = new detallepedidoModelCompleto
                        {
                            correlativo = Convert.ToInt32(row["correlativo"]),
                            producto_id = Convert.ToInt32(row["producto_id"]),
                            id_masas = Convert.ToInt32(row["id_masas"]),
                            id_relleno = Convert.ToInt32(row["id_relleno"]),
                            cantidad_porciones = Convert.ToInt32(row["cantidad_porciones"]),
                            precio_unitario = Convert.ToDouble(row["precio_unitario"]),
                            nombre = row["nombre"].ToString(),
                            descripcion = row["descripcion"].ToString(),
                            precio_online = Convert.ToDouble(row["precio_online"]),
                            sabor_masa = row["sabor_masa"].ToString(),
                            sabor_relleno = row["sabor_relleno"].ToString(),
                            total = Convert.ToDouble(row["precio_unitario"]) * Convert.ToInt32(row["cantidad_porciones"])
                        };
                        pedido.detalles.Add(desglose);
                    }

                    // Obtener imágenes de referencia
                    var queryImagenes = $@"
                SELECT correlativo, id_cotizacion_online, ruta, observacion
                FROM imagen_referencia_online
                WHERE id_cotizacion_online = '{pedido.id_cotizacion_online}'";

                    var resultadoImagenes = db.ExecuteQuery(queryImagenes);

                    foreach (DataRow row in resultadoImagenes.Rows)
                    {
                        var imagen = new imagenreferenciaonlineModel
                        {
                            correlativo = Convert.ToInt32(row["correlativo"]),
                            id_cotizacion_online = Convert.ToInt32(row["id_cotizacion_online"]),
                            ruta = row["ruta"].ToString(),
                            observacion = row["observacion"].ToString()
                        };
                        pedido.imagenes.Add(imagen);
                    }

                    // Obtener observaciones
                    var queryObservaciones = $@"
                SELECT correlativo, id_cotizacion_online, Observacion
                FROM observacion_cotizacion_online
                WHERE id_cotizacion_online = '{pedido.id_cotizacion_online}'";

                    var resultadoObservaciones = db.ExecuteQuery(queryObservaciones);

                    foreach (DataRow row in resultadoObservaciones.Rows)
                    {
                        var observacion = new observacion_cotizacion_onlineModel
                        {
                            correlativo = Convert.ToInt32(row["correlativo"]),
                            id_cotizacion_online = Convert.ToInt32(row["id_cotizacion_online"]),
                            Observacion = row["Observacion"].ToString()
                        };
                        pedido.Observacion.Add(observacion);
                    }

                    // Agregar el pedido completo a la lista
                    listaPedidos.Add(pedido);
                }

                return Ok(listaPedidos);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al obtener los pedidos: {ex.Message}");
            }
        }

        [HttpPost("Nuevo-pedido")]
        public IActionResult NuevoPedido([FromBody] pedidoModelCompleto nuevoPedido)
        {
            try
            {
                // Insertar cotización online
                var queryInsertarCotizacion = $@"
                INSERT INTO cotizacion_online 
                (descripcion, precio_aproximado, envio, hora, fecha, direccion, estado, mano_obra, presupuesto_insumos, cliente_id, total_presupuesto) 
                VALUES 
                ('{nuevoPedido.descripcion}', {nuevoPedido.precio_aproximado}, {nuevoPedido.envio}, '{nuevoPedido.hora}', '{nuevoPedido.fecha}', 
                '{nuevoPedido.direccion}', 1, {nuevoPedido.mano_obra}, {nuevoPedido.presupuesto_insumos}, {nuevoPedido.cliente_id}, {nuevoPedido.total_presupuesto}); 
                SELECT LAST_INSERT_ID();";

                var resultadoCotizacion = db.ExecuteQuery(queryInsertarCotizacion);


                // Obtener el ID de la cotización recién insertada
                var cotizacionId = Convert.ToInt32(resultadoCotizacion.Rows[0][0]);

                // Insertar desgloses online (con precio_pastelera)
                if (nuevoPedido.desglosesOnline != null && nuevoPedido.desglosesOnline.Count > 0)
                {
                    foreach (var desgloseOnline in nuevoPedido.desglosesOnline)
                    {
                        var queryInsertarDesgloseOnline = $@"
                INSERT INTO desglose_online (id_cotizacion_online, id_producto, cantidad, precio_pastelera, subtotal)
                VALUES ({cotizacionId}, {desgloseOnline.id_producto}, {desgloseOnline.cantidad}, {desgloseOnline.precio_pastelera}, {desgloseOnline.subtotal})";
                        db.ExecuteQuery(queryInsertarDesgloseOnline);
                    }
                }

                // Insertar imágenes de referencia online
                if (nuevoPedido.imagenes != null && nuevoPedido.imagenes.Count > 0)
                {
                    foreach (var imagen in nuevoPedido.imagenes)
                    {
                        var queryInsertarImagen = $@"
                INSERT INTO imagen_referencia_online (id_cotizacion_online, ruta, observacion) 
                VALUES ({cotizacionId}, '{imagen.ruta}', '{imagen.observacion}')";
                        db.ExecuteQuery(queryInsertarImagen);
                    }
                }

                // Insertar pedido
                var queryInsertarPedido = $@"
                INSERT INTO pedido (id_estado, observaciones, cotizacion_online_id) 
                VALUES (1, '{nuevoPedido.observaciones}', {cotizacionId}); 
                SELECT LAST_INSERT_ID();";
                var resultadoInsertarPedido = db.ExecuteQuery(queryInsertarPedido);

                // Obtener el ID del pedido recién insertado
                var pedidoId = Convert.ToInt32(resultadoInsertarPedido.Rows[0][0]);

                // Insertar desgloses en detalle_pedido
                if (nuevoPedido.detalles != null && nuevoPedido.detalles.Count > 0)
                {
                    foreach (var desglose in nuevoPedido.detalles)
                    {
                        var total = desglose.precio_unitario * desglose.cantidad_porciones;

                        var queryInsertarDesglose = $@"
                        INSERT INTO detalle_pedido (id_pedido, producto_id, id_masas, id_relleno, cantidad_porciones, precio_unitario, total)
                        VALUES ({pedidoId}, {desglose.producto_id}, {desglose.id_masas}, {desglose.id_relleno}, {desglose.cantidad_porciones}, {desglose.precio_unitario}, {total})";
                        db.ExecuteQuery(queryInsertarDesglose);
                    }
                }

                //// Insertar observaciones
                //if (nuevoPedido.Observacion != null && nuevoPedido.Observacion.Count > 0)
                //{
                //    foreach (var observacion in nuevoPedido.Observacion)
                //    {
                //        var queryInsertarObservacion = $@"
                //        INSERT INTO observacion_cotizacion_online (id_cotizacion_online, Observacion) 
                //        VALUES ({cotizacionId}, '{observacion.Observacion}')";
                //        db.ExecuteQuery(queryInsertarObservacion);
                //    }
                //}

                return Ok("Pedido y cotización registrados correctamente.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al registrar el pedido: {ex.Message}");
            }
        }


        //----------------------------Fin Pedido-----------------------------------------------------------------



        [HttpPost("insertar-imagen-observacion")]
        public async Task<IActionResult> InsertarImagenObservacion([FromForm] List<IFormFile>? imagenes, [FromForm] string observacion, [FromForm] int id_cotizacion_online)
        {
            try
            {
                //    // Verificar que se haya proporcionado un id de cotización online válido
                if (observacion != null)
                {
                    var queryInsertarObservacion = $@"
                                INSERT INTO observacion_cotizacion_online (id_cotizacion_online, Observacion) 
                                VALUES ({id_cotizacion_online}, '{observacion}')";
                    db.ExecuteQuery(queryInsertarObservacion);
                }
                if (imagenes != null)
                {

                    foreach (var imagen in imagenes)
                    {
                        string ruta = await UploadToFirebase(imagen);

                        var queryInsertarImagen = $@"
                        INSERT INTO imagen_referencia_online (id_cotizacion_online, ruta) 
                        VALUES ({id_cotizacion_online}, '{ruta}')";

                        // Ejecutar la consulta
                        var resultado = db.ExecuteQuery(queryInsertarImagen);
                    }
                }


                return Ok("Observacion Y/O Imagenes insertadas correctamente");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al insertar la imagen: {ex.Message}");
            }
        }

        [HttpPost("cambio-estado-pedido")]
        public IActionResult nuevoUsuario([FromBody] cambioEstado cambio)
        {
            try
            {
                    var queryInsertar = $"UPDATE pedido set id_estado = '{cambio.id_estado}' WHERE id = '{cambio.id_pedido}'";
                    db.ExecuteQuery(queryInsertar);
                    return Ok("estado cambiado");
            }
            catch (Exception ex)
            {
                return BadRequest("Error al registrar al Usuario");
            }
        }





    }
}
