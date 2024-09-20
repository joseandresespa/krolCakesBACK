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
        public int? id_estado { get; set; }                 //proviene del modelo pedido
        public string? observaciones_pedido { get; set; }          //proviene del modelo pedido
        public string? id_cotizacion_online { get; set; }   //proviene del modelo pedido

    }
}
