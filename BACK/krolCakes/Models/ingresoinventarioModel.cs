namespace krolCakes.Models
{
    public class ingresoinventarioModel
    {
        public int? correlativo { get; set; }
        public int? id_insumo_utensilio { get; set; }
        public int? cantidad { get; set; }
        public double? precio_unitario { get; set; }
        public int? id_compra_inventario { get; set; }
        public double? subtotal { get; set; }

    }
    public class ingresoinventarioModelCompleto
    {
        public ingresoinventarioModel? ingresoinventario { get; set; }
        public insumoutensilioModel? insumoutensilio { get; set; }
        public comprainventarioModel? comprainventario { get; set; }
    }
}
