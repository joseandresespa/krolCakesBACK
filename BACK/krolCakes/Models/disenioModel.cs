namespace krolCakes.Models
{
    public class disenioModel
    {
        public int? correlativo { get; set; }
        public string? disenio_final { get; set; }
        public int? id_pedido { get; set; }
        public double? precio { get; set; }
    }
    public class disenioModelCompleto
    {
        public disenioModel? disenio { get; set; }
        public pedidoModel? pedido { get; set; }
    }
}
