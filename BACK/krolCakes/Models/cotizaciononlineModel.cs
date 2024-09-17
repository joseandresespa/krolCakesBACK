﻿namespace krolCakes.Models
{
    public class cotizaciononlineModel
    {
        public int? id { get; set; } 
        public string? descripcion { get; set; }
        public int? telefono { get; set; }
        public double? precio_aproximado { get; set; }
        public Boolean? envio { get; set; }
        public string? nombre { get; set; }
        public string? fecha { get; set; }
        public string? hora { get; set; }
        public string? direccion { get; set; }
        public List<imagenreferenciaonlineModel>? imagenes { get; set; }
        public List<desgloseonlineModel>? desgloses { get; set; }
        public List<observacion_cotizacion_onlineModel>? Observacion { get; set; }
        public Boolean? estado { get; set; }
        public double? mano_obra { get; set; }
        public double? presupuesto_insumos { get; set; }
        public double? total_presupuesto { get; set; }

    }
}
