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
        public int? correlativo { get; set; }        //proviene de modelo detallepedido
        public int? id_pedido { get; set; }          //proviene de modelo detallepedido
        public int? producto_id { get; set; }        //proviene de modelo detallepedido
        public int? id_masas { get; set; }           //proviene de modelo detallepedido
        public int? id_relleno { get; set; }         //proviene de modelo detallepedido
        public int? cantidad_porciones { get; set; }     //proviene de modelo detallepedido
        public double? precio_unitario { get; set; }     //proviene de modelo detallepedido
        public double? total { get; set; }           //proviene de modelo detallepedido
        public DateOnly? fecha { get; set; }         //proviene de modelo pedido
        public TimeOnly? hora { get; set; }          //proviene de modelo pedido
        public int? id_estado { get; set; }          //proviene de modelo pedido
        public int? id_cliente { get; set; }         //proviene de modelo pedido
        public string? observaciones { get; set; }   //proviene de modelo pedido
        public string? direccion { get; set; }       //proviene de modelo pedido
        public int? id_tipo_entrega { get; set; }    //proviene de modelo pedido
        public double? precio_total { get; set; }    //proviene de modelo pedido        
        public string? nombre { get; set; }         //nombre de producto
        public string? descripcion { get; set; }    //proviene de modelo producto
        public double? precio_online { get; set; }  //proviene de modelo producto
        public string? sabor_masa { get; set; }     //proviene de modelo masas
        public string? sabor_relleno { get; set; } //proviene de modelo relleno

    }
}
