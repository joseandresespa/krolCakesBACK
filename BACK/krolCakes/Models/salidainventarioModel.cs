namespace krolCakes.Models
{
    public class salidainventarioModel
    {
        public int? correlativo { get; set; }
        public int? id_insumo_utensilio { get; set; }
        public string? cantidad { get; set; }
        public int? id_motivo_salida { get; set; }
    }
    public class salidainventarioModelCompleto
    {
        public int? correlativo { get; set; }               //proviene de modelo salidainventario
        public int? id_insumo_utensilio { get; set; }       //proviene de modelo salidainventario
        public string? cantidad { get; set; }               //proviene de modelo salidainventario
        public int? id_motivo_salida { get; set; }          //proviene de modelo salidainventario
        public int? id_tipo_insumo { get; set; }    //proviene de modelo insumoutensilio
        public string? nombre { get; set; }         //proviene de modelo insumoutensilio
        public int? id_unidad_medida { get; set; }  //proviene de modelo insumoutensilio
        public double? precio_unitario { get; set; }    //proviene de modelo insumoutensilio
        public int? cantidad_insumo_utensilio { get; set; }    //proviene de modelo insumoutensilio
        public bool? inventarioRenovable { get; set; }  //proviene de modelo insumoutensilio
        public DateOnly? fecha_ingreso { get; set; }    //proviene de modelo insumoutensilio
        public DateOnly? fecha_vencimiento { get; set; }    //proviene de modelo insumoutensilio
        public string? nombre_motivo_salida { get; set; }   //proviene de modelo motivosalida
    }
}
