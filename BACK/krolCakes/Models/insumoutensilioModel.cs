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
        public insumoutensilioModel? insumoutensilio { get; set; }
        public tipoinsumoutensilioModel? tipoinsumoutensilio { get; set; }
        public unidadmedidaModel? unidadmedida { get; set; }
    }

}
