﻿namespace krolCakes.Models
{
    public class pedidoModel
    {
        public int? id { get; set; }
        public DateOnly? fecha { get; set; }
        public TimeOnly? hora { get; set; }
        public int? id_estado { get; set; }
        public int? id_cliente { get; set; }
        public string? observaciones { get; set; }
        public string? direccion { get; set; }
        public int? id_tipo_entrega { get; set; }
        public double? precio_total { get; set; }
    }
}
