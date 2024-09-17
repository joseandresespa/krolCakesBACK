﻿namespace krolCakes.Models
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
        public string? descripcion { get; set; }       //proviene del modelo cotizacion_online
        public int? telefono { get; set; }              //proviene del modelo cotizacion_online
        public double? precio_aproximado { get; set; }  //proviene del modelo cotizacion_online
        public Boolean? envio { get; set; }             //proviene del modelo cotizacion_online
        public string? nombre { get; set; }             //proviene del modelo cotizacion_online
        public string? fecha { get; set; }              //proviene del modelo cotizacion_online
        public string? hora { get; set; }               //proviene del modelo cotizacion_online
        public string? direccion { get; set; }          //proviene del modelo cotizacion_online
        public List<imagenreferenciaonlineModel>? imagenes { get; set; }    //proviene del modelo cotizacion_online
        public List<desgloseonlineModel>? desgloses { get; set; }   //proviene del modelo cotizacion_online
        public List<observacion_cotizacion_onlineModel>? Observacion { get; set; }  //proviene del modelo cotizacion_online
        public Boolean? estado { get; set; }    //proviene del modelo cotizacion_online
        public double? mano_obra { get; set; }   //proviene del modelo cotizacion_online
        public double? presupuesto_insumos { get; set; }    //proviene del modelo cotizacion_online
        public double? total_presupuesto { get; set; }      //proviene del modelo cotizacion_online

    }
}
