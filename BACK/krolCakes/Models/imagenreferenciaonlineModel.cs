namespace krolCakes.Models
{
    public class imagenreferenciaonlineModel
    {
        public int? correlativo { get; set; }
        public int? id_cotizacion_online { get; set; }
        public string? ruta { get; set; }
        public string? observacion { get; set; }
    }
    public class imagenreferenciaonlineModelCompleto
    {
        public int? correlativo { get; set; }           //proviene de modelo imagenreferenciaonline  
        public int? id_cotizacion_online { get; set; }  //proviene de modelo imagenreferenciaonline
        public string? ruta { get; set; }               //proviene de modelo imagenreferenciaonline
        public string? observacion { get; set; }        //proviene de modelo imagenreferenciaonline        
        public string? descripcion { get; set; }       //proviene de modelo cotizaciononline
        public int? telefono { get; set; }            //proviene de modelo cotizaciononline
        public int? porciones { get; set; }           //proviene de modelo cotizaciononline
        public int? cant_cupcakes { get; set; }       //proviene de modelo cotizaciononline
        public double? precio_aproximado { get; set; }  //proviene de modelo cotizaciononline
        public bool? envio { get; set; }                //proviene de modelo cotizaciononline

    }
}
