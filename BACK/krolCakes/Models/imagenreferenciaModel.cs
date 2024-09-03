namespace krolCakes.Models
{
    public class imagenreferenciaModel
    {
        public int? id { get; set; }
        public int? id_pedido { get; set; }
        public string? imagen { get; set; }
        public string? observaciones { get; set; }
    }
    public class imagenreferenciaModelCompleto
    {
        public int? id { get; set; }            //proviene de modelo imagenreferencia
        public int? id_pedido { get; set; }     //proviene de modelo imagenreferencia    
        public string? imagen { get; set; }     //proviene de modelo imagenreferencia
        public string? observaciones { get; set; }  //proviene de modelo imagenreferencia
        public DateOnly? fecha { get; set; }    //proviene de modelo pedido
        public TimeOnly? hora { get; set; }     //proviene de modelo pedido
        public int? id_estado { get; set; }     //proviene de modelo pedido
        public int? id_cliente { get; set; }    //proviene de modelo pedido
        public string? observacionespedido { get; set; }  //proviene de modelo pedido
        public string? direccion { get; set; }      //proviene de modelo pedido
        public int? id_tipo_entrega { get; set; }   //proviene de modelo pedido
        public double? precio_total { get; set; }   //proviene de modelo pedido

    }
}
