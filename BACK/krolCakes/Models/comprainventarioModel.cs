﻿namespace krolCakes.Models
{
    public class comprainventarioModel
    {
        public int? id { get; set; }
        public double? total { get; set; }
        public DateOnly? fecha_compra { get; set; }
        public int? id_proveedor { get; set; }
    }

    public class comprainventarioModelCompleto
    {
        public comprainventarioModel? comprainventario { get; set; }
        public proveedorModel? proveedor { get; set; }
    }

}