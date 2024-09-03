namespace krolCakes.Models
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
        public int? correlativo { get; set; }            //proviene de modelo  preciosugerido
        public int? id_insumo_utensilio { get; set; }    //proviene de modelo  preciosugerido
        public int? id_unidad_medida_precio_sugerido { get; set; }   //proviene de modelo  preciosugerido
        public double? precio { get; set; }  //proviene de modelo  preciosugerido
        public int? id_tipo_insumo { get; set; }    //proviene de modelo insumoutensilio
        public string? nombre { get; set; }         //proviene de modelo insumoutensilio
        public int? id_unidad_medida { get; set; }  //proviene de modelo insumoutensilio
        public double? precio_unitario_insumo_utensilio { get; set; }    //proviene de modelo insumoutensilio
        public int? cantidad_insumo_utensilio { get; set; }        //proviene de modelo insumoutensilio
        public bool? inventarioRenovable { get; set; }  //proviene de modelo insumoutensilio
        public DateOnly? fecha_ingreso { get; set; }    //proviene de modelo insumoutensilio
        public DateOnly? fecha_vencimiento { get; set; }    //proviene de modelo insumoutensilio
        public string? unidad_medida_precio_sugerido { get; set; }   //proviene de modelo unidadmedidapreciosugerido
    }
}
