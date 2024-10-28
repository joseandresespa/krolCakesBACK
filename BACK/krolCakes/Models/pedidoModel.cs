    namespace krolCakes.Models
{
    public class pedidoModel
    {
        public int? id { get; set; }
        public int? id_estado { get; set; }  
        public string? observaciones { get; set; } 
        public string? id_cotizacion_online { get; set; }
        
    }
    public class pedidoModelCompleto
    {
        public int? id { get; set; }                        //proviene del modelo pedido                                  
        public int? id_estado { get; set; }                 //proviene del modelo pedido
        public string? observaciones { get; set; }          //proviene del modelo pedido
        
        public string? estado { get; set; }     //proviene del modelo estado
        public string? id_cotizacion_online { get; set; }   //proviene del modelo pedido
        public string? descripcion { get; set; }        //proviene del modelo cotizacion online 
        public double? precio_aproximado { get; set; }  //proviene del modelo cotizacion online 
        public Boolean? envio { get; set; }             //proviene del modelo cotizacion online 
        public string? hora { get; set; }               //proviene del modelo cotizacion online 
        public string? fecha { get; set; }              //proviene del modelo cotizacion online 
        public string? direccion { get; set; }          //proviene del modelo cotizacion online 
        public Boolean? estado_cotizacion_online { get; set; }   //proviene del modelo cotizacion online 
        public double? mano_obra { get; set; }          //proviene del modelo cotizacion online 
        public double? presupuesto_insumos { get; set; }    //proviene del modelo cotizacion online 
        public double? total_presupuesto { get; set; }      //proviene del modelo cotizacion online 
        public int? cliente_id { get; set; }                //proviene del modelo cotizacion online 
        public List<desgloseonlineModel>? desglosesOnline { get; set; }   //proviene del modelo cotizacion online
        public List<imagenreferenciaonlineModel>? imagenes { get; set; }       //proviene del modelo cotizacion online 
        public List<detallepedidoModelCompleto>? detalles { get; set; }   //proviene del modelo cotizacion online 
        public List<observacion_cotizacion_onlineModel>? Observacion { get; set; }  //proviene del modelo cotizacion online 
        public string? nombre { get; set; }     //proviene del modelo cliente
        public int? telefono { get; set; }      //proviene del modelo cliente
        public string? nit { get; set; }        //proviene del modelo cliente
        public int? idCosto { get; set; }
        public double? costo { get; set; }     
        public double? ganancia { get; set; }
        public List<detallecostoModel>? detalleCosto { get; set; }  //proviene del modelo cotizacion online 
        public List<pastelrealizadoModel>? imagenesFin { get; set; }  //proviene del modelo cotizacion online 

    }

    public class ConfirmarPedidoModel
    {
        public int? id_cotizacion_online { get; set; }
        public string? observaciones { get; set; } 
        public int? id_pedido { get; set; }              
        public List<detallepedidoModel>? detalles { get; set; }
        public double? mano_obra { get; set; }          //proviene del modelo cotizacion online 
        public double? presupuesto_insumos { get; set; }    //proviene del modelo cotizacion online 
        public double? total_presupuesto { get; set; }

    }

}
