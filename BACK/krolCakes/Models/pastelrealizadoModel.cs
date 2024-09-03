namespace krolCakes.Models
{
    public class pastelrealizadoModel
    {
        public int? id { get; set; }
        public int? id_tipo_evento { get; set; }
        public int? id_pedido { get; set; }
        public string? imagen { get; set; }
    }
    public class pastelrealizadoModelCompleto
    {
        public int? id { get; set; }                //proviene de modelo pastelrealizado
        public int? id_tipo_evento { get; set; }    //proviene de modelo pastelrealizado
        public int? id_pedido { get; set; }         //proviene de modelo pastelrealizado
        public string? imagen { get; set; }         //proviene de modelo pastelrealizado
        public string? nombre { get; set; }         //nombre de tipoevento
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
