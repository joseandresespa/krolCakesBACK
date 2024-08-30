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
        public salidainventarioModel? salidainventario { get; set; }
        public insumoutensilioModel? insumoutensilio { get; set; }
        public motivosalidaModel? motivosalida { get; set; }
    }
}
