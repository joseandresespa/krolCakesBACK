namespace krolCakes.Models
{
    public class imagenreferenciaonlineModel
    {
        public int? correlativo { get; set; }
        public int? id_cotizacion_online { get; set; }
        public string? ruta { get; set; }
        public string? observacion { get; set; }
    }
    public class imagenreferenciaonlineModelCompleto
    {
        public imagenreferenciaonlineModel? imagenreferenciaonline { get; set; }
        public cotizaciononlineModel? cotizaciononline { get; set; }
    }
}
