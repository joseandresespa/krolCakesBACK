namespace krolCakes.Models
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
        public int? id { get; set; }               //proviene de modelo comprainventario 
        public double? total { get; set; }         //proviene de modelo comprainventario
        public string? fecha_compra { get; set; } //proviene de modelo comprainventario
        public int? id_proveedor { get; set; }      //proviene de modelo comprainventario
        public string? nombre { get; set; }         // nombre proveedor
        public int? telefono { get; set; }          //proviene de modelo proveedor
        public string? descripcion { get; set; }    //proviene de modelo proveedor
        public List<ingresoinventarioModel>? detalleCompra { get; set; }

    }

}