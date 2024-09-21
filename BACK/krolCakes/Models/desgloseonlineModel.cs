namespace krolCakes.Models
{
    public class desgloseonlineModel
    {
        public int? correlativo { get; set; }
        public int? id_cotizacion_online { get; set; }
        public int? id_producto { get; set; }
        public double? subtotal { get; set; }
        public int? cantidad { get; set; }
        public double? precio_pastelera { get; set; }

    }
    public class desgloseonlineModelCompleto
    {
        public int? correlativo { get; set; }      //proviene de modelo desgloseonline
        public double? precio { get; set; }             //proviene de modelo desgloseonline
        public int? id_cotizacion_online { get; set; }  //proviene de modelo desgloseonline
        public int? id_producto { get; set; }           //proviene de modelo desgloseonline
        public double? subtotal { get; set; }            //proviene de modelo desgloseonline
        public int? cantidad { get; set; }               //proviene de modelo desgloseonline
        public double? precio_pastelera { get; set; }      //proviene de modelo desgloseonline
        public string? nombrep { get; set; }         // nombre de producto
        public string? descripcionproducto { get; set; }     //proviene de modelo producto
        public double? precio_online { get; set; }      //proviene de modelo producto

    }
}
