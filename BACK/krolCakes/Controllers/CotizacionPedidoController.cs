using krolCakes.Models;
using krolCakes.Provider;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Win32;
using MySql.Data.MySqlClient;
using Mysqlx.Crud;
using System.Data;

namespace krolCakes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CotizacionPedidoController : ControllerBase
    {

        private readonly DatabaseProvider db;
        private readonly Progra progra;

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
                    foreach (var url in urls)
                    {
                        var queryInsertImagen = $"INSERT INTO imagen_referencia_online (ruta, id_cotizacion_online) " +
                                                $"VALUES ('{url}',  {idCotizacion})";
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


                return Ok("Cotización online registrada correctamente");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al registrar la cotización online: {ex.Message}");
            }
        }



        //---------------------Fin cotizacion online-------------------------------------------------------------------------------


    }
}
