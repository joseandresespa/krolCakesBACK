namespace krolCakes.Models
{
    public class detallecostoModel
    {
        public int? correlativo { get; set; }
        public int? id_costo { get; set; }
        public int? id_insumo_utensilio { get; set; }
        public double? cantidad { get; set; }
        public int? id_unidad_medida { get; set; }
    }
    public class detallecostoModelCompleto
    {
        public int? correlativo { get; set; }        //proviene de modelo detallecosto
        public int? id_costo { get; set; }            //proviene de modelo detallecosto
        public int? id_insumo_utensilio { get; set; }     //proviene de modelo detallecosto
        public double? cantidad { get; set; }        //proviene de modelo detallecosto
        public int? id_pedido { get; set; }         //proviene de modelo costo
        public double? costo { get; set; }          //proviene de modelo costo
        public double? ganancia { get; set; }       //proviene de modelo costo      
        public int? id_tipo_insumo { get; set; }    //proviene de modelo insumoutensilio
        public string? nombre { get; set; }         //proviene de modelo insumoutensilio
        public int? id_unidad_medida { get; set; }  //proviene de modelo insumoutensilio
        public double? precio_unitario { get; set; }    //proviene de modelo insumoutensilio
        public int? cantidad_insumo_utensilio { get; set; }    //proviene de modelo insumoutensilio
        public bool? inventarioRenovable { get; set; }  //proviene de modelo insumoutensilio
        public DateOnly? fecha_ingreso { get; set; }    //proviene de modelo insumoutensilio
        public DateOnly? fecha_vencimiento { get; set; }    //proviene de modelo insumoutensilio

    }

}
