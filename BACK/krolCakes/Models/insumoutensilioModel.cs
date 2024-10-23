namespace krolCakes.Models
{
    public class insumoutensilioModel
    {
        public int? id { get; set; }
        public int? id_tipo_insumo { get; set; }
        public string? nombre { get; set; }
        public int? id_unidad_medida { get; set; }
        public double? precio_unitario { get; set; }
        public int? cantidad { get; set; }
        public bool? inventarioRenovable { get; set; }
        public DateOnly? fecha_ingreso { get; set; }
        public DateOnly? fecha_vencimiento { get; set; }
    }
    public class insumoutensilioModelCompleto
    {
        public int? id { get; set; }                //proviene de modelo insumoutensilio
        public int? id_tipo_insumo { get; set; }    //proviene de modelo insumoutensilio
        public string? nombre { get; set; }         //proviene de modelo insumoutensilio
        public int? id_unidad_medida { get; set; }  //proviene de modelo insumoutensilio
        public double? precio_unitario { get; set; }    //proviene de modelo insumoutensilio
        public int? cantidad { get; set; }        //proviene de modelo insumoutensilio
        public bool? inventarioRenovable { get; set; }  //proviene de modelo insumoutensilio
        public string? fecha_ingreso { get; set; }    //proviene de modelo insumoutensilio
        public string? fecha_vencimiento { get; set; }    //proviene de modelo insumoutensilio
        public string? tipo { get; set; }   //proviene de modelo tipoinsumoutensilio
        public string? nombre_unidad_medida { get; set; }     //nombre de modelo unidadmedida
    }

}
