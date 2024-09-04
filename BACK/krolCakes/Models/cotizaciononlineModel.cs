namespace krolCakes.Models
{
    public class cotizaciononlineModel
    {
        public int? id { get; set; }
        public string? descripcion { get; set; }
        public int? telefono { get; set; }
        public int? porciones { get; set; }
        public int? cant_cupcakes { get; set; }
        public double? precio_aproximado { get; set; }
        public bool? envio { get; set; }
        public double? total { get; set; }
    }
}
