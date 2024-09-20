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
        public int? correlativo { get; set; }       //proviene de modelo disenio
        public string? disenio_final { get; set; }  //proviene de modelo disenio
        public int? id_pedido { get; set; }        //proviene de modelo disenio
        public double? precio { get; set; }       //proviene de modelo disenio                                 
        public int? id_estado { get; set; }                 //proviene del modelo pedido
        public string? observaciones { get; set; }          //proviene del modelo pedido
        public string? id_cotizacion_online { get; set; }   //proviene del modelo pedido


    }
}
