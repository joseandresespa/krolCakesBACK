namespace krolCakes.Models
{
    public class desgloseonlineModel
    {
        public int? correlativo { get; set; }
        public double? precio { get; set; }
        public int? id_cotizacion_online { get; set; }
        public int? id_producto { get; set; }
    }
    public class desgloseonlineModelCompleto
    {
        public desgloseonlineModel? desgloseonline { get; set; }
        public cotizaciononlineModel? cotizaciononline { get; set; }
        public productoModel? producto { get; set; }
    }
}
