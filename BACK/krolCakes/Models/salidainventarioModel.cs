namespace krolCakes.Models
{
    public class salidainventarioModel
    {
        public int? id { get; set; }
        public string? fecha { get; set; }
        public string? notas { get; set; }
        public List<detalleSalidaModelCompleto>? detalleSalida { get; set; }
    }
    public class detalleSalidaModelCompleto
    {
        public int? correlativo { get; set; }
        public int? cantidad { get; set; }
        public int? id_insumo_utensilio { get; set; }    //proviene de modelo insumoutensilio
        public string? nombre { get; set; }         //proviene de modelo insumoutensilio
        public int? id_motivo_salida { get; set; }//proviene de modelo insumoutensilio
        public string? motivo { get; set; }   //proviene de modelo motivosalida
        public int? id_unidad_medida { get; set; }//proviene de modelo insumoutensilio
        public string? unidad { get; set; }//proviene de unidad de medida
        public int? id_encabezado_salida { get; set; }
        public int? cantInsumo { get; set; }
    }
}
