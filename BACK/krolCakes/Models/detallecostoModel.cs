namespace krolCakes.Models
{
    public class detallecostoModel
    {
        public int? correlativo { get; set; }
        public int? id_costo { get; set; }
        public int? id_insumo_utensilio { get; set; }
        public double? cantidad { get; set; }
    }
    public class detallecostoModelCompleto
    {
        public detallecostoModel? detallecosto { get; set; }
        public costoModel? costo { get; set; }
        public insumoutensilioModel? insumoutensilio { get; set; }
    }

 }
