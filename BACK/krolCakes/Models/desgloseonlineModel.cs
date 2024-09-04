namespace krolCakes.Models
{
    public class desgloseonlineModel
    {
        public int? correlativo { get; set; }
        public double? precio { get; set; }
        public int? id_cotizacion_online { get; set; }
        public int? id_producto { get; set; }
        public double? subtotal { get; set; }
        public int? cantidad { get; set; }
    }
    public class desgloseonlineModelCompleto
    {
        public int? correlativo { get; set; } = 0;      //proviene de modelo desgloseonline
        public double? precio { get; set; }             //proviene de modelo desgloseonline
        public int? id_cotizacion_online { get; set; }  //proviene de modelo desgloseonline
        public int? id_producto { get; set; }           //proviene de modelo desgloseonline
        public double? subtotal { get; set; }
        public int? cantidad { get; set; }                                               
        public string? descripcion { get; set; }       //proviene de modelo cotizaciononline  
        public int? telefono { get; set; }            //proviene de modelo cotizaciononline 
        public int? porciones { get; set; }           //proviene de modelo cotizaciononline   
        public int? cant_cupcakes { get; set; }       //proviene de modelo cotizaciononline   
        public double? precio_aproximado { get; set; }  //proviene de modelo cotizaciononline 
        public bool? envio { get; set; }               //proviene de modelo cotizaciononline 
        public string? nombre { get; set; }         // nombre de producto
        public string? descripcionproducto { get; set; }     //proviene de modelo producto
        public double? precio_online { get; set; }      //proviene de modelo producto
    }
}
