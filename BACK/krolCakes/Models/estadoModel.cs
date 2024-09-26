namespace krolCakes.Models
{
    public class estadoModel
    {
        public int? id { get; set; }
        public string? estado { get; set; }
    }

    public class cambioEstado
    {
        public int id_pedido { get; set; }
        public int id_estado { get; set; }
    }
}
