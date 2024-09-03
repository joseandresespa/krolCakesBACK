namespace krolCakes.Models
{
    public class costoModel
    {
        public int? id { get; set; }
        public int? id_pedido { get; set; }
        public double? costo { get; set; }
        public double? ganancia { get; set; }
    }

    public class costoModelCompleto
    {
        public int? id { get; set; }            //proviene de modelo costo
        public int? id_pedido { get; set; }     //proviene de modelo costo
        public double? costo { get; set; }      //proviene de modelo costo
        public double? ganancia { get; set; }   //proviene de modelo costo
        public DateOnly? fecha { get; set; }    //proviene de modelo pedido
        public TimeOnly? hora { get; set; }     //proviene de modelo pedido
        public int? id_estado { get; set; }     //proviene de modelo pedido
        public int? id_cliente { get; set; }    //proviene de modelo pedido
        public string? observaciones { get; set; }  //proviene de modelo pedido
        public string? direccion { get; set; }      //proviene de modelo pedido
        public int? id_tipo_entrega { get; set; }   //proviene de modelo pedido
        public double? precio_total { get; set; }   //proviene de modelo pedido
    }
}
