using krolCakes.Models;
using krolCakes.Provider;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;



namespace krolCakes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class inventarioController : ControllerBase
    {
        private readonly DatabaseProvider db;

        public inventarioController(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("ConnectionString");
            db = new DatabaseProvider(connectionString);
        }


        [HttpGet("compras")]
        public IActionResult GetCompras()
        {
            try
            {
                var query = @"SELECT a.id,a.total,DATE_FORMAT(a.fecha_compra, '%Y-%m-%d') AS fecha_compra,
                            a.id_proveedor, b.nombre, b.telefono, b.descripcion
                            FROM compra_inventario a
                            JOIN proveedor b ON a.id_proveedor = b.id;";
                var resultado = db.ExecuteQuery(query);

                var compras = resultado.AsEnumerable().Select(row => new comprainventarioModelCompleto
                {
                    id = row.Field<int?>("id"),
                    total = row.Field<double?>("total"),
                    fecha_compra = row.Field<string>("fecha_compra"),
                    id_proveedor = row.Field<int?>("id_proveedor"),
                    nombre = row.Field<string>("nombre"),//proveedor
                    telefono = row.Field<int?>("telefono"),  // Cambiado a nullable int
                    descripcion = row.Field<string>("descripcion"),
                    detalleCompra = new List<ingresoinventarioModel>()
                }).ToList();

                if (!compras.Any())
                {
                    return Ok(compras);  // Devuelve la lista vacía si no hay compras
                }

                // Realiza una sola consulta para obtener todos los detalles de compras
                var queryDetalles = $@"
                        SELECT a.correlativo, a.id_insumo_utensilio, a.cantidad, a.precio_unitario, a.id_compra_inventario, a.subtotal,
                                b.nombre AS nombreInsumo, b.id_unidad_medida, c.nombre AS nombreUnidadMedida
                        FROM ingreso_inventario a
                        JOIN insumo_utensilio b ON a.id_insumo_utensilio = b.id
                        JOIN unidad_medida c ON b.id_unidad_medida = c.id
                        WHERE a.id_compra_inventario IN ({string.Join(",", compras.Select(c => c.id))});
        ";
                var resultadoDetalles = db.ExecuteQuery(queryDetalles);

                // Agrupa los detalles por compra
                var detallesAgrupados = resultadoDetalles.AsEnumerable().GroupBy(row => row.Field<int>("id_compra_inventario"));

                foreach (var compra in compras)
                {
                    var detalles = detallesAgrupados.FirstOrDefault(g => g.Key == compra.id);
                    if (detalles != null)
                    {
                        compra.detalleCompra = detalles.Select(row => new ingresoinventarioModel
                        {
                            correlativo = row.Field<int>("correlativo"),
                            id_insumo_utensilio = row.Field<int>("id_insumo_utensilio"),
                            cantidad = row.Field<int>("cantidad"), 
                            precio_unitario = row.Field<double>("precio_unitario"),
                            subtotal = row.Field<double>("subtotal"),
                            nombre = row.Field<string>("nombreInsumo"),
                            id_unidad_medida = row.Field<int>("id_unidad_medida"),
                            nombreUnidad = row.Field<string>("nombreUnidadMedida")
                        }).ToList();
                    }
                }

                return Ok(compras);
            }
            catch (Exception ex)
            {
                return BadRequest("Error al obtener las compras con detalles.");
            }
        }


        [HttpPost("nueva-compra")]
        public IActionResult NuevaCompra([FromBody] comprainventarioModelCompleto compra)
        {
            try
            {

                // Inserta la compra
                var queryInsertar = $"INSERT INTO compra_inventario (total, fecha_compra, id_proveedor) " +
                                    $"VALUES ('{compra.total}', '{compra.fecha_compra}', '{compra.id_proveedor}');" +
                                    $"SELECT LAST_INSERT_ID();";
                var resultado = db.ExecuteQuery(queryInsertar);
                if (resultado.Rows.Count > 0)
                {
                    int costoId = Convert.ToInt32(resultado.Rows[0][0]);

                    if (compra.detalleCompra != null) 
                    {
                        foreach (var detalle in compra.detalleCompra)
                        {
                            if (detalle != null)
                            {
                                var querydetalles = $"INSERT INTO ingreso_inventario (id_insumo_utensilio, cantidad, precio_unitario, " +
                                                    $"id_compra_inventario, subtotal) " +
                                                    $"VALUES ('{detalle.id_insumo_utensilio}', '{detalle.cantidad}', '{detalle.precio_unitario}', " +
                                                    $"'{costoId}','{detalle.subtotal}');" +
                                                    $"UPDATE insumo_utensilio SET precio_unitario = {detalle.precio_unitario}, " +
                                                    $"cantidad = cantidad + {detalle.cantidad}" +
                                                    $"WHERE id = '{detalle.id_insumo_utensilio}';";
                                var resultadoDetalles = db.ExecuteQuery(querydetalles);
                            }
                        }
                    }
                    return Ok("Unidad de compra registrada correctamente");
                }
                else
                {
                    throw new Exception("No se pudo obtener el ID insertado.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest("Error al registrar la Compra     .");
            }
        }

        //----------------------------------------------FIN ENTRADAS COMPRAS INVENTARIO---------------------------------------------------

        [HttpGet("salidas")]
        public IActionResult GetSalidas()
        {
            try
            {
                var query = @"SELECT id_salida, DATE_FORMAT(fecha, '%Y-%m-%d') AS fecha, notas FROM encabezado_salida;";
                var resultado = db.ExecuteQuery(query);

                var salidas = resultado.AsEnumerable().Select(row => new salidainventarioModel
                {
                    id = row.Field<int?>("id_salida"),
                    fecha = row.Field<string>("fecha"),
                    notas = row.Field<string>("notas"),
                    detalleSalida = new List<detalleSalidaModelCompleto>()
                }).ToList();

                if (!salidas.Any())
                {
                    return Ok(salidas);  // Devuelve la lista vacía si no hay compras
                }

                // Realiza una sola consulta para obtener todos los detalles de compras
                var queryDetalles = $@"
                        SELECT a.correlativo, a.id_insumo_utensilio, a.cantidad, a.id_motivo_salida, a.id_encabezado_salida,
                        b.nombre , b.id_unidad_medida, b.cantidad AS cantInsumo, c.nombre AS unidad, d.nombre AS motivo
                        FROM salida_inventario a
                        JOIN insumo_utensilio b ON a.id_insumo_utensilio = b.id
                        JOIN unidad_medida c ON b.id_unidad_medida = c.id
                        JOIN motivo_salida d ON a.id_motivo_salida = d.id
                        WHERE a.id_encabezado_salida IN ({string.Join(",", salidas.Select(s => s.id))});";
                var resultadoDetalles = db.ExecuteQuery(queryDetalles);

                // Agrupa los detalles por compra
                var detallesAgrupados = resultadoDetalles.AsEnumerable().GroupBy(row => row.Field<int>("id_encabezado_salida"));

                foreach (var salida in salidas)
                {
                    var detalles = detallesAgrupados.FirstOrDefault(g => g.Key == salida.id);
                    if (detalles != null)
                    {
                        salida.detalleSalida = detalles.Select(row => new detalleSalidaModelCompleto
                        {
                            correlativo = row.Field<int>("correlativo"),
                            id_insumo_utensilio = row.Field<int>("id_insumo_utensilio"),
                            cantidad = row.Field<int>("cantidad"),
                            id_motivo_salida = row.Field<int>("id_motivo_salida"),
                            id_encabezado_salida = row.Field<int>("id_encabezado_salida"),
                            nombre = row.Field<string>("nombre"),
                            id_unidad_medida = row.Field<int>("id_unidad_medida"),
                            cantInsumo = row.Field<int>("cantInsumo"),
                            motivo = row.Field<string>("motivo"),
                            unidad = row.Field<string>("unidad"),
                            
                        }).ToList();
                    }
                }

                return Ok(salidas);
            }
            catch (Exception ex)
            {
                return BadRequest("Error al obtener las compras con detalles.");
            }
        }


        [HttpPost("nueva-salida")]
        public IActionResult NuevaSalida([FromBody] salidainventarioModel salida)
        {
            try
            {

                // Inserta la Salida
                var queryInsertar = $"INSERT INTO encabezado_salida (fecha, notas) " +
                                    $"VALUES ('{salida.fecha}', '{salida.notas}');" +
                                    $"SELECT LAST_INSERT_ID();";
                var resultado = db.ExecuteQuery(queryInsertar);
                if (resultado.Rows.Count > 0)
                {
                    int salidaId = Convert.ToInt32(resultado.Rows[0][0]);

                    if (salida.detalleSalida != null)
                    {
                        foreach (var detalle in salida.detalleSalida)
                        {
                            if (detalle != null)
                            {
                                // Inserta los detalles en la tabla salida_inventario
                                var queryInsert = $"INSERT INTO salida_inventario (id_insumo_utensilio, cantidad, id_motivo_salida, id_encabezado_salida) " +
                                                  $"VALUES ('{detalle.id_insumo_utensilio}', '{detalle.cantidad}', '{detalle.id_motivo_salida}', " +
                                                  $"'{salidaId}');";

                                // Actualiza la cantidad en la tabla insumo_utensilio
                                var queryUpdate = $"UPDATE insumo_utensilio SET cantidad = cantidad - {detalle.cantidad} " +
                                                  $"WHERE id = '{detalle.id_insumo_utensilio}';";

                                // Ejecuta las consultas
                                var resultadoInsert = db.ExecuteQuery(queryInsert);
                                var resultadoUpdate = db.ExecuteQuery(queryUpdate);
                            }
                        }
                    }
                    return Ok("Unidad de Salida registrada correctamente");
                }
                else
                {
                    throw new Exception("No se pudo obtener el ID insertado.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest("Error al registrar la Salida.");
            }
        }

    }
}
