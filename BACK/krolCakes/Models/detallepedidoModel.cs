namespace krolCakes.Models
{
    public class detallepedidoModel
    {
        public int? correlativo { get; set; }
        public int? id_pedido { get; set; }
        public int? producto_id { get; set; }
        public int? id_masas { get; set; }
        public int? id_relleno { get; set; }
        public int? cantidad_porciones { get; set; }
        public double? precio_unitario { get; set; }
        public double? total { get; set; }
    }
}
