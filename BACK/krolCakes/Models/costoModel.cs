﻿namespace krolCakes.Models
{
    public class costoModel
    {
        public int? id { get; set; }
        public int? id_pedido { get; set; }
        public double? costo { get; set; }
        public double? ganancia { get; set; }
    }

    public class costoModelCompleto
    {
        public costoModel? costo { get; set; }
        public pedidoModel? pedido { get; set; }

    }
}