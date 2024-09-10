namespace krolCakes.Models
{
    public class cotizaciononlineModel
    {
        public int? id { get; set; }
        public string? nombre { get; set; }
        public string? descripcion { get; set; }
        public string? direccion { get; set; }
        public int? telefono { get; set; }
        public DateOnly? fecha { get; set; }
        public TimeOnly? hora { get; set; }
        public List<imagenreferenciaonlineModel>? imagenes { get; set; }
        public List<productoModel>? productos { get; set; }
    }
}
