namespace krolCakes.Models
{
    public class cotizaciononlineModel
    {
        public int? id { get; set; } 
        public string? descripcion { get; set; }
        public double? precio_aproximado { get; set; }
        public Boolean? envio { get; set; }
        public string? hora { get; set; }
        public string? fecha { get; set; }
        public string? direccion { get; set; }
        public Boolean? estado { get; set; }
        public double? mano_obra { get; set; }
        public double? presupuesto_insumos { get; set; }
        public double? total_presupuesto { get; set; }
        public int? cliente_id { get; set; }
        public List<imagenreferenciaonlineModel>? imagenes { get; set; }
        public List<desgloseonlineModel>? desgloses { get; set; }
        public List<observacion_cotizacion_onlineModel>? Observacion { get; set; }
        
    }
    public class cotizaciononlineModelCompleto
    {
        public int? id { get; set; }                    //proviene del modelo cotizacion online 
        public string? descripcion { get; set; }        //proviene del modelo cotizacion online 
        public double? precio_aproximado { get; set; }  //proviene del modelo cotizacion online 
        public Boolean? envio { get; set; }             //proviene del modelo cotizacion online 
        public string? hora { get; set; }               //proviene del modelo cotizacion online 
        public string? fecha { get; set; }              //proviene del modelo cotizacion online 
        public string? direccion { get; set; }          //proviene del modelo cotizacion online 
        public Boolean? estado { get; set; }            //proviene del modelo cotizacion online 
        public double? mano_obra { get; set; }          //proviene del modelo cotizacion online 
        public double? presupuesto_insumos { get; set; }    //proviene del modelo cotizacion online 
        public double? total_presupuesto { get; set; }      //proviene del modelo cotizacion online 
        public int? cliente_id { get; set; }                //proviene del modelo cotizacion online 
        public List<imagenreferenciaonlineModel>? imagenes { get; set; }       //proviene del modelo cotizacion online 
        public List<desgloseonlineModel>? desgloses { get; set; }   //proviene del modelo cotizacion online 
        public List<observacion_cotizacion_onlineModel>? Observacion { get; set; }  //proviene del modelo cotizacion online 
        public string? nombre { get; set; }     //proviene del modelo cliente
        public int? telefono { get; set; }      //proviene del modelo cliente
        public string? nit { get; set; }        //proviene del modelo cliente
    }
}
