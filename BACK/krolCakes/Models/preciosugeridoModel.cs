﻿namespace krolCakes.Models
{
    public class preciosugeridoModel
    {
        public int? correlativo { get; set; }
        public int? id_insumo_utensilio { get; set; }
        public int? id_unidad_medida_precio_sugerido { get; set; }
        public double? precio { get; set; }
    }
    public class preciosugeridoModelCompleto
    {
        public preciosugeridoModel? preciosugerido { get; set; }
        public insumoutensilioModel? insumoutensilio { get; set; }
        public unidadmedidapreciosugeridoModel? unidadmedidapreciosugerido { get; set; }
    }
}