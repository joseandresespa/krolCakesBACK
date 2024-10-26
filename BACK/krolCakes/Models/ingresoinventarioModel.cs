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
        public string? nombre { get; set; }//proviene de modelo insumoutensilio
        public int? id_unidad_medida { get; set; }//proviene de modelo insumoutensilio
        public string? nombreUnidad { get; set; }//proviene de unidad de medida

    }
    public class ingresoinventarioModelCompleto
    {
        public int? correlativo { get; set; }        //proviene de modelo ingresoinventario
        public int? id_insumo_utensilio { get; set; }   //proviene de modelo ingresoinventario
        public int? cantidad { get; set; }          //proviene de modelo ingresoinventario
        public double? precio_unitario { get; set; }    //proviene de modelo ingresoinventario
        public int? id_compra_inventario { get; set; }  //proviene de modelo ingresoinventario
        public double? subtotal { get; set; }           //proviene de modelo ingresoinventario
        public int? id_tipo_insumo { get; set; }    //proviene de modelo insumoutensilio

        public int? id_unidad_medida { get; set; }  //proviene de modelo insumoutensilio
        public double? precio_unitario_insumo_utensilio { get; set; }    //proviene de modelo insumoutensilio
        public int? cantidad_insumo_utensilio { get; set; }        //proviene de modelo insumoutensilio
        public bool? inventarioRenovable { get; set; }  //proviene de modelo insumoutensilio
        public DateOnly? fecha_ingreso { get; set; }    //proviene de modelo insumoutensilio
        public DateOnly? fecha_vencimiento { get; set; }    //proviene de modelo insumoutensilio
        public double? total { get; set; }         //proviene de modelo comprainventario
        public DateOnly? fecha_compra { get; set; } //proviene de modelo comprainventario
        public int? id_proveedor { get; set; }      //proviene de modelo comprainventario
    }
}
