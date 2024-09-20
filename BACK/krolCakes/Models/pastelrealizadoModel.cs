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
        public int? id_estado { get; set; }                 //proviene del modelo pedido
        public string? observaciones { get; set; }          //proviene del modelo pedido
        public string? id_cotizacion_online { get; set; }   //proviene del modelo pedido
    }
}
