namespace krolCakes.Models
{
    public class pedidoModel
    {
        public int? id { get; set; }
        public DateOnly? fecha { get; set; }
        public TimeOnly? hora { get; set; }
        public int? id_estado { get; set; }
        public int? id_cliente { get; set; }
        public string? observaciones { get; set; }
        public string? direccion { get; set; }
        public int? id_tipo_entrega { get; set; }
        public double? precio_total { get; set; }
        public double? mano_obra { get; set; }
        public double? presupuesto_insumos { get; set; }
    }
    public class pedidoModelCompleto
    {
        public int id { get; set; }             //proviene de modelo pedido
        public DateOnly? fecha { get; set; }    //proviene de modelo pedido
        public TimeOnly? hora { get; set; }     //proviene de modelo pedido
        public int? id_estado { get; set; }     //proviene de modelo pedido
        public int? id_cliente { get; set; }    //proviene de modelo pedido
        public string? observaciones { get; set; }  //proviene de modelo pedido
        public string? direccion { get; set; }      //proviene de modelo pedido
        public int? id_tipo_entrega { get; set; }   //proviene de modelo pedido
        public double? precio_total { get; set; }   //proviene de modelo pedido
        public double? mano_obra { get; set; }
        public double? presupuesto_insumos { get; set; }
        public string? estado { get; set; }         //proviene de modelo estado      
        public string? nombre { get; set; }        //nombre de cliente 
        public int? telefono { get; set; }        //proviene de modelo cliente   
        public string? nit { get; set; }         //proviene de modelo cliente
        public string? nombre_tipo_entrega { get; set; }     //nombre de tipoentrega


    }

}
