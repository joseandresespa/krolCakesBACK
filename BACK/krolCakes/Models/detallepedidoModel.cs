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
    public class detallepedidoModelCompleto
    {
        public detallepedidoModel? detallepedido { get; set; }
        public pedidoModel? pedido { get; set; }
        public productoModel? producto { get; set; }
        public masaModel? masa { get; set; }
        public rellenoModel? relleno { get; set; }
    }
}
