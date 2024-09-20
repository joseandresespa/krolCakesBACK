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
        public int? id_estado { get; set; }                 //proviene del modelo pedido
        public string? observaciones { get; set; }          //proviene del modelo pedido
        public string? id_cotizacion_online { get; set; }   //proviene del modelo pedido
    }
}
