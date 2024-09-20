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
        public string? nombrep { get; set; }         // nombre de producto
        public string? descripcionproducto { get; set; }     //proviene de modelo producto
        public double? precio_online { get; set; }      //proviene de modelo producto

    }
}
